namespace Rein.Sniper.Modules
{
    using ReinCore;
    using RoR2;
    using Rein.Sniper.Components;
    using System;
    using UnityEngine.Networking;

    internal static class NetworkModule
    {
        internal static void SetupNetworking()
        {
            if( !NetworkCore.RegisterMessageType<ResetSkillsMessage>() )
            {
#if ASSERT
                Log.Error( "Failed to register network message for skill resets" );
#endif
            }
        }


        internal struct ResetSkillsMessage : INetMessage
        {
            internal ResetSkillsMessage( SniperCharacterBody body )
            {
                this.body = body;
            }

            void INetMessage.OnRecieved() => CatalogModule.ResetSkills( this.body, false );
            void ISerializableObject.Serialize( NetworkWriter writer ) => writer.Write( this.body.networkIdentity );
            void ISerializableObject.Deserialize( NetworkReader reader ) => this.body = reader.ReadNetworkIdentity()?.GetComponent<SniperCharacterBody>();

            private SniperCharacterBody body;
        }

        internal struct PlasmaApplyMessage : INetMessage
        {
            internal PlasmaApplyMessage(CharacterBody? target, CharacterBody? attacker, Single damageMultiplier, Single duration, Single procCoef, Boolean crit, HurtBox hit)
            {
                this.target = target;
                this.attacker = attacker;
                this.damageMultiplier = damageMultiplier;
                this.duration = duration;
                this.procCoef = procCoef;
                this.crit = crit;
                this.hurtbox = hit;
            }

            private CharacterBody? target;
            private CharacterBody? attacker;
            private HurtBox? hurtbox;
            private Single damageMultiplier;
            private Single duration;
            private Single procCoef;
            private Boolean crit;


            void INetMessage.OnRecieved()
            {
                if(NetworkServer.active)
                {
                    DotTypes.PlasmaDot.Apply(this.target, this.attacker, this.damageMultiplier, this.duration, this.procCoef, this.crit, this.hurtbox);
                }
            }

            void ISerializableObject.Deserialize(NetworkReader reader)
            {
                this.target = reader.ReadNetworkIdentity()?.GetComponent<CharacterBody>();
                this.attacker = reader.ReadNetworkIdentity()?.GetComponent<CharacterBody>();
                this.hurtbox = reader.ReadHurtBoxReference().ResolveHurtBox();
                this.damageMultiplier = reader.ReadSingle();
                this.duration = reader.ReadSingle();
                this.procCoef = reader.ReadSingle();
            }

            void ISerializableObject.Serialize(NetworkWriter writer)
            {
                writer.Write(this.target?.networkIdentity);
                writer.Write(this.attacker?.networkIdentity);
                writer.Write(HurtBoxReference.FromHurtBox(this.hurtbox));
                writer.Write(this.damageMultiplier);
                writer.Write(this.duration);
                writer.Write(this.procCoef);
            }
        }
    }
}