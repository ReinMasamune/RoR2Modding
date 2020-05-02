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
using Sniper.Data;
using Sniper.States.Bases;

namespace Sniper.States.Secondary
{
    internal class QuickScope : ScopeBaseState
    {
        const Single damageMultiplier = 3f;

        internal override Boolean usesCharge { get; } = false;
        internal override Boolean usesStock { get; } = true;
        internal override Single currentCharge { get; }
        internal override UInt32 currentStock { get; }
        // TODO: Implement

        internal override void OnFired()
        {

        }
        internal override BulletModifier ReadModifier()
        {
            var mod = new BulletModifier();
            mod.damageMultiplier = damageMultiplier;
            return default;
        }
    }
}
