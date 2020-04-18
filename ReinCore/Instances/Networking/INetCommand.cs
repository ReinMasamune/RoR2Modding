using System;
using UnityEngine.Networking;

namespace ReinCore
{
    public interface INetCommand
    {
        void OnRecieved();
    }

    public static class INetCommandExtensions
    {
        public static void Send( this INetCommand command, NetworkDestination destination, Int32 recievedFrom = -1 )
        {
            if( destination.ShouldRun() )
            {
                command.OnRecieved();
            }

            if( destination.ShouldSend() )
            {
                var header = destination.GetHeader( NetworkCore.GetNetworkCoreHash(command.GetType()));

                if( NetworkServer.active )
                {
                    for( Int32 i = 0; i < NetworkServer.connections.Count; ++i )
                    {
                        if( i == recievedFrom ) continue;
                        var conn = NetworkServer.connections[i];
                        if( conn == null ) continue;

                        using( Writer netWriter = NetworkCore.GetWriter( NetworkCore.commandIndex, conn, QosType.Reliable ) )
                        {
                            NetworkWriter writer = netWriter;
                            writer.Write( header );
                        }
                    }
                }

                if( NetworkClient.active )
                {
                    using( Writer netWriter = NetworkCore.GetWriter( NetworkCore.commandIndex, ClientScene.readyConnection, QosType.Reliable ) )
                    {
                        NetworkWriter writer = netWriter;
                        writer.Write( header );
                    }
                }
            }
        }
    }
}
