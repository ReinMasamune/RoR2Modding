using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using BepInEx;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RoR2Plugin
{
    // TODO: Prefab API docs
    public static class PrefabAPI
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="nameToSet"></param>
        /// <param name="registerNetwork"></param>
        /// <param name="file"></param>
        /// <param name="member"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static GameObject InstantiateClone( this GameObject g, System.String nameToSet, System.Boolean registerNetwork = true, [CallerFilePath] System.String file = "", [CallerMemberName] System.String member = "", [CallerLineNumber] System.Int32 line = 0 )
        {
            GameObject prefab = MonoBehaviour.Instantiate<GameObject>(g, GetParent().transform);
            prefab.name = nameToSet;
            if( registerNetwork )
            {
                RegisterPrefabInternal( prefab, file, member, line );
            }
            return prefab;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param Object to register="g"></param>
        /// <param Internal="file"></param>
        /// <param Internal="member"></param>
        /// <param Internal="line"></param>
        public static void RegisterNetworkPrefab( this GameObject g, [CallerFilePath] System.String file = "", [CallerMemberName] System.String member = "", [CallerLineNumber] System.Int32 line = 0 ) => RegisterPrefabInternal( g, file, member, line );

        #region Internal

        private static GameObject parent;
        private static GameObject GetParent()
        {
            if( !parent )
            {
                parent = new GameObject( "ModsAreHere" );
                MonoBehaviour.DontDestroyOnLoad( parent );
                parent.SetActive( false );

                On.RoR2.Util.IsPrefab += ( orig, obj ) =>
                {
                    if( obj.transform.parent && obj.transform.parent.gameObject.name == "ModsAreHere" ) return true;
                    return orig( obj );
                };
            }

            return parent;
        }

        private struct HashStruct
        {
            public GameObject prefab;
            public System.String goName;
            public System.String callPath;
            public System.String callMember;
            public System.Int32 callLine;
        }

        private static List<HashStruct> thingsToHash = new List<HashStruct>();

        private static System.Boolean needToRegister = false;

        private static void RegisterPrefabInternal( GameObject prefab, System.String callPath, System.String callMember, System.Int32 callLine )
        {
            HashStruct h = new HashStruct
            {
                prefab = prefab,
                goName = prefab.name,
                callPath = callPath,
                callMember = callMember,
                callLine = callLine
            };
            thingsToHash.Add( h );
            SetupRegistrationEvent();
        }

        private static void SetupRegistrationEvent()
        {
            if( !needToRegister )
            {
                needToRegister = true;
                On.RoR2.Networking.GameNetworkManager.OnStartClient += RegisterClientPrefabsNStuff;
            }
        }

        private static NetworkHash128 nullHash = new NetworkHash128
        {
            i0 = 0,
            i1 = 0,
            i2 = 0,
            i3 = 0,
            i4 = 0,
            i5 = 0,
            i6 = 0,
            i7 = 0,
            i8 = 0,
            i9 = 0,
            i10 = 0,
            i11 = 0,
            i12 = 0,
            i13 = 0,
            i14 = 0,
            i15 = 0
        };

        private static void RegisterClientPrefabsNStuff( On.RoR2.Networking.GameNetworkManager.orig_OnStartClient orig, RoR2.Networking.GameNetworkManager self, UnityEngine.Networking.NetworkClient newClient )
        {
            orig( self, newClient );
            foreach( HashStruct h in thingsToHash )
            {
                if( h.prefab.GetComponent<NetworkIdentity>() != null) h.prefab.GetComponent<NetworkIdentity>().SetFieldValue<NetworkHash128>( "m_AssetId", nullHash );
                ClientScene.RegisterPrefab( h.prefab, NetworkHash128.Parse( MakeHash( h.goName + h.callPath + h.callMember + h.callLine.ToString() ) ) );
            }
        }

        private static System.String MakeHash( System.String s )
        {
            MD5 hash = MD5.Create();
            System.Byte[] prehash = hash.ComputeHash( Encoding.UTF8.GetBytes( s ) );

            StringBuilder sb = new StringBuilder();

            for( System.Int32 i = 0; i < prehash.Length; i++ )
            {
                sb.Append( prehash[i].ToString( "x2" ) );
            }

            return sb.ToString();
        }
        #endregion
    }
}
