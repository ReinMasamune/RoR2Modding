namespace Rein.Sniper.States.Primary.Reload
{
    using System;
    using EntityStates;
    using Rein.Sniper.Enums;
    using Rein.Sniper.Modules;
    using Rein.Sniper.States.Bases;

    using UnityEngine;

    internal class MagReload : SniperSkillBaseState, ISniperReloadState
    {
        private const Single baseDuration = 0.4f;


        private Single duration;

        public ReloadTier reloadTier { get; set; }

        private Transform gunTransform;

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
