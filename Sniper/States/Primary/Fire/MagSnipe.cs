﻿//namespace Sniper.States.Primary.Fire
//{
//    using System;

//    using Sniper.Expansions;
//    using Sniper.States.Bases;

//    internal class MagSnipe : SnipeBaseState
//    {
//        private const Single damageRatio = 2f;
//        private const Single force = 100f;


//        protected sealed override Single baseDuration { get; } = 0.2f;
//        protected sealed override Single recoilStrength { get; } = 4f;

//        protected override void ModifyBullet( ExpandableBulletAttack bullet )
//        {
//            bullet.damage *= damageRatio;
//            bullet.force = force;
//        }
//    }
//}
