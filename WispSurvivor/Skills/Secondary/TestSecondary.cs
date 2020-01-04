using EntityStates;
using RoR2;
using RoR2.Orbs;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace RogueWispPlugin.Skills.Secondary
{
    public class TestSecondary : BaseState
    {
        public static Double chargeUsed = 15.0;

        public static Single baseDuration = 1f;
        public static Single scanDelay = 0.25f;
        public static Single fireDelay = 0.5f;
        public static Single damageRatio = 3.0f;
        public static Single chargeScaler = 0.75f;
        public static Single radius = 8f;
        public static Single returnIdlePercent = 0.5f;

        private Single duration;

        private UInt32 skin = 0;

        private Boolean hasFired = false;

        private ChildLocator childLoc;
        private Animator anim;
        private Components.WispPassiveController passive;

        public override void OnEnter()
        {
            base.OnEnter();

            this.passive = this.gameObject.GetComponent<Components.WispPassiveController>();

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
            this.PlayCrossfade( "Gesture", "Idle", 0.2f );
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
            Components.WispPassiveController.ChargeState chargeState = this.passive.UseCharge( chargeUsed, chargeScaler );
            this.PlayCrossfade( "Gesture", "FireBomb", "ChargeBomb.playbackRate", this.duration * (1f - fireDelay), 0.2f );
            this.hasFired = true;
            if( !NetworkServer.active )
            {
                return;
            }

            Vector3 dir = this.GetAimRay().direction;
            dir.y = 0f;
            dir = Vector3.Normalize( dir );

            Orbs.SparkOrb nextOrb = new Orbs.SparkOrb();

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
            nextOrb.innerRadScale = 0.5f;
            nextOrb.stepsLeft = 1 + (Int32)Math.Truncate( chargeState.chargeConsumed / 10.0 );
            nextOrb.team = TeamComponent.GetObjectTeam( this.gameObject );
            nextOrb.skin = this.skin;

            OrbManager.instance.AddOrb( nextOrb );
        }
    }
}
