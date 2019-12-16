using Engi133769OMEGA.Components;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Engi133769OMEGA.Skills.Other
{
    public class DriverState : EntityStates.GenericCharacterVehicleSeated
    {

        public override void OnEnter()
        {
            base.OnEnter();
            if( NetworkServer.active )
            {
                characterBody.AddBuff( BuffIndex.HiddenInvincibility );
                characterBody.AddBuff( BuffIndex.Cloak );
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if( base.skillLocator && base.skillLocator.special)
            {
                base.skillLocator.special.rechargeStopwatch = 0f;
            }

        }

        public override void OnExit()
        {
            base.OnExit();
            if( NetworkServer.active )
            {
                characterBody.RemoveBuff( BuffIndex.HiddenInvincibility );
                characterBody.RemoveBuff( BuffIndex.Cloak );
            }
        }


        // TODO: Relay input stuff to turret
        // TODO: Relay turret stuff to engi
    }
}
