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
        public static double chargeUsed = 20.0;
        public static float chargeScaler = 0.1f;

        public static float baseBlazeOrbRadius = 20f;
        public static float baseBlazeOrbDuration = 10f;
        public static float baseBlazeOrbTickfreq = 1f;
        public static float baseBlazeOrbMinDur = 10f;
        public static float baseBlazeOrbDurationPerStack = 4f;
        public static uint baseBlazeOrbBundleSize = 1u;
        public static float baseBlazeOrbBundleSendFreq = 1f;
        public static float baseBlazeOrbStackExchangeRate = 0.25f;

        public static float baseIgniteOrbDebuffTimeMult = 0.1f;
        public static float baseIgniteOrbDuration = 4f;
        public static float baseIgniteOrbProcCoef = 0.05f;
        public static float baseIgniteOrbTickDamage = 0.1f;
        public static float baseIgniteOrbTickFreq = 2f;
        public static float baseIgniteOrbBaseStacksOnDeath = 0.5f;
        public static float baseIgniteOrbStacksPerSecOnDeath = 0.5f;
        public static float baseIgniteOrbExpireStacksMult = 0.25f;

        private static float flareTime = 0.25f;

        public float blazeOrbRadius;
        private float blazeOrbDuration;
        private float blazeOrbTickFreq;
        private float blazeOrbMinDur;
        private float blazeOrbDurationPerStack;
        private uint blazeOrbBundleSize;
        private float blazeOrbBundleSendFreq;
        private float blazeOrbStackExchangeRate;
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

        public DamageColorIndex igniteOrbDamageColor = DamageColorIndex.WeakPoint;

        private bool hasFired = false;

        private Components.WispPassiveController passive;
        private Components.WispFlareController flare;
        
        //Orb params and stuff
        public override void OnEnter()
        {
            base.OnEnter();
            passive = gameObject.GetComponent<Components.WispPassiveController>();
            flare = GetComponent<Components.WispFlareController>();
            skin = characterBody.skinIndex;

            //blazeOrbRadius = baseBlazeOrbRadius;
            blazeOrbDuration = baseBlazeOrbDuration;
            blazeOrbTickFreq = baseBlazeOrbTickfreq;
            blazeOrbMinDur = baseBlazeOrbMinDur;
            blazeOrbDurationPerStack = baseBlazeOrbDurationPerStack;
            blazeOrbStackExchangeRate = baseBlazeOrbStackExchangeRate;

            igniteOrbDebuffTimeMult = baseIgniteOrbDebuffTimeMult;
            igniteOrbDuration = baseIgniteOrbDuration;
            igniteOrbProcCoef = baseIgniteOrbProcCoef;
            igniteOrbTickDamage = baseIgniteOrbTickDamage * damageStat;
            igniteOrbTickFreq = baseIgniteOrbTickFreq;
            igniteOrbBaseStacksOnDeath = baseIgniteOrbBaseStacksOnDeath;
            igniteOrbStacksPerSecOnDeath = baseIgniteOrbStacksPerSecOnDeath;
            igniteOrbExpireStacksMult = baseIgniteOrbExpireStacksMult;
            //crit = RollCrit();

            flare.intensity = 1f;
        }

        public override void Update()
        {
            base.Update();
            //Scale flare down based on time / flareTime
            flare.intensity = 1f - (age / flareTime);
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
            flare.intensity = 0f;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(orbOrigin);
            writer.Write(orbNormal);
            writer.Write(blazeOrbRadius);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            orbOrigin = reader.ReadVector3();
            orbNormal = reader.ReadVector3();
            blazeOrbRadius = reader.ReadSingle();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        private void FireOrb()
        {
            if (hasFired) return;
            hasFired = true;
            //var chargeState = passive.ConsumePercentCharge(chargeUsed);
            if (!NetworkServer.active) return;

            BlazeOrb blaze = new BlazeOrb();

            //float chargeMult = (1f + 0.5f * ((float)((chargeState.chargeLeft + chargeState.chargeConsumed) / 100.0)));

            blazeOrbBundleSendFreq = baseBlazeOrbBundleSendFreq;
            blazeOrbBundleSize = (uint)Mathf.CeilToInt(baseBlazeOrbBundleSize); 
            blazeOrbBundleSize = baseBlazeOrbBundleSize;
            blazeOrbStackExchangeRate = baseBlazeOrbStackExchangeRate;


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
            blaze.blazeOrbBundleSendFreq = blazeOrbBundleSendFreq;
            blaze.blazeOrbStackBundleSize = blazeOrbBundleSize;
            blaze.blazeStackExchangeRate = blazeOrbStackExchangeRate;

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

