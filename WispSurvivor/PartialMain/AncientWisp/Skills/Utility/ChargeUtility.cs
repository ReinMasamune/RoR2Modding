using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace RogueWispPlugin
{
#if ANCIENTWISP
    internal partial class Main
    {
        internal class AWChargeUtility : BaseState
        {
            const Single baseChargeTime = 5.0f;

            private Single duration;


            public override void OnEnter()
            {
                base.OnEnter();

                this.duration = baseChargeTime / base.attackSpeedStat;

                //Effect
                //Animation
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();

                if( base.fixedAge >= this.duration && base.isAuthority )
                {
                    base.outer.SetNextState( new AWFireUtility() );
                    return;
                }
            }

            public override void OnExit()
            {
                //Destroy effects
                base.OnExit();
            }

            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.PrioritySkill;
            }
        }
    }
#endif
}
