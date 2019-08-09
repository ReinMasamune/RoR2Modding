using RoR2;
using UnityEngine;
using ReinSniperRework;
using System.Collections;
using System.Collections.Generic;

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

                LayerMask mask = LayerIndex.world.mask | LayerIndex.entityPrecise.mask;

                if (data.g_zoomed)
                {
                    //shotRad = data.p_ntShotRadius;
                    mask = LayerIndex.world.mask;
                }
                else
                {
                    shotRad = data.p_ntShotRadius;
                }

                bool crit = base.RollCrit();

                float shotTotalDamage = data.p_shotDamage;
                shotTotalDamage *= reloadMod;
                shotTotalDamage *= chargeMod;

                float wlStart = 680f - 100f*data.g_reloadTier;

                float wavelength = wlStart - ( data.g_shotCharge * 100f );
                Debug.Log(wavelength);

                Color col = WavelengthToRGB(wavelength);
                Debug.Log(col);

                //data.p_tracerPart2.trailMaterial.SetColor(5, new Color(r, g, b, a));
                //data.p_tracerPart2.trailMaterial.SetColor(82, new Color(r, g, b, a));
                //data.p_tracerPart2.trailMaterial.SetColor(83, new Color(r, g, b, a));
                data.p_tracerPSR.trailMaterial.SetColor(152, col);    //Main color

                data.p_tracerHitL.color = col;
               
                //data.p_tracerFL.light.color = new Color(r, g, b);
                //data.p_tracerFL.light.intensity *= 0.5f;

                RaycastHit rh;
                float dist = data.p_maxRange;
                if (Util.CharacterRaycast(base.gameObject, aimRay , out rh, dist, mask, QueryTriggerInteraction.UseGlobal ) )
                {
                    dist = rh.distance;
                }

                dist = Mathf.Pow(dist, 0.6f);
                //Debug.Log(dist);
                data.p_tracer.beamDensity = 5f / (1f + dist);
                //Debug.Log(data.p_tracer.beamDensity);

                data.p_tracerHitL.intensity = data.p_hitLIBase * ( 1f + data.g_shotCharge * data.p_hitLIScale);
                data.p_tracerHitL.range = data.p_hitLRBase * ( 1f + data.g_shotCharge * data.p_hitLRScale );

                //Bullet stuff for later, when you feel like fixing this. It was fine before.
                BulletAttack bul = new BulletAttack();
                bul.owner = base.gameObject;
                bul.weapon = base.gameObject;
                bul.damage = shotTotalDamage * this.damageStat;
                bul.isCrit = crit;
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
                bul.stopperMask = mask;

                bul.Fire();


                if (consumeChargeAfterShot)
                {
                    data.g_shotCharge = 0f;
                }

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

        private Color WavelengthToRGB( float wl )
        {
            Color col = new Color();

            float rScale = 1f;
            float gScale = 1f;
            float bScale = 1f;

            float r = 0f;
            float g = 0f;
            float b = 0f;
            float a = 1f;

            if( wl >= 380f && wl < 440f )
            {
                r = -(wl - 440f) / (440f - 380f);
                g = 0f;
                b = 1f;
            }
            if( wl >= 440f && wl < 490f )
            {
                r = 0f;
                g = (wl - 440f) / (490f - 440f);
                b = 1f;
            }
            if( wl >= 490f && wl < 510f )
            {
                r = 0f;
                g = 1f;
                b = -(wl - 510f) / (510f - 490f);
            }
            if( wl >= 510f && wl < 580f )
            {
                r = (wl - 510f) / (580f - 510f);
                g = 1f;
                b = 0f;
            }
            if( wl >= 580f && wl < 645f )
            {
                r = 1f;
                g = -(wl - 645f) / (645f - 580f);
                b = 0f;
            }
            if( wl >= 645f && wl <= 780f )
            {
                r = 1f;
                g = 0f;
                b = 0f;
            }

            if( r > g && r > b )
            {
                r *= 25f;
            }
            else if( g > r && g > b )
            {
                g *= 25f;
            }
            else if( b > r && b > g )
            {
                b *= 25f;
            }
            else
            {
                r *= 25f;
                g *= 25f;
                b *= 25f;
            }

            col.r = r * rScale;
            col.g = g * gScale;
            col.b = b * bScale;
            col.a = a;

            return col;
        }

    }
}