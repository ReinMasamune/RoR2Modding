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
    public class HuntressPrimary1 : BaseState
    {
        //Consts
        private const string fireSoundString = "Play_huntress_m1_shoot";
        private const float baseDuration = 0.5f;

        //Internal vars
        private float duration;
        private ChildLocator childLoc;
        private Animator anim;
        private float timer;
        private bool firedArrow = false;
        private GameObject projPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/Arrow");

        public override void OnEnter()
        {
            base.OnEnter();

            Transform modelTrans = base.GetModelTransform();

            Util.PlayScaledSound(fireSoundString, base.gameObject, attackSpeedStat);

            duration = baseDuration / attackSpeedStat;

            PlayCrossfade("Gesture, Override", "FireSeekingShot", "FireSeekingShot.playbackRate", duration, duration * 0.2f / attackSpeedStat);
            PlayCrossfade("Gesture, Additive", "FireSeekingShot", "FireSeekingShot.playbackRate", duration, duration * 0.2f / attackSpeedStat);

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
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            timer += Time.fixedDeltaTime;
            if( anim.GetFloat("FireSeekingShot.fire") > 0.0f )
            {
                fireArrow();
            }
            if( timer > duration && isAuthority )
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            fireArrow();
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
            if( firedArrow || !NetworkServer.active )
            {
                return;
            }
            firedArrow = true;

            Ray aim = GetAimRay();

            FireProjectileInfo info = new FireProjectileInfo
            {
                projectilePrefab = projPrefab,
                position = aim.origin,
                rotation = Util.QuaternionSafeLookRotation(aim.direction),
                owner = gameObject,
                useSpeedOverride = false,
                useFuseOverride = false,
                damage = damageStat * 1.5f,
                force = 1f,
                crit = base.RollCrit(),
                damageColorIndex = DamageColorIndex.Default,
                procChainMask = new ProcChainMask()
            };

            ProjectileManager.instance.FireProjectile(info);
        }
    }
}
