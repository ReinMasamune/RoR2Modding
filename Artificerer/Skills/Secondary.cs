using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using ReinArtificerer;

namespace EntityStates.ReinArtificerer.Artificer.Weapon
{
    public class Secondary : BaseState
    {
        ReinDataLibrary data;

        private float stopwatch;
        private float windDownDuration;
        private float chargeDuration;
        private bool hasFiredBomb;
        private string muzzleString;
        private Transform muzzleTransform;
        private Animator animator;
        private ChildLocator childLocator;
        private GameObject chargeEffectInstance;
        private GameObject defaultCrosshairPrefab;
        private GameObject tempMuzzleFlash;
        private GameObject tempChargeEffect;

        public override void OnEnter()
        {
            base.OnEnter();

            data = base.GetComponent<ReinDataLibrary>();

            int elem = Mathf.RoundToInt(Random.Range(1f, 3f));

            tempMuzzleFlash = data.s_l_muzzle;
            tempChargeEffect = data.s_l_charge;

            data.s_proxBeams.enabled = false;

            switch( elem )
            {
                case 1:
                    Chat.AddMessage("Fire");
                    tempMuzzleFlash = data.s_f_muzzle;
                    data.s_control.ghostPrefab = data.s_f_projectile;
                    tempChargeEffect = data.s_f_charge;
                    break;

                case 2:
                    Chat.AddMessage("Ice");
                    tempMuzzleFlash = data.s_i_muzzle;
                    data.s_control.ghostPrefab = data.s_i_projectile;
                    tempChargeEffect = data.s_i_charge;
                    break;

                case 3:
                    Chat.AddMessage("Lightning");
                    tempMuzzleFlash = data.s_l_muzzle;
                    data.s_control.ghostPrefab = data.s_l_projectile;
                    tempChargeEffect = data.s_l_charge;
                    break;

                default:
                    Debug.Log("WTf");
                    break;
            }
            if( elem == 1 )
            {

            }
            if( elem == 2 )
            {

            }
            if( elem == 3 )
            {
                data.s_proxBeams.enabled = true;
            }




            stopwatch = 0f;

            windDownDuration = data.s_winddownDuration / attackSpeedStat;

            chargeDuration = data.s_chargeDuration / attackSpeedStat;

            Util.PlayScaledSound(data.s_chargeSound, base.gameObject, attackSpeedStat);
            base.characterBody.SetAimTimer(chargeDuration + windDownDuration + 2f);
            muzzleString = "MuzzleBetween";
            animator = base.GetModelAnimator();
            if (animator)
            {
                childLocator = animator.GetComponent<ChildLocator>();
            }
            if (childLocator)
            {
                muzzleTransform = childLocator.FindChild(muzzleString);
                if (muzzleTransform && data.s_l_charge)
                {
                    chargeEffectInstance = UnityEngine.Object.Instantiate<GameObject>(tempChargeEffect, muzzleTransform.position, muzzleTransform.rotation);
                    chargeEffectInstance.transform.parent = muzzleTransform;
                    ScaleParticleSystemDuration component2 = chargeEffectInstance.GetComponent<ScaleParticleSystemDuration>();
                    ObjectScaleCurve component3 = chargeEffectInstance.GetComponent<ObjectScaleCurve>();
                    if (component2)
                    {
                        component2.newDuration = chargeDuration;
                    }
                    if (component3)
                    {
                        component3.timeMax = chargeDuration;
                    }
                }
            }
            base.PlayAnimation("Gesture, Additive", "ChargeNovaBomb", "ChargeNovaBomb.playbackRate", chargeDuration);
            defaultCrosshairPrefab = base.characterBody.crosshairPrefab;
            if (data.s_crosshairOverridePrefab)
            {
                base.characterBody.crosshairPrefab = data.s_crosshairOverridePrefab;
            }
        }

        public override void Update()
        {
            base.Update();
            base.characterBody.SetSpreadBloom(Util.Remap(GetChargeProgress(), 0f, 1f, data.s_minRadius, data.s_maxRadius), true);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (!hasFiredBomb && (stopwatch >= chargeDuration || !base.inputBank.skill2.down) && !hasFiredBomb && stopwatch >= 0.5f)
            {
                FireNovaBomb();
            }
            if (stopwatch >= windDownDuration && hasFiredBomb && base.isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            if (chargeEffectInstance)
            {
                EntityState.Destroy(chargeEffectInstance);
            }
            base.characterBody.crosshairPrefab = defaultCrosshairPrefab;
            base.OnExit();
        }

        private float GetChargeProgress()
        {
            return Mathf.Clamp01(stopwatch / chargeDuration);
        }

        private void FireNovaBomb()
        {
            hasFiredBomb = true;
            base.PlayAnimation("Gesture, Additive", "FireNovaBomb", "FireNovaBomb.playbackRate", windDownDuration);
            Ray aimRay = base.GetAimRay();
            if (chargeEffectInstance)
            {
                EntityState.Destroy(chargeEffectInstance);
            }
            if (data.s_l_muzzle)
            {
                EffectManager.instance.SimpleMuzzleFlash(data.s_l_muzzle, base.gameObject, "MuzzleLeft", false);
                EffectManager.instance.SimpleMuzzleFlash(data.s_l_muzzle, base.gameObject, "MuzzleRight", false);
            }
            if (base.isAuthority)
            {
                float chargeProgress = GetChargeProgress();
                if (data.s_l_projectile != null)
                {
                    float num = Util.Remap(chargeProgress, 0f, 1f, data.s_minDamageCoef, data.s_maxDamageCoef);
                    float num2 = chargeProgress * data.s_l_hitForce;
                    Ray aimRay2 = base.GetAimRay();
                    Vector3 direction = aimRay2.direction;
                    Vector3 origin = aimRay2.origin;
                    ProjectileManager.instance.FireProjectile(data.s_projectile, origin, Util.QuaternionSafeLookRotation(direction), base.gameObject, damageStat * num, num2, Util.CheckRoll(critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
                }
                if (base.characterMotor)
                {
                    base.characterMotor.ApplyForce(aimRay.direction * (-data.s_l_selfForce * chargeProgress), false, false);
                }
            }
            base.characterBody.crosshairPrefab = defaultCrosshairPrefab;
            stopwatch = 0f;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
