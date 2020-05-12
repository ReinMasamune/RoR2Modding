using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        public class ClientOrbController : NetworkBehaviour
        {
            private readonly List<BaseClientOrb> activeOrbs = new List<BaseClientOrb>();
            private readonly List<BaseClientOrb> destroy = new List<BaseClientOrb>();
            private NetworkIdentity identity;

            private Boolean isAuthority
            {
                get => RoR2.Util.HasEffectiveAuthority( this.identity );
            }

            private void Awake()
            {
                this.identity = base.GetComponent<NetworkIdentity>();
            }

            public void AddClientOrb( BaseClientOrb newOrb )
            {
                if( !this.isAuthority )
                {
                    //Debug.Log( "AddClientOrb called without authority" );
                    return;
                }
                if( newOrb == null )
                {
                    //Debug.Log( "Null orb not added" );
                    return;
                }
                newOrb.outer = this;
                newOrb.Begin();

                if( newOrb.totalDuration == 0f )
                {
                    //Debug.Log( "Orb failed to assign duration" );
                    return;
                }

                newOrb.remainingDuration = newOrb.totalDuration;

                this.activeOrbs.Add( newOrb );
            }

            private void UpdateClientOrb( BaseClientOrb orb, Single deltaT )
            {
                if( !this.isAuthority )
                {
                    //Debug.Log( "UpdateClientOrb called without authority" );
                    return;
                }

                if( !this.activeOrbs.Contains( orb ) )
                {
                    //Debug.Log( "UpdateClientOrb called on non-existant orb" );
                    return;
                }

                orb.remainingDuration -= deltaT;
                orb.Tick( deltaT );

                if( orb.remainingDuration <= 0f )
                {
                    this.EndClientOrb( orb );
                }
            }

            private void EndClientOrb( BaseClientOrb orb )
            {
                if( !this.isAuthority )
                {
                    //Debug.Log( "EndClientOrb called without authority" );
                    return;
                }
                if( !this.activeOrbs.Contains( orb ) )
                {
                    //Debug.Log( "EndClientOrb called on non-existant orb" );
                    return;
                }
                orb.End();
                this.destroy.Add( orb );
            }

            private void FixedUpdate()
            {
                if( !this.isAuthority ) return;

                Single deltaT = Time.fixedDeltaTime;
                foreach( BaseClientOrb orb in this.activeOrbs )
                {
                    this.UpdateClientOrb( orb, deltaT );
                }
                foreach( BaseClientOrb orb in this.destroy )
                {
                    this.activeOrbs.Remove( orb );
                }
                this.destroy.Clear();
            }
        }
    }
}