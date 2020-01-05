using EntityStates;
using System;

namespace RogueWispPlugin
{
    internal partial class Main
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

                    if( base.isAuthority )
                    {
                        base.outer.SetNextStateToMain();
                    }
                }
            }

            public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Any;
        }
    }
}