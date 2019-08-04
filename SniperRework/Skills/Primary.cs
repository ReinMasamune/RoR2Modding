using RoR2;
using UnityEngine;
using ReinSniperRework;

namespace EntityStates.ReinSniperRework.SniperWeapon
{
    public class SniperPrimary : BaseState
    {
        ReinDataLibrary data;

        private float duration;
        private bool consumeChargeAfterShot;

        public override void OnEnter()
        {
            base.OnEnter();
            data = base.GetComponent<ReinDataLibrary>();

            if( data )
            {
                SkillLocator skills = base.skillLocator;
                Ray aimRay = base.GetAimRay();
                base.StartAimMode(aimRay, 2f, false);
                duration = data.p_reloadStartDelay / this.attackSpeedStat;

                int chargeTier = data.g_chargeTier;

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
                consumeChargeAfterShot = chargeTier < 2;

                float reloadMod = 1.0f;

                switch (data.g_reloadTier)
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

                float chargeMod = 1.0f;
                float shotCharge = data.g_shotCharge;
                float shotCoef;
                float shotRad;

                switch (chargeTier)
                {
                    case 0:
                        chargeMod *= data.p_chargeT0Mod;
                        shotCoef = data.p_shotCoef;
                        shotRad = data.p_t0ShotRadius;
                        chargeMod *= 1f + data.p_chargeT0Scale * shotCharge;
                        break;
                    case 1:
                        chargeMod *= data.p_chargeT1Mod;
                        shotCoef = data.p_chargeT1Coef;
                        shotRad = data.p_t1ShotRadius;
                        chargeMod *= 1f + data.p_chargeT1Scale * shotCharge;
                        break;
                    case 2:
                        chargeMod *= data.p_chargeT2Mod;
                        shotCoef = data.p_chargeT2Coef;
                        shotRad = data.p_t2ShotRadius;
                        chargeMod *= 1f + data.p_chargeT2Scale * shotCharge;
                        break;
                    default:
                        chargeMod *= 1f;
                        shotCoef = data.p_shotCoef;
                        shotRad = data.p_ntShotRadius;
                        Debug.Log("Charge tier is invalid");
                        break;
                }

                if (data.g_zoomed)
                {
                    shotRad = data.p_ntShotRadius;
                }

                if (consumeChargeAfterShot)
                {
                    data.g_shotCharge = 0f;
                }

                float shotTotalDamage = data.p_shotDamage;
                shotTotalDamage *= reloadMod;
                shotTotalDamage *= chargeMod;

                //Bullet stuff for later, when you feel like fixing this. It was fine before.
                BulletAttack bul = new BulletAttack();
                bul.owner = base.gameObject;
                bul.weapon = base.gameObject;
                bul.damage = shotTotalDamage * this.damageStat;
                bul.isCrit = base.RollCrit();
                bul.force = shotTotalDamage * data.p_shotForce / data.p_shotDamage;
                bul.procCoefficient = shotCoef;
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
                if (data.g_zoomed)
                {
                    bul.stopperMask = LayerIndex.world.mask;
                }
                else
                {
                    bul.stopperMask = LayerIndex.entityPrecise.mask;
                }

                bul.Fire();

                Util.PlaySound(data.p_attackSoundString, base.gameObject);

                base.AddRecoil(-1f * data.p_recoilAmplitude, -2f * data.p_recoilAmplitude, -0.5f * data.p_recoilAmplitude, 0.5f * data.p_recoilAmplitude);
                base.characterMotor.ApplyForce(base.inputBank.aimDirection * -1f * data.p_recoilForceMult);

                //base.PlayAnimation("Gesture", "FireShotgun", "FireShotgun.playbackRate", this.duration * 1.5f);
                //base.PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", this.duration * 1.5f);

                if (data.p_effectPrefab)
                {
                    //EffectManager.instance.SimpleMuzzleFlash(data.p_effectPrefab, base.gameObject, data.p_muzzleName, false);
                }
            }
            else
            {
                Debug.Log("Missing data library you moron");
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.isSprinting = false;
            if (base.fixedAge >= duration && base.isAuthority)
            {
                this.outer.SetNextState(new SniperReload());
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if( base.fixedAge >= ( this.duration ) )
            {
                return InterruptPriority.Any;
            }
            else
            {
                return InterruptPriority.Death;
            }
        }
    }
}