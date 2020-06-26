namespace ReinCore
{
    using System;
    using System.Collections.Generic;

    using RoR2.Networking;

    using UnityEngine.Networking;

    public static partial class NetworkCore
    {
        public static Boolean loaded { get; internal set; } = false;
        public static Int16 messageIndex { get => 27182; }
        public static Int16 commandIndex { get => 8182; }
        public static Int16 requestIndex { get => 31415; }
        public static Int16 replyIndex { get => 5131; }

        public static Boolean RegisterMessageType<TMessage>() where TMessage : INetMessage, new()
        {
            var inst = new TMessage();
            Type type = inst.GetType();

            Int32 hash = GetNetworkCoreHash( type );
            if( netMessages.ContainsKey( hash ) )
            {
                Log.Error( "Tried to register a message type with a duplicate hash" );
                return false;
            } else
            {
                netMessages[hash] = inst;
                return true;
            }
        }

        public static Boolean RegisterCommandType<TCommand>() where TCommand : INetCommand, new()
        {
            var inst = new TCommand();
            Type type = inst.GetType();
            Int32 hash = GetNetworkCoreHash( type );

            if( netCommands.ContainsKey( hash ) )
            {
                Log.Error( "Tried to register a command type with a duplicate hash" );
                return false;
            } else
            {
                netCommands[hash] = inst;
                return true;
            }
        }

        public static Boolean RegisterRequestType<TRequest,TReply>()
            where TRequest : INetRequest<TRequest,TReply>, new()
            where TReply : INetRequestReply<TRequest,TReply>, new()
        {
            var inst = new TRequest();
            Type type = inst.GetType();
            Int32 hash = GetNetworkCoreHash( type );

            if( netRequests.ContainsKey( hash ) )
            {
                Log.Error( "Tried to register a request type with a duplicate hash" );
                return false;
            } else
            {
                netRequests[hash] = new RequestPerformer<TRequest,TReply>( inst, new TReply() );
                return true;
            }
        }

        static NetworkCore()
        {
            _ = RegisterMessageType<DamageMessage>();
            _ = RegisterMessageType<BuffMessage>();
            _ = RegisterMessageType<DoTMessage>();
            _ = RegisterMessageType<OrbMessage>();

            GameNetworkManager.onStartServerGlobal += RegisterServerMessages;
            GameNetworkManager.onStartClientGlobal += RegisterClientMessages;

            loaded = true;
        }

        internal static Int32 GetNetworkCoreHash( Type type ) => String.Format( "{0}{1}", type.Assembly.FullName, type.FullName ).GetHashCode();
        private static NetworkWriter universalWriter { get; } = new NetworkWriter();
        internal static Writer GetWriter( Int16 messageIndex, NetworkConnection connection, QosType qos ) => new Writer( universalWriter, messageIndex, connection, qos );
        private static void RegisterClientMessages( NetworkClient client )
        {
            Log.Message( "Client messages registered" );
            client.RegisterHandler( messageIndex, HandleMessageClient );
            client.RegisterHandler( commandIndex, HandleCommandClient );
            client.RegisterHandler( requestIndex, HandleRequestClient );
            client.RegisterHandler( replyIndex, HandleReplyClient );
        }
        private static void RegisterServerMessages()
        {
            Log.Message( "Server messages registered" );
            NetworkServer.RegisterHandler( messageIndex, HandleMessageServer );
            NetworkServer.RegisterHandler( commandIndex, HandleCommandServer );
            NetworkServer.RegisterHandler( requestIndex, HandleRequestServer );
            NetworkServer.RegisterHandler( replyIndex, HandleReplyServer );
        }
        private static readonly Dictionary<Int32,INetMessage> netMessages = new Dictionary<Int32, INetMessage>();
        private static readonly Dictionary<Int32,INetCommand> netCommands = new Dictionary<Int32, INetCommand>();
        private static readonly Dictionary<Int32,RequestPerformerBase> netRequests = new Dictionary<Int32, RequestPerformerBase>();

        private static void HandleCommandServer( NetworkMessage mag )
        {
            NetworkReader reader = mag.reader;
            Header header = reader.ReadNew<Header>();

            if( header.destination.ShouldRun() )
            {
                header.RemoveDestination( NetworkDestination.Server );

                if( netCommands.TryGetValue( header.typeCode, out INetCommand command ) )
                {
                    command.OnRecieved();
                } else
                {
                    Log.Error( "Unhandled command recieved, you may be missing mods" );
                }
            }

            if( header.destination.ShouldSend() )
            {
                Int32 recievedFrom = mag.conn.connectionId;
                for( Int32 i = 0; i < NetworkServer.connections.Count; ++i )
                {
                    if( i == recievedFrom )
                    {
                        continue;
                    }

                    NetworkConnection conn = NetworkServer.connections[i];
                    if( conn == null )
                    {
                        continue;
                    }

                    using( Writer netWriter = GetWriter( commandIndex, conn, QosType.Reliable ) )
                    {
                        NetworkWriter writer = netWriter;
                        writer.Write( header );
                    }
                }

            }
        }

        private static void HandleMessageServer( NetworkMessage msg )
        {
            NetworkReader reader = msg.reader;
            Header header = reader.ReadNew<Header>();

            if( header.destination.ShouldRun() )
            {
                header.RemoveDestination( NetworkDestination.Server );

                if( netMessages.TryGetValue( header.typeCode, out INetMessage message ) )
                {
                    message.Deserialize( reader );
                    message.OnRecieved();
                } else
                {
                    Log.Error( "Unhandled message recieved, you may be missing mods" );
                }
            }

            if( header.destination.ShouldSend() )
            {
                Int32 recievedFrom = msg.conn.connectionId;
                Byte[] bytes = reader.ReadBytes( (Int32)(reader.Length - reader.Position) );
                for( Int32 i = 0; i < NetworkServer.connections.Count; ++i )
                {
                    if( i == recievedFrom )
                    {
                        continue;
                    }

                    NetworkConnection conn = NetworkServer.connections[i];
                    if( conn == null )
                    {
                        continue;
                    }

                    using( Writer netWriter = GetWriter( messageIndex, conn, QosType.Reliable ) )
                    {
                        NetworkWriter writer = netWriter;
                        writer.Write( header );
                        writer.WriteBytesFull( bytes );
                    }
                }
            }
        }



        private static void HandleRequestServer( NetworkMessage msg )
        {
            NetworkReader reader = msg.reader;
            Header header = reader.ReadNew<Header>();

            if( header.destination.ShouldRun() )
            {
                header.RemoveDestination( NetworkDestination.Server );

                if( netRequests.TryGetValue( header.typeCode, out RequestPerformerBase requestPerformer ) )
                {
                    var reply = requestPerformer.PerformRequest( reader );
                    var replyHeader = new Header(header.typeCode, NetworkDestination.Clients );

                    using( Writer netWriter = GetWriter( replyIndex, msg.conn, QosType.Reliable ) )
                    {
                        NetworkWriter writer = netWriter;
                        writer.Write( replyHeader );
                        writer.Write( reply );
                    }
                } else
                {
                    Log.Error( "Unhandled message recieved, you may be missing mods" );
                }
            }

            if( header.destination.ShouldSend() )
            {
                Int32 recievedFrom = msg.conn.connectionId;
                Byte[] bytes = reader.ReadBytes( (Int32)(reader.Length - reader.Position) );
                for( Int32 i = 0; i < NetworkServer.connections.Count; ++i )
                {
                    if( i == recievedFrom )
                    {
                        continue;
                    }

                    NetworkConnection conn = NetworkServer.connections[i];
                    if( conn == null )
                    {
                        continue;
                    }

                    using( Writer netWriter = GetWriter( requestIndex, conn, QosType.Reliable ) )
                    {
                        NetworkWriter writer = netWriter;
                        writer.Write( header );
                        writer.WriteBytesFull( bytes );
                    }
                }
            }
        }

        private static void HandleReplyServer( NetworkMessage msg )
        {
            NetworkReader reader = msg.reader;
            Header header = reader.ReadNew<Header>();

            if( header.destination.ShouldRun() )
            {
                header.RemoveDestination( NetworkDestination.Server );

                if( netMessages.TryGetValue( header.typeCode, out INetMessage message ) )
                {
                    message.Deserialize( reader );
                    message.OnRecieved();
                } else
                {
                    Log.Error( "Unhandled message recieved, you may be missing mods" );
                }
            }

            if( header.destination.ShouldSend() )
            {
                Int32 recievedFrom = msg.conn.connectionId;
                Byte[] bytes = reader.ReadBytes( (Int32)(reader.Length - reader.Position) );
                for( Int32 i = 0; i < NetworkServer.connections.Count; ++i )
                {
                    if( i == recievedFrom )
                    {
                        continue;
                    }

                    NetworkConnection conn = NetworkServer.connections[i];
                    if( conn == null )
                    {
                        continue;
                    }

                    using( Writer netWriter = GetWriter( messageIndex, conn, QosType.Reliable ) )
                    {
                        NetworkWriter writer = netWriter;
                        writer.Write( header );
                        writer.WriteBytesFull( bytes );
                    }
                }
            }
        }

        private static void HandleCommandClient( NetworkMessage msg )
        {
            NetworkReader reader = msg.reader;
            Header header = reader.ReadNew<Header>();

            if( header.destination.ShouldRun() )
            {
                header.RemoveDestination( NetworkDestination.Clients );

                if( netCommands.TryGetValue( header.typeCode, out INetCommand command ) )
                {
                    command.OnRecieved();
                } else
                {
                    Log.Error( "Unhandled command recieved, you may be missing mods" );
                }
            }
        }

        private static void HandleMessageClient( NetworkMessage msg )
        {
            NetworkReader reader = msg.reader;
            Header header = reader.ReadNew<Header>();

            if( header.destination.ShouldRun() )
            {
                header.RemoveDestination( NetworkDestination.Clients );

                if( netMessages.TryGetValue( header.typeCode, out INetMessage message ) )
                {
                    message.Deserialize( reader );
                    message.OnRecieved();
                } else
                {
                    Log.Error( "Unhandled message recieved, you may be missing mods" );
                }
            }
        }

        private static void HandleRequestClient( NetworkMessage msg )
        {

        }

        private static void HandleReplyClient( NetworkMessage msg )
        {

        }
    }
}
