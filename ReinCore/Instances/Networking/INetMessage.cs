namespace ReinCore
{
    using System;
    using UnityEngine.Networking;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface INetMessage : ISerializableObject
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        void OnRecieved();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class INetMessageExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void Send( this INetMessage message, NetworkDestination destination, Int32 recievedFrom = -1 )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( destination.ShouldRun() )
            {
                message.OnRecieved();
            }

            if( destination.ShouldSend() )
            {
                Header header = destination.GetHeader( NetworkCore.GetNetworkCoreHash(message.GetType()));

                if( NetworkServer.active )
                {
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

                        using( Writer netWriter = NetworkCore.GetWriter( NetworkCore.messageIndex, conn, QosType.Reliable ) )
                        {
                            NetworkWriter writer = netWriter;
                            writer.Write( header );
                            writer.Write( message );
                        }
                    }
                }

                if( NetworkClient.active )
                {
                    using( Writer netWriter = NetworkCore.GetWriter( NetworkCore.messageIndex, ClientScene.readyConnection, QosType.Reliable ) )
                    {
                        NetworkWriter writer = netWriter;
                        writer.Write( header );
                        writer.Write( message );
                    }
                }
            }
        }
    }
}
