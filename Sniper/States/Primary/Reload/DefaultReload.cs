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
using Sniper.Modules;

namespace Sniper.States.Primary.Reload
{
    internal class DefaultReload : SniperSkillBaseState, ISniperReloadState
    {
        const Single baseDuration = 0.3f;


        private Single duration;

        public ReloadTier reloadTier { get; set; }


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / this.attackSpeedStat;

            base.PlayAnimation( "Gesture, Additive", "Reload" );
            SoundModule.PlayLoad( base.gameObject, this.reloadTier );
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
