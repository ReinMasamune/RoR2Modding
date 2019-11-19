using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

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

        public static System.Double baseChargeUsedPerSec = 10.0;

        public static System.Single baseMinChargeDuration = 1f;
        public static System.Single baseMaxChargeDuration = 3f;
        public static System.Single baseRecoveryDuration = 1f;
        public static System.Single baseFireDelay = 0.25f;
        public static System.Single baseDamageScaler = 17.5f;
        public static System.Single chargeScaler = 0.75f;
        public static System.Single timeScaler = 0.75f;

        private System.Double chargeUsedPerSec;
        private System.Double chargeLevel = 0;
        private System.Double idealChargeLevel = 0;

        private System.Single minChargeDuration;
        private System.Single maxChargeDuration;
        private System.Single fireDelay;
        private System.Single timer = 0f;
        private System.Single chargeTimer = 0f;

        private System.UInt32 skin = 0;

        private System.Boolean fire = false;
        private System.Boolean firing = false;
        private System.Boolean fired = false;
        private System.Boolean rotated = false;

        private Transform muzzle;
        private Components.WispPassiveController passive;
        private GameObject chargeEffect;



        public override void OnEnter()
        {
            base.OnEnter();
            this.passive = this.gameObject.GetComponent<Components.WispPassiveController>();
            this.skin = this.characterBody.skinIndex;

            this.minChargeDuration = baseMinChargeDuration / this.attackSpeedStat;
            this.maxChargeDuration = baseMaxChargeDuration / this.attackSpeedStat;
            this.chargeUsedPerSec = baseChargeUsedPerSec * this.attackSpeedStat;
            this.fireDelay = baseFireDelay / this.attackSpeedStat;

            this.characterBody.SetAimTimer( this.maxChargeDuration + 2f );
            this.PlayAnimation( "Body", "SpecialTransform", "SpecialTransform.playbackRate", this.minChargeDuration );
            RoR2.Util.PlaySound( "Play_greater_wisp_active_loop", this.gameObject );
            this.GetComponent<Components.WispAimAnimationController>().StartCannonMode( this.minChargeDuration * 0.8f, 5.0f );

            this.muzzle = this.GetModelTransform().Find( "CannonPivot" ).Find( "AncientWispArmature" ).Find( "Head" );

            //characterBody.AddBuff(BuffIndex.Slow50);



            //Crosshair
        }

        public override void Update() => base.Update();//Crosshair stuff

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.characterBody.isSprinting = false;
            switch( this.state )
            {
                case CannonSubState.Charging:
                    this.ChargingState( Time.fixedDeltaTime );
                    break;

                case CannonSubState.Charged:
                    this.ChargedState( Time.fixedDeltaTime );
                    break;

                case CannonSubState.Firing:
                    this.FiringState( Time.fixedDeltaTime );
                    break;

                case CannonSubState.Fired:
                    this.FiredState( Time.fixedDeltaTime );
                    break;
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

        private void ChargingState( System.Single t )
        {
            Components.WispPassiveController.ChargeState chargeState = this.passive.UseChargeDrain( this.chargeUsedPerSec, t );
            this.chargeLevel += chargeState.chargeConsumed;
            this.idealChargeLevel += this.chargeUsedPerSec * t;
            this.timer += t;

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

            if( this.timer > this.minChargeDuration )
            {
                this.state = CannonSubState.Charged;
                this.timer = 0f;
            }
        }

        private void ChargedState( System.Single t )
        {
            this.chargeTimer += t;
            Components.WispPassiveController.ChargeState chargeState = this.passive.UseChargeDrain( this.chargeUsedPerSec, t );
            this.chargeLevel += chargeState.chargeConsumed;
            this.idealChargeLevel += this.chargeUsedPerSec * t;

            this.timer += t;
            if( this.timer + this.minChargeDuration > this.maxChargeDuration || (this.inputBank && !this.inputBank.skill4.down) )
            {
                this.state = CannonSubState.Firing;
                this.timer = 0f;
            }
        }

        private void FiringState( System.Single t )
        {
            this.timer += t;
            System.Single scale = 1f - (this.timer / this.fireDelay);
            this.chargeEffect.transform.localScale = new Vector3( scale, scale, scale );

            if( this.timer > this.fireDelay )
            {
                if( this.chargeEffect ) MonoBehaviour.Destroy( this.chargeEffect );
                this.Fire();
                this.state = CannonSubState.Fired;
                this.timer = 0f;
            }
        }

        private void FiredState( System.Single t )
        {
            if( this.isAuthority )
            {
                this.outer.SetNextState( new CremationRecovery() );
            }
        }

        private void Fire()
        {
            RoR2.Util.PlaySound( "Stop_greater_wisp_active_loop", this.gameObject );
            RoR2.Util.PlaySound( "Play_item_use_BFG_fire", this.gameObject );

            Ray r = this.GetAimRay();

            System.Single chargeMult = Components.WispPassiveController.GetDrainScaler( this.chargeLevel, this.idealChargeLevel, chargeScaler );

            System.Single timeMult = this.chargeTimer / (this.maxChargeDuration - this.minChargeDuration) - 1f;
            timeMult *= timeScaler;
            timeMult += 1f;

            FireProjectileInfo proj = new FireProjectileInfo
            {
                projectilePrefab = Modules.WispProjectileModule.specialProjPrefabs[this.skin],
                position = r.origin,
                rotation = RoR2.Util.QuaternionSafeLookRotation(r.direction),
                owner = gameObject,
                damage = this.damageStat * baseDamageScaler * chargeMult * timeMult,
                force = 50f,
                crit = this.RollCrit(),
                damageColorIndex = DamageColorIndex.Default,
            };


            this.fired = true;

            if( !this.isAuthority ) return;

            ProjectileManager.instance.FireProjectile( proj );
        }
    }
}
