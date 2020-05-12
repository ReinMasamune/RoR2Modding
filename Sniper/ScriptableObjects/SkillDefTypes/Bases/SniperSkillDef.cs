namespace Sniper.SkillDefTypes.Bases
{
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
    using Sniper.States.Bases;

    internal class SniperSkillDef : SkillDef
    {
        internal static SniperSkillDef Create<TActivationState>( String stateMachineName ) where TActivationState : EntityState
        {
            SniperSkillDef def = CreateInstance<SniperSkillDef>();
            def.activationState = SkillsCore.StateType<TActivationState>();
            def.activationStateMachineName = stateMachineName;

            return def;
        }

    }
}
