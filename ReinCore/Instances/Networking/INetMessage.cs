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
        [Obsolete("Use generic overload instead", true)]
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

                        if(NetworkServer.localClientActive && NetworkServer.localConnections.Contains(conn))
                        {
                            continue;
                        }

                        using( Writer netWriter = NetworkCore.GetWriter( NetworkCore.messageIndex, conn, NetworkCore.qos) )
                        {
                            NetworkWriter writer = netWriter;
                            writer.Write( header );
                            writer.Write( message );
                        }
                    }
                } else if( NetworkClient.active )
                {
                    using( Writer netWriter = NetworkCore.GetWriter( NetworkCore.messageIndex, ClientScene.readyConnection, NetworkCore.qos) )
                    {
                        NetworkWriter writer = netWriter;
                        writer.Write( header );
                        writer.Write( message );
                    }
                }
            }
        }

        public static void Send<T>(this T message, NetworkDestination destination, Int32 recievedFrom = -1)
            where T : INetMessage
        {
            if(destination.ShouldRun())
            {
                message.OnRecieved();
            }

            if(destination.ShouldSend())
            {
                Header header = destination.GetHeader( NetworkCore.GetNetworkCoreHash(typeof(T)));

                if(NetworkServer.active)
                {
                    for(Int32 i = 0; i < NetworkServer.connections.Count; ++i)
                    {
                        if(i == recievedFrom)
                        {
                            continue;
                        }

                        NetworkConnection conn = NetworkServer.connections[i];
                        if(conn == null)
                        {
                            continue;
                        }

                        if(NetworkServer.localClientActive && NetworkServer.localConnections.Contains(conn))
                        {
                            continue;
                        }

                        using(Writer netWriter = NetworkCore.GetWriter(NetworkCore.messageIndex, conn, NetworkCore.qos))
                        {
                            NetworkWriter writer = netWriter;
                            writer.Write(header);
                            writer.Write(message);
                        }
                    }
                } else if(NetworkClient.active)
                {
                    using(Writer netWriter = NetworkCore.GetWriter(NetworkCore.messageIndex, ClientScene.readyConnection, NetworkCore.qos))
                    {
                        NetworkWriter writer = netWriter;
                        writer.Write(header);
                        writer.Write(message);
                    }
                }
            }
        }
    }
}
