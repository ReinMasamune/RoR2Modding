namespace Sniper.States.Secondary
{
    using System;

    using Sniper.Data;
    using Sniper.States.Bases;

    internal class QuickScope : ScopeBaseState
    {
        private const Single baseStartDelay = 0.25f;
        private const Single damageMultiplier = 2f;


        internal override Boolean usesCharge { get; } = false;
        internal override Boolean usesStock { get; } = true;
        internal override Single currentCharge { get; }
        internal override UInt32 currentStock { get; }

        private Single startDelay;

        public override void OnEnter()
        {
            base.OnEnter();

            this.startDelay = baseStartDelay / base.attackSpeedStat;
        }

        internal override Boolean OnFired() => base.fixedAge >= this.startDelay;
        internal override BulletModifier ReadModifier()
        {
            var mod = new BulletModifier
            {
                damageMultiplier = damageMultiplier,
                charge = 0.25f,

            };

            return base.fixedAge >= this.startDelay ? mod : default;
        }
    }
}
