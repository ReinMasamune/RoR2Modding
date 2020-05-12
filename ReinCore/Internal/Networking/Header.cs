namespace ReinCore
{
    using System;

    using UnityEngine.Networking;

    internal class Header : ISerializableObject
    {
        public Header() { }
        internal Header( Int32 typeCode, NetworkDestination dest )
        {
            this.typeCode = typeCode;
            this.destination = dest;
        }

        internal Int32 typeCode { get; private set; }
        internal NetworkDestination destination { get; private set; }

        internal void RemoveDestination( NetworkDestination destination ) => this.destination &= ~destination;


        public void Serialize( NetworkWriter writer )
        {
            writer.Write( this.typeCode );
            writer.Write( (Byte)this.destination );
        }

        public void Deserialize( NetworkReader reader )
        {
            this.typeCode = reader.ReadInt32();
            this.destination = (NetworkDestination)reader.ReadByte();
        }
    }
}
