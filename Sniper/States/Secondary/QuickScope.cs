namespace Rein.Sniper.States.Secondary
{
    using System;

    using Rein.Sniper.Data;
    using Rein.Sniper.States.Bases;

    using UnityEngine;

    internal class QuickScope : ScopeBaseState
    {
        private const Single baseStartDelay = 0.35f;
        private const Single damageMultiplier = 0.75f;

        internal override Single currentCharge => 0f;
        internal override Boolean isReady { get => base.shouldRunDelay && this.delayTimer >= this.startDelay; }
        internal override Single readyFrac { get => Mathf.Clamp01( this.delayTimer / this.startDelay ); }

        private Single startDelay;
        private Single delayTimer;

        public override void OnEnter()
        {
            base.OnEnter();

            this.startDelay = baseStartDelay / base.attackSpeedStat;
            this.delayTimer = 0f;
            base.StartAimMode( 2f );
        }

        internal override Boolean OnFired() => base.fixedAge >= this.startDelay;
        internal override BulletModifier ReadModifier()
        {
            var mod = new BulletModifier
            {
                damageMultiplier = damageMultiplier,
                charge = 0.35f,

            };

            return base.fixedAge >= this.startDelay ? mod : default;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(base.shouldRunDelay)
            {
                if(this.delayTimer < this.startDelay)
                {
                    this.delayTimer += Time.fixedDeltaTime;
                } else
                {
                    this.delayTimer = this.startDelay;
                }
            } else
            {
                this.delayTimer = 0f;
            }
            base.characterBody.SetAimTimer( 2f );
        }

        internal override void ResetCharge() { }
    }
}
