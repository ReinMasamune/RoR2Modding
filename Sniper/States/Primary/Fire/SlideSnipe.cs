using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using RoR2.Networking;
using UnityEngine;
using KinematicCharacterController;
using EntityStates;
using RoR2.Skills;
using System.Reflection;
using Sniper.Expansions;
using Sniper.Enums;
using Sniper.States.Bases;

namespace Sniper.States.Primary.Fire
{
    internal class SlideSnipe : SnipeBaseState
    {
        const Single damageRatio = 2.75f;
        const Single force = 500f;

        protected sealed override Single baseDuration { get; } = 0.2f;
        protected sealed override Single recoilStrength { get; } = 4f;

        protected override void ModifyBullet( ExpandableBulletAttack bullet )
        {
            bullet.damage *= damageRatio;
            bullet.force = force;
        }
    }
}
