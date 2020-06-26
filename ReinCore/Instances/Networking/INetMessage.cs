namespace ReinCore
{
    using System;

    using UnityEngine.Networking;

    public interface INetMessage : ISerializableObject
    {
        void OnRecieved();
    }

    public static class INetMessageExtensions
    {
        public static void Send( this INetMessage message, NetworkDestination destination, Int32 recievedFrom = -1 )
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
