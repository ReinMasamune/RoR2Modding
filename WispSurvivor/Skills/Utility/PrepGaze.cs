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
        public static float castRadius = 0.25f;

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


            //Do the face flare thing
            //Create a prediction beam to aim location
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
                if (Physics.SphereCast(r, castRadius, out rh, maxRange, LayerIndex.world.mask | LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal))
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

