namespace Sniper.States.Primary.Reload
{
    using System;
    using EntityStates;
    using Sniper.Enums;
    using Sniper.States.Bases;

    internal class MagReload : SniperSkillBaseState, ISniperReloadState
    {
        private const Single baseDuration = 0.75f;


        private Single duration;

        public ReloadTier reloadTier { get; set; }


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
        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.PrioritySkill;
    }
}
