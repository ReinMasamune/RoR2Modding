using EntityStates;
using RoR2;
using RoR2.Orbs;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
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


            public static Double baseChargeAdded = 5.0;

            public static Single baseDuration = 0.625f;
            public static Single chargeScaler = 0.5f;
            public static Single scanDelay = 0.25f;
            public static Single fireDelay = 0.185f;
            public static Single maxRange = 35f;
            public static Single maxAngle = 7.5f;
            public static Single damageRatio = 2.5f;
            public static Single radius = 6f;
            public static Single returnIdlePercent = 0.55f;

            private Single duration;

            private Double chargeAdded;

            private Boolean hasFired = false;
            private Boolean scanned = false;
            private Boolean synced = false;


            private UInt32 skin = 0;

            private Vector3 targetVec;

            private ChildLocator childLoc;
            private Animator anim;
            private HurtBox target;
            private BullseyeSearch search = new BullseyeSearch();
            private WispPassiveController passive;

            public override void OnEnter()
            {
                base.OnEnter();
                this.passive = this.gameObject.GetComponent<WispPassiveController>();

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
                if( this.skillLocator.primary.stock > 0 ) this.skillLocator.primary.stock -= 1; else this.skillLocator.primary.stock = 0;
                this.chargeAdded = baseChargeAdded * (this.skillLocator.primary.stock > 0 ? 2f : 1f);
                this.duration = baseDuration * (this.skillLocator.primary.stock > 0 ? 1f : 2f) / this.attackSpeedStat;


                //duration = baseDuration * (skillLocator.primary.maxStock*0.5f + 0.5f)) / (attackSpeedStat * (0.5f + 0.5f*( Math.Max(skillLocator.primary.stock, 0) + skillLocator.primary.stockToConsume)));

                //Animations
                this.anim = this.GetModelAnimator();
                modelTrans = this.GetModelTransform();
                this.childLoc = modelTrans.GetComponent<ChildLocator>();

                String animStr = this.anim.GetCurrentAnimatorStateInfo(this.anim.GetLayerIndex("Gesture")).IsName("Throw1") ? "Throw2" : "Throw1";

                //int layerInd = anim.GetLayerIndex("Gesture");
                //if (anim.GetCurrentAnimatorStateInfo(layerInd).IsName("Throw1"))
                //{
                this.PlayCrossfade( "Gesture", animStr, "Throw.playbackRate", this.duration / (1f - returnIdlePercent), 0.2f );
                //} else
                //{
                //    PlayCrossfade("Gesture", "Throw1", "Throw.playbackRate", duration / (1f - returnIdlePercent), 0.2f);
                //}

                if( this.isAuthority )
                {
                    this.skin = this.characterBody.skinIndex;
                    this.GetTarget();
                }



                if( this.characterBody )
                {
                    this.characterBody.SetAimTimer( this.duration + 1f );
                }
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                //if( !scanned && fixedAge > duration * scanDelay * fireDelay )
                //{

                //}
                if( !this.hasFired && this.fixedAge > this.duration * fireDelay )
                {
                    this.FireOrb();
                }
                if( this.fixedAge > this.duration && this.isAuthority )
                {
                    if( this.inputBank && this.skillLocator && this.inputBank.skill1.down && this.skillLocator.primary.stock >= this.skillLocator.primary.skillDef.requiredStock && !this.characterBody.isSprinting )
                    {
                        switch( this.state )
                        {
                            case WispPrimaryAnimState.First:
                                this.state = WispPrimaryAnimState.Right;
                                break;
                            case WispPrimaryAnimState.Left:
                                this.state = WispPrimaryAnimState.Right;
                                break;
                            case WispPrimaryAnimState.Right:
                                this.state = WispPrimaryAnimState.Left;
                                break;
                        }
                        this.outer.SetNextState( new TestPrimary
                        {
                            state = state
                        } );


                        return;

                    } else
                    {
                        this.PlayCrossfade( "Gesture", "Idle", 0.2f );
                        this.outer.SetNextStateToMain();
                    }

                    return;
                }
            }

            public override void OnExit()
            {
                base.OnExit();
                this.FireOrb();
            }

            public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Skill;

            public override void OnSerialize( NetworkWriter writer )
            {
                base.OnSerialize( writer );
                writer.Write( HurtBoxReference.FromHurtBox( this.target ) );
                writer.Write( this.targetVec );
                writer.Write( this.skin );
            }

            public override void OnDeserialize( NetworkReader reader )
            {
                base.OnDeserialize( reader );
                this.target = reader.ReadHurtBoxReference().ResolveHurtBox();
                this.targetVec = reader.ReadVector3();
                this.skin = reader.ReadUInt32();
                this.synced = true;
            }

            private void GetTarget()
            {
                this.scanned = true;

                Ray r = this.GetAimRay();

                this.search.teamMaskFilter = TeamMask.all;
                this.search.teamMaskFilter.RemoveTeam( TeamComponent.GetObjectTeam( this.gameObject ) );
                this.search.filterByLoS = true;
                this.search.searchOrigin = r.origin;
                this.search.searchDirection = r.direction;
                this.search.sortMode = BullseyeSearch.SortMode.DistanceAndAngle;
                this.search.maxDistanceFilter = maxRange;
                this.search.maxAngleFilter = maxAngle;
                this.search.RefreshCandidates();
                this.target = this.search.GetResults().FirstOrDefault<HurtBox>();

                RaycastHit rh;

                if( Physics.Raycast( r, out rh, maxRange, LayerIndex.world.mask | LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal ) )
                {
                    this.targetVec = rh.point;
                } else
                {
                    this.targetVec = r.GetPoint( maxRange );
                }
            }

            private void FireOrb()
            {
                if( this.hasFired ) return;
                this.passive.AddCharge( this.chargeAdded );
                this.hasFired = true;
                if( !NetworkServer.active ) return;

                SnapOrb arrow = new SnapOrb();

                if( !this.synced )
                {
                    this.GetTarget();
                    this.skin = this.characterBody.skinIndex;
                }

                arrow.damage = this.damageStat * (damageRatio + (chargeScaler * (Single)(this.passive.ReadCharge() - 100) / 100));
                arrow.crit = this.RollCrit();
                arrow.team = TeamComponent.GetObjectTeam( this.gameObject );
                arrow.attacker = this.gameObject;
                arrow.procCoef = 1.0f;
                arrow.radius = radius;
                arrow.skin = this.skin;

                Transform trans = this.childLoc.FindChild("MuzzleRight");

                //Muzzle flash

                arrow.origin = trans.position;

                arrow.speed = 150f;

                if( this.target )
                {
                    arrow.target = this.target;
                    arrow.useTarget = true;
                    this.targetVec = this.target.transform.position;
                } else
                {
                    arrow.useTarget = false;
                }

                arrow.targetPos = this.targetVec;


                OrbManager.instance.AddOrb( arrow );
            }
        }
    }
#endif
}