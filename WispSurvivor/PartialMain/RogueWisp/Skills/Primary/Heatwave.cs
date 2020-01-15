using EntityStates;
using RoR2;
using System;
using UnityEngine;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        public class Heatwave : BaseState, RoR2.Skills.SteppedSkillDef.IStepSetter
        {
            public enum Direction
            {
                Left = 0,
                Right = 1
            }

            public void SetStep( Int32 i ) => this.state = (Direction)i;

            public Direction state = Direction.Right;

            public static Single baseDuration = 0.375f;
            public static Single fireStartFrac = 0.35f;
            public static Single maxRange = 75f;
            public static Single noStockSpeedMult = 0.5f;
            public static Single damageMult = 3.5f;
            public static Single radius = 2.0f;
            public static Single falloffStart = 0.35f;
            public static Single endFalloffMult = 0.25f;

            private Single duration;
            private Single fireDelay;
            private Single damageValue;

            private Boolean hasFired = false;
            private Boolean hitWorld = false;

            private Vector3 targetVec;
            private Vector3 targetNormal;

            private UInt32 skin = 0;

            private ChildLocator childLoc;
            private Animator anim;
            private WispPassiveController passive;
            private ClientOrbController orbControl;

            public override void OnEnter()
            {
                base.OnEnter();
                Transform modelTrans = base.GetModelTransform();
                this.passive = base.gameObject.GetComponent<WispPassiveController>();
                this.orbControl = base.gameObject.GetComponent<ClientOrbController>();

                Boolean hasStock = base.skillLocator.primary.stock > 0;
                base.skillLocator.primary.stock = hasStock ? base.skillLocator.primary.stock - 1 : 0;
                base.skillLocator.primary.rechargeStopwatch = 0f;

                Single effectiveAttackSpeed = base.attackSpeedStat * ( hasStock ? 1.0f : noStockSpeedMult );

                this.duration = baseDuration / effectiveAttackSpeed;
                this.fireDelay = this.duration * fireStartFrac;
                this.damageValue = base.damageStat * damageMult;

                this.anim = base.GetModelAnimator();
                String animStr = this.state != Direction.Right ? "Throw2" : "Throw1";
                base.PlayCrossfade( "Gesture", animStr, "Throw.playbackRate", this.duration * 5f, 0.35f );
                RoR2.Util.PlaySound( "Play_huntress_m2_throw", base.gameObject );

                base.characterBody.SetAimTimer( this.duration + 1f );
                base.characterBody.isSprinting = false;
                this.skin = base.characterBody.skinIndex;
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                if( !this.hasFired )
                {
                    if( base.fixedAge >= this.fireDelay )
                    {
                        this.FireOrb();
                    }
                } else
                {
                    if( base.fixedAge >= this.duration )
                    {
                        if( base.isAuthority )
                        {
                            base.outer.SetNextState( new HeatwaveWindDown() );
                        }
                    }
                }
            }

            public override void OnExit()
            {
                base.OnExit();
                if( !this.hasFired )
                {
                    this.FireOrb();
                }
            }

            public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Skill;

            private void FireOrb()
            {
                if( this.hasFired ) return;
                this.hasFired = true;
                if( !this.isAuthority ) return;

                this.GetTarget();

                HeatwaveClientOrb snap = new HeatwaveClientOrb();

                snap.damage = this.damageValue;
                snap.crit = this.RollCrit();
                snap.team = TeamComponent.GetObjectTeam( base.gameObject );
                snap.attacker = base.gameObject;
                snap.procCoef = 1.0f;
                snap.radius = radius;
                snap.skin = this.skin;
                Transform trans = base.FindModelChild("MuzzleRight");
                snap.startPos = trans.position;
                snap.speed = 250f;
                snap.targetPos = this.targetVec;
                snap.chargeRestore = 5f;
                snap.force = 100f;
                snap.range = maxRange;
                snap.falloffStart = falloffStart;
                snap.endFalloffMult = endFalloffMult;

                this.orbControl.AddClientOrb( snap );
            }

            private void GetTarget()
            {
                Ray r = base.GetAimRay();

                RaycastHit rh;

                if( Physics.Raycast( r, out rh, maxRange, LayerIndex.world.mask | LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal ) )
                {
                    r.direction = rh.point - r.origin;
                    if( Physics.Raycast( r, out rh, maxRange, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ) )
                    {
                        this.targetVec = rh.point;
                        this.hitWorld = true;
                        this.targetNormal = rh.normal;
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
#endif
}