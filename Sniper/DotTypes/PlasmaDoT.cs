namespace Rein.Sniper.DotTypes
{
    using RoR2;

    using System;

    using ReinCore;

    using UnityEngine.Networking;
    using Rein.Sniper.Modules;
    using UnityEngine;
    using Mono.Security.X509.Extensions;

    internal struct PlasmaDot : IDot<PlasmaDot, PlasmaDot.PlasmaStack, PlasmaDot.PlasmaUpdate, PlasmaDot.PlasmaPersist>
    {
        private const Single tickFreq = SkillsModule.plasmaTickFreq;
        private const Single tickInterval = 1f / tickFreq;


        public Boolean sendToClients => true;

        internal static void Apply(CharacterBody target, CharacterBody attacker, Single damageMultiplier, Single duration, Single procCoef, Boolean crit, HurtBox hit)
        {
            if(NetworkServer.active)
            {
                ApplyServer(target, attacker, damageMultiplier, duration, procCoef, crit, hit);
            } else
            {
                new NetworkModule.PlasmaApplyMessage(target, attacker, damageMultiplier, duration, procCoef, crit, hit).Send(NetworkDestination.Server);
            }
        }

        private static void ApplyServer(CharacterBody target, CharacterBody attacker, Single damageMultiplier, Single duration, Single procCoef, Boolean crit, HurtBox hit)
        {
            DotController<PlasmaDot, PlasmaStack, PlasmaUpdate, PlasmaPersist>.ServerInflictDot(target, new(damageMultiplier, duration, procCoef, crit, attacker, hit));
        }

        private struct PlasmaStack : IDotStackData<PlasmaDot, PlasmaDot.PlasmaStack, PlasmaDot.PlasmaUpdate, PlasmaDot.PlasmaPersist>
        {
            public Boolean shouldRemove => this.remainingTicks <= 0;

            internal PlasmaStack(Single damageMultiplier, Single duration, Single procCoef, Boolean crit, CharacterBody attacker, HurtBox hurtbox)
            {
                this.blob = new Blob
                {
                    damageMultiplier = damageMultiplier,
                    procCoef = procCoef,
                    crit = crit,
                    remainingTicks = Mathf.RoundToInt(duration * tickFreq),
                    timer = 0f,
                };
                this.hurtbox = hurtbox;
                this.attacker = attacker;
            }
            private struct Blob
            {
                internal Single damageMultiplier;
                internal Single procCoef;
                internal Boolean crit;
                internal Int32 remainingTicks;
                internal Single timer;
            }

            private Single damageMultiplier => this.blob.damageMultiplier;
            private Boolean crit => this.blob.crit;
            private Single procCoef => this.blob.procCoef;
            private Int32 remainingTicks { get => this.blob.remainingTicks; set => this.blob.remainingTicks = value; }
            private Single timer { get => this.blob.timer; set => this.blob.timer = value; }

            private Blob blob;
            private HurtBox? hurtbox;
            private CharacterBody attacker;

            
            

            

            public void OnApplied(ref PlasmaPersist ctx)
            {
                ctx.targetBody.value?.AddBuff(CatalogModule.plasmaBurnDebuff);
            }
            public void OnExpired(ref PlasmaPersist ctx)
            {
                ctx.targetBody.value?.RemoveBuff(CatalogModule.plasmaBurnDebuff);
            }

            public void OnCleanseRecieved()
            {
                this.remainingTicks = 0;
            }

            public void Process(Single deltaTime, ref PlasmaUpdate updateContext)
            {
                this.timer += deltaTime;

                while(this.timer >= tickInterval && this.remainingTicks > 0)
                {
                    this.timer -= tickInterval;
                    this.remainingTicks--;

                    CharacterBody? targetBody = updateContext.persist.targetBody;
                    if(targetBody is null) continue;
                    var targetHc = targetBody.healthComponent.Safe();
                    if(targetHc is null || !targetHc.alive) continue;
                    var damage = new DamageInfo
                    {
                        attacker = this.attacker.gameObject,
                        crit = this.crit,
                        damage = this.attacker.damage * this.damageMultiplier,
                        damageColorIndex = CatalogModule.plasmaDamageColor,
                        damageType = DamageType.Generic,
                        dotIndex = DotController.DotIndex.None,
                        force = Vector3.zero,
                        inflictor = null,
                        position = this.hurtbox?.transform?.position ?? Util.GetCorePosition(targetBody),
                        procChainMask = default,
                        procCoefficient = this.procCoef,
                        rejected = false
                    };

                    targetHc.TakeDamage(damage);
                    GlobalEventManager.instance.OnHitEnemy(damage, targetBody?.gameObject);
                    GlobalEventManager.instance.OnHitAll(damage, targetBody?.gameObject);
                }
                updateContext.stackCounter++;
            }

            public void Deserialize(NetworkReader reader)
            {
                reader.ReadBits(ref this.blob);
                this.hurtbox = reader.ReadHurtBoxReference().ResolveHurtBox();
                this.attacker = reader.ReadNetworkIdentity()?.GetComponent<CharacterBody>()!;
            }
            public void Serialize(NetworkWriter writer)
            {
                writer.WriteBits(this.blob);
                writer.Write(HurtBoxReference.FromHurtBox(this.hurtbox));
                writer.Write(this.attacker.networkIdentity);
            }
        }
        private struct PlasmaUpdate : IDotUpdateContext<PlasmaDot, PlasmaDot.PlasmaStack, PlasmaDot.PlasmaUpdate, PlasmaDot.PlasmaPersist>
        {
            internal PlasmaPersist persist { get; }
            internal Int32 stackCounter { get; set; }
            internal PlasmaUpdate(PlasmaPersist persist)
            {
                this.persist = persist;
                this.stackCounter = 0;
            }
        }
        private struct PlasmaPersist : IDotPersistContext<PlasmaDot, PlasmaDot.PlasmaStack, PlasmaDot.PlasmaUpdate, PlasmaDot.PlasmaPersist>
        {
            public UnityRef<CharacterBody> targetBody { get; set; }

            public Boolean shouldRemove { get; private set; }

            public void AllExpired()
            {
                
            }

            public void HandleUpdateContext(in PlasmaUpdate context)
            {
                this.shouldRemove = context.stackCounter <= 0;
            }

            public PlasmaUpdate InitUpdateContext()
            {
                return new(this);
            }

            public void OnFirstStackApplied()
            {
                //Apply visual overlay
            }

            public void OnLastStackRemoved()
            {
                //Remove visual overlay
            }
        }
    }
}