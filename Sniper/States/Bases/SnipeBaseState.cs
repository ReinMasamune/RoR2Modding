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
using System.Diagnostics;

namespace Sniper.States.Bases
{
    internal abstract class SnipeBaseState : SniperSkillBaseState
    {
#if PROFILESNIPE
        private static UInt64 timeTotal = 0ul;
        private static Dictionary<String,(UInt64 counter, UInt64 ticks)> timeKeeper = new Dictionary<String, (UInt64 counter, UInt64 ticks)>();
        private static void KeepTime( System.Diagnostics.Stopwatch timer, String method )
        {
            timer.Stop();
            if( !timeKeeper.ContainsKey( method ) )
            {
                timeKeeper[method] = (0ul,0ul);
                timer.Restart();
                return;
            }
            var cur = timeKeeper[method];
            cur.counter++;
            var total = cur.ticks += (UInt64)timer.ElapsedTicks;
            timeKeeper[method] = cur;
            timeTotal += (UInt64)timer.ElapsedTicks;

            Log.Warning( String.Format( "{0}:\nTotal Ticks: {1}\nTotal seconds: {2}\nPercent of time: {3}%\nAverage ticks: {4}\nTotalTimesCalled: {5}", method, total, (Double)total / (Double)Stopwatch.Frequency, 100.0 * (Double)total / (Double)timeTotal, (Double)total / cur.counter, cur.counter ) );
     
            timer.Reset();
            timer.Start();
        }
#endif
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
#if PROFILESNIPE
            var timer = System.Diagnostics.Stopwatch.StartNew();
#endif
            var aimRay = GetAimRay();
#if PROFILESNIPE
            KeepTime( timer, "GetAimRay" );
#endif
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
#if PROFILESNIPE
            KeepTime( timer, "create" );
#endif
            this.ModifyBullet( bullet );
#if PROFILESNIPE
            KeepTime( timer, "Mod1" );
#endif
            this.reloadParams.ModifyBullet( bullet, this.reloadTier );
#if PROFILESNIPE
            KeepTime( timer, "Mod2" );
#endif
            characterBody.ammo.ModifyBullet( bullet );
#if PROFILESNIPE
            KeepTime( timer, "Mod3" );
#endif
            characterBody.passive.ModifyBullet( bullet );
#if PROFILESNIPE
            KeepTime( timer, "Mod4" );
#endif

            var data = characterBody.scopeInstanceData;
#if PROFILESNIPE
            KeepTime( timer, "GetData" );
#endif
            if( data != null && data.shouldModify ) data.SendFired().Apply( bullet );
#if PROFILESNIPE
            KeepTime( timer, "SendFired" );
#endif
            bullet.Fire();
#if PROFILESNIPE
            KeepTime( timer, "Fire" );
#endif
            AddRecoil( -1f * this.recoilStrength, -3f * this.recoilStrength, -0.2f * this.recoilStrength, 0.2f * this.recoilStrength );
#if PROFILESNIPE
            KeepTime( timer, "Recoil" );
            timer.Stop();
#endif
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
