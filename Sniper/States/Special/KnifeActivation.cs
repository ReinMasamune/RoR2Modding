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
using Sniper.SkillDefs;
using Sniper.States.Bases;

namespace Sniper.States.Special
{
    internal class KnifeActivation : ActivationBaseState<KnifeSkillData>
    {
        // TODO: Implement
        internal override KnifeSkillData CreateSkillData()
        {
            base.data = new KnifeSkillData();
            return base.data;
        }
    }
}
