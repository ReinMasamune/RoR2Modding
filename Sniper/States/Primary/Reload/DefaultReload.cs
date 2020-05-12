namespace Sniper.States.Primary.Reload
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
    using Sniper.States.Bases;
    using Sniper.Enums;
    using Sniper.Modules;

    internal class DefaultReload : SniperSkillBaseState, ISniperReloadState
    {
        const Single baseDuration = 0.75f;


        private Single duration;

        public ReloadTier reloadTier { get; set; }

        private Transform gunTransform;
        private readonly Transform origParent;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / this.attackSpeedStat;

            base.PlayAnimation( "Gesture, Additive", "Reload", "rateReload", this.duration );
            SoundModule.PlayLoad( base.gameObject, this.reloadTier );

            this.gunTransform = base.FindModelChild( "RailgunBone" );
            this.gunTransform.SetParent( base.FindModelChild( "LeftWeapon" ), true );
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if( base.isAuthority && base.fixedAge >= this.duration )
            {
                base.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            this.gunTransform.SetParent( base.FindModelChild( "RailgunDefaultPosition" ), true );
            this.gunTransform.localPosition = Vector3.zero;
            this.gunTransform.localRotation = Quaternion.identity;
            this.gunTransform.localScale = Vector3.one;
        }

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.PrioritySkill;
    }
}
