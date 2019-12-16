using EntityStates;
using System;

namespace WispSurvivor.Skills.Primary
{
    public class HeatwaveWindDown : BaseState
    {
        public static Single time = 0.5f;

        public override void OnEnter() => base.OnEnter();

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if( base.fixedAge > 0.75f )
            {
                base.PlayCrossfade( "Gesture", "Idle", time / base.attackSpeedStat );

                if( base.hasAuthority )
                {
                    base.outer.SetNextStateToMain();
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Any;
    }
}
