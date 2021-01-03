namespace Rein.Sniper.Orbs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    using JetBrains.Annotations;

    using Rein.Instances.HitDetection;
    using Rein.Sniper.Ammo;
    using Rein.Sniper.Modules;

    using ReinCore;

    using RoR2;
    using RoR2.Orbs;

    using UnityEngine;
    using UnityEngine.Networking;

    internal sealed class SporeOrb : Orb, ISerializableObject
    {
        const Single tickInterval = 0.35f;
        const Single minRadius = 3f;
        const Single maxRadius = 10f;
        const Single baseDuration = 10f;
        internal const Single expansionSpeed = 1.5f;

        internal static EffectIndex orbEffect = EffectIndex.Invalid;



        internal Boolean crit;
        internal Single power;
        internal Single boost;
        internal TeamIndex attackerTeam => this.attacker.teamComponent.teamIndex;
        internal CharacterBody attacker;






        private Single damageMult;
        // TODO: Pool these--
        private HashSet<HealthComponent> currentlyInside = new();
        private Dictionary<HealthComponent, Single> timers = new(); 
        

        public void Deserialize(NetworkReader reader)
        {
            this.origin = reader.ReadVector3();
            this.crit = reader.ReadBoolean();
            this.duration = reader.ReadSingle();
            this.boost = reader.ReadSingle();
            this.power = reader.ReadSingle();
            var netId = reader.ReadNetworkIdentity();
            if(netId)
            {
                this.attacker = netId.GetComponent<CharacterBody>();
            }
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.origin);
            writer.Write(this.crit);
            writer.Write(this.duration);
            writer.Write(this.boost);
            writer.Write(this.power);
            writer.Write(this.attacker.networkIdentity);
        }

        public unsafe override void Begin()
        {
            Single* ptr = stackalloc Single[1];
            var trigger = new SphereTriggerCallbackProvider<Callbacks>();
            var data = new EffectData();

            this.damageMult = this.boost * this.power;
            trigger.cb.orb = this;
            data.genericFloat = trigger.cb.maxDuration = this.duration *= this.boost;
            data.origin = trigger.cb.origin = this.origin;
            data.scale = trigger.cb.minRadius = minRadius;
            ptr[0] = trigger.cb.maxRadius = maxRadius * this.boost;
            data.genericUInt = *((UInt32*)ptr);
            trigger.Start();

            if(orbEffect == EffectIndex.Invalid)
            {
                orbEffect = EffectCatalog.FindEffectIndexFromPrefab(VFXModule.GetSporeOrbPrefab());
            }
            EffectManager.SpawnEffect(orbEffect, data, true);
            base.Begin();
        }

        private struct Callbacks : ITriggerCallbacks<Callbacks>
        {
            internal Single maxDuration;
            internal Single minRadius;
            internal Single maxRadius;
            internal Vector3 origin;
            internal SporeOrb orb;

            private Single elapsedTime;

            public QueryTriggerInteraction queryTriggerInteraction => QueryTriggerInteraction.UseGlobal;
            public Int32 layerMask => LayerIndex.entityPrecise.mask.value;
            public Int32 maxHits => 1000;
            public Single interval => 1f / (1f + Mathf.Abs(ConfigModule.sporeScanResolution));

            public Boolean shouldEnd => this.elapsedTime >= this.maxDuration;
            public Vector3 position => this.origin;
            public Single radius => Mathf.Lerp(this.minRadius, this.maxRadius, this.elapsedTime * SporeOrb.expansionSpeed / this.maxDuration);

            public void PassTime(Single delta)
            {
                static Boolean NotNull(HealthComponent hc) => hc && hc.alive;

                this.elapsedTime += delta;
                foreach(var v in this.orb.currentlyInside.Where(NotNull))
                {
                    if(!this.orb.timers.TryGetValue(v, out var t))
                    {
                        t = 0f;
                    }
                    t += delta;

                    while(t >= tickInterval)
                    {
                        t -= tickInterval;

                        if(v.body.teamComponent.teamIndex == this.orb.attackerTeam)
                        {
                            if(this.orb.crit)
                            {
                                var ogch = v.body.critHeal;
                                v.body.critHeal = 100f;
                                v.body.healthComponent.Heal(tickInterval * v.body.healthComponent.fullCombinedHealth * SporeContext.healPercentBase * this.orb.damageMult, default);
                                v.body.critHeal = ogch;
                            } else
                            {
                                v.body.healthComponent.Heal(tickInterval * v.body.healthComponent.fullCombinedHealth * SporeContext.healPercentBase * this.orb.damageMult, default);
                            }
                        } else
                        {
                            //Tick damage
                            var d = new DamageInfo()
                            {
                                attacker = this.orb.attacker.gameObject,
                                crit = this.orb.crit,
                                damage = this.orb.attacker.damage * this.orb.damageMult * tickInterval * SporeContext.zoneDamageMult,
                                damageColorIndex = DamageColorIndex.Default,
                                damageType = DamageType.SlowOnHit,
                                dotIndex = DotController.DotIndex.None,
                                force = Vector3.zero,
                                inflictor = null,
                                position = v.transform.position,
                                procChainMask = default,
                                procCoefficient = SporeContext.zoneProcCoef,
                                rejected = false,
                            };
                            NetworkingHelpers.DealDamage(d, v.body.mainHurtBox, true, true, true);
                        }
                    }

                    this.orb.timers[v] = t;
                }
            }


            public void PreSphereCheck()
            {
                this.orb.currentlyInside.Clear();
            }
            public void OnColliderInSphere(Collider col)
            {
                if(!col) return;
                if(col.GetComponent<HurtBox>() is not HurtBox hb || !hb) return;
                if(hb.healthComponent is not HealthComponent hc || !hc) return;
                if(hc.body.teamComponent.teamIndex != this.orb.attackerTeam && !FriendlyFireManager.ShouldSplashHitProceed(hc, this.orb.attackerTeam)) return;
                this.orb.currentlyInside.Add(hc);
            }
        }

    }

}
