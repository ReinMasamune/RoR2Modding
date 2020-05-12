#if ROGUEWISP
using System;

using EntityStates;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        public class HeatwaveWindDown : BaseState
        {
            const Single time = 0.5f;

            public override void OnEnter() => base.OnEnter();

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                if( base.fixedAge > 0.75f )
                {
                    if( base.isAuthority )
                    {
                        base.outer.SetNextStateToMain();
                    }
                }
            }

            public override void OnExit()
            {
                base.PlayCrossfade( "Gesture", "Idle", time / base.attackSpeedStat );
                base.OnExit();
            }

            public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Any;
        }
    }

}
#endif