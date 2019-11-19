using EntityStates;
using System;

namespace WispSurvivor.Skills.Primary
{
    public class HeatwaveWindDown : BaseState
    {
        public static Single time = 0.3f;

        public override void OnEnter()
        {
            base.OnEnter();
            this.PlayCrossfade( "Gesture", "Idle", time );

            if( this.isAuthority )
            {
                this.outer.SetNextStateToMain();
            }


        }

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Any;
    }
}
