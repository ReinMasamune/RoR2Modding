﻿namespace ReinCore
{
    using System;

    using UnityEngine.Networking;

    public interface INetCommand
    {
        void OnRecieved();
    }

    public static class INetCommandExtensions
    {
        [Obsolete("Use generic overload instead", true)]
        public static void Send( this INetCommand command, NetworkDestination destination, Int32 recievedFrom = -1 )
        {
            if( destination.ShouldRun() )
            {
                command.OnRecieved();
            }

            if( destination.ShouldSend() )
            {
                Header header = destination.GetHeader( NetworkCore.GetNetworkCoreHash(command.GetType()));

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

                        using( Writer netWriter = NetworkCore.GetWriter( NetworkCore.commandIndex, conn, NetworkCore.qos) )
                        {
                            NetworkWriter writer = netWriter;
                            writer.Write( header );
                        }
                    }
                } else if( NetworkClient.active )
                {
                    using( Writer netWriter = NetworkCore.GetWriter( NetworkCore.commandIndex, ClientScene.readyConnection, NetworkCore.qos) )
                    {
                        NetworkWriter writer = netWriter;
                        writer.Write( header );
                    }
                }
            }
        }

        public static void Send<T>(this T command, NetworkDestination destination, Int32 recievedFrom = -1)
            where T : INetCommand
        {
            if(destination.ShouldRun())
            {
                command.OnRecieved();
            }

            if(destination.ShouldSend())
            {
                Header header = destination.GetHeader( NetworkCore.GetNetworkCoreHash(command.GetType()));

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

                        using(Writer netWriter = NetworkCore.GetWriter(NetworkCore.commandIndex, conn, NetworkCore.qos))
                        {
                            NetworkWriter writer = netWriter;
                            writer.Write(header);
                        }
                    }
                } else if(NetworkClient.active)
                {
                    using(Writer netWriter = NetworkCore.GetWriter(NetworkCore.commandIndex, ClientScene.readyConnection, NetworkCore.qos))
                    {
                        NetworkWriter writer = netWriter;
                        writer.Write(header);
                    }
                }
            }
        }
    }
}
