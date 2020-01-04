namespace AlternativeAcrid.States.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using EntityStates;

    public class PrepPoisonJump : BasePrepJump
    {
        public override EntityState NextState()
        {
            return new EntityStates.Croco.Leap();
        }
    }
}
