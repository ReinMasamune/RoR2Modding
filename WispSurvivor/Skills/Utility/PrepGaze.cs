using RoR2;
using EntityStates;
using UnityEngine;
using RoR2.Orbs;
using UnityEngine.Networking;

namespace WispSurvivor.Skills.Utility
{
    public class PrepGaze : BaseState
    {
        public static float maxRange = 50f;
        public static float flareDuration = 0.2f;

        public static float baseBlazeOrbRadius = 10f;
        public static float baseBlazeOrbDuration = 10f;
        public static float baseBlazeOrbTickfreq = 1f;
        public static float baseBlazeOrbMinDur = 1f;
        public static float baseBlazeOrbDurationPerStack = 1f;

        public static float baseIgniteOrbDebuffTimeMult = 1f;
        public static float baseIgniteOrbDuration = 10f;
        public static float baseIgniteOrbProcCoef = 1f;
        public static float baseIgniteOrbTickDamage = 1f;
        public static float baseIgniteOrbTickFreq = 1f;
        public static float baseIgniteOrbBaseStacksOnDeath = 1f;
        public static float baseIgniteOrbStacksPerSecOnDeath = 1f;
        public static float baseIgniteOrbExpireStacksMult = 1f;


        public static DamageColorIndex igniteOrbDamageColor = DamageColorIndex.Default;
        //Orb settings here

        private float blazeOrbRadius;
        private float blazeOrbDuration;
        private float blazeOrbTickFreq;
        private float blazeOrbMinDur;
        private float blazeOrbDurationPerStack;

        private float igniteOrbDebuffTimeMult;
        private float igniteOrbDuration;
        private float igniteOrbProcCoef;
        private float igniteOrbTickDamage;
        private float igniteOrbTickFreq;
        private float igniteOrbStacksPerSecOnDeath;
        private float igniteOrbBaseStacksOnDeath;
        private float igniteOrbExpireStacksMult;

        private uint skin = 0;

        private Vector3 normal;

        private Components.WispPassiveController passive;
        private GameObject line;
        private LineRenderer lr;
        private Transform end;
        private Transform castStart;

        public override void OnEnter()
        {
            base.OnEnter();

            passive = gameObject.GetComponent<Components.WispPassiveController>();

            //Do I need authority check?
            skin = characterBody.skinIndex;

            //Do the face flare thing
            //Create a prediction beam to aim location


            blazeOrbRadius = baseBlazeOrbRadius;
            blazeOrbDuration = baseBlazeOrbDuration;
            blazeOrbTickFreq = baseBlazeOrbTickfreq;
            blazeOrbMinDur = baseBlazeOrbMinDur;
            blazeOrbDurationPerStack = baseBlazeOrbDurationPerStack;

            igniteOrbDebuffTimeMult = baseIgniteOrbDebuffTimeMult;
            igniteOrbDuration = baseIgniteOrbDuration;
            igniteOrbProcCoef = baseIgniteOrbProcCoef;
            igniteOrbTickDamage = baseIgniteOrbTickDamage;
            igniteOrbTickFreq = baseIgniteOrbTickFreq;
            igniteOrbBaseStacksOnDeath = baseIgniteOrbBaseStacksOnDeath;
            igniteOrbStacksPerSecOnDeath = baseIgniteOrbStacksPerSecOnDeath;
            igniteOrbExpireStacksMult = baseIgniteOrbExpireStacksMult;
        }

        public override void Update()
        {
            base.Update();
            //Update the beam position
            if( !line )
            {
                Transform muzzle = GetModelTransform().Find("CannonPivot").Find("AncientWispArmature").Find("Head");
                line = UnityEngine.Object.Instantiate<GameObject>(Modules.WispEffectModule.utilityAim[0], muzzle.TransformPoint(0f, 0.1f, 0f), muzzle.rotation, muzzle);
                lr = line.GetComponent<LineRenderer>();
                end = line.transform.Find("lineEnd");
            }

            if (line)
            {
                Ray r = GetAimRay();

                RaycastHit rh;
                if (Physics.Raycast(r, out rh, maxRange, LayerIndex.world.mask | LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal))
                {
                    end.position = rh.point;
                    normal = rh.normal;
                }
                else
                {
                    end.position = r.GetPoint(maxRange);
                    normal = Vector3.up;
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            characterBody.SetAimTimer(1f);
            if( inputBank && isAuthority && !inputBank.skill3.down )
            {
                //Get the target position
                outer.SetNextState(new FireGaze
                {
                    orbOrigin = end.position,
                    orbNormal = normal,
                    skin = skin,
                    flareTime = flareDuration,
                    blazeOrbDuration = blazeOrbDuration,
                    blazeOrbRadius = blazeOrbRadius,
                    blazeOrbTickFreq = blazeOrbTickFreq,
                    blazeOrbMinDur = blazeOrbMinDur,
                    blazeOrbDurationPerStack = blazeOrbDurationPerStack,
                    igniteOrbDamageColor = igniteOrbDamageColor,
                    igniteOrbDebuffTimeMult = igniteOrbDebuffTimeMult,
                    igniteOrbDuration = igniteOrbDuration,
                    igniteOrbProcCoef = igniteOrbProcCoef,
                    igniteOrbTickDamage = igniteOrbTickDamage,
                    igniteOrbTickFreq = igniteOrbTickFreq,
                    igniteOrbBaseStacksOnDeath = igniteOrbBaseStacksOnDeath,
                    igniteOrbStacksPerSecOnDeath = igniteOrbStacksPerSecOnDeath,
                    igniteOrbExpireStacksMult = igniteOrbExpireStacksMult,
                    crit = RollCrit()
                });
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            //Destroy the beam marker
            Destroy(line);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
        }
    }
}

