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
using Sniper.Enums;

namespace Sniper.States.Primary.Reload
{
    internal class MagReload : SniperSkillBaseState, ISniperReloadState
    {
        const Single baseDuration = 0.75f;


        private Single duration;

        public ReloadTier reloadTier { get; set; }

        // TODO: Implement state

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / this.attackSpeedStat;

            // TODO: Play Animation
            // TODO: Play Sound
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if( base.isAuthority && base.fixedAge >= this.duration )
            {
                base.outer.SetNextStateToMain();
            }
        }
    }
}
