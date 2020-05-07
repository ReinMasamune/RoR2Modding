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
using Sniper.SkillDefTypes.Bases;

namespace Sniper.SkillDefs
{
    internal class DecoySkillData : SkillData
    {
        internal override Boolean IsDataValid()
        {
            // TODO: Implement DecoySkillData.IsDataValid
            return true;
        }
    }
}
