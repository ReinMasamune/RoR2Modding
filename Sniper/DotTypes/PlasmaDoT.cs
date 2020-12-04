namespace Rein.Sniper.DotTypes
{
    using RoR2;

    using System;

    using ReinCore;

    using UnityEngine.Networking;
    using Rein.Sniper.Modules;
    using UnityEngine;
    using Mono.Security.X509.Extensions;
    using Rein.Sniper.Ammo;

    internal struct PlasmaDot : IDot<PlasmaDot, PlasmaDot.PlasmaStack, PlasmaDot.PlasmaUpdate, PlasmaDot.PlasmaPersist>
    {

        private const Single tickFreq = PlasmaContext.plasmaTickFreq;
        private const Single tickInterval = 1f / tickFreq;



        public Boolean processOnClients => true;

        internal static void Apply(CharacterBody target, CharacterBody attacker, Single damageMultiplier, Single duration, Single procCoef, Boolean crit, Vector3 localPos, Vector3 normal, HurtBox hit)
        {
            DotController<PlasmaDot, PlasmaStack, PlasmaUpdate, PlasmaPersist>.InflictDot(target, new(damageMultiplier, duration, procCoef, crit, localPos, normal, attacker, hit));
        }

        internal static void Register()
        {
            DotController<PlasmaDot, PlasmaStack, PlasmaUpdate, PlasmaPersist>.Register();
        }

        internal struct PlasmaStack : IDotStackData<PlasmaDot, PlasmaDot.PlasmaStack, PlasmaDot.PlasmaUpdate, PlasmaDot.PlasmaPersist>
        {
            public Boolean shouldRemove => this.remainingTicks <= 0;

            internal PlasmaStack(Single damageMultiplier, Single duration, Single procCoef, Boolean crit, Vector3 localPos, Vector3 normal, CharacterBody attacker, HurtBox hurtbox)
            {
                this.blob = new Blob
                {
                    damageMultiplier = damageMultiplier,
                    procCoef = procCoef,
                    crit = crit,
                    remainingTicks = Mathf.RoundToInt(duration * tickFreq),
                    timer = 0f,
                    localPos = localPos,
                    normal = normal,
                };
                this.hurtbox = hurtbox;
                this.attacker = attacker;
                this.effectInstance = null;
            }
            private struct Blob
            {
                internal Single damageMultiplier;
                internal Single procCoef;
                internal Boolean crit;
                internal Int32 remainingTicks;
                internal Single timer;
                internal Vector3 localPos;
                internal Vector3 normal;
            }

            private Single damageMultiplier => this.blob.damageMultiplier;
            private Boolean crit => this.blob.crit;
            private Single procCoef => this.blob.procCoef;
            private Int32 remainingTicks { get => this.blob.remainingTicks; set => this.blob.remainingTicks = value; }
            private Single timer { get => this.blob.timer; set => this.blob.timer = value; }
            private Vector3 localPos => this.blob.localPos;
            private Vector3 normal => this.blob.normal;

            

            private Blob blob;
            private HurtBox? hurtbox;
            private CharacterBody attacker;
            private GameObject effectInstance;

            
            

            

            public void OnApplied(ref PlasmaPersist ctx)
            {
                if(NetworkServer.active)
                {
                    ctx.targetBody.value?.AddBuff(CatalogModule.plasmaBurnDebuff);
                }
                var transformTarget = this.hurtbox?.transform;
                if(transformTarget == null) return;
                var position = transformTarget.TransformPoint(this.localPos);
                this.effectInstance = UnityEngine.Object.Instantiate(VFXModule.GetPlasmaBurnPrefab(), position, Util.QuaternionSafeLookRotation(this.normal), transformTarget);
            }
            public void OnExpired(ref PlasmaPersist ctx)
            {
                if(NetworkServer.active)
                {
                    ctx.targetBody.value?.RemoveBuff(CatalogModule.plasmaBurnDebuff);
                }
                if(this.effectInstance) MonoBehaviour.Destroy(this.effectInstance);
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

                    if(NetworkServer.active)
                    {
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


            public unsafe UInt32 size => (UInt32)(sizeof(Blob) + sizeof(NetworkInstanceId) + sizeof(NetworkInstanceId) + sizeof(Int16));

            public unsafe void Serialize(Byte* to) => new RWPtr(to)
                .WriteStruct(this.blob)
                .WriteNetObj(this.attacker)
                .Write(this.hurtbox);


            public unsafe void Deserialize(Byte* from) => new RWPtr(from)
                .ReadStruct(out this.blob)
                .ReadNetObj(out this.attacker)
                .Read(out this.hurtbox);
        }
        internal struct PlasmaUpdate : IDotUpdateContext<PlasmaDot, PlasmaDot.PlasmaStack, PlasmaDot.PlasmaUpdate, PlasmaDot.PlasmaPersist>
        {
            internal PlasmaPersist persist { get; }
            internal Int32 stackCounter { get; set; }
            internal PlasmaUpdate(PlasmaPersist persist)
            {
                this.persist = persist;
                this.stackCounter = 0;
            }
        }
        internal struct PlasmaPersist : IDotPersistContext<PlasmaDot, PlasmaDot.PlasmaStack, PlasmaDot.PlasmaUpdate, PlasmaDot.PlasmaPersist>
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