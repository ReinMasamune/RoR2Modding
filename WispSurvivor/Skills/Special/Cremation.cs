using RoR2;
using RoR2.Projectile;
using EntityStates;
using UnityEngine;
using UnityEngine.Networking;

namespace WispSurvivor.Skills.Special
{
    public class Cremation : BaseState
    {
        public enum CannonSubState
        {
            Charging = 0,
            Charged = 1,
            Firing = 2,
            Fired = 3
        }

        private CannonSubState state = CannonSubState.Charging;

        public static double baseChargeUsedPerSec = 10.0;

        public static float baseMinChargeDuration = 1f;
        public static float baseMaxChargeDuration = 3f;
        public static float baseRecoveryDuration = 1f;
        public static float baseFireDelay = 0.25f;
        public static float baseDamageScaler = 17.5f;
        public static float chargeScaler = 0.5f;
        public static float timeScaler = 0.75f;

        private double chargeUsedPerSec;
        private double chargeLevel = 0;
        private double idealChargeLevel = 0;

        private float minChargeDuration;
        private float maxChargeDuration;
        private float fireDelay;
        private float timer = 0f;
        private float chargeTimer = 0f;

        private uint skin = 0;

        private bool fire = false;
        private bool firing = false;
        private bool fired = false;
        private bool rotated = false;

        private Transform muzzle;
        private Components.WispPassiveController passive;
        private GameObject chargeEffect;



        public override void OnEnter()
        {
            base.OnEnter();
            passive = gameObject.GetComponent<Components.WispPassiveController>();
            skin = characterBody.skinIndex;

            minChargeDuration = baseMinChargeDuration / attackSpeedStat;
            maxChargeDuration = baseMaxChargeDuration / attackSpeedStat;
            chargeUsedPerSec = baseChargeUsedPerSec * attackSpeedStat;
            fireDelay = baseFireDelay / attackSpeedStat;

            characterBody.SetAimTimer(maxChargeDuration + 2f);
            PlayAnimation("Body", "SpecialTransform", "SpecialTransform.playbackRate", minChargeDuration);
            RoR2.Util.PlaySound("Play_greater_wisp_active_loop", gameObject);

            muzzle = GetModelTransform().Find("CannonPivot").Find("AncientWispArmature").Find("Head");

            //characterBody.AddBuff(BuffIndex.Slow50);



            //Crosshair
        }

        public override void Update()
        {
            base.Update();
            //Crosshair stuff
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            characterBody.isSprinting = false;
            switch( state )
            {
                case CannonSubState.Charging:
                    ChargingState(Time.fixedDeltaTime);
                    break;

                case CannonSubState.Charged:
                    ChargedState(Time.fixedDeltaTime);
                    break;

                case CannonSubState.Firing:
                    FiringState(Time.fixedDeltaTime);
                    break;

                case CannonSubState.Fired:
                    FiredState(Time.fixedDeltaTime);
                    break;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if( chargeEffect )
            {
                chargeEffect.transform.Find("Sparks").gameObject.SetActive(false);


                chargeEffect.GetComponent<ObjectScaleCurve>().enabled = false;
                //chargeEffect.GetComponent<ScaleParticleSystemDuration>().enabled = false;
                //MonoBehaviour.Destroy(chargeEffect);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        private void ChargingState(float t)
        {
            var chargeState = passive.UseChargeDrain(chargeUsedPerSec, t);
            chargeLevel += chargeState.chargeConsumed;
            idealChargeLevel += chargeUsedPerSec * t;
            timer += t;

            if( timer > minChargeDuration * 0.8f && !rotated )
            {
                GetComponent<Components.WispAimAnimationController>().cannonMode = true;
                rotated = true;
                chargeEffect = UnityEngine.Object.Instantiate<GameObject>(Modules.WispEffectModule.specialCharge[skin], muzzle.TransformPoint(new Vector3(0f, 0.1f, -0.5f)), muzzle.rotation);
                chargeEffect.transform.parent = muzzle;
                ScaleParticleSystemDuration scaler = chargeEffect.GetComponent<ScaleParticleSystemDuration>();
                ObjectScaleCurve scaleCurve = chargeEffect.GetComponent<ObjectScaleCurve>();
                if (scaler) scaler.newDuration = maxChargeDuration - timer;
                if (scaleCurve) scaleCurve.timeMax = maxChargeDuration - timer;
            }

            if( timer > minChargeDuration )
            {
                state = CannonSubState.Charged;
                timer = 0f;
            }
        }

        private void ChargedState( float t )
        {
            chargeTimer += t;
            var chargeState = passive.UseChargeDrain(chargeUsedPerSec, t);
            chargeLevel += chargeState.chargeConsumed;
            idealChargeLevel += chargeUsedPerSec * t;

            timer += t;
            if( timer + minChargeDuration > maxChargeDuration || ( inputBank && !inputBank.skill4.down ))
            {
                state = CannonSubState.Firing;
                timer = 0f;
            }
        }

        private void FiringState( float t )
        {
            timer += t;
            float scale = 1f - (timer / fireDelay);
            chargeEffect.transform.localScale = new Vector3(scale, scale, scale);

            if ( timer > fireDelay )
            {
                if (chargeEffect) MonoBehaviour.Destroy(chargeEffect);
                Fire();
                state = CannonSubState.Fired;
                timer = 0f;
            }
        }

        private void FiredState( float t )
        {
            if( isAuthority )
            {
                outer.SetNextState(new CremationRecovery());
            }
        }

        private void Fire()
        {
            RoR2.Util.PlaySound("Stop_greater_wisp_active_loop", gameObject);
            RoR2.Util.PlaySound("Play_item_use_BFG_fire", gameObject);

            Ray r = GetAimRay();

            float chargeMult = Components.WispPassiveController.GetDrainScaler(chargeLevel, idealChargeLevel, chargeScaler );

            float timeMult = chargeTimer / (maxChargeDuration - minChargeDuration) - 1f;
            timeMult *= timeScaler;
            timeMult += 1f;

            FireProjectileInfo proj = new FireProjectileInfo
            {
                projectilePrefab = Modules.WispProjectileModule.specialProjPrefabs[skin],
                position = r.origin,
                rotation = RoR2.Util.QuaternionSafeLookRotation(r.direction),
                owner = gameObject,
                damage = damageStat * baseDamageScaler * chargeMult * timeMult,
                force = 50f,
                crit = RollCrit(),
                damageColorIndex = DamageColorIndex.Default,
            };


            fired = true;

            if (!isAuthority) return;

            ProjectileManager.instance.FireProjectile(proj);
        }
    }
}
