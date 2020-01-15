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
        internal class SniperCharging : BaseState
        {
            internal Single charge
            {
                get
                {
                    return this.chargeTimer / maxChargeTime;
                }
            }

            internal Single damageMult
            {
                get
                {
                    this.skillLocator.secondary.DeductStock( 1 );
                    return zoomedDamageMult;
                }
            }

            private Single t1DamageMult
            {
                get
                {
                    return 1f + (this.charge * t1Scale);
                }
            }

            private Single t2DamageMult
            {
                get
                {
                    return 1f + (this.charge * t2Scale);
                }
            }

            internal Boolean isActive { get; private set; }

            const Single maxChargeTime = 5.0f;
            internal const Single t1StartFrac = 0.35f;
            internal const Single t2StartFrac = 0.9f;
            const Single t1Start = t1StartFrac * maxChargeTime;
            const Single t2Start = t2StartFrac * maxChargeTime;
            const Single t1Scale = 3f;
            const Single t2Scale = 9f;
            const Single startZoom = 3f;
            const Single minZoom = 1.25f;
            const Single maxZoom = 10f;
            const Single baseFOV = 60f;
            const Single scrollScale = 5f;
            const Single zoomedDamageMult = 3.0f;



            private Single chargeTimer = 0f;
            private Single zoom = 0f;

            private GameObject origCrosshair;

            public override void OnEnter()
            {
                base.OnEnter();
                base.gameObject.GetComponent<SniperUIController>().SetCharging( this );
                this.isActive = true;
                if( base.cameraTargetParams )
                {
                    base.cameraTargetParams.aimMode = CameraTargetParams.AimType.FirstPerson;
                    this.zoom = startZoom;
                    base.cameraTargetParams.fovOverride = this.ZoomToFOV( this.zoom );
                }

                if( base.characterBody )
                {
                    this.origCrosshair = base.characterBody.crosshairPrefab;
                    base.characterBody.crosshairPrefab = Main.instance.scopeCrosshair;

                    if( NetworkServer.active )
                    {
                        base.characterBody.AddBuff( BuffIndex.Slow50 );
                    }
                }

                
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                base.characterBody.isSprinting = false;

                if( base.cameraTargetParams.aimMode != CameraTargetParams.AimType.FirstPerson ) base.cameraTargetParams.aimMode = CameraTargetParams.AimType.FirstPerson;

                if( Input.mouseScrollDelta.y != 0f )
                {
                    this.zoom = Mathf.Clamp( this.zoom + Input.mouseScrollDelta.y * scrollScale, minZoom, maxZoom );
                    base.cameraTargetParams.fovOverride = this.ZoomToFOV( this.zoom );
                }

                if( base.isAuthority && ( !base.inputBank || !base.inputBank.skill2.down || base.skillLocator.secondary.stock == 0 ) )
                {
                    base.outer.SetNextStateToMain();
                }
            }

            public override void OnExit()
            {
                base.OnExit();
                this.isActive = false;
                if( base.cameraTargetParams )
                {
                    base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
                    base.cameraTargetParams.fovOverride = -1f;
                }
                if( base.characterBody )
                {
                    base.characterBody.crosshairPrefab = this.origCrosshair;
                    if( NetworkServer.active )
                    {
                        base.characterBody.RemoveBuff( BuffIndex.Slow50 );
                    }
                }
            }

            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.PrioritySkill;
            }

            private Single ZoomToFOV( Single zoom )
            {
                Single startPoint = baseFOV * Mathf.Deg2Rad;
                startPoint *= Mathf.Tan( startPoint );
                startPoint /= zoom;
                startPoint = Mathf.Atan( startPoint );
                startPoint *= Mathf.Rad2Deg;
                return startPoint;
            }
        }
    }
}


