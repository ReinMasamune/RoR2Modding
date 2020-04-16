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
using Sniper.Skills;
using Sniper.Enums;
using Sniper.Data;

namespace Sniper.Skills
{
    internal abstract class ScopeBaseState : SniperSkillBaseState
    {
        internal SniperScopeSkillDef.ScopeInstanceData instanceData;

        internal abstract BulletModifier ReadModifier();
        internal abstract void OnFired();

        internal BulletModifier SendFired()
        {
            var mod = this.ReadModifier();
            this.OnFired();
            return mod;
        }
    }
}
