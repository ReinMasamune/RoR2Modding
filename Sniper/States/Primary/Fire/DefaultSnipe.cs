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
    internal class DefaultSnipe : SnipeBaseState
    {
        const Single damageRatio = 2.5f;
        const Single force = 1000f;


        protected sealed override Single baseDuration { get; } = 0.2f;
        protected sealed override Single recoilStrength { get; } = 4f;


        // TODO: Implement State
        protected override void ModifyBullet( ExpandableBulletAttack bullet )
        {
            bullet.damage *= damageRatio;
            bullet.force = force;
        }
    }
}
