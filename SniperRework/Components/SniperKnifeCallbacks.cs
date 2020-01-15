using EntityStates;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace ReinSniperRework
{
    internal partial class Main
    {
        internal class SniperKnifeCallbacks : MonoBehaviour
        {
            public SerializableEntityStateType targetState;
            public InterruptPriority interruptPriority;

            private ProjectileController controller;
            private ProjectileFuse fuse;
            private ProjectileCallOnOwnerNearby ownerCall;
            private GameObject owner;
            private EntityStateMachine targetMachine;

            private UnityAction fuseAction;
            private UnityAction ownerAction;

            public void Awake()
            {
                this.fuseAction += this.FuseFunction;
                this.ownerAction += this.OwnerNearbyFunction;
                this.controller = base.gameObject.GetComponent<ProjectileController>();
                this.fuse = base.gameObject.GetComponent<ProjectileFuse>();
                this.ownerCall = base.gameObject.GetComponent<ProjectileCallOnOwnerNearby>();
                this.fuse.onFuse.AddListener( this.fuseAction );
                this.ownerCall.onOwnerEnter.AddListener( this.ownerAction );
                this.ownerCall.enabled = false;
            }

            public void Start()
            {
                this.owner = controller.Networkowner;
                foreach( EntityStateMachine esm in this.owner.GetComponents<EntityStateMachine>() )
                {
                    if( esm.customName == "Weapon" ) this.targetMachine = esm;
                }
            }

            public void FuseFunction()
            {
                Main.instance.Logger.LogWarning( "Fuse triggered" );
                this.ownerCall.enabled = true;
            }

            public void OwnerNearbyFunction()
            {
                Main.instance.Logger.LogWarning( "Owner nearby" );
                this.targetMachine.SetInterruptState( EntityState.Instantiate( this.targetState ), this.interruptPriority );
                Destroy( base.gameObject );
            }
        }
    }
}
