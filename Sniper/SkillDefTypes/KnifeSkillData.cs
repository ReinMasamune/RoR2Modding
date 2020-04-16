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
    internal class KnifeSkillData : SkillData
    {
        internal override Boolean IsDataValid()
        {
            // TODO: Implement
            return true;
        }
    }
}
