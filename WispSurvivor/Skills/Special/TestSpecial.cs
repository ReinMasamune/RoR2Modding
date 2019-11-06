using RoR2;
using EntityStates;
using UnityEngine;
using UnityEngine.Networking;

namespace WispSurvivor.Skills.Special
{
    public class TestSpecial : BaseState
    {
        public static double baseChargeUsedPerSec = 20.0;

        public static float baseMinChargeDuration = 1f;
        public static float baseMaxChargeDuration = 3f;
        public static float baseRecoveryDuration = 1f;
        public static float baseFireDelay = 0.25f;

        private double chargeUsedPerSec;
        private double chargeLevel = 0;
        private double idealChargeLevel = 0;

        private float minChargeDuration;
        private float maxChargeDuration;
        private float recoveryDuration;
        private float fireDelay;
        private float timer = 0f;


        private Vector3 startVel;

        private uint skin = 0;
        private uint soundID1;


        private bool fire = false;
        private bool firing = false;
        private bool rotated = false;

        private Animator anim;
        private ChildLocator childLoc;
        private Transform muzzle;
        private Components.WispPassiveController passive;
        private GameObject chargeEffect;



        public override void OnEnter()
        {
            base.OnEnter();

            passive = gameObject.GetComponent<Components.WispPassiveController>();

            minChargeDuration = baseMinChargeDuration / attackSpeedStat;
            maxChargeDuration = baseMaxChargeDuration / attackSpeedStat;
            recoveryDuration = baseRecoveryDuration / attackSpeedStat;
            chargeUsedPerSec = baseChargeUsedPerSec * attackSpeedStat;
            fireDelay = baseFireDelay / attackSpeedStat;

            if( isAuthority )
            {
                skin = characterBody.skinIndex;
            }

            //Start the sound (get ID maybe?)

            characterBody.SetAimTimer(maxChargeDuration + recoveryDuration + 2f);

            //PlayCrossfade("Gesture", "ChargeRHCannon", "ChargeRHCannon.playbackRate", minChargeDuration, 0.2f);
            PlayAnimation("Body", "SpecialTransform", "SpecialTransform.playbackRate", minChargeDuration);

            anim = GetModelAnimator();

            if( anim )
            {
                childLoc = anim.GetComponent<ChildLocator>();
                anim.SetBool("isCannon", true);
            }

            muzzle = GetModelTransform().Find("CannonPivot").Find("AncientWispArmature").Find("Head");

            characterBody.AddBuff(BuffIndex.Slow50);

            //Animation
            //Crosshair

            soundID1 = RoR2.Util.PlaySound("Play_greater_wisp_active_loop", gameObject);
        }

        public override void Update()
        {
            base.Update();
            //Crosshair stuff
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            timer += Time.fixedDeltaTime;
            //characterMotor.velocity.y = Mathf.Max(0f, characterMotor.velocity.y);

            chargeLevel += passive.DrainCharge(chargeUsedPerSec * Time.fixedDeltaTime);
            idealChargeLevel += chargeUsedPerSec * Time.fixedDeltaTime;

            if( timer > minChargeDuration * 0.8f && !rotated)
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

            if( !fire && ( ( inputBank && !inputBank.skill4.down ) || timer > maxChargeDuration ) )
            {
                fire = true;
            }

            if( isAuthority && fire && !firing && timer >= minChargeDuration )
            {
                outer.SetNextState(new Skills.Special.TestSpecialFire
                {
                    chargeTimer = (timer - minChargeDuration) / (maxChargeDuration-minChargeDuration),
                    chargeLevel = chargeLevel,
                    idealChargeLevel = idealChargeLevel,
                    recoveryTime = recoveryDuration,
                    fireDelay = fireDelay,
                    skin = skin,
                    effectInstance = chargeEffect
                });
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

        public override void OnSerialize(NetworkWriter writer)
        {
            if( isAuthority )
            {
                writer.Write(skin);
            }
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            skin = reader.ReadUInt32();
        }
    }
}
