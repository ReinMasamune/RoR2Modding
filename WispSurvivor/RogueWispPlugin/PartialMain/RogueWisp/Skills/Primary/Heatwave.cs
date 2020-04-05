#if ROGUEWISP
using EntityStates;
using RoR2;
using System;
using UnityEngine;

namespace Rein.RogueWispPlugin
{

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

            const Single baseDuration = 0.875f;
            const Single fireStartFrac = 0.35f;
            public const Single maxRange = 80f;
            const Single noStockSpeedMult = 0.5f;
            const Single damageMult = 1.65f;
            const Single radius = 2.0f;
            const Single falloffStart = 0.25f;
            const Single endFalloffMult = 0.35f;

            private Single duration;
            private Single fireDelay;
            private Single damageValue;

            private Boolean hasFired = false;
            private Boolean hitWorld = false;

            private Vector3 targetVec;
            private Vector3 targetNormal;

            private UInt32 skin = 0;

            private readonly ChildLocator childLoc;
            private Animator anim;
            private WispPassiveController passive;
            private ClientOrbController orbControl;

            private Boolean isDoppleganger;

            public override void OnEnter()
            {
                base.OnEnter();

                Transform modelTrans = base.GetModelTransform();
                this.passive = base.gameObject.GetComponent<WispPassiveController>();
                this.orbControl = base.gameObject.GetComponent<ClientOrbController>();

                //Boolean hasStock = base.skillLocator.primary.stock > 0;
                //base.skillLocator.primary.stock = hasStock ? base.skillLocator.primary.stock - 1 : 0;
                //base.skillLocator.primary.rechargeStopwatch = 0f;

                //Single effectiveAttackSpeed = base.attackSpeedStat * ( hasStock ? 1.0f : noStockSpeedMult );
                Single effectiveAttackSpeed = base.attackSpeedStat;

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

                this.isDoppleganger = base.characterBody.inventory.GetItemCount( ItemIndex.InvadingDoppelganger ) != 0;
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
                    base.PlayCrossfade( "Gesture", "Idle", 0.5f );
                }
            }

            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.Skill;
            }

            private void FireOrb()
            {
                if( this.hasFired ) return;
                this.hasFired = true;
                if( !this.isAuthority ) return;
                //this.GetTarget();

                HeatwaveClientOrb snap = new HeatwaveClientOrb();

                var aim = base.GetAimRay();

                snap.damage = this.damageValue;
                snap.crit = this.RollCrit();
                snap.team = TeamComponent.GetObjectTeam( base.gameObject );
                snap.attacker = base.gameObject;
                snap.procCoef = 1.0f;
                snap.radius = radius * ( this.isDoppleganger ? 0.65f : 1f );
                snap.skin = this.skin;
                Transform trans = base.FindModelChild("MuzzleRight");
                snap.startPos = trans.position;
                snap.speed = 250f * ( this.isDoppleganger ? 0.35f : 1f );
                //snap.targetPos = this.targetVec;
                snap.useTargetPos = false;
                snap.origin = aim.origin;
                snap.direction = aim.direction;
                snap.chargeRestore = 5f / Mathf.Sqrt(Mathf.Max(1f, base.attackSpeedStat ) );
                snap.force = 100f;
                snap.range = maxRange;
                snap.falloffStart = falloffStart;
                snap.endFalloffMult = endFalloffMult;
                snap.attackerBody = base.characterBody;
                snap.stopAtWorld = true;
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

}
#endif