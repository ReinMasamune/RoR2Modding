namespace ReinCore
{
    using System;
    using UnityEngine.Networking;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface INetCommand
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        void OnRecieved();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class INetCommandExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void Send( this INetCommand command, NetworkDestination destination, Int32 recievedFrom = -1 )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
