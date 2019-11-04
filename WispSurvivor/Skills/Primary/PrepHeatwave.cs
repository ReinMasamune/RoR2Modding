using RoR2;
using EntityStates;
using UnityEngine;
using System;
using RoR2.Orbs;
using UnityEngine.Networking;
using System.Linq;

namespace WispSurvivor.Skills.Primary
{
    public class PrepHeatwave : BaseState
    {
        public static float basePrepDuration = 0.2f;
        public static float baseFireDuration = 0.425f;
        public static float maxRange = 40f;
        public static float maxAngle = 7.5f;
        public static float noStockSpeedMult = 0.5f;
        public static float animReturnPercent = 0.55f;

        public static double baseChargeAdded = 10.0;
        public static double noStockChargeMult = 0.5;

        private float prepDuration;
        private float fireDuration;
        private float initAS;

        private double chargeAdded;

        private bool crit;

        private uint skin = 0;

        private Vector3 targetVec;

        private Components.WispPassiveController passive;
        private Animator anim;
        private BullseyeSearch search = new BullseyeSearch();
        private HurtBox target;

        public override void OnEnter()
        {
            base.OnEnter();
            passive = gameObject.GetComponent<Components.WispPassiveController>();
            skin = characterBody.skinIndex;

            bool hasStock = skillLocator.primary.stock > 0;
            skillLocator.primary.stock = hasStock ? skillLocator.primary.stock - 1 : 0;
            initAS = attackSpeedStat * (hasStock ? 1f : noStockSpeedMult);

            prepDuration = basePrepDuration / initAS;
            fireDuration = baseFireDuration / initAS;
            float totalDuration = prepDuration + fireDuration;
            chargeAdded = baseChargeAdded * (hasStock ? 1.0 : noStockChargeMult);
            passive.AddCharge(chargeAdded);

            Transform modelTrans = base.GetModelTransform();
            anim = GetModelAnimator();
            modelTrans = GetModelTransform();
            string animStr = anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Gesture")).IsName("Throw1") ? "Throw2" : "Throw1";
            PlayCrossfade("Gesture", animStr, "Throw.playbackRate", totalDuration / (1f - animReturnPercent), 0.2f);

            if( characterBody )
            {
                characterBody.SetAimTimer(totalDuration + 1f);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            characterBody.isSprinting = false;

            if( fixedAge >= prepDuration && isAuthority )
            {
                GetTarget();
                this.outer.SetNextState(new FireHeatwave
                {
                    initAS = initAS,
                    target = target,
                    targetVec = targetVec
                });
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }


        private void GetTarget()
        {
            Ray r = GetAimRay();

            search.teamMaskFilter = TeamMask.all;
            search.teamMaskFilter.RemoveTeam(TeamComponent.GetObjectTeam(gameObject));
            search.filterByLoS = true;
            search.searchOrigin = r.origin;
            search.searchDirection = r.direction;
            search.sortMode = BullseyeSearch.SortMode.DistanceAndAngle;
            search.maxDistanceFilter = maxRange;
            search.maxAngleFilter = maxAngle;
            search.RefreshCandidates();
            target = search.GetResults().FirstOrDefault<HurtBox>();

            RaycastHit rh;

            if (Physics.Raycast(r, out rh, maxRange, LayerIndex.world.mask | LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal))
            {
                targetVec = rh.point;
            } else
            {
                targetVec = r.GetPoint(maxRange);
            }
        }
    }
}
