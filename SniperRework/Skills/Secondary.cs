using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using ReinSniperRework;

namespace EntityStates.ReinSniperRework.SniperWeapon
{
    public class SniperSecondary : BaseState
    {
        ReinDataLibrary data;

        private GameObject originalCrosshairPrefab;

        private int slowTier = 0;

        private float curZoom;

        public override void OnEnter()
        {
            base.OnEnter();
            data = base.GetComponent<ReinDataLibrary>();
            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.aimMode = CameraTargetParams.AimType.FirstPerson;
                curZoom = data.s_startZoom;
                base.cameraTargetParams.fovOverride = zoomToFov(curZoom);
            }
            if (base.characterBody)
            {
                this.originalCrosshairPrefab = base.characterBody.crosshairPrefab;
                base.characterBody.crosshairPrefab = data.s_crosshairPrefab;
            }
            data.g_ui.showChargeBar = true;
            data.g_shotCharge = 0f;
            data.g_zoomed = true;
        }

        public override void OnExit()
        {
            data.g_ui.showChargeBar = false;
            data.g_shotCharge = 0f;
            data.g_zoomed = false;
            if (NetworkServer.active && base.characterBody)
            {
                UpdateSlowTier(0);
            }
            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
                base.cameraTargetParams.fovOverride = -1f;
            }
            if (base.characterBody)
            {
                base.characterBody.crosshairPrefab = this.originalCrosshairPrefab;
            }
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.characterBody.isSprinting = false;
            if( data.g_shotCharge < 1.0f )
            {
                float addedCharge = Time.fixedDeltaTime * base.attackSpeedStat / data.s_chargeTime;
                data.g_shotCharge += addedCharge;
            }
            if( data.g_shotCharge >= 1.0f )
            {
                data.g_shotCharge = 1.0f;
            }

            if( data.g_shotCharge >= data.s_boost1Start )
            {
                if (data.g_shotCharge >= data.s_boost2Start)
                {
                    data.g_chargeTier = 2;
                }
                else
                {
                    data.g_chargeTier = 1;
                }
            }
            else
            {
                data.g_chargeTier = 0;
            }

            UpdateSlowTier(data.g_chargeTier + 1);

            if( base.cameraTargetParams.aimMode != CameraTargetParams.AimType.FirstPerson )
            {
                base.cameraTargetParams.aimMode = CameraTargetParams.AimType.FirstPerson;
            }

            if (Input.mouseScrollDelta.y != 0f)
            {
                curZoom = Mathf.Clamp(curZoom + Input.mouseScrollDelta.y * data.s_scrollScale, data.s_minZoom, data.s_maxZoom);

                base.cameraTargetParams.fovOverride = zoomToFov(curZoom);
            }

            if (base.isAuthority && (!base.inputBank || !base.inputBank.skill2.down ))
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        private void UpdateSlowTier(int newSlow)
        {
            if (newSlow == slowTier)
            {
                return;
            }
            else
            {
                bool hasOldBuff = true;
                bool hasNewBuff = true;
                BuffIndex oldBuff = new BuffIndex();
                BuffIndex newBuff = new BuffIndex();
                switch (slowTier)
                {
                    case 1:
                        oldBuff = BuffIndex.Slow30;
                        break;

                    case 2:
                        oldBuff = BuffIndex.Slow50;
                        break;

                    case 3:
                        oldBuff = BuffIndex.Slow80;
                        break;

                    default:
                        hasOldBuff = false;
                        break;
                }

                switch (newSlow)
                {
                    case 1:
                        newBuff = BuffIndex.Slow30;
                        break;

                    case 2:
                        newBuff = BuffIndex.Slow50;
                        break;

                    case 3:
                        newBuff = BuffIndex.Slow80;
                        break;

                    default:
                        hasNewBuff = false;
                        break;
                }

                if (hasOldBuff)
                {
                    base.characterBody.RemoveBuff(oldBuff);
                }
                if (hasNewBuff)
                {
                    base.characterBody.AddBuff(newBuff);
                }

                slowTier = newSlow;
            }
        }

        private float zoomToFov(float zoom)
        {
            float startPoint = data.s_baseFOV * Mathf.Deg2Rad;
            startPoint = Mathf.Tan(startPoint);
            startPoint = startPoint / zoom;
            startPoint = Mathf.Atan(startPoint);
            startPoint *= Mathf.Rad2Deg;
            return startPoint;
        }
    }
}
