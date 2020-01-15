using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;
using UnityEngine.Networking;

namespace ReinSniperRework
{
    internal partial class Main
    {
        internal class SniperFireSidearm : BaseState
        {
            const Single baseDuration = 0.2f;


            private Single duration;
            public override void OnEnter()
            {
                base.OnEnter();

                base.characterBody.SetSpreadBloom( 0.2f, false );
                this.duration = baseDuration / base.attackSpeedStat;

                base.PlayAnimation( "Gesture Additive, Right", "FirePistol, Right" );
            }
        }
    }
}


