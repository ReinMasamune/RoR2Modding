using RoR2;
using EntityStates;
using UnityEngine;
using RoR2.Orbs;
using UnityEngine.Networking;
using WispSurvivor.Orbs;
using System.Collections.Generic;

namespace WispSurvivor.Skills.Utility
{
    public class FireGaze : BaseState
    {
        public float flareTime;
        public float blazeOrbRadius;
        public float blazeOrbDuration;
        public float blazeOrbTickFreq;
        public float blazeOrbMinDur;
        public float blazeOrbDurationPerStack;
        public float igniteOrbDuration;
        public float igniteOrbProcCoef;
        public float igniteOrbTickDamage;
        public float igniteOrbTickFreq;
        public float igniteOrbDebuffTimeMult;
        public float igniteOrbStacksPerSecOnDeath;
        public float igniteOrbBaseStacksOnDeath;
        public float igniteOrbExpireStacksMult;

        public bool crit;

        public uint skin;

        public Vector3 orbOrigin;
        public Vector3 orbNormal;

        public DamageColorIndex igniteOrbDamageColor;

        private bool hasFired = false;
        
        //Orb params and stuff
        public override void OnEnter()
        {
            base.OnEnter();

            //Bigger face flare
            //Sync the position with server

        }

        public override void Update()
        {
            base.Update();
            //Scale flare down based on time / flareTime
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            FireOrb();
            if( fixedAge > flareTime && isAuthority )
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(skin);
            writer.Write(orbOrigin);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            skin = reader.ReadUInt32();
            orbOrigin = reader.ReadVector3();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        private void FireOrb()
        {
            if (hasFired) return;
            hasFired = true;
            if (!NetworkServer.active) return;

            BlazeOrb blaze = new BlazeOrb();


            //Unorganized shit
            blaze.origin = orbOrigin;
            blaze.normal = orbNormal;
            blaze.skin = skin;
            blaze.team = TeamComponent.GetObjectTeam(gameObject);
            blaze.attacker = gameObject;
            blaze.crit = crit;
            blaze.children = new List<IgnitionOrb>();

            blaze.blazeTime = blazeOrbDuration;
            blaze.blazeRadius = blazeOrbRadius;
            blaze.blazeFreq = blazeOrbTickFreq;
            blaze.blazeMinDurToContinue = blazeOrbMinDur;
            blaze.blazeDurationPerStack = blazeOrbDurationPerStack;

            blaze.igniteDamageColor = igniteOrbDamageColor;
            blaze.igniteDebuffTimeMult = igniteOrbDebuffTimeMult;
            blaze.igniteDuration = igniteOrbDuration;
            blaze.igniteProcCoef = igniteOrbProcCoef;
            blaze.igniteTickDamage = igniteOrbTickDamage;
            blaze.igniteTickFreq = igniteOrbTickFreq;
            blaze.igniteStacksPerSecOnDeath = igniteOrbStacksPerSecOnDeath;
            blaze.igniteBaseStacksOnDeath = igniteOrbBaseStacksOnDeath;
            blaze.igniteExpireStacksMult = igniteOrbExpireStacksMult;
            

            OrbManager.instance.AddOrb(blaze);
        }
    }
}

