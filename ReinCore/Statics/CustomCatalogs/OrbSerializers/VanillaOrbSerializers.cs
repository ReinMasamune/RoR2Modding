namespace ReinCore
{ 
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;

    using RoR2;
    using RoR2.Orbs;

    using UnityEngine.Networking;

    internal static class VanillaOrbSerializers
    {
        public static void AddAll()
        {
            OrbSerializerCatalog.Add(new LightningOrbSerializer());
        }

        internal abstract class VanillaOrbSerializer<TOrb> : IOrbSerializer
            where TOrb : Orb
        {
            Type IOrbSerializer.targetType => typeof(TOrb);
            String OrbSerializerCatalog.ICatalogDef.guid => typeof(TOrb).FullName;
            OrbSerializerCatalog.Entry? OrbSerializerCatalog.ICatalogDef.entry { get; set; }

            Orb IOrbSerializer.CreateInstance() => this.CreateInstance();


            void ISerializer<Orb>.Deserialize(NetworkReader reader, Orb target)
            {
                if(target is not TOrb orb) throw new InvalidOperationException("Tried to deserialize improper orb type");
                this.Deserialize(reader, ref orb);
            }

            void ISerializer<Orb>.Serialize(NetworkWriter writer, Orb target)
            {
                if(target is not TOrb orb) throw new InvalidOperationException("Tried to serialize improper orb type");
                this.Serialize(writer, orb);
            }

            protected abstract TOrb CreateInstance();
            protected virtual void Deserialize(NetworkReader reader, ref TOrb orb)
            {
                orb.origin = reader.ReadVector3();
                orb.target = reader.ReadHurtBoxReference().ResolveHurtBox();
                orb.arrivalTime = reader.ReadSingle();
                orb.duration = reader.ReadSingle();
            }
            protected virtual void Serialize(NetworkWriter writer, TOrb orb)
            {
                writer.Write(orb.origin);
                writer.Write(HurtBoxReference.FromHurtBox(orb.target));
                writer.Write(orb.arrivalTime);
                writer.Write(orb.duration);
            }
        }


        internal class LightningOrbSerializer : VanillaOrbSerializer<LightningOrb>
        {
            protected override LightningOrb CreateInstance() => new();

            protected override void Deserialize(NetworkReader reader, ref LightningOrb orb)
            {
                base.Deserialize(reader, ref orb);
                orb.speed = reader.ReadSingle();
                orb.damageValue = reader.ReadSingle();
                orb.attacker = reader.ReadGameObject();
                orb.inflictor = reader.ReadGameObject();
                orb.bouncesRemaining = reader.ReadInt32();

                //Needs null checks
                var c = reader.ReadInt32();
                orb.bouncedObjects = new(c);
                for(Int32 i = 0; i < c; i++)
                {
                    orb.bouncedObjects.Add(reader.ReadNetworkIdentity().GetComponent<CharacterBody>().healthComponent);
                }

                orb.teamIndex = reader.ReadTeamIndex();
                orb.isCrit = reader.ReadBoolean();
                orb.procChainMask = reader.ReadProcChainMask();
                orb.procCoefficient = reader.ReadSingle();
                orb.damageColorIndex = reader.ReadDamageColorIndex();
                orb.range = reader.ReadSingle();
                orb.damageCoefficientPerBounce = reader.ReadSingle();
                orb.targetsToFindPerBounce = reader.ReadInt32();
                orb.damageType = reader.ReadDamageType();
                orb.lightningType = reader.ReadBits<LightningOrb.LightningType>();
            }

            protected override void Serialize(NetworkWriter writer, LightningOrb orb)
            {
                base.Serialize(writer, orb);
                writer.Write(orb.speed);
                writer.Write(orb.damageValue);
                writer.Write(orb.attacker);
                writer.Write(orb.inflictor);
                writer.Write(orb.bouncesRemaining);

                writer.Write(orb.bouncedObjects.Count);
                for(Int32 i = 0; i < orb.bouncedObjects.Count; ++i)
                {
                    writer.Write(orb.bouncedObjects[i].body);
                }

                writer.Write(orb.teamIndex);
                writer.Write(orb.isCrit);
                writer.Write(orb.procChainMask);
                writer.Write(orb.procCoefficient);
                writer.Write(orb.damageColorIndex);
                writer.Write(orb.range);
                writer.Write(orb.damageCoefficientPerBounce);
                writer.Write(orb.targetsToFindPerBounce);
                writer.Write(orb.damageType);
                writer.WriteBits(orb.lightningType);
            }
        }
    }
}
