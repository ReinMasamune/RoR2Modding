namespace ReinCore
{
    using System;
    using System.Collections.Generic;

    using RoR2.Networking;

    using UnityEngine.Networking;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static partial class NetworkCore
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean loaded { get; internal set; } = false;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Int16 messageIndex { get => 27182; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Int16 commandIndex { get => 8182; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        ////public static Int16 requestIndex { get => 8459; }
        ////public static Int16 responseIndex { get => 04523; }
        //public static Int32 channel { get => (Int32)QosType.Reliable; }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean RegisterMessageType<TMessage>() where TMessage : INetMessage, new()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean RegisterCommandType<TCommand>() where TCommand : INetCommand, new()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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

        static NetworkCore()
        {
            NetworkServer.RegisterHandler( messageIndex, HandleMessageServer );
            NetworkServer.RegisterHandler( commandIndex, HandleCommandServer );

            _ = RegisterMessageType<DamageMessage>();
            _ = RegisterMessageType<BuffMessage>();
            _ = RegisterMessageType<DoTMessage>();
            _ = RegisterMessageType<OrbMessage>();

            GameNetworkManager.onStartClientGlobal += RegisterClientMessages;

            loaded = true;
        }

        internal static Int32 GetNetworkCoreHash( Type type ) => String.Format( "{0}{1}", type.Assembly.FullName, type.FullName ).GetHashCode();
        private static NetworkWriter universalWriter { get; } = new NetworkWriter();
        internal static Writer GetWriter( Int16 messageIndex, NetworkConnection connection, QosType qos ) => new Writer( universalWriter, messageIndex, connection, qos );
        private static void RegisterClientMessages( NetworkClient client )
        {
            client.RegisterHandler( messageIndex, HandleMessageClient );
            client.RegisterHandler( commandIndex, HandleCommandClient );
        }
        private static readonly Dictionary<Int32,INetMessage> netMessages = new Dictionary<Int32, INetMessage>();
        private static readonly Dictionary<Int32,INetCommand> netCommands = new Dictionary<Int32, INetCommand>();

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
    }
}
