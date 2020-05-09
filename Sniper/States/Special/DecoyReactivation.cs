using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using RoR2.Networking;
using UnityEngine;
using KinematicCharacterController;
using EntityStates;
using RoR2.Skills;
using System.Reflection;
using Sniper.Expansions;
using Sniper.Enums;
using Sniper.States.Bases;
using Sniper.SkillDefs;
using UnityEngine.Networking;
using Sniper.Modules;

namespace Sniper.States.Special
{
    internal class DecoyReactivation : ReactivationBaseState<DecoySkillData>
    {
        public unsafe override void OnEnter()
        {
            Log.Warning( "Reactivation OnEnter" );
            base.OnEnter();

            if( NetworkServer.active )
            {
                Log.Warning( "Reactiv server" );
                var list = base.characterBody?.master?._GetDeployablesList();
                foreach( var v in list )
                {
                    if( v.slot == DecoyModule.deployableSlot )
                    {
                        Log.Warning( "Match found" );
                        var dep = v.deployable;
                        if( dep == null )
                        {
                            Log.Warning( "Null Deployable" );
                            continue;
                        }
                        var obj = dep.gameObject;
                        if( obj == null )
                        {
                            Log.Warning( "Null obj" );
                            continue;
                        }
                        var master = obj.GetComponent<CharacterMaster>();
                        if( master )
                        {
                            master.TrueKill();
                            Log.Warning( "Master killed" );
                        }
                    }
                }
            }

            if( base.isAuthority )
            {
                base.outer.SetNextStateToMain();
            }
        }
    }
}
