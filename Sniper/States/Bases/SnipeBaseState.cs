using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using RoR2.Networking;
using UnityEngine;
using KinematicCharacterController;
using EntityStates;
using RoR2.Skills;
using System.Reflection;
using Sniper.Expansions;
using Sniper.Skills;
using Sniper.Enums;
using Sniper.Data;

namespace Sniper.Skills
{
    internal abstract class SnipeBaseState : SniperSkillBaseState
    {
        protected abstract Single baseDuration { get; }
        protected abstract Single recoilStrength { get; }

        internal ReloadParams reloadParams { get; set; }

        internal ReloadTier reloadTier { private get; set; }

        private Boolean bulletFired = false;
        private Single duration;

        protected abstract void ModifyBullet( ExpandableBulletAttack bullet );

        private void FireBullet()
        {
            if( this.bulletFired ) return;
            var aimRay = base.GetAimRay();
            var bullet = new ExpandableBulletAttack
            {
                aimVector = aimRay.direction,
                attackerBody = base.characterBody,
                bulletCount = 1,
                damage = base.characterBody.damage,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                falloffModel = BulletAttack.FalloffModel.None,
                force = 1f,
                HitEffectNormal = true,
                hitEffectPrefab = null,
                hitMask = LayerIndex.entityPrecise.mask | LayerIndex.world.mask,
                isCrit = base.RollCrit(),
                maxDistance = 1000f,
                maxSpread = 0f,
                minSpread = 0f,
                muzzleName = "",
                origin = aimRay.origin,
                owner = base.gameObject,
                procChainMask = default,
                procCoefficient = 1f,
                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                radius = 1f,
                smartCollision = true,
                sniper = false,
                spreadPitchScale = 1f,
                spreadYawScale = 1f,
                stopperMask = LayerIndex.world.mask | LayerIndex.entityPrecise.mask,
                tracerEffectPrefab = null,
                weapon = null,
            };
            this.ModifyBullet( bullet );
            this.reloadParams.ModifyBullet( bullet, this.reloadTier );
            base.characterBody.ammo.ModifyBullet( bullet );
            base.characterBody.passive.ModifyBullet( bullet );

            var data = base.characterBody.scopeInstanceData;
            if( data != null && data.shouldModify )
            {
                data.SendFired().Apply(bullet);
            }


            bullet.Fire();
            base.AddRecoil( -1f * this.recoilStrength, -3f * this.recoilStrength, -0.2f * this.recoilStrength, 0.2f * this.recoilStrength );
            this.bulletFired = true;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / base.characterBody.attackSpeed;
            if( base.isAuthority )
            {
                this.FireBullet();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if( base.isAuthority && base.fixedAge >= this.duration )
            {
                base.outer.SetNextStateToMain();
            }
        }
    }
}
