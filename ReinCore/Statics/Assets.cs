namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using UnityEngine;

    public static class AssetsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static TAsset LoadAsset<TAsset>( Enum index ) where TAsset : UnityEngine.Object
        {
            //var timer = new Stopwatch();
            //timer.Start();

            if( !AssetsCore.loaded )
            {
                throw new CoreNotLoadedException( nameof( AssetsCore ) );
            }
            if( !AssetsCore.MatchAssetIndexType( typeof( TAsset ), index.GetType() ) )
            {
                throw new ArgumentException( "Incorrect index type", nameof( index ) );
            }

            TAsset asset = AssetLibrary<TAsset>.GetAsset( index );
            //timer.Stop();
            //Log.Warning( String.Format( "Time for asset {0}: {1} ticks, {2} ms", index.GetName(), timer.ElapsedTicks, timer.ElapsedMilliseconds ) );
            return asset;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean CanLoadAsset<TAsset>( Enum index ) where TAsset : UnityEngine.Object
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( !AssetsCore.loaded )
            {
                throw new CoreNotLoadedException( nameof( AssetsCore ) );
            }
            if( !AssetsCore.MatchAssetIndexType( typeof( TAsset ), index.GetType() ) )
            {
                throw new ArgumentException( "Incorrect index type", "index" );
            }
            return AssetLibrary<TAsset>.CanGetAsset( index );
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean Loadable( Enum index )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( AssetsCore.assetAssociations.TryGetValue( index.GetType(), out Type assetType ) )
            {
                return AssetsCore.ReflectedCanLoadAsset( assetType, index );
            }
            throw new ArgumentException( "Invalid index type" );
        }

        static AssetsCore()
        {
            Log.Warning( "AssetsCore loaded" );
            AssetsCore.canLoadAsset = typeof( AssetsCore ).GetMethod( "CanLoadAsset", BindingFlags.Static | BindingFlags.Public );
            AssetLibrary<GameObject>.SetIndexType( typeof( PrefabIndex ) );
            AssetLibrary<Mesh>.SetIndexType( typeof( MeshIndex ) );
            AssetLibrary<Material>.SetIndexType( typeof( MaterialIndex ) );
            AssetLibrary<Shader>.SetIndexType( typeof( ShaderIndex ) );
            AssetLibrary<Texture2D>.SetIndexType( typeof( Texture2DIndex ) );
            AssetsCore.loaded = PrefabInitializer.Initialize();
            if( AssetsCore.loaded )
            {
                AssetsCore.loaded &= MeshInitializer.Initialize();
            } else
            {
                Log.Error( String.Format( "{0} failed to load", "PrefabInitializer" ) );
            }
            if( AssetsCore.loaded )
            {
                AssetsCore.loaded &= MaterialInitializer.Initialize();
            } else
            {
                Log.Error( String.Format( "{0} failed to load", "MeshInitializer" ) );
            }
            if( AssetsCore.loaded )
            {
                AssetsCore.loaded &= ShaderInitializer.Initialize();
            } else
            {
                Log.Error( String.Format( "{0} failed to load", "MaterialInitializer" ) );
            }
            if( AssetsCore.loaded )
            {
                AssetsCore.loaded &= TextureInitializer.Initialize();
            } else
            {
                Log.Error( String.Format( "{0} failed to load", "ShaderInitializer" ) );
            }
            if( !AssetsCore.loaded )
            {
                Log.Error( String.Format( "{0} failed to load", "TextureInitializer" ) );
            }

            Log.Warning( "AssetsCore loaded" );
        }


        internal static Dictionary<Type, Type> indexAssociations = new Dictionary<Type, Type>();

        internal static Dictionary<Type, Type> assetAssociations = new Dictionary<Type, Type>();

        internal static Boolean MatchAssetIndexType( Type assetType, Type enumType ) => AssetsCore.indexAssociations.TryGetValue( assetType, out Type type ) ? enumType == type : false;


        private static readonly MethodInfo canLoadAsset;

        private static readonly Dictionary<Type, AssetsCore.CanLoadAssetDelegate> delegateCache = new Dictionary<Type, AssetsCore.CanLoadAssetDelegate>();

        private static Boolean ReflectedCanLoadAsset( Type assetType, Enum index )
        {
            if( !AssetsCore.delegateCache.TryGetValue( assetType, out CanLoadAssetDelegate canLoadAssetDelegate ) )
            {
                MethodInfo method = AssetsCore.canLoadAsset.MakeGenericMethod(new Type[]
                {
                    assetType
                });
                canLoadAssetDelegate = (AssetsCore.CanLoadAssetDelegate)Delegate.CreateDelegate( typeof( AssetsCore.CanLoadAssetDelegate ), method );
                AssetsCore.delegateCache[assetType] = canLoadAssetDelegate;
            }
            return canLoadAssetDelegate( index );
        }

        private delegate Boolean CanLoadAssetDelegate( Enum index );
    }
}
