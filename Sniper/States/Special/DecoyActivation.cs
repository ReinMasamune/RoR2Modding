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
    internal class DecoyActivation : ActivationBaseState<DecoySkillData>
    {
        // TODO: Implement
        internal override DecoySkillData CreateSkillData()
        {
            base.data = new DecoySkillData();
            return base.data;
        }
    }
}
