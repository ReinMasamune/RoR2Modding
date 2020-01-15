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
        internal class SniperPrepSidearm : BaseState
        {
            const Single basePrepDuration = 0.3f;


            private Single duration;


            public override void OnEnter()
            {
                base.OnEnter();
                this.duration = basePrepDuration / base.attackSpeedStat;

                base.PlayAnimation( "Gesture", "PrepBarrage", "PrepBarrage.playbackRate", this.duration );
                Util.PlaySound( EntityStates.Commando.CommandoWeapon.PrepBarrage.prepBarrageSoundString, base.gameObject );

                if( base.characterBody )
                {
                    base.characterBody.SetAimTimer( this.duration );
                }
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();

                if( base.fixedAge >= this.duration && base.isAuthority )
                {
                    base.outer.SetNextState( new SniperFireSidearm() );
                    return;
                }
            }

            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.PrioritySkill;
            }
        }
    }
}


