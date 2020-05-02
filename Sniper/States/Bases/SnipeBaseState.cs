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
using Sniper.Enums;
using Sniper.Data;
using Sniper.Modules;

namespace Sniper.States.Bases
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
            if( this.bulletFired )
                return;
            var aimRay = GetAimRay();
            var bullet = new ExpandableBulletAttack
            {
                aimVector = aimRay.direction,
                attackerBody = characterBody,
                bulletCount = 1,
                damage = characterBody.damage,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                falloffModel = BulletAttack.FalloffModel.None,
                force = 1f,
                HitEffectNormal = true,
                hitEffectPrefab = null,
                hitMask = LayerIndex.entityPrecise.mask | LayerIndex.world.mask,
                isCrit = RollCrit(),
                maxDistance = 1000f,
                maxSpread = 0f,
                minSpread = 0f,
                muzzleName = "MuzzleRailgun",
                origin = aimRay.origin,
                owner = gameObject,
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
            characterBody.ammo.ModifyBullet( bullet );
            characterBody.passive.ModifyBullet( bullet );

            var data = characterBody.scopeInstanceData;
            if( data != null && data.shouldModify ) data.SendFired().Apply( bullet );



            bullet.Fire();
            AddRecoil( -1f * this.recoilStrength, -3f * this.recoilStrength, -0.2f * this.recoilStrength, 0.2f * this.recoilStrength );
            this.bulletFired = true;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            base.GetModelAnimator().SetBool( "shouldAim", true );
            this.duration = this.baseDuration / characterBody.attackSpeed;
            base.StartAimMode( 8f, false );
            base.PlayAnimation( "Gesture, Additive", "Shoot" );
            if( isAuthority ) this.FireBullet();

            SoundModule.PlayFire( base.gameObject, 0f );

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if( isAuthority && fixedAge >= this.duration )
                outer.SetNextStateToMain();
        }
    }
}
