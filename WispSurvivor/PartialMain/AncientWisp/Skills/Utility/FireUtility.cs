using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace RogueWispPlugin
{
#if ANCIENTWISP
    internal partial class Main
    {
        internal class AWFireUtility : BaseState
        {
            const Single baseDuration = 0.5f;
            const Single damageCoef = 1.0f;


            private Single duration;

            public override void OnEnter()
            {
                base.OnEnter();

                this.duration = baseDuration / base.attackSpeedStat;

                //Fire projectile
                //Animation
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();

                if( base.fixedAge >= this.duration && base.isAuthority )
                {
                    base.outer.SetNextStateToMain();
                }
            }

            public override void OnExit()
            {
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
