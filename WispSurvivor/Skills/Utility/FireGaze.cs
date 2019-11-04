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
        public static float baseBlazeOrbRadius = 10f;
        public static float baseBlazeOrbDuration = 10f;
        public static float baseBlazeOrbTickfreq = 1f;
        public static float baseBlazeOrbMinDur = 1f;
        public static float baseBlazeOrbDurationPerStack = 0.25f;

        public static float baseIgniteOrbDebuffTimeMult = 1f;
        public static float baseIgniteOrbDuration = 3f;
        public static float baseIgniteOrbProcCoef = 0.05f;
        public static float baseIgniteOrbTickDamage = 0.1f;
        public static float baseIgniteOrbTickFreq = 2f;
        public static float baseIgniteOrbBaseStacksOnDeath = 1f;
        public static float baseIgniteOrbStacksPerSecOnDeath = 1f;
        public static float baseIgniteOrbExpireStacksMult = 1f;

        private float flareTime;
        private float blazeOrbRadius;
        private float blazeOrbDuration;
        private float blazeOrbTickFreq;
        private float blazeOrbMinDur;
        private float blazeOrbDurationPerStack;
        private float igniteOrbDuration;
        private float igniteOrbProcCoef;
        private float igniteOrbTickDamage;
        private float igniteOrbTickFreq;
        private float igniteOrbDebuffTimeMult;
        private float igniteOrbStacksPerSecOnDeath;
        private float igniteOrbBaseStacksOnDeath;
        private float igniteOrbExpireStacksMult;

        private bool crit;

        private uint skin;

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
            skin = characterBody.skinIndex;

            blazeOrbRadius = baseBlazeOrbRadius;
            blazeOrbDuration = baseBlazeOrbDuration;
            blazeOrbTickFreq = baseBlazeOrbTickfreq;
            blazeOrbMinDur = baseBlazeOrbMinDur;
            blazeOrbDurationPerStack = baseBlazeOrbDurationPerStack;

            igniteOrbDebuffTimeMult = baseIgniteOrbDebuffTimeMult;
            igniteOrbDuration = baseIgniteOrbDuration;
            igniteOrbProcCoef = baseIgniteOrbProcCoef;
            igniteOrbTickDamage = baseIgniteOrbTickDamage * damageStat;
            igniteOrbTickFreq = baseIgniteOrbTickFreq;
            igniteOrbBaseStacksOnDeath = baseIgniteOrbBaseStacksOnDeath;
            igniteOrbStacksPerSecOnDeath = baseIgniteOrbStacksPerSecOnDeath;
            igniteOrbExpireStacksMult = baseIgniteOrbExpireStacksMult;

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
            writer.Write(orbOrigin);
            writer.Write(orbNormal);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            orbOrigin = reader.ReadVector3();
            orbNormal = reader.ReadVector3();
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

