namespace ReinCore
{
    using System;

    using RoR2;

    using UnityEngine;
    using UnityEngine.Networking;


    internal struct DoTMessage : INetMessage
    {
        public void Serialize( NetworkWriter writer )
        {
            writer.Write( this.victim );
            writer.Write( this.attacker );
            writer.WritePackedIndex32( (Int32)this.dotIndex );
            writer.Write( this.duration );
            writer.Write( this.damageMultiplier );
        }

        public void Deserialize( NetworkReader reader )
        {
            this.victim = reader.ReadGameObject();
            this.attacker = reader.ReadGameObject();
            this.dotIndex = (DotController.DotIndex)reader.ReadPackedIndex32();
            this.duration = reader.ReadSingle();
            this.damageMultiplier = reader.ReadSingle();
        }

        public void OnRecieved() => DotController.InflictDot( this.victim, this.attacker, this.dotIndex, this.duration, this.damageMultiplier );

        internal DoTMessage( GameObject victimObject, GameObject attackerObject, DotController.DotIndex dotIndex, Single duration, Single damageMultiplier )
        {
            this.victim = victimObject;
            this.attacker = attackerObject;
            this.dotIndex = dotIndex;
            this.duration = duration;
            this.damageMultiplier = damageMultiplier;
        }

        private GameObject victim;
        private GameObject attacker;
        private DotController.DotIndex dotIndex;
        private Single duration;
        private Single damageMultiplier;
    }
}
