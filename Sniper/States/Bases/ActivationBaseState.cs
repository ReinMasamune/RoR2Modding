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
using Sniper.Components;
using Sniper.SkillDefTypes.Bases;

namespace Sniper.States.Bases
{
    internal abstract class ActivationBaseState<TSkillData> : SniperSkillBaseState where TSkillData : SkillData, new()
    {
        internal TSkillData data;
        internal abstract TSkillData CreateSkillData();
    }
}
