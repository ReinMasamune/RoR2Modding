namespace ReinCore
{
    using System;
    using RoR2;
    using UnityEngine.Networking;

    internal struct DamageMessage : INetMessage
    {
        public void Serialize( NetworkWriter writer )
        {
            writer.Write( this.damage );
            writer.Write( HurtBoxReference.FromHurtBox( this.target ) );
            Byte flags = (Byte)0u;
            flags |= this.callHitWorld ? (Byte)1u : (Byte)0u;
            flags <<= 1;
            flags |= this.callHitEnemy ? (Byte)1u : (Byte)0u;
            flags <<= 1;
            flags |= this.callDamage ? (Byte)1u : (Byte)0u;
            writer.Write( flags );
        }

        public void Deserialize( NetworkReader reader )
        {
            this.damage = reader.ReadDamageInfo();
            this.target = reader.ReadHurtBoxReference().ResolveHurtBox();
            Byte flags = reader.ReadByte();
            Byte mask = (Byte)0b0000_0001u;
            this.callDamage = ( flags & mask ) > 0;
            flags >>= 1;
            this.callHitEnemy = ( flags & mask ) > 0;
            flags >>= 1;
            this.callHitWorld = ( flags & mask ) > 0;
        }

        public void OnRecieved() => NetworkingHelpers.DealDamage( this.damage, this.target, this.callDamage, this.callHitEnemy, this.callHitWorld );

        internal DamageMessage( DamageInfo damage, HurtBox target, Boolean callDamage, Boolean callHitEnemy, Boolean callHitWorld )
        {
            this.damage = damage;
            this.target = target;
            this.callDamage = callDamage;
            this.callHitEnemy = callHitEnemy;
            this.callHitWorld = callHitWorld;
        }

        private DamageInfo damage;
        private HurtBox target;
        private Boolean callDamage;
        private Boolean callHitEnemy;
        private Boolean callHitWorld;
    }
}
