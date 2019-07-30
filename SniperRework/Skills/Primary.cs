using BepInEx;
using System;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Reflection;
using ReinSniperRework;
using RoR2.UI;

namespace EntityStates.ReinSniperRework.SniperWeapon
{
    public class SniperPrimary : BaseState
    {
        ReinDataLibrary data;

        private float reloadMod;
        private float chargeMod;
        private float shotCharge;
        private float shotTotalDamage;
        private float duration;
        private float shotRad;

        private int reloadTier;
        private int chargeTier;

        private SkillLocator skills;

        private bool consumeChargeAfterShot;
        private bool inputReleased;

        public override void OnEnter()
        {
            base.OnEnter();
            data = base.GetComponent<ReinDataLibrary>();

            if (data.g_reload != null && data.g_charge != null)
            {
                if (data.g_reload.FireReady())
                {
                    reloadTier = data.g_reload.GetReloadTier();
                    chargeTier = data.g_charge.GetChargeTier();
                    skills = base.skillLocator;

                    Ray aimRay = base.GetAimRay();
                    base.StartAimMode(aimRay, 2f, false);
                    this.duration = data.p_baseDuration / this.attackSpeedStat;

                    if (chargeTier == 2)
                    {
                        if (skills.secondary.stock > 0)
                        {
                            skills.secondary.DeductStock(1);
                        }
                        else
                        {
                            chargeTier -= 1;
                        }
                    }
                    consumeChargeAfterShot = chargeTier == 1;

                    reloadMod = 1.0f;

                    switch (reloadTier)
                    {
                        case 0:
                            reloadMod *= data.p_reloadT0Mod;
                            break;
                        case 1:
                            reloadMod *= data.p_reloadT1Mod;
                            break;
                        case 2:
                            reloadMod *= data.p_reloadT2Mod;
                            break;
                        default:
                            reloadMod *= 1.0f;
                            Debug.Log("Reload tier invalid");
                            break;
                    }

                    chargeMod = 1.0f;
                    shotCharge = data.g_charge.GetCharge();

                    switch (chargeTier)
                    {
                        case 0:
                            chargeMod *= data.p_chargeT0Mod;
                            shotRad = data.p_t0ShotRadius;
                            chargeMod *= 1f + data.p_chargeT0Scale * shotCharge;
                            break;
                        case 1:
                            chargeMod *= data.p_chargeT1Mod;
                            shotRad = data.p_t1ShotRadius;
                            chargeMod *= 1f + data.p_chargeT1Scale * shotCharge;
                            break;
                        case 2:
                            chargeMod *= data.p_chargeT2Mod;
                            shotRad = data.p_t2ShotRadius;
                            chargeMod *= 1f + data.p_chargeT2Scale * shotCharge;
                            break;
                        default:
                            chargeMod *= 1f;
                            shotRad = data.p_ntShotRadius;
                            Debug.Log("Charge tier is invalid");
                            break;
                    }

                    if( data.g_charge.charging )
                    {
                        shotRad = data.p_ntShotRadius;
                    }

                    if (consumeChargeAfterShot)
                    {
                        data.g_charge.UpdateCharge(0f);
                    }

                    shotTotalDamage = data.p_shotDamage;
                    shotTotalDamage *= reloadMod;
                    shotTotalDamage *= chargeMod;

                    //Bullet stuff for later, when you feel like fixing this. It was fine before.
                    BulletAttack bul = new BulletAttack();
                    bul.owner = base.gameObject;
                    bul.weapon = base.gameObject;
                    bul.damage = shotTotalDamage * this.damageStat;
                    bul.isCrit = base.RollCrit();
                    bul.force = shotTotalDamage * data.p_shotForce / data.p_shotDamage;
                    bul.procCoefficient = data.p_shotCoef;
                    bul.sniper = true;
                    bul.falloffModel = BulletAttack.FalloffModel.None;
                    bul.tracerEffectPrefab = data.p_tracerEffectPrefab;
                    bul.hitEffectPrefab = data.p_hitEffectPrefab;
                    bul.origin = aimRay.origin;
                    bul.aimVector = aimRay.direction;
                    bul.minSpread = 0f;
                    bul.maxSpread = 0f;
                    bul.bulletCount = 1;
                    bul.muzzleName = data.p_muzzleName;
                    bul.radius = shotRad;
                    bul.maxDistance = data.p_maxRange;
                    bul.smartCollision = data.p_shotSmartCollision;
                    if (data.g_charge.charging)
                    {
                        bul.stopperMask = LayerIndex.world.mask;
                    }

                    bul.Fire();

                    data.g_reload.UpdateAttackSpeed(this.attackSpeedStat);
                    data.g_reload.Shot();

                    Util.PlaySound(data.p_attackSoundString, base.gameObject);

                    base.AddRecoil(-1f * data.p_recoilAmplitude, -2f * data.p_recoilAmplitude, -0.5f * data.p_recoilAmplitude, 0.5f * data.p_recoilAmplitude);
                    base.characterMotor.ApplyForce(base.inputBank.aimDirection * -1f * data.p_recoilForceMult);

                    base.PlayAnimation("Gesture", "FireShotgun", "FireShotgun.playbackRate", this.duration * 1.5f);
                    base.PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", this.duration * 1.5f);

                    if (data.p_effectPrefab)
                    {
                        EffectManager.instance.SimpleMuzzleFlash(data.p_effectPrefab, base.gameObject, data.p_muzzleName, false);
                    }
                }
                else
                {
                    if (data.g_reload.Reload())
                    {
                        Util.PlaySound(data.p_reloadSoundString, this.gameObject);
                        base.characterMotor.ApplyForce(new Vector3(0f, data.p_reloadForceMult, 0f));
                    }
                }
            }
            else
            {
                Debug.Log("Missing component on character");
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.inputBank)
            {
                this.inputReleased |= !base.inputBank.skill1.down;
            }
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (this.inputReleased && base.fixedAge >= data.p_interruptInterval / this.attackSpeedStat)
            {
                return InterruptPriority.Any;
            }
            return InterruptPriority.Skill;
        }
    }
}