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

namespace Sniper.Skills
{
    internal class DecoySkillDef : ReactivatedSkillDef<DecoySkillData>
    {
        internal static DecoySkillDef Create<TActivation,TReactivation>( String activationMachine, String reactivationMachine )
            where TActivation : ActivationBaseState<DecoySkillData>
            where TReactivation : ReactivationBaseState<DecoySkillData>
        {
            var def = ScriptableObject.CreateInstance<DecoySkillDef>();
            def.InitStates<TActivation, TReactivation>( activationMachine, reactivationMachine );
            return def;
        }
    }
}
