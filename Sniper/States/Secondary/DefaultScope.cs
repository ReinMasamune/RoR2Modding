namespace Sniper.States.Secondary
{
    using System;

    using RoR2;

    using Sniper.Data;
    using Sniper.States.Bases;

    using UnityEngine;
    using UnityEngine.Networking;

    internal class DefaultScope : ScopeBaseState
    {
        private const Single maxCharge = 1f;
        private const Single baseStartDelay = 0f;
        private const Single chargePerSecond = 0.15f;
        private const Single minModifier = 1f;
        private const Single maxModifier = 10f;
        private const Single speedScalar = 0.25f;

        private static readonly AnimationCurve damageCurve = new AnimationCurve
        (
            new Keyframe(0f, minModifier, 0f, 0f, 0f, 0.8f ),
            new Keyframe(0.3f, minModifier, 0f, 0f, 0f, 0.8f ),
            //new Keyframe(0.2f, minModifier, 0f, 0f ),
            new Keyframe(1f, maxModifier, 0f, 0f, 0f, 0f )
        );


        private Single startDelay;

        internal override Boolean usesCharge { get; } = true;
        internal override Boolean usesStock { get; } = true;
        internal override Single currentCharge { get => this.charge; }
        internal override UInt32 currentStock { get; } = 0u;
        internal Single charge = 0f;


        private Single delayTimer = 0f;

        internal override Boolean OnFired()
        {
            this.charge = 0f;
            this.delayTimer = 0f;
            return base.fixedAge >= this.startDelay;
        }
        internal override BulletModifier ReadModifier()
        {
            return base.fixedAge >= this.startDelay
                ? new BulletModifier
                {
                    damageMultiplier = this.GetDamageMultiplier(),
                    charge = this.charge,
                }
                : ( default );
        }


        public override void OnEnter()
        {
            base.OnEnter();
            this.startDelay = baseStartDelay / base.attackSpeedStat;
            base.StartAimMode( 2f );
            if( NetworkServer.active )
            {
                this.characterBody.AddBuff( BuffIndex.Slow50 );
            }
            this.charge = base.startingCharge;

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.SetAimTimer( 2f );
            if( this.delayTimer >= this.startDelay )
            {
                if( this.charge < maxCharge )
                {
                    this.charge += SniperMain.dt * chargePerSecond * this.characterBody.attackSpeed / ( 1f + ( base.characterMotor.velocity.magnitude * speedScalar ) );
                } else
                {
                    this.charge = maxCharge;
                }
            } else
            {
                this.delayTimer += SniperMain.dt;
            }
        }

        public override void OnExit()
        {
            if( NetworkServer.active )
            {
                base.characterBody.RemoveBuff( BuffIndex.Slow50 );
            }
            base.OnExit();
        }


        private Single GetDamageMultiplier() => damageCurve.Evaluate( this.charge / maxCharge );
    }
}
