﻿using EntityStates;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin
{
#if ANCIENTWISP
    internal partial class Main
    {
        internal class AWFirePrimary : BaseState
        {
            const Single baseDuration = 0.5f;
            const Single damageCoef = 1.0f;

            private Single duration;

            public override void OnEnter()
            {
                base.OnEnter();
                this.duration = baseDuration / base.attackSpeedStat;

                Ray aim = base.GetAimRay();

                base.PlayAnimation( "Gesture", "FireRHCannon", "FireRHCannon.playbackRate", this.duration );


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
                        projectilePrefab = Main.instance.AW_primaryProj,
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
                base.PlayAnimation( "Gesture", "Idle" );
            }

            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.Skill;
            }
        }
    }
#endif
}