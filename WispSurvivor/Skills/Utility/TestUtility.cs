using RoR2;
using EntityStates;
using UnityEngine;
using RoR2.Orbs;
using UnityEngine.Networking;

namespace WispSurvivor.Skills.Utility
{
    public class TestUtility : BaseState
    {
        public static double chargeUsed = 10.0;

        public static float baseDuration = 0.2f;
        public static float maxRange = 50f;
        public static float returnIdlePercent = 0.5f;
        public static float chargeScaler = 1f;

        public static float radius = 3f;
        public static float baseTime = 5f;
        public static float targetResetFreq = 0.5f;
        public static float damageMult = 0.25f;
        public static float baseDebuffTime = 1.5f;
        public static float dotTimeMult = 1f;
        public static float tickFreq = 1f;
        public static float minTime = 1f;
        public static float spreadLossFrac = 1.1f;

        private double charge;

        private float duration;

        private bool hasFired = false;
        private bool scanned = false;
        private bool synced = false;
        private bool useTarget = false;

        private uint skin = 0;

        private Vector3 targetVec;
        private Vector3 normal;

        private ChildLocator childLoc;
        private Animator anim;
        private HurtBox target;
        private BullseyeSearch search = new BullseyeSearch();
        private Components.WispPassiveController passive;

        public override void OnEnter()
        {
            base.OnEnter();

            passive = gameObject.GetComponent<Components.WispPassiveController>();

            Transform modelTrans = base.GetModelTransform();
            //Sound

            duration = baseDuration;

            if (characterBody)
            {
                characterBody.SetAimTimer(duration + 1f);
            }

            skin = characterBody.skinIndex;

            charge = passive.ConsumeCharge(chargeUsed);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //FireOrb();
            if (fixedAge > duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            if (isAuthority)
            {
                writer.Write(HurtBoxReference.FromHurtBox(target));
                writer.Write(targetVec);
                writer.Write(skin);
            }
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            if (!isAuthority)
            {
                target = reader.ReadHurtBoxReference().ResolveHurtBox();
                targetVec = reader.ReadVector3();
                skin = reader.ReadUInt32();
                synced = true;
            }
        }

        private void GetTarget()
        {
            useTarget = false;
            Ray r = GetAimRay();

            RaycastHit rh;

            if (Physics.SphereCast( r, 0.25f, out rh, maxRange, LayerIndex.world.mask | LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal))
            {
                targetVec = rh.point;
                normal = rh.normal;

                Collider col = rh.collider;
                if( col )
                {
                    HurtBox hb = col.GetComponent<HurtBox>();
                    if( hb )
                    {
                        TeamIndex team = TeamComponent.GetObjectTeam(hb.healthComponent.gameObject);
                        if( team != TeamComponent.GetObjectTeam(gameObject) )
                        {
                            target = hb;
                            useTarget = true;
                        }
                    }
                }
            }
            else
            {
                targetVec = r.GetPoint(maxRange);
                normal = Vector3.up;
            }
        }

        /*
        private void FireOrb()
        {
            if (!NetworkServer.active) return;

            GetTarget();

            float time = baseTime + (chargeScaler * (float)(charge - 100) / 100);
            float debuffTime = baseDebuffTime + (chargeScaler * (float)(charge - 100) / 100);

            Orbs.BlazeOrb orb = new Orbs.BlazeOrb();

            orb.attacker = gameObject;
            orb.bRadius = radius;
            orb.bTargetResetFreq = targetResetFreq;
            orb.crit = RollCrit();
            orb.iDamageMult = damageMult;
            orb.iDebuffTime = debuffTime;
            orb.iDotMult = dotTimeMult;
            orb.iTickFreq = tickFreq;
            orb.minTime = minTime;
            orb.normal = normal;
            orb.origin = targetVec;
            orb.skin = skin;
            orb.spreadTimeDiv = spreadLossFrac;
            orb.team = TeamComponent.GetObjectTeam(gameObject);
            orb.time = time;

            OrbManager.instance.AddOrb(orb);
        }
        */
    }
}

