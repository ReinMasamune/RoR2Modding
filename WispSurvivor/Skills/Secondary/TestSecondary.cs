using RoR2;
using EntityStates;
using UnityEngine;
using System;
using RoR2.Orbs;
using UnityEngine.Networking;

namespace WispSurvivor.Skills.Secondary
{
    public class TestSecondary : BaseState
    {
        public static double chargeUsed = 15.0;

        public static float baseDuration = 1f;
        public static float scanDelay = 0.25f;
        public static float fireDelay = 0.5f;
        public static float damageRatio = 3.0f;
        public static float chargeScaler = 0.5f;
        public static float radius = 8f;
        public static float returnIdlePercent = 0.5f;

        private float duration;

        private uint skin = 0;

        private bool hasFired = false;

        private ChildLocator childLoc;
        private Animator anim;
        private Components.WispPassiveController passive;

        public override void OnEnter()
        {
            base.OnEnter();

            passive = gameObject.GetComponent<Components.WispPassiveController>();

            Transform modelTrans = base.GetModelTransform();
            //Sound

            duration = baseDuration / attackSpeedStat;

            //Animations

            if (modelTrans)
            {
                childLoc = modelTrans.GetComponent<ChildLocator>();
                anim = modelTrans.GetComponent<Animator>();
            }

            if (anim)
            {
                PlayCrossfade("Gesture", "ChargeBomb", "ChargeBomb.playbackRate" , duration * fireDelay, 0.2f);
            }

            if (characterBody)
            {
                characterBody.SetAimTimer(duration + 1f);
            }

            if( isAuthority )
            {
                skin = characterBody.skinIndex;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!hasFired && fixedAge > duration * fireDelay)
            {
                FireOrb();
            }
            if (fixedAge > duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            FireOrb();
            PlayCrossfade("Gesture", "Idle", 0.2f);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
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
            if( !isAuthority )
            {
                skin = reader.ReadUInt32();
            }
        }

        private void FireOrb()
        {
            if (hasFired) return;
            var chargeState = passive.UseCharge(chargeUsed, chargeScaler);
            PlayCrossfade("Gesture", "FireBomb", "ChargeBomb.playbackRate", duration * (1f - fireDelay), 0.2f);
            hasFired = true;
            if (!NetworkServer.active)
            {
                return;
            }

            Vector3 dir = GetAimRay().direction;
            dir.y = 0f;
            dir = Vector3.Normalize(dir);

            Orbs.SparkOrb nextOrb = new Orbs.SparkOrb();

            nextOrb.attacker = gameObject;
            nextOrb.crit = RollCrit();
            nextOrb.damage = damageStat * damageRatio * chargeState.chargeScaler;
            nextOrb.damageColor = DamageColorIndex.Default;
            nextOrb.direction = dir;
            nextOrb.origin = GetAimRay().origin;
            nextOrb.procCoef = 1.0f;
            nextOrb.isFirst = true;
            nextOrb.radius = radius;
            nextOrb.scale = 1.0f;
            nextOrb.speed = 50.0f;
            nextOrb.stepDist = 8.0f;
            nextOrb.stepHeight = 5.0f;
            nextOrb.maxFall = 25f;
            nextOrb.innerRadScale = 0.5f;
            nextOrb.stepsLeft = 1 + (int)Math.Truncate( chargeState.chargeConsumed / 10.0 );
            nextOrb.team = TeamComponent.GetObjectTeam(gameObject);
            nextOrb.skin = skin;

            OrbManager.instance.AddOrb(nextOrb);
        }
    }
}
