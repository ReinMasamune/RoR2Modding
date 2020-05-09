using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;
using Sniper.SkillDefs;
using UnityEngine.Networking;

namespace Sniper.Components
{
    internal class DecoyDeployableSync : NetworkBehaviour, IRuntimePrefabComponent
    {
        #region Networking
#pragma warning disable IDE1006 // Naming Styles
        //private static readonly Int32 _rpcIndex_BodyKilled;
#pragma warning restore IDE1006 // Naming Styles
        static DecoyDeployableSync()
        {
            //_rpcIndex_BodyKilled = 3141755;
            //NetworkBehaviour.RegisterRpcDelegate( typeof( DecoyDeployableSync ), _rpcIndex_BodyKilled, ClientReciever_InformBodyKilled );
        }

        //private static void ClientReciever_InformBodyKilled( NetworkBehaviour obj, NetworkReader reader )
        //{
        //    if( !NetworkClient.active ) return;
        //    ( obj as DecoyDeployableSync ).BodyKilled();
        //}

        internal void BodyKilled()
        {
            if( !NetworkClient.active )
            {
                //if( NetworkServer.active )
                //{
                //    Log.Warning( "Sending body killed" );
                //    var writer = new NetworkWriter();
                //    writer.Write( 0 );
                //    writer.Write( (Int16)( (UInt16)2 ) );
                //    writer.WritePackedUInt32( (UInt32)_rpcIndex_BodyKilled );
                //    writer.Write( base.netId );
                //    writer.FinishMessage();
                //    NetworkServer.SendWriterToReady( base.gameObject, writer, 0 );
                //} else Log.Error( "What are you???" );
            } else
            {
                var inst = ( this.owner?.GetBody()?.skillLocator?.special?.skillInstanceData as DecoySkillDef.ReactivationInstanceData );
                if( inst != null )
                {
                    inst.InvalidateReactivation();
                }
            }
        }
        #endregion

        [SerializeField]
        private CharacterBody body;


        private CharacterMaster master;
        private CharacterMaster owner;
        private void Awake()
        {

#if ASSERT
            if( this.body == null ) Log.ErrorL( "Body was null" );
#endif
        }

        private void Start()
        {
            this.master = this.body.master;
#if ASSERT
            if( this.master == null ) Log.ErrorL( "Master was null" );
#endif

            this.owner = this.master.minionOwnership?.ownerMaster;
#if ASSERT
            if( this.owner == null ) Log.ErrorL( "Owner was null" );
#endif
        }

        #region Prefab only
        void IRuntimePrefabComponent.InitializePrefab()
        {
            this.body = base.gameObject.GetComponent<CharacterBody>();
        }

        #endregion
    }
}
