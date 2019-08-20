using RoR2;
using UnityEngine;
using ReinSniperRework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering;

namespace EntityStates.ReinSniperRework.SniperWeapon
{
    public class SniperPrimary : BaseState
    {
        ReinDataLibrary data;

        private float timer = 0f;
        private bool consumeChargeAfterShot;
        private Vector3 bounceDir1;
        private Vector3 bounceDir2;
        private Vector3 bouncePos;
        private Vector3 bounceNorm;

        public override void OnEnter()
        {
            base.OnEnter();
            data = base.GetComponent<ReinDataLibrary>();
            if( data )
            {
                timer = 0f;
                SkillLocator skills = base.skillLocator;
                Ray aimRay = base.GetAimRay();
                base.StartAimMode(aimRay, 2f, false);
                LayerMask mask = LayerIndex.world.mask | LayerIndex.entityPrecise.mask;

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

                bool bounce = false;

                switch (data.g_reloadTier)
                {
                    case 0:
                        reloadMod *= data.p_reloadT0Mod;
                        break;
                    case 1:
                        reloadMod *= data.p_reloadT1Mod;
                        mask -= LayerIndex.entityPrecise.mask;
                        break;
                    case 2:
                        reloadMod *= data.p_reloadT2Mod;
                        mask -= LayerIndex.entityPrecise.mask;
                        bounce = true;
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
                        //chargeMod *= data.p_chargeT0Mod;
                        shotCoef = data.p_shotCoef;
                        shotRad = data.p_t0ShotRadius;
                        chargeMod *= 1f + ( data.p_chargeT0Mod + data.p_chargeT0Scale * shotCharge );
                        break;
                    case 1:
                        //chargeMod *= data.p_chargeT1Mod;
                        shotCoef = data.p_chargeT1Coef;
                        shotRad = data.p_t1ShotRadius;
                        chargeMod *= 1f + (data.p_chargeT1Mod + data.p_chargeT1Scale * shotCharge );
                        break;
                    case 2:
                        //chargeMod *= data.p_chargeT2Mod;
                        shotCoef = data.p_chargeT2Coef;
                        shotRad = data.p_t2ShotRadius;
                        chargeMod *= 1f + (data.p_chargeT2Mod + data.p_chargeT2Scale * shotCharge);
                        break;
                    default:
                        //chargeMod *= 1f;
                        shotCoef = data.p_shotCoef;
                        shotRad = data.p_ntShotRadius;
                        Debug.Log("Charge tier is invalid");
                        break;
                }

                if (data.g_zoomed)
                {
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
                Color col = WavelengthToRGB(wavelength);

                data.p_tracerHitL.color = col;

                col.r *= 10f;
                col.g *= 10f;
                col.b *= 10f;

                foreach( Material mat in data.p_matsToEdit )
                {
                    mat.SetColor("_TintColor", col);
                    //mat.SetColor("_EmissionColor", col);
                }
                //data.p_tracerPSR.trailMaterial.SetColor("_TintColor", col);

                RaycastHit rh;
                float dist = data.p_maxRange;
                if (Util.CharacterRaycast(base.gameObject, aimRay , out rh, dist, mask, QueryTriggerInteraction.UseGlobal ) )
                {
                    dist = rh.distance;
                    bounceDir1 = aimRay.direction;
                    bounceNorm = rh.normal;
                    bouncePos = rh.point;
                    bounceDir2 = Vector3.Reflect(bounceDir1, bounceNorm);
                }
                else
                {
                    bouncePos = aimRay.origin + aimRay.direction * data.p_maxRange / 3f;
                    bounce = false;
                }
                //dist *= 2f;
                //dist = Mathf.Pow(dist, 0.6f);
                //data.p_tracer.beamDensity = 10f / (1f+dist);
                data.p_tracer.speed = 1500f;
                //data.p_tracer.beamDensity = 10f;

                data.p_tracerHitL.intensity = data.p_hitLIBase * ( 1f + data.g_shotCharge * data.p_hitLIScale);
                data.p_tracerHitL.range = data.p_hitLRBase * ( 1f + data.g_shotCharge * data.p_hitLRScale );

                BulletAttack bul = new BulletAttack();
                bul.owner = base.gameObject;
                bul.weapon = base.gameObject;
                bul.damage = shotTotalDamage * this.damageStat;
                bul.isCrit = crit;
                bul.force = shotTotalDamage * data.p_shotForce / data.p_shotDamage;
                bul.procCoefficient = shotCoef;
                bul.sniper = true;
                bul.falloffModel = BulletAttack.FalloffModel.None;
                //bul.tracerEffectPrefab = data.p_tracerEffectPrefab;
                bul.hitEffectPrefab = data.p_hitEffectPrefab;
                bul.origin = aimRay.origin;
                bul.aimVector = aimRay.direction;
                bul.minSpread = 0f;
                bul.maxSpread = 0f;
                bul.bulletCount = 1;
                //bul.muzzleName = data.p_muzzleName;
                bul.radius = shotRad;
                bul.maxDistance = data.p_maxRange;
                bul.smartCollision = data.p_shotSmartCollision;
                bul.stopperMask = mask;
                //bul.damageType = DamageType.PoisonOnHit;

                bul.Fire();

                EffectData effectData1 = new EffectData
                {
                    origin = bouncePos,
                    start = aimRay.origin
                };
                data.p_tracerEffectPrefab.SetActive(true);
                EffectManager.instance.SpawnEffect(data.p_tracerEffectPrefab, effectData1, false);
                data.p_tracerEffectPrefab.SetActive(false);

                RaycastHit hitInfo;
                float radius = shotRad;

                while( bounce )
                {
                    float ang = Vector3.Dot(bounceDir1, bounceDir2);
                    float chance = (ang + 1f) * 50f;
                    bounce = Util.CheckRoll(chance, base.characterBody.master);
                    if( bounce )
                    {
                        radius *= 1.1f;
                        BulletAttack bul2 = new BulletAttack();
                        bul2.owner = base.gameObject;
                        bul2.weapon = base.gameObject;
                        bul2.damage = shotTotalDamage * this.damageStat;
                        bul2.isCrit = crit;
                        bul2.force = shotTotalDamage * data.p_shotForce / data.p_shotDamage;
                        bul2.procCoefficient = shotCoef;
                        bul2.sniper = true;
                        bul2.falloffModel = BulletAttack.FalloffModel.None;
                        //bul2.tracerEffectPrefab = data.p_tracerEffectPrefab;
                        bul2.hitEffectPrefab = data.p_hitEffectPrefab;
                        bul2.origin = bouncePos;
                        bul2.aimVector = bounceDir2;
                        bul2.minSpread = 0f;
                        bul2.maxSpread = 0f;
                        bul2.bulletCount = 1;
                        //bul2.muzzleName = data.p_muzzleName;
                        bul2.radius = radius;
                        bul2.maxDistance = data.p_maxRange;
                        bul2.smartCollision = data.p_shotSmartCollision;
                        bul2.stopperMask = mask;
                        bul2.Fire();

                        float dist2 = data.p_maxRange;
                        Vector3 effOrigin = bouncePos;
                        Vector3 effStartPos = bouncePos + bounceDir2 * data.p_maxRange / 3f;

                        if( Physics.Raycast( bouncePos , bounceDir2 , out hitInfo, data.p_maxRange, mask ) )
                        {
                            dist2 = hitInfo.distance;
                            bouncePos = hitInfo.point;
                            bounceDir1 = bounceDir2;
                            bounceNorm = hitInfo.normal;
                            bounceDir2 = Vector3.Reflect(bounceDir1, bounceNorm);
                            effStartPos = bouncePos;
                        }
                        else
                        {
                            bounce = false;
                        }
                        //dist2 *= 2f;
                        //dist2 = Mathf.Pow(dist2, 0.6f);
                        //data.p_tracer.beamDensity = 10f /(1f + dist2);

                        EffectData effectData = new EffectData
                        {
                            origin = effOrigin,
                            start = effStartPos
                        };
                        data.p_tracerEffectPrefab.SetActive(true);
                        EffectManager.instance.SpawnEffect(data.p_tracerEffectPrefab, effectData, false);
                        data.p_tracerEffectPrefab.SetActive(false);
                    }
                }

                if (consumeChargeAfterShot)
                {
                    data.g_shotCharge = 0f;
                }
                Util.PlayScaledSound(data.p_attackSound, base.gameObject, 0.65f);
                base.AddRecoil(-1f * data.p_recoilAmplitude, -2f * data.p_recoilAmplitude, -0.5f * data.p_recoilAmplitude, 0.5f * data.p_recoilAmplitude);
                base.characterMotor.ApplyForce(base.inputBank.aimDirection * -1f * data.p_recoilForceMult);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            timer += Time.fixedDeltaTime * characterBody.attackSpeed;
            base.characterBody.isSprinting = false;
            if (timer >= data.p_reloadStartDelay && base.isAuthority)
            {
                this.outer.SetNextState(new SniperReload());
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if( timer >= ( data.p_reloadStartDelay ) )
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

            col.r = r * rScale;
            col.g = g * gScale;
            col.b = b * bScale;
            col.a = a;

            return col;
        }

    }
}