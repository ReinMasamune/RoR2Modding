using EntityStates;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        public class FireGaze : BaseState
        {
            public static System.Double chargeUsed = 20.0;
            public static System.Single chargeScaler = 0.1f;

            public static System.Single baseBlazeOrbRadius = 20f;
            public static System.Single baseBlazeOrbDuration = 10f;
            public static System.Single baseBlazeOrbTickfreq = 1f;
            public static System.Single baseBlazeOrbExpireStacksMult = 10f;
            public static System.Single baseBlazeOrbBuffDuration = 4f;
            public static System.UInt32 baseBlazeOrbBundleSize = 1u;
            public static System.Single baseBlazeOrbBundleSendFreq = 1f;
            public static System.Single baseBlazeOrbBonusBundlePercent = 0.25f;

            public static System.Single baseIgniteOrbStacksPerTick = 0.25f;
            public static System.Single baseIgniteOrbDuration = 4f;
            public static System.Single baseIgniteOrbProcCoef = 0.05f;
            public static System.Single baseIgniteOrbTickDamage = 0.1f;
            public static System.Single baseIgniteOrbTickFreq = 2f;
            public static System.Single baseIgniteOrbBaseStacksOnDeath = 0.5f;
            public static System.Single baseIgniteOrbDeathStacksMult = 0.5f;
            public static System.Single baseIgniteOrbExpireStacksMult = 0f;

            private static readonly System.Single flareTime = 0.35f;

            public System.Single blazeOrbRadius;
            private System.Single blazeOrbDuration;
            private System.Single blazeOrbTickFreq;
            private System.Single blazeOrbExpireStacksMult;
            private System.Single blazeOrbBuffDuration;
            private System.UInt32 blazeOrbBundleSize;
            private System.Single blazeOrbBundleSendFreq;
            private System.Single blazeOrbBonusBundlePercent;
            private System.Single igniteOrbDuration;
            private System.Single igniteOrbProcCoef;
            private System.Single igniteOrbTickDamage;
            private System.Single igniteOrbTickFreq;
            private System.Single igniteOrbStacksPerTick;
            private System.Single igniteOrbDeathStacksMult;
            private System.Single igniteOrbBaseStacksOnDeath;
            private System.Single igniteOrbExpireStacksMult;

            private readonly System.Boolean crit;

            private System.UInt32 skin;

            public Vector3 orbOrigin;
            public Vector3 orbNormal;

            public DamageColorIndex igniteOrbDamageColor = DamageColorIndex.WeakPoint;

            private System.Boolean hasFired = false;

            private WispPassiveController passive;
            private WispFlareController flare;

            //Orb params and stuff
            public override void OnEnter()
            {
                base.OnEnter();
                this.passive = this.gameObject.GetComponent<WispPassiveController>();
                this.flare = this.GetComponent<WispFlareController>();
                this.skin = this.characterBody.skinIndex;

                //blazeOrbRadius = baseBlazeOrbRadius;
                this.blazeOrbDuration = baseBlazeOrbDuration;
                this.blazeOrbTickFreq = baseBlazeOrbTickfreq;
                this.blazeOrbExpireStacksMult = baseBlazeOrbExpireStacksMult;
                this.blazeOrbBuffDuration = baseBlazeOrbBuffDuration;
                this.blazeOrbBonusBundlePercent = baseBlazeOrbBonusBundlePercent;

                this.igniteOrbStacksPerTick = baseIgniteOrbStacksPerTick;
                this.igniteOrbDuration = baseIgniteOrbDuration;
                this.igniteOrbProcCoef = baseIgniteOrbProcCoef;
                this.igniteOrbTickDamage = baseIgniteOrbTickDamage * this.damageStat;
                this.igniteOrbTickFreq = baseIgniteOrbTickFreq;
                this.igniteOrbBaseStacksOnDeath = baseIgniteOrbBaseStacksOnDeath;
                this.igniteOrbDeathStacksMult = baseIgniteOrbDeathStacksMult;
                this.igniteOrbExpireStacksMult = baseIgniteOrbExpireStacksMult;
                //crit = RollCrit();

                this.flare.intensity = 1f;
            }

            public override void Update()
            {
                base.Update();
                //Scale flare down based on time / flareTime
                this.flare.intensity = 1f - (this.age / flareTime);
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();

                this.FireOrb();
                if( this.fixedAge > flareTime && this.isAuthority )
                {
                    this.outer.SetNextStateToMain();
                }
            }

            public override void OnExit()
            {
                base.OnExit();
                this.flare.intensity = 0f;
            }

            public override void OnSerialize( NetworkWriter writer )
            {
                base.OnSerialize( writer );
                writer.Write( this.orbOrigin );
                writer.Write( this.orbNormal );
                writer.Write( this.blazeOrbRadius );
            }

            public override void OnDeserialize( NetworkReader reader )
            {
                base.OnDeserialize( reader );
                this.orbOrigin = reader.ReadVector3();
                this.orbNormal = reader.ReadVector3();
                this.blazeOrbRadius = reader.ReadSingle();
            }

            public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Death;

            private void FireOrb()
            {
                if( this.hasFired ) return;
                this.hasFired = true;
                //var chargeState = passive.ConsumePercentCharge(chargeUsed);
                if( !NetworkServer.active ) return;

                BlazeOrb blaze = new BlazeOrb();

                //float chargeMult = (1f + 0.5f * ((float)((chargeState.chargeLeft + chargeState.chargeConsumed) / 100.0)));

                this.blazeOrbBundleSendFreq = baseBlazeOrbBundleSendFreq;
                this.blazeOrbBundleSize = (System.UInt32)Mathf.CeilToInt( baseBlazeOrbBundleSize );
                this.blazeOrbBundleSize = baseBlazeOrbBundleSize;
                this.blazeOrbBonusBundlePercent = baseBlazeOrbBonusBundlePercent;


                //Unorganized shit
                blaze.origin = this.orbOrigin;
                blaze.normal = this.orbNormal;
                blaze.skin = this.skin;
                blaze.team = TeamComponent.GetObjectTeam( this.gameObject );
                blaze.attacker = this.gameObject;
                blaze.crit = this.crit;
                blaze.children = new List<IgnitionOrb>();

                blaze.blazeTime = this.blazeOrbDuration;
                blaze.blazeRadius = this.blazeOrbRadius;
                blaze.blazeFreq = this.blazeOrbTickFreq;
                blaze.blazeExpireStacksMult = this.blazeOrbExpireStacksMult;
                blaze.blazeBuffDuration = this.blazeOrbBuffDuration;
                blaze.blazeOrbBundleSendFreq = this.blazeOrbBundleSendFreq;
                blaze.blazeOrbStackBundleSize = this.blazeOrbBundleSize;
                blaze.blazeBonusBundlePercent = this.blazeOrbBonusBundlePercent;

                blaze.igniteDamageColor = this.igniteOrbDamageColor;
                blaze.igniteStacksPerTick = this.igniteOrbStacksPerTick;
                blaze.igniteDuration = this.igniteOrbDuration;
                blaze.igniteProcCoef = this.igniteOrbProcCoef;
                blaze.igniteTickDamage = this.igniteOrbTickDamage;
                blaze.igniteTickFreq = this.igniteOrbTickFreq;
                blaze.igniteDeathStacksMult = this.igniteOrbDeathStacksMult;
                blaze.igniteBaseStacksOnDeath = this.igniteOrbBaseStacksOnDeath;
                blaze.igniteExpireStacksMult = this.igniteOrbExpireStacksMult;

                OrbManager.instance.AddOrb( blaze );
            }
        }
    }
#endif
}