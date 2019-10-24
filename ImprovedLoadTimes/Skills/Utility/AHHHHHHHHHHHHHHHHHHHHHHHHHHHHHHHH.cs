using EntityStates;
using UnityEngine;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using R2API;
using R2API.Utils;

namespace ImprovedLoadTimes.Skills.Utility
{
    public class AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH : EntityStates.Loader.ChargeFist
    {
        private bool noConsequitiveNormalPunches = false;
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
            if( !noConsequitiveNormalPunches )
            {
                noConsequitiveNormalPunches = true;
                float chrg = ((EntityStates.Loader.ChargeFist)this).GetFieldValue<float>("charge");
                Debug.Log(chrg);
                outer.SetState(new ImprovedLoadTimes.Skills.Utility.ONEPUNCH
                {
                    charge = chrg
                });

            }

        }
    }
}
