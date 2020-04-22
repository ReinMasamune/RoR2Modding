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
using Sniper.SkillDefTypes.Bases;
using Sniper.States.Bases;

namespace Sniper.SkillDefs
{
    internal class KnifeSkillDef : ReactivatedSkillDef<KnifeSkillData>
    {
        internal static KnifeSkillDef Create<TActivation, TReactivation>( String activationMachine, String reactivationMachine )
            where TActivation : ActivationBaseState<KnifeSkillData>
            where TReactivation : ReactivationBaseState<KnifeSkillData>
        {
            var def = ScriptableObject.CreateInstance<KnifeSkillDef>();
            def.InitStates<TActivation, TReactivation>( activationMachine, reactivationMachine );
            return def;
        }
    }
}
