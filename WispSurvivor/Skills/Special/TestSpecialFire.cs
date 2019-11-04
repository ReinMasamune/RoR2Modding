using RoR2;
using EntityStates;
using UnityEngine;
using RoR2.Projectile;
using UnityEngine.Networking;

namespace WispSurvivor.Skills.Special
{
    public class TestSpecialFire : BaseState
    {
        public float chargeTimer;
        public double chargeLevel;
        public double idealChargeLevel;
        public float recoveryTime;
        public float fireDelay;
        public static float baseDamageScaler = 15.0f;
        public static float chargeScaler = 10.0f;
        public GameObject effectInstance;

        public uint skin;

        public uint soundID1;

        private bool fired = false;
        private bool rotated = false;

        private float timer = 0f;

        private Components.WispPassiveController passive;

        public override void OnEnter()
        {
            base.OnEnter();

            passive = gameObject.GetComponent<Components.WispPassiveController>();

            //PlayCrossfade("Gesture", "Throw1", "Throw.playbackRate", fireDelay / 0.25f, 0.2f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            timer += Time.fixedDeltaTime;

            if( !fired )
            {
                //characterMotor.velocity.y = Mathf.Max(0f, characterMotor.velocity.y);
                float scale = 1f - ( timer / fireDelay );
                effectInstance.transform.localScale = new Vector3(scale, scale, scale);
            }

            if( timer > fireDelay && !fired)
            {
                PlayAnimation("Body", "SpecialFire", "SpecialFire.playbackRate", recoveryTime);
                RoR2.Util.PlaySound("Stop_greater_wisp_active_loop", gameObject);
                RoR2.Util.PlaySound("Play_item_use_BFG_fire", gameObject);
                MonoBehaviour.Destroy(effectInstance);
                Fire();
            }

            if( timer > fireDelay + recoveryTime * 0.5f && !rotated)
            {
                GetComponent<Components.WispAimAnimationController>().cannonMode = false;
                rotated = true;
            }

            if( timer > recoveryTime + fireDelay )
            {
                //PlayCrossfade("Gesture", "Idle", 0.2f);
                //PlayAnimation("Body", "Idle", "", 0.2f);
                RoR2.Util.PlaySound("Stop_item_use_BFG_loop", gameObject);

                if( isAuthority )
                {
                    outer.SetNextStateToMain();
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if( !fired )
            {
                Fire();
            }
            GetModelAnimator().SetBool("isCannon", false);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        private void Fire()
        {
            if (NetworkServer.active) characterBody.RemoveBuff(BuffIndex.Slow50);
            double charge = passive.ReadCharge();

            Ray r = GetAimRay();

            float damageMult1 = 0.25f + 0.75f * chargeTimer;
            float damageMult2 = 0.25f + 0.75f * (idealChargeLevel > 0 ? (float)(chargeLevel / idealChargeLevel) : 1f);
            float damageMult3 = 1.0f + 0.5f * (float)((charge + chargeLevel - 100.0) / 100.0);

            FireProjectileInfo proj = new FireProjectileInfo
            {
                projectilePrefab = Modules.WispProjectileModule.specialProjPrefabs[skin],
                position = r.origin,
                rotation = RoR2.Util.QuaternionSafeLookRotation(r.direction),
                owner = gameObject,
                damage = damageStat * baseDamageScaler * damageMult1 * damageMult2 * damageMult3,
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
