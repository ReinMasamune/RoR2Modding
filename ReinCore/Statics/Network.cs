using System;
using System.Collections.Generic;
using Mono.Cecil;
using RoR2;
using RoR2.Networking;
using UnityEngine.Networking;

namespace ReinCore
{
    public static partial class NetworkCore
    {
        public static Boolean loaded { get; internal set; } = false;
        public static Int16 messageIndex { get => 27182; }
        public static Int16 commandIndex { get => 8182; }
        ////public static Int16 requestIndex { get => 8459; }
        ////public static Int16 responseIndex { get => 04523; }
        //public static Int32 channel { get => (Int32)QosType.Reliable; }

        public static Boolean RegisterMessageType<TMessage>() where TMessage : INetMessage, new()
        {
            var inst = new TMessage();
            var type = inst.GetType();

            var hash = GetNetworkCoreHash( type );
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
            var type = inst.GetType();
            var hash = GetNetworkCoreHash( type );

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

        internal static Int32 GetNetworkCoreHash( Type type )
        {
            return String.Format( "{0}{1}", type.Assembly.FullName, type.FullName ).GetHashCode();
        }
        private static NetworkWriter universalWriter { get; } = new NetworkWriter();
        internal static Writer GetWriter( Int16 messageIndex, NetworkConnection connection, QosType qos )
        {
            return new Writer( universalWriter, messageIndex, connection, qos );
        }
        private static void RegisterClientMessages( NetworkClient client )
        {
            client.RegisterHandler( messageIndex, HandleMessageClient );
            client.RegisterHandler( commandIndex, HandleCommandClient );
        }
        private static Dictionary<Int32,INetMessage> netMessages = new Dictionary<Int32, INetMessage>();
        private static Dictionary<Int32,INetCommand> netCommands = new Dictionary<Int32, INetCommand>();

        private static void HandleCommandServer( NetworkMessage mag )
        {
            var reader = mag.reader;
            var header = reader.ReadNew<Header>();

            if( header.destination.ShouldRun() )
            {
                header.RemoveDestination( NetworkDestination.Server );

                if( netCommands.TryGetValue( header.typeCode, out var command ) )
                {
                    command.OnRecieved();
                } else
                {
                    Log.Error( "Unhandled command recieved, you may be missing mods" );
                }
            }

            if( header.destination.ShouldSend() )
            {
                var recievedFrom = mag.conn.connectionId;
                for( Int32 i = 0; i < NetworkServer.connections.Count; ++i )
                {
                    if( i == recievedFrom ) continue;
                    var conn = NetworkServer.connections[i];
                    if( conn == null ) continue;
                    using( var netWriter = GetWriter( commandIndex, conn, QosType.Reliable ) )
                    {
                        NetworkWriter writer = netWriter;
                        writer.Write( header );
                    }
                }
                
            }
        }

        private static void HandleMessageServer( NetworkMessage msg )
        {
            var reader = msg.reader;
            var header = reader.ReadNew<Header>();

            if( header.destination.ShouldRun() )
            {
                header.RemoveDestination( NetworkDestination.Server );

                if( netMessages.TryGetValue( header.typeCode, out var message ) )
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
                var recievedFrom = msg.conn.connectionId;
                var bytes = reader.ReadBytes( (Int32)(reader.Length - reader.Position) );
                for( Int32 i = 0; i < NetworkServer.connections.Count; ++i )
                {
                    if( i == recievedFrom ) continue;
                    var conn = NetworkServer.connections[i];
                    if( conn == null ) continue;

                    using( var netWriter = GetWriter( messageIndex, conn, QosType.Reliable ) )
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
            var reader = msg.reader;
            var header = reader.ReadNew<Header>();

            if( header.destination.ShouldRun() )
            {
                header.RemoveDestination( NetworkDestination.Clients );

                if( netCommands.TryGetValue( header.typeCode, out var command ) )
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
            var reader = msg.reader;
            var header = reader.ReadNew<Header>();

            if( header.destination.ShouldRun() )
            {
                header.RemoveDestination( NetworkDestination.Clients );

                if( netMessages.TryGetValue( header.typeCode, out var message ) )
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
