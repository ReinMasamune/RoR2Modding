using EntityStates;
using UnityEngine;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace ReinHuntressSkills.Skills.Primary
{
    public class HuntressPrimary2 : BaseState
    {
        //Consts
        private const string fireSoundString = "Play_huntress_m1_shoot";
        private const float baseDuration = 1.0f;
        private const int baseArrowsToFire = 5;
        private const float spacingFrac = 0.5f;
        private const float baseSpread = 0.275f;
        private const float flatSpread = 0.005f;
        private const int maxArrowsPerFrame = 15;

        //Internal vars
        private float duration;
        private float arrowTimer;
        private float arrowTime;
        private float arrowFireEnd;
        private int frameArrowCounter;
        private int arrowCounter;
        private ChildLocator childLoc;
        private Animator anim;
        private float timer;
        private GameObject projPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/Arrow");

        public override void OnEnter()
        {
            base.OnEnter();

            Transform modelTrans = base.GetModelTransform();

            ProjectileTargetComponent projTarget = projPrefab.GetComponent<ProjectileTargetComponent>();
            if (!projTarget)
            {
                projTarget = projPrefab.AddComponent<ProjectileTargetComponent>();
            }


            ProjectileDirectionalTargetFinder projFinder = projPrefab.GetComponent<ProjectileDirectionalTargetFinder>();
            if (!projFinder)
            {
                projFinder = projPrefab.AddComponent<ProjectileDirectionalTargetFinder>();
            }
            projFinder.lookCone = 6.5f;
            projFinder.lookRange = 60.0f;
            projFinder.targetSearchInterval = 0.15f;
            projFinder.onlySearchIfNoTarget = false;
            projFinder.allowTargetLoss = true;
            projFinder.testLoS = true;
            projFinder.ignoreAir = false;

            ProjectileSteerTowardTarget projSteer = projPrefab.GetComponent<ProjectileSteerTowardTarget>();
            if (!projSteer)
            {
                projSteer = projPrefab.AddComponent<ProjectileSteerTowardTarget>();
            }
            projSteer.rotationSpeed = 45.0f;

            ProjectileSimple projSimp = projPrefab.GetComponent<ProjectileSimple>();
            if( !projSimp )
            {
                Debug.Log("Dafuq you done?");
            }
            projSimp.updateAfterFiring = true;



            duration = baseDuration;
            arrowFireEnd = duration * spacingFrac;
            arrowTime = (duration - arrowFireEnd) / baseArrowsToFire / attackSpeedStat;

            this.projPrefab.GetComponent<ProjectileController>().procCoefficient = 0.75f;
            

            PlayCrossfade("Gesture, Override", "FireSeekingShot", "FireSeekingShot.playbackRate", arrowTime, arrowTime * 0.2f / attackSpeedStat);
            PlayCrossfade("Gesture, Additive", "FireSeekingShot", "FireSeekingShot.playbackRate", arrowTime, arrowTime * 0.2f / attackSpeedStat);

            if( modelTrans )
            {
                childLoc = modelTrans.GetComponent<ChildLocator>();
                anim = modelTrans.GetComponent<Animator>();
            }
            if( characterBody )
            {
                characterBody.SetAimTimer(duration + 1.0f);
            }
            timer = 0.0f;
            arrowTimer = 0.0f;
            arrowCounter = 0;
            fireArrow();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            timer += Time.fixedDeltaTime;
            frameArrowCounter = 0;
            if( timer <= arrowFireEnd )
            {
                arrowTimer += Time.fixedDeltaTime;
            }
            while( arrowTimer > arrowTime && frameArrowCounter++ <= maxArrowsPerFrame )
            {
                fireArrow();
                arrowTimer -= arrowTime;
            }
            //if( anim.GetFloat("FireSeekingShot.fire") > 0.0f )
            //{
            //    fireArrow();
            //}
            if( timer > duration && isAuthority )
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            //fireArrow();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        //public override void OnSerialize(NetworkWriter writer)
        //{
        //    writer.Write(HurtBoxReference)
        //}

        //public override void OnDeserialize(NetworkReader reader)
        //{
        //    base.OnDeserialize(reader);
        //}

        private void fireArrow()
        {
            //if( firedArrow || !NetworkServer.active )
            //{
            //    return;
            //}
            //firedArrow = true;

            Ray aim = GetAimRay();

            PlayCrossfade("Gesture, Override", "FireSeekingShot", "FireSeekingShot.playbackRate", arrowTime, arrowTime * 0.2f / attackSpeedStat);
            PlayCrossfade("Gesture, Additive", "FireSeekingShot", "FireSeekingShot.playbackRate", arrowTime, arrowTime * 0.2f / attackSpeedStat);

            Util.PlayScaledSound(fireSoundString, base.gameObject, attackSpeedStat);

            FireProjectileInfo info = new FireProjectileInfo
            {
                projectilePrefab = projPrefab,
                position = aim.origin,
                rotation = Util.QuaternionSafeLookRotation(CalculateSpreadVector(aim.direction , characterBody.spreadBloomAngle , 0f , 2 )),
                owner = gameObject,
                useSpeedOverride = false,
                useFuseOverride = false,
                damage = damageStat * 0.6f,
                force = 1f,
                crit = base.RollCrit(),
                damageColorIndex = DamageColorIndex.Default,
                procChainMask = new ProcChainMask()
            };

            ProjectileManager.instance.FireProjectile(info);
            arrowCounter++;
            base.characterBody.AddSpreadBloom(baseSpread / Mathf.Sqrt(arrowCounter) + flatSpread);
        }

        private Vector3 CalculateSpreadVector(Vector3 forward , float maxSpread , float forcedDistrib , int samples )
        {
            //Chat.AddMessage(maxSpread.ToString());
            Vector3 result = forward;
            Vector3 temp = Vector3.left;
            Vector3.OrthoNormalize(ref forward, ref temp);

            float totalp = 0f;
            for( int i = 0; i<=samples; i++ )
            {
                totalp += UnityEngine.Random.Range(maxSpread * forcedDistrib, maxSpread);
            }
            totalp /= samples;

            float totalr = UnityEngine.Random.Range(Single.Epsilon, 360f);

            Quaternion q1 = Quaternion.AngleAxis(totalp, temp);
            result = q1 * result;

            q1 = Quaternion.AngleAxis(totalr, forward);
            result = q1 * result;

            return result;
        }
    }
}
