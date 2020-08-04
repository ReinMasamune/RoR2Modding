namespace Sniper.States.Special
{
    using System.Collections.Generic;

    using ReinCore;

    using RoR2;

    using Sniper.Modules;
    using Sniper.SkillDefs;
    using Sniper.States.Bases;

    using UnityEngine;
    using UnityEngine.Networking;

    internal class DecoyReactivation : ReactivationBaseState<DecoySkillData>
    {
        public override void OnEnter()
        {
            base.OnEnter();

            if( NetworkServer.active )
            {
                List<DeployableInfo> list = base.characterBody?.master?.deployablesList;
                foreach( DeployableInfo v in list )
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
