#if ANCIENTWISP
using EntityStates;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        internal class AWFirePrimary : BaseState
        {
            const Single baseDuration = 0.5f;
            const Single damageCoef = 1f;

            private Single duration;

            public override void OnEnter()
            {
                base.OnEnter();
                this.duration = baseDuration / base.attackSpeedStat;

                Ray aim = base.GetAimRay();

                base.PlayCrossfade( "Gesture", "Throw1", "Throw.playbackRate", this.duration * 2f, 0.2f );


                //Muzzle flash


                var muzzleTransform = base.GetModelTransform().GetComponent<ChildLocator>().FindChild( "MuzzleRight" );

                if( base.isAuthority )
                {
                    ProjectileManager.instance.FireProjectile( new FireProjectileInfo
                    {
                        crit = base.RollCrit(),
                        damage = base.damageStat * damageCoef,
                        damageColorIndex = RoR2.DamageColorIndex.Default,
                        force = 0f,
                        owner = base.gameObject,
                        position = muzzleTransform.position,
                        procChainMask = default,
                        projectilePrefab = Main.AW_primaryProjectile,
                        rotation = Util.QuaternionSafeLookRotation( aim.direction ),

                    } );
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

            public override void OnExit()
            {
                base.OnExit();
                base.PlayCrossfade( "Gesture", "Idle", 0.2f );
            }

            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.PrioritySkill;
            }
        }
    }

}
#endif