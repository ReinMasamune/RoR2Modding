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

namespace Sniper.Skills
{
    internal abstract class SniperSkillBaseState : BaseSkillState
    {
        protected new SniperCharacterBody characterBody
        {
            get => base.characterBody as SniperCharacterBody;
        }
    }
}
