using System;
using UnityEngine.Networking;

namespace ReinCore
{
    internal class Writer : IDisposable
    {
        internal Writer(NetworkWriter writer, Int16 messageIndex, NetworkConnection target, QosType qos )
        {
            this.netWriter = writer;
            this.connection = target;
            this.qos = qos;
            writer.StartMessage( messageIndex );
        }

        private NetworkWriter netWriter;
        private NetworkConnection connection;
        private QosType qos;

        public static implicit operator NetworkWriter( Writer writer )
        {
            return writer.netWriter;
        }

        public void Dispose()
        {
            this.netWriter.FinishMessage();
            this.connection.SendWriter( this.netWriter, (Int32)this.qos );
        }
    }
}
