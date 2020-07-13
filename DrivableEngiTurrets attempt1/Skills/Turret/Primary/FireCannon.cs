using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engi133769OMEGA.Skills.Turret.Primary
{
    public class FireCannon : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
