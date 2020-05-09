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
            base.OnEnter();

            if( NetworkServer.active )
            {
                List<DeployableInfo> list = base.characterBody?.master?._GetDeployablesList();
                foreach( var v in list )
                {
                    if( v.slot == DecoyModule.deployableSlot )
                    {
                        Deployable dep = v.deployable;
                        if( dep == null )
                        {
                            continue;
                        }
                        GameObject obj = dep.gameObject;
                        if( obj == null )
                        {
                            continue;
                        }
                        CharacterMaster master = obj.GetComponent<CharacterMaster>();
                        if( master )
                        {
                            master.TrueKill();
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
