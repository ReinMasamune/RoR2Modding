namespace Sniper.States.Secondary
{
    using System;

    using Sniper.Data;
    using Sniper.States.Bases;

    using UnityEngine;

    internal class QuickScope : ScopeBaseState
    {
        private const Single baseStartDelay = 0.35f;
        private const Single damageMultiplier = 0.75f;

        internal override Single currentCharge { get; }
        internal override Boolean isReady { get => base.fixedAge >= this.startDelay; }
        internal override Single readyFrac { get => Mathf.Clamp01( base.fixedAge / this.startDelay ); }

        private Single startDelay;

        public override void OnEnter()
        {
            base.OnEnter();

            this.startDelay = baseStartDelay / base.attackSpeedStat;
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
            base.characterBody.SetAimTimer( 2f );
        }
    }
}
