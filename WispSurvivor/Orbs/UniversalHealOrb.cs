using RoR2;
using System;
using UnityEngine;

namespace RogueWispPlugin
{
#if ANCIENTWISP
    internal partial class Main
    {
        public class UniversalHealOrb : RoR2.Orbs.Orb
        {
            public HealType healType;
            public HealTarget healTarget;

            public Single value;
            public Single speed;

            public GameObject effectPrefab;

            public UniversalHealOrb( HurtBox target, GameObject effectPrefab, Vector3 origin, Single value, Single speed, HealType healType = HealType.Flat, HealTarget healTarget = HealTarget.Health )
            {
                base.target = target;
                base.origin = origin;

                this.effectPrefab = effectPrefab;
                this.value = value;
                this.speed = speed;
                this.healType = healType;
                this.healTarget = healTarget;
            }


            public enum HealType
            {
                Flat = 0,
                PercentMax = 1,
                PercentCurrent = 2,
                PercentMissing = 3
            }
            public enum HealTarget
            {
                Health = 0,
                Shield = 1,
                Barrier = 2
            }

            private struct HealContext
            {
                public Single max;
                public Single current;
            }

            public override void Begin()
            {
                if( base.target )
                {
                    base.duration = Vector3.Distance( base.target.transform.position, base.origin ) / this.speed;
                    EffectData data = new EffectData
                    {
                        origin = base.origin,
                        genericFloat = base.duration
                    };
                    data.SetHurtBoxReference( base.target );
                    EffectManager.SpawnEffect( this.effectPrefab, data, true );
                }
            }

            public override void OnArrival()
            {
                if( base.target )
                {
                    var hc = this.target.healthComponent;
                    if( hc )
                    {
                        var context = new HealContext();
                        switch( this.healTarget )
                        {
                            default:
                                Main.LogE( "Unhandled HealTarget" );
                                break;
                            case HealTarget.Health:
                                context.current = hc.health;
                                context.max = hc.fullHealth;
                                break;
                            case HealTarget.Shield:
                                context.current = hc.shield;
                                context.max = hc.fullShield;
                                break;
                            case HealTarget.Barrier:
                                context.current = hc.barrier;
                                context.max = hc.fullBarrier;
                                break;
                        }

                        var healValue = 0f;
                        switch( this.healType )
                        {
                            default:
                                Main.LogE( "Unhandled HealType" );
                                break;
                            case HealType.Flat:
                                healValue = this.value;
                                break;
                            case HealType.PercentMax:
                                healValue = this.value * context.max;
                                break;
                            case HealType.PercentCurrent:
                                healValue = this.value * context.current;
                                break;
                            case HealType.PercentMissing:
                                healValue = this.value * (context.max - context.current);
                                break;
                        }

                        switch( this.healTarget )
                        {
                            default:
                                Main.LogE( "Unhandled HealType" );
                                break;
                            case HealTarget.Health:
                                hc.Heal( healValue, default, true );
                                break;
                            case HealTarget.Shield:
                                hc.RechargeShield( healValue );
                                break;
                            case HealTarget.Barrier:
                                hc.AddBarrier( healValue );
                                break;
                        }

                    }
                }
            }
        }
    }
#endif
}