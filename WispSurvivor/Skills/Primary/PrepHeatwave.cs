using EntityStates;
using RoR2;
using System;
using UnityEngine;

namespace WispSurvivor.Skills.Primary
{
    public class PrepHeatwave : BaseState
    {
        public static Single baseTotalDuration = 0.625f;
        public static Single basePrepDuration = 0.115f;
        public static Single baseFireDuration = baseTotalDuration-basePrepDuration;
        public static Single maxRange = 75f;
        public static Single maxAngle = 7.5f;
        public static Single noStockSpeedMult = 0.5f;
        public static Single animReturnPercent = 0.25f;

        public static Double baseChargeAdded = 15.0;
        public static Double noStockChargeMult = 0.5;

        private Single prepDuration;
        private Single fireDuration;
        private Single initAS;

        private Vector3 targetVec;

        private Components.WispPassiveController passive;
        private Animator anim;
        private BullseyeSearch search = new BullseyeSearch();
        private HurtBox target;

        public override void OnEnter()
        {
            base.OnEnter();
            this.passive = this.gameObject.GetComponent<Components.WispPassiveController>();
            Boolean hasStock = this.skillLocator.primary.stock > 0;
            this.skillLocator.primary.stock = hasStock ? this.skillLocator.primary.stock - 1 : 0;
            this.skillLocator.primary.rechargeStopwatch = 0f;
            this.initAS = this.attackSpeedStat * (hasStock ? 1f : noStockSpeedMult);

            this.prepDuration = basePrepDuration / this.initAS;
            this.fireDuration = baseFireDuration / this.initAS;
            Single totalDuration = this.prepDuration + this.fireDuration;

            Transform modelTrans = base.GetModelTransform();
            this.anim = this.GetModelAnimator();
            modelTrans = this.GetModelTransform();
            String animStr = this.anim.GetCurrentAnimatorStateInfo(this.anim.GetLayerIndex("Gesture")).IsName("Throw1") ? "Throw2" : "Throw1";
            this.PlayCrossfade( "Gesture", animStr, "Throw.playbackRate", totalDuration * 2f, 0.2f );
            RoR2.Util.PlaySound( "Play_huntress_m2_throw", this.gameObject );

            this.characterBody.SetAimTimer( totalDuration + 1f );
            this.characterBody.isSprinting = false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //this.characterBody.isSprinting = false;

            if( this.fixedAge >= this.prepDuration && this.isAuthority )
            {
                this.GetTarget();
                this.outer.SetNextState( new FireHeatwave
                {
                    initAS = initAS,
                    target = target,
                    targetVec = targetVec
                } );
            }
        }

        public override void OnExit() => base.OnExit();

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Skill;

        private void GetTarget()
        {
            Ray r = this.GetAimRay();

            //this.search.teamMaskFilter = TeamMask.all;
            //this.search.teamMaskFilter.RemoveTeam( TeamComponent.GetObjectTeam( this.gameObject ) );
            //this.search.filterByLoS = true;
            //this.search.searchOrigin = r.origin;
            //this.search.searchDirection = r.direction;
            //this.search.sortMode = BullseyeSearch.SortMode.DistanceAndAngle;
            //this.search.maxDistanceFilter = maxRange;
            //this.search.maxAngleFilter = maxAngle;
            //this.search.RefreshCandidates();
            //this.target = this.search.GetResults().FirstOrDefault<HurtBox>();

            RaycastHit rh;

            if( Physics.Raycast( r, out rh, maxRange, LayerIndex.world.mask | LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal ) )
            {
                r.direction = rh.point - r.origin;
                if( Physics.Raycast( r, out rh, maxRange, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ) )
                {
                    this.targetVec = rh.point;
                } else
                {
                    this.targetVec = r.GetPoint( maxRange );
                }

            } else
            {
                this.targetVec = r.GetPoint( maxRange );
            }
        }
    }
}
