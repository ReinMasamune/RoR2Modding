using RoR2;
using UnityEngine;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        internal class SnapOrb : RoR2.Orbs.Orb
        {
            public System.Single speed = 200f;
            public System.Single damage = 1f;
            public System.Single scale = 1f;
            public System.Single procCoef = 1f;
            public System.Single radius = 1f;
            public System.UInt32 skin = 0;

            public Vector3 targetPos;

            public System.Boolean crit = false;
            public System.Boolean useTarget = false;

            public TeamIndex team;
            public DamageColorIndex damageColor;

            public GameObject attacker;
            public ProcChainMask procMask;

            private Vector3 lastPos;

            public override void Begin()
            {
                if( !this.target )
                {
                    this.useTarget = false;
                }

                if( this.useTarget )
                {
                    this.lastPos = this.target.transform.position;
                } else
                {
                    this.lastPos = this.targetPos;
                }

                this.duration = Vector3.Distance( this.lastPos, this.origin ) / this.speed + 0.15f;
                EffectData effectData = new EffectData
                {
                    origin = origin,
                    genericFloat = duration,
                    genericBool = useTarget,
                    start = lastPos
                };
                effectData.SetHurtBoxReference( this.target );
                EffectManager.SpawnEffect( Main.primaryOrbEffect, effectData, true );
            }

            public override void OnArrival()
            {
                if( this.useTarget )
                {
                    this.lastPos = this.targetPos;
                } else
                {
                    this.lastPos = this.targetPos;
                }
                EffectData effect = new EffectData
                {
                    origin = lastPos,
                    scale = 0.5f
                };

                //EffectManager.SpawnEffect( Main.primaryExplosionEffects[this.skin], effect, true );

                if( this.attacker )
                {
                    new BlastAttack
                    {
                        attacker = attacker,
                        baseDamage = damage,
                        baseForce = 0f,
                        bonusForce = Vector3.zero,
                        attackerFiltering = AttackerFiltering.Default,
                        crit = crit,
                        damageColorIndex = damageColor,
                        damageType = DamageType.Generic,
                        falloffModel = BlastAttack.FalloffModel.None,
                        inflictor = null,
                        position = lastPos,
                        procChainMask = procMask,
                        procCoefficient = procCoef,
                        radius = radius,
                        teamIndex = team
                    }.Fire();
                }
            }
        }
    }
}