namespace ReinCore
{
    using System;

    using UnityEngine.Networking;

    public interface INetRequest<TRequest,TReply> : ISerializableObject
        where TRequest : INetRequest<TRequest,TReply>
        where TReply : INetRequestReply<TRequest,TReply>
    {
        TReply OnRequestRecieved();
    }

    public interface INetRequestReply<TRequest,TReply> : ISerializableObject
        where TRequest: INetRequest<TRequest,TReply>
        where TReply: INetRequestReply<TRequest,TReply>
    {
        void OnReplyRecieved();
    }

    public static class INetRequestExtensions
    {
        public static void Send<TRequest,TReply>( this TRequest request, NetworkDestination destination, Int32 recievedFrom = -1 )
            where TRequest : INetRequest<TRequest,TReply>
            where TReply: INetRequestReply<TRequest,TReply>
        {
            if( destination.ShouldRun() )
            {
                request.OnRequestRecieved().OnReplyRecieved();
            }

            if( destination.ShouldSend() )
            {
                Header header = destination.GetHeader( NetworkCore.GetNetworkCoreHash(request.GetType()));

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

                        using( Writer netWriter = NetworkCore.GetWriter( NetworkCore.messageIndex, conn, NetworkCore.qos) )
                        {
                            NetworkWriter writer = netWriter;
                            writer.Write( header );
                            writer.Write( request );
                        }
                    }
                }

                if( NetworkClient.active )
                {
                    using( Writer netWriter = NetworkCore.GetWriter( NetworkCore.messageIndex, ClientScene.readyConnection, NetworkCore.qos) )
                    {
                        NetworkWriter writer = netWriter;
                        writer.Write( header );
                        writer.Write( request );
                    }
                }
            }
        }
    }
}
