namespace ReinCore
{
    using System;

    using UnityEngine.Networking;

    internal class Writer : IDisposable
    {
        internal Writer( NetworkWriter writer, Int16 messageIndex, NetworkConnection target, QosType qos )
        {
            this.netWriter = writer;
            this.connection = target;
            this.qos = qos;
            writer.StartMessage( messageIndex );
        }

        private readonly NetworkWriter netWriter;
        private readonly NetworkConnection connection;
        private readonly QosType qos;

        public static implicit operator NetworkWriter( Writer writer ) => writer.netWriter;

        public void Dispose()
        {
            this.netWriter.FinishMessage();
            _ = this.connection.SendWriter( this.netWriter, (Int32)this.qos );
        }
    }
}
