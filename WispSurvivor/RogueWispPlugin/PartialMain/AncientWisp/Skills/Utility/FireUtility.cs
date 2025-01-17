﻿#if ANCIENTWISP
using System;

using EntityStates;

using RoR2;
using RoR2.Projectile;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        internal class AWFireUtility : BaseState
        {
            const Single baseDuration = 0.5f;
            const Single damageCoef = 0.3f;


            private Single duration;

            public override void OnEnter()
            {
                base.OnEnter();

                this.duration = baseDuration / base.attackSpeedStat;

                if( base.isAuthority )
                {
                    var r = base.GetAimRay();
                    ProjectileManager.instance.FireProjectile( new FireProjectileInfo
                    {
                        crit = base.RollCrit(),
                        damage = damageCoef * base.damageStat,
                        damageColorIndex = RoR2.DamageColorIndex.Default,
                        force = 0f,
                        owner = base.gameObject,
                        position = r.origin,
                        procChainMask = default,
                        projectilePrefab = Main.AW_utilityProjectile,
                        rotation = Util.QuaternionSafeLookRotation( r.direction ),
                    } );
                }

                base.PlayCrossfade( "Gesture", "FireBomb", "FireBomb.playbackRate", this.duration, 0.2f );

                Util.PlayScaledSound( "Play_imp_overlord_attack1_throw", base.gameObject, 0.5f );
                Util.PlayScaledSound( "Play_imp_overlord_attack1_throw", base.gameObject, 0.5f );
                Util.PlayScaledSound( "Play_imp_overlord_attack1_throw", base.gameObject, 0.5f );
                Util.PlayScaledSound( "Play_imp_overlord_attack1_throw", base.gameObject, 0.5f );
                Util.PlayScaledSound( "Play_imp_overlord_attack1_throw", base.gameObject, 0.5f );
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();

                if( base.fixedAge >= this.duration && base.isAuthority )
                {
                    base.outer.SetNextStateToMain();
                }
            }

            public override void OnExit()
            {
                base.OnExit();
            }

            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.PrioritySkill;
            }
        }
    }
}
#endif