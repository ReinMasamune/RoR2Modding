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
using Sniper.States.Bases;
using Sniper.SkillDefs;
using UnityEngine.Networking;

namespace Sniper.States.Special
{
    internal class DecoyReactivation : ReactivationBaseState<DecoySkillData>
    {
        public override void OnEnter()
        {
            base.OnEnter();

            if( NetworkServer.active )
            {
                base.characterBody?.master?.minionOwnership?.gr
            }
        }
    }
}
