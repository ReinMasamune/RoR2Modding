namespace ReinCore
{
    using System;
    using RoR2;
    using UnityEngine.Networking;

    internal struct BuffMessage : INetMessage
    {
        public void Serialize( NetworkWriter writer )
        {
            writer.Write( this.body.gameObject );
            writer.WriteBuffIndex( this.buff );
            writer.Write( this.stacks );
            writer.Write( this.duration );
        }

        public void Deserialize( NetworkReader reader )
        {
            this.body = reader.ReadGameObject().GetComponent<CharacterBody>();
            this.buff = reader.ReadBuffIndex();
            this.stacks = reader.ReadInt32();
            this.duration = reader.ReadSingle();
        }

        public void OnRecieved() => this.body.ApplyBuff( this.buff, this.stacks, this.duration );

        internal BuffMessage( CharacterBody body, BuffIndex buff, Int32 stacks, Single duration  )
        {
            this.body = body;
            this.buff = buff;
            this.stacks = stacks;
            this.duration = duration;
        }

        private CharacterBody body;
        private BuffIndex buff;
        private Int32 stacks;
        private Single duration;
    }
}
