using RoR2;
using EntityStates;
using UnityEngine;
using System;
using RoR2.Orbs;
using UnityEngine.Networking;
using System.Linq;

namespace WispSurvivor.Skills.Primary
{
    public class TestPrimary : BaseState
    {
        public enum WispPrimaryAnimState
        {
            First = 0,
            Left = 1,
            Right = 2
        }

        public WispPrimaryAnimState state = WispPrimaryAnimState.First;


        public static double baseChargeAdded = 5.0;

        public static float baseDuration = 0.625f;
        public static float chargeScaler = 0.5f;
        public static float scanDelay = 0.25f;
        public static float fireDelay = 0.185f;
        public static float maxRange = 35f;
        public static float maxAngle = 7.5f;
        public static float damageRatio = 2.5f;
        public static float radius = 6f;
        public static float returnIdlePercent = 0.55f;

        private float duration;

        private double chargeAdded;

        private bool hasFired = false;
        private bool scanned = false;
        private bool synced = false;


        private uint skin = 0;

        private Vector3 targetVec;

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

            

            //int stockMissing = skillLocator.primary.maxStock - skillLocator.primary.stock + skillLocator.primary.stockToConsume;
            //duration = baseDuration / (attackSpeedStat / stockMissing);
            //float stockFrac = (skillLocator.primary.stock + skillLocator.primary.stockToConsume) / skillLocator.primary.maxStock;
            //duration = baseDuration / (attackSpeedStat * stockFrac);

           
            //chargeAdded = baseChargeAdded / Mathf.Sqrt(attackSpeedStat);
            

            //float stockMax = skillLocator.primary.maxStock;
            //float curStock = skillLocator.primary.stock;
            //float stockFrac = 0.5f + 0.5f * (stockMax+1) / (Math.Max( 0,curStock)+1);
            //duration = baseDuration * stockFrac / attackSpeedStat;
            if (skillLocator.primary.stock > 0) skillLocator.primary.stock -= 1; else skillLocator.primary.stock = 0;
            chargeAdded = baseChargeAdded * (skillLocator.primary.stock > 0 ? 2f : 1f);
            duration = baseDuration * (skillLocator.primary.stock > 0 ? 1f : 2f) / attackSpeedStat;


            //duration = baseDuration * (skillLocator.primary.maxStock*0.5f + 0.5f)) / (attackSpeedStat * (0.5f + 0.5f*( Math.Max(skillLocator.primary.stock, 0) + skillLocator.primary.stockToConsume)));

            //Animations
            anim = GetModelAnimator();
            modelTrans = GetModelTransform();
            childLoc = modelTrans.GetComponent<ChildLocator>();

            string animStr = anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Gesture")).IsName("Throw1") ? "Throw2" : "Throw1";
                
            //int layerInd = anim.GetLayerIndex("Gesture");
            //if (anim.GetCurrentAnimatorStateInfo(layerInd).IsName("Throw1"))
            //{
                PlayCrossfade("Gesture", animStr, "Throw.playbackRate", duration / (1f - returnIdlePercent), 0.2f);
            //} else
            //{
            //    PlayCrossfade("Gesture", "Throw1", "Throw.playbackRate", duration / (1f - returnIdlePercent), 0.2f);
            //}

            if( isAuthority )
            {
                skin = characterBody.skinIndex;
                GetTarget();
            }



            if( characterBody )
            {
                characterBody.SetAimTimer(duration + 1f);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //if( !scanned && fixedAge > duration * scanDelay * fireDelay )
            //{
                
            //}
            if( !hasFired && fixedAge > duration * fireDelay )
            {
                FireOrb();
            }
            if( fixedAge > duration && isAuthority )
            {
                if( inputBank && skillLocator && inputBank.skill1.down && skillLocator.primary.stock >= skillLocator.primary.requiredStock && !characterBody.isSprinting )
                {
                    switch( state )
                    {
                        case WispPrimaryAnimState.First:
                            state = WispPrimaryAnimState.Right;
                            break;
                        case WispPrimaryAnimState.Left:
                            state = WispPrimaryAnimState.Right;
                            break;
                        case WispPrimaryAnimState.Right:
                            state = WispPrimaryAnimState.Left;
                            break;
                    }
                    outer.SetNextState(new TestPrimary
                    {
                        state = state
                    });


                    return;

                } else
                {
                    PlayCrossfade("Gesture", "Idle", 0.2f);
                    outer.SetNextStateToMain();
                }

                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            FireOrb();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(HurtBoxReference.FromHurtBox(target));
            writer.Write(targetVec);
            writer.Write(skin);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            target = reader.ReadHurtBoxReference().ResolveHurtBox();
            targetVec = reader.ReadVector3();
            skin = reader.ReadUInt32();
            synced = true;
        }

        private void GetTarget()
        {
            scanned = true;

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
            
            if( Physics.Raycast( r , out rh , maxRange , LayerIndex.world.mask | LayerIndex.entityPrecise.mask , QueryTriggerInteraction.UseGlobal ) )
            {
                targetVec = rh.point;
            }
            else
            {
                targetVec = r.GetPoint(maxRange);
            }
        }

        private void FireOrb()
        {
            if (hasFired) return;
            passive.AddCharge(chargeAdded);
            hasFired = true;
            if (!NetworkServer.active) return;

            Orbs.SnapOrb arrow = new Orbs.SnapOrb();

            if( !synced )
            {
                GetTarget();
                skin = characterBody.skinIndex;
            }

            arrow.damage = damageStat * ( damageRatio + ( chargeScaler * (float)( passive.ReadCharge() - 100 ) / 100 ));
            arrow.crit = RollCrit();
            arrow.team = TeamComponent.GetObjectTeam(gameObject);
            arrow.attacker = gameObject;
            arrow.procCoef = 1.0f;
            arrow.radius = radius;
            arrow.skin = skin;

            Transform trans = childLoc.FindChild("MuzzleRight");

            //Muzzle flash

            arrow.origin = trans.position;

            arrow.speed = 150f;

            if( target )
            {
                arrow.target = target;
                arrow.useTarget = true;
                targetVec = target.transform.position;
            }
            else
            {
                arrow.useTarget = false;
            }

            arrow.targetPos = targetVec;


            OrbManager.instance.AddOrb(arrow);
        }
    }
}
