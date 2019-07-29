using BepInEx;
using System;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Reflection;
using ReinSniperRework;
using RoR2.UI;

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
            if (data.g_charge)
            {
                data.g_charge.ShowBar(true);
                data.g_charge.UpdateCharge(0f);
            }
        }

        public override void OnExit()
        {
            if (data.g_charge)
            {
                data.g_charge.ShowBar(false);
                data.g_charge.UpdateCharge(0f);
            }
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
            data.g_charge.AddCharge(Time.fixedDeltaTime * this.attackSpeedStat);
            UpdateSlowTier(data.g_charge.GetChargeTier() + 1);

            if (Input.mouseScrollDelta.y != 0f)
            {
                curZoom = Mathf.Clamp(curZoom + Input.mouseScrollDelta.y * data.s_scrollScale, data.s_minZoom, data.s_maxZoom);

                base.cameraTargetParams.fovOverride = zoomToFov(curZoom);
            }

            if (base.isAuthority && (!base.inputBank || !base.inputBank.skill2.down || base.characterBody.isSprinting))
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
