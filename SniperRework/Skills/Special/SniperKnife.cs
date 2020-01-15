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
using RoR2.Projectile;

namespace ReinSniperRework
{
    internal partial class Main
    {
        internal class SniperKnife : BaseState
        {
            const Single baseDuration = 0.3f;
            const Single prepFrac = 0.45f;
            const Single baseDelay = baseDuration * prepFrac;
            const Single recoil = 0.1f;
            const Single damageCoef = 2.0f;
            const Single force = 1.0f;



            private Single duration;
            private Single delay;
            private Boolean hasFired = false;

            public override void OnEnter()
            {
                base.OnEnter();
                this.duration = baseDuration / base.attackSpeedStat;
                this.delay = baseDelay / base.attackSpeedStat;

                base.PlayAnimation( "Gesture, Additive", "ThrowGrenade", "FireFMJ.playbackRate", this.duration * 2f );
                base.PlayAnimation( "Gesture, Override", "ThrowGrenade", "FireFMJ.playbackRate", this.duration * 2f );
                //SOUND
                if( base.characterBody ) base.characterBody.SetAimTimer( 2f );
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                if( this.fixedAge >= this.delay && !this.hasFired )
                {
                    this.Fire();
                }
                if( this.fixedAge >= this.duration && base.isAuthority )
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }

            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.PrioritySkill;
            }

            public void Fire()
            {
                string muzzle = "MuzzleRight";

                Util.PlaySound( "Play_item_proc_dagger_spawn", base.gameObject );

                base.AddRecoil( -2f * recoil, -3f * recoil, -1f * recoil, recoil );
                this.hasFired = true;
                var ray = base.GetAimRay();
                var projInfo = new FireProjectileInfo
                {
                    crit = base.RollCrit(),
                    damage = damageCoef * base.damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    force = force,
                    owner = base.gameObject,
                    position = base.FindModelChild("MuzzleRight").position,
                    procChainMask = default,
                    projectilePrefab = Main.instance.knifeProjectile,
                    rotation = Util.QuaternionSafeLookRotation( ray.direction ),
                };
                if( base.isAuthority ) ProjectileManager.instance.FireProjectile( projInfo );
            }
        }
    }
}


