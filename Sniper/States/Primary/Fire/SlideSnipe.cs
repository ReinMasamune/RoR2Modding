namespace Sniper.States.Primary.Fire
{
    using System;

    using Sniper.Expansions;
    using Sniper.States.Bases;

    internal class SlideSnipe : SnipeBaseState
    {
        private const Single damageRatio = 2.6f;
        private const Single force = 500f;

        protected sealed override Single baseDuration { get; } = 0.2f;
        protected sealed override Single recoilStrength { get; } = 4f;

        protected override void ModifyBullet( ExpandableBulletAttack bullet )
        {
            bullet.damage *= damageRatio;
            bullet.force = force;
        }
    }
}
