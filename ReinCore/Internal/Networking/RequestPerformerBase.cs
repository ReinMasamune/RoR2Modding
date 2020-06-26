namespace ReinCore
{
    using System;

    using UnityEngine.Networking;

    internal abstract class RequestPerformerBase
    {
        internal abstract ISerializableObject PerformRequest( NetworkReader reader );
        internal abstract void PerformReply( NetworkReader reader );
    }
}
