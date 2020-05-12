namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;
    using BepInEx;
    using UnityEngine;
    using UnityEngine.Networking;

    // TODO: Docs for PrefabsCore

    /// <summary>
    /// 
    /// </summary>
    public static class PrefabsCore
    {
        /// <summary>
        /// 
        /// </summary>
        public static Boolean loaded { get; internal set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orginal"></param>
        /// <param name="name"></param>
        /// <param name="registerNetwork"></param>
        /// <param name="file"></param>
        /// <param name="member"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static GameObject ClonePrefab( this GameObject orginal, String name, Boolean registerNetwork, [CallerFilePath] String file = "", [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 )
        {
            if( parent == null || loaded == false )
            {
                throw new CoreNotLoadedException( nameof( PrefabsCore ) );
            }

            GameObject obj = UnityEngine.Object.Instantiate<GameObject>( orginal, parentTransform );
            obj.name = name;
            if( registerNetwork )
            {
                if( obj.GetComponent<NetworkIdentity>() == null )
                {
                    throw new ArgumentException( "Cloned prefab did not have a network identity, will not register" );
                }

                hashedObjects.Add( new HashedObject( obj, file, member, line ) );
            }

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="registerNetwork"></param>
        /// <param name="file"></param>
        /// <param name="member"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static GameObject CreatePrefab( String name, Boolean registerNetwork, [CallerFilePath] String file = "", [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 )
        {
            if( parent == null || loaded == false )
            {
                throw new CoreNotLoadedException( nameof( PrefabsCore ) );
            }

            var obj = new GameObject( name );
            obj.transform.SetParent( parentTransform, true );

            if( registerNetwork )
            {
                _ = obj.AddComponent<NetworkIdentity>();
                hashedObjects.Add( new HashedObject( obj, file, member, line ) );
            }

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="registerNetwork"></param>
        /// <param name="file"></param>
        /// <param name="member"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static GameObject CreateUIPrefab( String name, Boolean registerNetwork, [CallerFilePath] String file = "", [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 )
        {
            if( parent == null || loaded == false )
            {
                throw new CoreNotLoadedException( nameof( PrefabsCore ) );
            }

            var obj = new GameObject( name, typeof( RectTransform ) );
            obj.transform.SetParent( parentTransform, false );

            if( registerNetwork )
            {
                _ = obj.AddComponent<NetworkIdentity>();
                hashedObjects.Add( new HashedObject( obj, file, member, line ) );
            }

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="file"></param>
        /// <param name="member"></param>
        /// <param name="line"></param>
        public static void RegisterNetwork( this GameObject prefab, [CallerFilePath] String file = "", [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 )
        {
            if( parent == null || loaded == false )
            {
                throw new CoreNotLoadedException( nameof( PrefabsCore ) );
            }

            if( prefab.GetComponent<NetworkIdentity>() == null )
            {
                throw new ArgumentException( "Prefab must have a networkidentity, will not register" );
            }

            hashedObjects.Add( new HashedObject( prefab, file, member, line ) );
        }

        static PrefabsCore()
        {
            parent = new GameObject( "ModdedPrefabs" );
            parent.SetActive( false );
            UnityEngine.Object.DontDestroyOnLoad( parent );
            parentTransform = parent.transform;

            ParameterExpression instanceParam1 = Expression.Parameter( typeof( NetworkIdentity), "instance" );
            ParameterExpression valueParam1 = Expression.Parameter( typeof( NetworkHash128 ), "value" );
            MemberExpression fieldParam1 = Expression.Field( instanceParam1, typeof(NetworkIdentity), "m_AssetId" );
            BinaryExpression assignExpr1 = Expression.Assign( fieldParam1, valueParam1 );
            set_AssetId = Expression.Lambda<SetAssetIdDelegate>( assignExpr1, instanceParam1, valueParam1 ).Compile();

            RoR2.Networking.GameNetworkManager.onStartGlobal += GameNetworkManager_onStartGlobal;
            HooksCore.RoR2.Util.IsPrefab.On += IsPrefab_On;

            loaded = true;
        }

        private static Boolean IsPrefab_On( HooksCore.RoR2.Util.IsPrefab.Orig orig, GameObject gameObject ) => orig( gameObject ) || gameObject.transform.IsChildOf( parentTransform );

        private static readonly GameObject parent;
        private static readonly Transform parentTransform;
        private static readonly List<HashedObject> hashedObjects = new List<HashedObject>();

        private delegate void SetAssetIdDelegate( NetworkIdentity instance, NetworkHash128 value );
        private static readonly SetAssetIdDelegate set_AssetId;

        private struct HashedObject
        {
            private static readonly MD5 hashGen = MD5.Create();

            internal HashedObject( GameObject obj, String path, String member, Int32 line )
            {
                if( obj == null )
                {
                    throw new ArgumentNullException( "prefab" );
                }

                NetworkIdentity netID = obj.GetComponent<NetworkIdentity>();
                this.prefab = obj;
                this.netID = netID ?? throw new ArgumentException( "prefab does not have a NetworkIdentity component" );
                var sb = new StringBuilder();
                _ = sb.Append( this.prefab.name );
                _ = sb.Append( path );
                _ = sb.Append( member );
                _ = sb.Append( line );
                Byte[] bytes = hashGen.ComputeHash( Encoding.UTF8.GetBytes(sb.ToString()) );
                this.hash = new NetworkHash128
                {
                    i0 = bytes[0],
                    i1 = bytes[1],
                    i2 = bytes[2],
                    i3 = bytes[3],
                    i4 = bytes[4],
                    i5 = bytes[5],
                    i6 = bytes[6],
                    i7 = bytes[7],
                    i8 = bytes[8],
                    i9 = bytes[9],
                    i10 = bytes[10],
                    i11 = bytes[11],
                    i12 = bytes[12],
                    i13 = bytes[13],
                    i14 = bytes[14],
                    i15 = bytes[15],
                };
            }
            
            internal void Register()
            {
                ClientScene.UnregisterPrefab( this.prefab );
                set_AssetId( this.netID, default );
                ClientScene.RegisterPrefab( this.prefab, this.hash );
            }

            private readonly GameObject prefab;
            private readonly NetworkIdentity netID;
            private NetworkHash128 hash;
        }
        private static void GameNetworkManager_onStartGlobal()
        {
            for( Int32 i = 0; i < hashedObjects.Count; ++i )
            {
                HashedObject cur = hashedObjects[i];
                cur.Register();
            }
        }
    }
}
