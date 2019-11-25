using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Acrid.Helpers
{
    public static class PrefabHelpers
    {
        /// <summary>
        /// Returns true if the GameObject has a component of type T
        /// Retuns false if the GameObject is null, or does not have a component of type T
        /// </summary>
        /// <typeparam Type to check="T"></typeparam>
        /// <param Object to check="g"></param>
        /// <returns></returns>
        public static System.Boolean HasComponent<T>( this GameObject g ) => g && g.GetComponent<T>() != null;

        /// <summary>
        /// Gets and returns component of type T from GameObject
        /// If there is no component of type T, adds one and returns it
        /// </summary>
        /// <typeparam Type to get="T"></typeparam>
        /// <param Object to check="g"></param>
        /// <returns></returns>
        public static T AddOrGetComponent<T>( this GameObject g ) where T : Component
        {
            T v = g.GetComponent<T>();
            if( v == null )
            {
                v = g.AddComponent<T>();
            }
            return v;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="g"></param>
        /// <returns></returns>
        public static T AddOrGetComponent<T>( this Transform g ) where T : Component => AddOrGetComponent<T>( g.gameObject );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="g"></param>
        /// <returns></returns>
        public static T AddOrGetComponent<T>( this Component g ) where T : Component => AddOrGetComponent<T>( g.gameObject );


        /// <summary>
        /// 
        /// </summary>
        /// <param Object to Clone="g"></param>
        /// <param Name it should have="nameToSet"></param>
        /// <param Should it be registered for networking="registerNetwork"></param>
        /// <param Internal="file"></param>
        /// <param Internal="member"></param>
        /// <param Internal="line"></param>
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
        private static void SkinDef_Awake( On.RoR2.SkinDef.orig_Awake orig, SkinDef self )
        {
            //Intentionally do nothing
        }

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
                if( h.prefab.HasComponent<NetworkIdentity>() ) h.prefab.GetComponent<NetworkIdentity>().SetFieldValue<NetworkHash128>( "m_AssetId", nullHash );
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
