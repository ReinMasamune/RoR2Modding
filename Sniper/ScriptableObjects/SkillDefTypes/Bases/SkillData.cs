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

    internal abstract class SkillData
    {
        internal abstract Boolean IsDataValid();
    }
}
