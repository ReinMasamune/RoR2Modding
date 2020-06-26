namespace ReinCore
{
    using System;

    using UnityEngine.Networking;

    internal sealed class RequestPerformer<TRequest,TReply> : RequestPerformerBase
        where TRequest : INetRequest<TRequest, TReply>
        where TReply : INetRequestReply<TRequest, TReply>
    {
        internal RequestPerformer( TRequest request, TReply reply )
        {
            this.request = request;
            this.reply = reply;
        }

        private readonly TRequest request;
        private readonly TReply reply;

        internal override ISerializableObject PerformRequest(NetworkReader reader) => throw new NotImplementedException();
        internal override void PerformReply( NetworkReader reader ) => throw new NotImplementedException();
    }
}
