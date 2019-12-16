/*
using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace WispSurvivor.Skills.Special
{
    public class TestSpecialFire : BaseState
    {
        public System.Single chargeTimer;
        public System.Double chargeLevel;
        public System.Double idealChargeLevel;
        public System.Single recoveryTime;
        public System.Single fireDelay;
        public static System.Single baseDamageScaler = 15.0f;
        public static System.Single chargeScaler = 10.0f;
        public GameObject effectInstance;

        public System.UInt32 skin;


        private System.Boolean fired = false;
        private System.Boolean rotated = false;

        private System.Single timer = 0f;

        private Components.WispPassiveController passive;

        public override void OnEnter()
        {
            base.OnEnter();

            this.passive = this.gameObject.GetComponent<Components.WispPassiveController>();

            //PlayCrossfade("Gesture", "Throw1", "Throw.playbackRate", fireDelay / 0.25f, 0.2f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.timer += Time.fixedDeltaTime;

            if( !this.fired )
            {
                //characterMotor.velocity.y = Mathf.Max(0f, characterMotor.velocity.y);
                System.Single scale = 1f - (this.timer / this.fireDelay);
                this.effectInstance.transform.localScale = new Vector3( scale, scale, scale );
            }

            if( this.timer > this.fireDelay && !this.fired )
            {
                this.PlayAnimation( "Body", "SpecialFire", "SpecialFire.playbackRate", this.recoveryTime );
                RoR2.Util.PlaySound( "Stop_greater_wisp_active_loop", this.gameObject );
                RoR2.Util.PlaySound( "Play_item_use_BFG_fire", this.gameObject );
                if( this.effectInstance ) MonoBehaviour.Destroy( this.effectInstance );
                this.Fire();
            }

            if( this.timer > this.fireDelay + this.recoveryTime * 0.5f && !this.rotated )
            {
                //GetComponent<Components.WispAimAnimationController>().cannonMode = false;
                this.rotated = true;
            }

            if( this.timer > this.recoveryTime + this.fireDelay )
            {
                //PlayCrossfade("Gesture", "Idle", 0.2f);
                //PlayAnimation("Body", "Idle", "", 0.2f);
                RoR2.Util.PlaySound( "Stop_item_use_BFG_loop", this.gameObject );

                if( this.isAuthority )
                {
                    this.outer.SetNextStateToMain();
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if( !this.fired )
            {
                this.Fire();
            }
            this.GetModelAnimator().SetBool( "isCannon", false );
        }

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Death;

        private void Fire()
        {
            if( NetworkServer.active ) this.characterBody.RemoveBuff( BuffIndex.Slow50 );
            System.Double charge = this.passive.ReadCharge();

            Ray r = this.GetAimRay();

            System.Single damageMult1 = 0.25f + 0.75f * this.chargeTimer;
            System.Single damageMult2 = 0.25f + 0.75f * (this.idealChargeLevel > 0 ? (System.Single)(this.chargeLevel / this.idealChargeLevel) : 1f);
            System.Single damageMult3 = 1.0f + 0.5f * (System.Single)((charge + this.chargeLevel - 100.0) / 100.0);

            FireProjectileInfo proj = new FireProjectileInfo
            {
                projectilePrefab = Modules.WispProjectileModule.specialProjPrefabs[this.skin],
                position = r.origin,
                rotation = RoR2.Util.QuaternionSafeLookRotation(r.direction),
                owner = gameObject,
                damage = this.damageStat * baseDamageScaler * damageMult1 * damageMult2 * damageMult3,
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
*/