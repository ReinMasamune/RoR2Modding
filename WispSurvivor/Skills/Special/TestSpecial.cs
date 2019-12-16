/*
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace WispSurvivor.Skills.Special
{
    public class TestSpecial : BaseState
    {
        public static System.Double baseChargeUsedPerSec = 20.0;

        public static System.Single baseMinChargeDuration = 1f;
        public static System.Single baseMaxChargeDuration = 3f;
        public static System.Single baseRecoveryDuration = 1f;
        public static System.Single baseFireDelay = 0.25f;

        private System.Double chargeUsedPerSec;
        private System.Double chargeLevel = 0;
        private System.Double idealChargeLevel = 0;

        private System.Single minChargeDuration;
        private System.Single maxChargeDuration;
        private System.Single recoveryDuration;
        private System.Single fireDelay;
        private System.Single timer = 0f;


        private Vector3 startVel;

        private System.UInt32 skin = 0;
        private System.UInt32 soundID1;


        private System.Boolean fire = false;
        private System.Boolean firing = false;
        private System.Boolean rotated = false;

        private Animator anim;
        private ChildLocator childLoc;
        private Transform muzzle;
        private Components.WispPassiveController passive;
        private GameObject chargeEffect;



        public override void OnEnter()
        {
            base.OnEnter();

            this.passive = this.gameObject.GetComponent<Components.WispPassiveController>();

            this.minChargeDuration = baseMinChargeDuration / this.attackSpeedStat;
            this.maxChargeDuration = baseMaxChargeDuration / this.attackSpeedStat;
            this.recoveryDuration = baseRecoveryDuration / this.attackSpeedStat;
            this.chargeUsedPerSec = baseChargeUsedPerSec * this.attackSpeedStat;
            this.fireDelay = baseFireDelay / this.attackSpeedStat;

            if( this.isAuthority )
            {
                this.skin = this.characterBody.skinIndex;
            }

            //Start the sound (get ID maybe?)

            this.characterBody.SetAimTimer( this.maxChargeDuration + this.recoveryDuration + 2f );

            //PlayCrossfade("Gesture", "ChargeRHCannon", "ChargeRHCannon.playbackRate", minChargeDuration, 0.2f);
            this.PlayAnimation( "Body", "SpecialTransform", "SpecialTransform.playbackRate", this.minChargeDuration );

            this.anim = this.GetModelAnimator();

            if( this.anim )
            {
                this.childLoc = this.anim.GetComponent<ChildLocator>();
                this.anim.SetBool( "isCannon", true );
            }

            this.muzzle = this.GetModelTransform().Find( "CannonPivot" ).Find( "AncientWispArmature" ).Find( "Head" );

            this.characterBody.AddBuff( BuffIndex.Slow50 );

            //Animation
            //Crosshair

            this.soundID1 = RoR2.Util.PlaySound( "Play_greater_wisp_active_loop", this.gameObject );
        }

        public override void Update() => base.Update();//Crosshair stuff

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.timer += Time.fixedDeltaTime;
            //characterMotor.velocity.y = Mathf.Max(0f, characterMotor.velocity.y);

            //chargeLevel += passive.DrainCharge(chargeUsedPerSec * Time.fixedDeltaTime);
            this.idealChargeLevel += this.chargeUsedPerSec * Time.fixedDeltaTime;

            if( this.timer > this.minChargeDuration * 0.8f && !this.rotated )
            {
                //GetComponent<Components.WispAimAnimationController>().cannonMode = true;
                this.rotated = true;
                this.chargeEffect = UnityEngine.Object.Instantiate<GameObject>( Modules.WispEffectModule.specialCharge[this.skin], this.muzzle.TransformPoint( new Vector3( 0f, 0.1f, -0.5f ) ), this.muzzle.rotation );
                this.chargeEffect.transform.parent = this.muzzle;
                ScaleParticleSystemDuration scaler = this.chargeEffect.GetComponent<ScaleParticleSystemDuration>();
                ObjectScaleCurve scaleCurve = this.chargeEffect.GetComponent<ObjectScaleCurve>();
                if( scaler ) scaler.newDuration = this.maxChargeDuration - this.timer;
                if( scaleCurve ) scaleCurve.timeMax = this.maxChargeDuration - this.timer;
            }

            if( !this.fire && ((this.inputBank && !this.inputBank.skill4.down) || this.timer > this.maxChargeDuration) )
            {
                this.fire = true;
            }

            if( this.isAuthority && this.fire && !this.firing && this.timer >= this.minChargeDuration )
            {
                this.outer.SetNextState( new Skills.Special.TestSpecialFire
                {
                    chargeTimer = (this.timer - this.minChargeDuration) / (this.maxChargeDuration - this.minChargeDuration),
                    chargeLevel = chargeLevel,
                    idealChargeLevel = idealChargeLevel,
                    recoveryTime = recoveryDuration,
                    fireDelay = fireDelay,
                    skin = skin,
                    effectInstance = chargeEffect
                } );
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if( this.chargeEffect )
            {
                this.chargeEffect.transform.Find( "Sparks" ).gameObject.SetActive( false );


                this.chargeEffect.GetComponent<ObjectScaleCurve>().enabled = false;
                //chargeEffect.GetComponent<ScaleParticleSystemDuration>().enabled = false;
                //MonoBehaviour.Destroy(chargeEffect);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Death;

        public override void OnSerialize( NetworkWriter writer )
        {
            if( this.isAuthority )
            {
                writer.Write( this.skin );
            }
        }

        public override void OnDeserialize( NetworkReader reader ) => this.skin = reader.ReadUInt32();
    }
}
*/