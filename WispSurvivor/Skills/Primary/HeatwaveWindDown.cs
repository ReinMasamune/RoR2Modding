using RoR2;
using EntityStates;
using UnityEngine;
using System;
using RoR2.Orbs;
using UnityEngine.Networking;
using System.Linq;

namespace WispSurvivor.Skills.Primary
{
    public class HeatwaveWindDown : BaseState
    {
        public static float time = 0.3f;

        public override void OnEnter()
        {
            base.OnEnter();
            PlayCrossfade("Gesture", "Idle", time);

            if( isAuthority )
            {
                outer.SetNextStateToMain();
            }


        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }
}
