#if ROGUEWISP
using System;

using EntityStates;

using RoR2;
using RoR2.Orbs;

using UnityEngine;
using UnityEngine.Networking;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        public class TestSecondary : BaseState
        {
            public const Double chargeUsed = 25.0;

            public const Single baseDuration = 0.75f;
            public const Single scanDelay = 0.25f;
            public const Single fireDelay = 0.5f;
            public const Single damageRatio = 2.5f;
            public const Single chargeScaler = 0.65f;
            public const Single radius = 5f;
            public const Single returnIdlePercent = 0.5f;

            private Single duration;

            private UInt32 skin = 0;

            private Boolean hasFired = false;

            private ChildLocator childLoc;
            private Animator anim;
            private WispPassiveController passive;

            public override void OnEnter()
            {
                base.OnEnter();

                this.passive = this.gameObject.GetComponent<WispPassiveController>();

                Transform modelTrans = base.GetModelTransform();
                //Sound

                this.duration = baseDuration / this.attackSpeedStat;

                //Animations

                if( modelTrans )
                {
                    this.childLoc = modelTrans.GetComponent<ChildLocator>();
                    this.anim = modelTrans.GetComponent<Animator>();
                }

                if( this.anim )
                {
                    this.PlayCrossfade( "Gesture", "ChargeBomb", "ChargeBomb.playbackRate", this.duration * fireDelay, 0.2f );
                }

                if( this.characterBody )
                {
                    this.characterBody.SetAimTimer( this.duration + 1f );
                }

                if( this.isAuthority )
                {
                    this.skin = this.characterBody.skinIndex;
                }
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                if( !this.hasFired && this.fixedAge > this.duration * fireDelay )
                {
                    this.FireOrb();
                }
                if( this.fixedAge > this.duration && this.isAuthority )
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }

            public override void OnExit()
            {
                base.OnExit();
                this.FireOrb();
                //this.PlayCrossfade( "Gesture", "Idle", 0.2f );
            }

            public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.PrioritySkill;

            public override void OnSerialize( NetworkWriter writer )
            {
                if( this.isAuthority )
                {
                    writer.Write( this.skin );
                }
            }

            public override void OnDeserialize( NetworkReader reader )
            {
                if( !this.isAuthority )
                {
                    this.skin = reader.ReadUInt32();
                }
            }

            private void FireOrb()
            {
                if( this.hasFired ) return;
                WispPassiveController.ChargeState chargeState = this.passive.UseCharge( chargeUsed, chargeScaler );
                this.PlayCrossfade( "Gesture", "FireBomb", "ChargeBomb.playbackRate", this.duration * ( 1f - fireDelay ), 0.2f );
                this.hasFired = true;
                if( !NetworkServer.active )
                {
                    return;
                }

                Vector3 dir = this.GetAimRay().direction;
                dir.y = 0f;
                dir = Vector3.Normalize( dir );

                SparkOrb nextOrb = new SparkOrb();

                nextOrb.attacker = this.gameObject;
                nextOrb.crit = this.RollCrit();
                nextOrb.damage = this.damageStat * damageRatio * chargeState.chargeScaler;
                nextOrb.damageColor = DamageColorIndex.Default;
                nextOrb.direction = dir;
                nextOrb.origin = this.GetAimRay().origin;
                nextOrb.procCoef = 1.0f;
                nextOrb.isFirst = true;
                nextOrb.radius = radius;
                nextOrb.scale = 1.0f;
                nextOrb.speed = 50.0f;
                nextOrb.stepDist = 8.0f;
                nextOrb.stepHeight = 5.0f;
                nextOrb.maxFall = 25f;
                nextOrb.innerRadScale = 0.25f;
                nextOrb.edgePenaltyMult = 0.35f;
                nextOrb.stepsLeft = 1 + (Int32)Math.Truncate( chargeState.chargeConsumed / 7.5 );
                nextOrb.team = TeamComponent.GetObjectTeam( this.gameObject );
                nextOrb.skin = this.skin;

                OrbManager.instance.AddOrb( nextOrb );
            }
        }
    }

}
#endif