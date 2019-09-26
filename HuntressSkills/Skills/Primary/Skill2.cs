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

        //Internal vars
        private float duration;
        private float arrowTimer;
        private float arrowTime;
        private float arrowFireEnd;
        private ChildLocator childLoc;
        private Animator anim;
        private float timer;
        private GameObject projPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/Arrow");

        public override void OnEnter()
        {
            base.OnEnter();

            Transform modelTrans = base.GetModelTransform();

            duration = baseDuration;
            arrowFireEnd = duration * spacingFrac;
            arrowTime = (duration - arrowFireEnd) / 5 / attackSpeedStat;
            

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
            fireArrow();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            timer += Time.fixedDeltaTime;
            if( timer <= arrowFireEnd )
            {
                arrowTimer += Time.fixedDeltaTime;
            }
            while( arrowTimer > arrowTime)
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
                rotation = Util.QuaternionSafeLookRotation(aim.direction),
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
        }
    }
}
