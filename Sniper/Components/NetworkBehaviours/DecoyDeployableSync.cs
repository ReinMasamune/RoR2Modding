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
    internal class DecoyDeployableSync : NetworkBehaviour
    {
        #region Networking
        private static Int32 _rpcIndex_BodyKilled;
        private static Int32 _rpcIndex_MasterSpawned;
        static DecoyDeployableSync()
        {
            _rpcIndex_BodyKilled = 31415;
            _rpcIndex_MasterSpawned = 314159;
            NetworkBehaviour.RegisterRpcDelegate( typeof( DecoyDeployableSync ), _rpcIndex_BodyKilled, ClientReciever_InformBodyKilled );
            NetworkBehaviour.RegisterRpcDelegate( typeof( DecoyDeployableSync ), _rpcIndex_MasterSpawned, ClientReciever_InformMasterSpawned );
        }

        private static void ClientReciever_InformMasterSpawned(NetworkBehaviour obj, NetworkReader reader )
        {
            if( !NetworkClient.active ) return;
            ( obj as DecoyDeployableSync ).MasterSpawned();
        }


        private static void ClientReciever_InformBodyKilled( NetworkBehaviour obj, NetworkReader reader )
        {
            if( !NetworkClient.active ) return;
            ( obj as DecoyDeployableSync ).BodyKilled();
        }

        internal void BodyKilled()
        {
            if( !NetworkClient.active )
            {
                if( NetworkServer.active )
                {
                    var writer = new NetworkWriter();
                    writer.Write( 0 );
                    writer.Write( (Int16)( (UInt16)2 ) );
                    writer.WritePackedUInt32( (UInt32)_rpcIndex_BodyKilled );
                    writer.Write( base.netId );
                    writer.FinishMessage();
                    NetworkServer.SendWriterToReady( base.gameObject, writer, 0 );
                } else Log.Error( "What are you???" );
            } else
            {

            }
        }

        internal void MasterSpawned()
        {
            if( !NetworkClient.active )
            {
                if( NetworkServer.active )
                {
                    var writer = new NetworkWriter();
                    writer.Write( 0 );
                    writer.Write( (Int16)( (UInt16)2 ) );
                    writer.WritePackedUInt32( (UInt32)_rpcIndex_MasterSpawned );
                    writer.Write( base.netId );
                    writer.FinishMessage();
                    NetworkServer.SendWriterToReady( base.gameObject, writer, 0 );
                } else Log.Error( "What are you???" );
            } else
            {

            }
        }
        #endregion

        [SerializeField]
        private CharacterBody body;

        private CharacterMaster master;
        private void Awake()
        {
#if ASSERT
            if( this.body == null ) Log.ErrorL( "Body was null" );
#endif      
        }
    }
}
