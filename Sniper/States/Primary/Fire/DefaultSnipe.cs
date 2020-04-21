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

namespace Sniper.Skills
{
    internal class DefaultSnipe : SnipeBaseState
    {
        const Single damageRatio = 1f;
        const Single force = 100f;


        protected sealed override Single baseDuration { get; } = 0.2f;
        protected sealed override Single recoilStrength { get; } = 5f;


        // TODO: Implement State
        protected override void ModifyBullet( ExpandableBulletAttack bullet )
        {
            bullet.damage *= damageRatio;
            bullet.force = force;
        }
    }
}
