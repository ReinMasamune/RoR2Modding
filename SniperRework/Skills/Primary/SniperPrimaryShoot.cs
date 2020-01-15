using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;

namespace ReinSniperRework
{
    internal partial class Main
    {
        internal class SniperShoot : BaseState
        {

            public enum ReloadQuality
            {
                None = 0,
                Normal = 1,
                Good = 2,
                Perfect = 3
            }

            public ReloadQuality quality;

            const Single baseMult = 2.5f;
            const Single normalMult = 1f;
            const Single goodMult = 1.5f;
            const Single perfectMult = 2f;
            const Single baseDuration = 0.75f;
            const Single baseForce = 1f;
            const Single normalSound = 0.75f;
            const Single chargedSound = 0.6f;

            private Boolean isCharged = false;
            private Single duration;
            private Single damage;

            public override void OnEnter()
            {
                base.OnEnter();

                base.characterBody.isSprinting = false;

                if( this.quality == ReloadQuality.None ) this.quality = ReloadQuality.Normal;

                var charge = base.gameObject.GetComponent<SniperUIController>().GetCharge();
                if( charge > 1.0f )
                {
                    this.isCharged = true;
                }

                this.duration = baseDuration / base.attackSpeedStat;

                this.damage = base.damageStat * baseMult * charge;
                switch( this.quality )
                {
                    case ReloadQuality.Normal:
                        this.damage *= normalMult;
                        break;
                    case ReloadQuality.Good:
                        this.damage *= goodMult;
                        break;
                    case ReloadQuality.Perfect:
                        this.damage *= perfectMult;
                        break;
                }

                base.characterBody.SetAimTimer( 2f );

                this.Fire();
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                if( base.isAuthority && base.fixedAge >= this.duration )
                {
                    base.outer.SetNextState( new SniperReload() );
                }
            }

            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.PrioritySkill;
            }

            private void Fire()
            {
                Util.PlayScaledSound( "Play_huntress_R_snipe_shoot", base.gameObject, this.isCharged ? chargedSound : normalSound );
                var aim = base.GetAimRay();

                if( base.isAuthority )
                {

                    BulletAttack bullet = new BulletAttack
                    {
                        aimVector = aim.direction,
                        bulletCount = 1u,
                        damage = this.damage,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.None,
                        force = baseForce,
                        isCrit = base.RollCrit(),
                        maxDistance = 1000f,
                        minSpread = 0f,
                        maxSpread = 0f,
                        owner = base.gameObject,
                        sniper = true,
                        smartCollision = true,
                        radius = 0.25f,
                        origin = aim.origin,
                        procChainMask = default,
                        procCoefficient = 1.0f,
                        tracerEffectPrefab = Main.instance.primaryTracer,
                        hitEffectPrefab = Main.instance.primaryImpact,
                        HitEffectNormal = false,
                        stopperMask = this.isCharged ? LayerIndex.world.mask : (LayerMask)( LayerIndex.world.mask | LayerIndex.entityPrecise.mask ),
                        muzzleName = "MuzzleLeft",
                        weapon = base.gameObject
                    };

                    bullet.Fire();
                }
            }
        }
    
    }
}


