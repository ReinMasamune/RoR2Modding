using System;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace ReinSniperRework
{
    public class HookMineInvader : MonoBehaviour
    {
        EntityStateMachine mach1;
        EntityStateMachine mach2;

        public GameObject wardPrefab;

        private bool activated = false;

        public GameObject owner;

        public void Start()
        {
            foreach( EntityStateMachine mach in gameObject.GetComponents<EntityStateMachine>() )
            {
                switch( mach.customName )
                {
                    case "Main":
                        mach1 = mach;
                        break;

                    case "Arming":
                        mach2 = mach;
                        break;

                    default:
                        break;
                }
            }
        }

        public void FixedUpdate()
        {
            if( mach1.state.GetType() == typeof( EntityStates.Engi.Mine.PreDetonate ) && !activated )
            {
                //Debug.Log("Mine activating");
                OnActivation();
            }
        }

        private void OnActivation()
        {
            if (NetworkServer.active)
            {
                if (wardPrefab)
                {
                    activated = true;
                    GameObject obj = UnityEngine.Object.Instantiate<GameObject>(wardPrefab, transform.position, Quaternion.identity);
                    BuffWard ward = obj.GetComponent<BuffWard>();
                    TeamFilter filter = obj.GetComponent<TeamFilter>();
                    obj.GetComponent<HookMineHooking>().owner = owner;
                    filter.teamIndex = (filter.teamIndex == TeamIndex.Player) ? TeamIndex.Monster : TeamIndex.Player;
                    NetworkServer.Spawn(obj);

                }
            }
        }
    }
}