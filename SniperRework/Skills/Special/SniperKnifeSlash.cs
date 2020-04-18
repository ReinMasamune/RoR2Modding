using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;
using UnityEngine.Networking;

namespace ReinSniperRework
{
    internal partial class Main
    {
        internal class SniperKnifeSlash : BaseState
        {
            const Single baseDuration = 0.1f;
            const Single damageCoef = 3.0f;

            private Single duration;

            public override void OnEnter()
            {
                base.OnEnter();
                this.duration = baseDuration / base.attackSpeedStat;

                Main.instance.Logger.LogWarning( "State entered" );

                Util.PlaySound( "Play_merc_m1_hard_swing", base.gameObject );


                if( base.isAuthority )
                {
                    new BlastAttack
                    {
                        attacker = base.gameObject,
                        baseDamage = base.damageStat * damageCoef,
                        baseForce = 0f,
                        bonusForce = Vector3.zero,
                        attackerFiltering = AttackerFiltering.NeverHit,
                        crit = base.RollCrit(),
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = Main.instance.resetOnKill,
                        falloffModel = BlastAttack.FalloffModel.None,
                        inflictor = base.gameObject,
                        losType = BlastAttack.LoSType.None,
                        position = Util.GetCorePosition( base.gameObject ),
                        procChainMask = default,
                        procCoefficient = 1.0f,
                        radius = 8f,
                        teamIndex = base.teamComponent.teamIndex
                    }.Fire();

                    EffectManager.SimpleMuzzleFlash( Main.instance.knifeSlash, base.gameObject, "MuzzleCenter", true );
                }


            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                if( base.fixedAge >= this.duration && base.isAuthority )
                {
                    base.outer.SetNextStateToMain();
                    return;
                }
            }

            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.PrioritySkill;
            }
        }
    }
}


