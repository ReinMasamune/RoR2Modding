using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace ReinCore
{
    public static class AssetsCore
    {
        public static Boolean loaded { get; private set; } = false;

		public static TAsset LoadAsset<TAsset>( Enum index ) where TAsset : UnityEngine.Object
		{
			if( !AssetsCore.loaded )
			{
				throw new CoreNotLoadedException( nameof(AssetsCore) );
			}
			if( !AssetsCore.MatchAssetIndexType( typeof( TAsset ), index.GetType() ) )
			{
				throw new ArgumentException( "Incorrect index type", nameof(index) );
			}
			return AssetLibrary<TAsset>.GetAsset( index );
		}

		public static Boolean CanLoadAsset<TAsset>( Enum index ) where TAsset : UnityEngine.Object
		{
			if( !AssetsCore.loaded )
			{
				throw new CoreNotLoadedException( nameof(AssetsCore) );
			}
			if( !AssetsCore.MatchAssetIndexType( typeof( TAsset ), index.GetType() ) )
			{
				throw new ArgumentException( "Incorrect index type", "index" );
			}
			return AssetLibrary<TAsset>.CanGetAsset( index );
		}

		public static bool Loadable( Enum index )
		{
			Type assetType;
			if( AssetsCore.assetAssociations.TryGetValue( index.GetType(), out assetType ) )
			{
				return AssetsCore.ReflectedCanLoadAsset( assetType, index );
			}
			throw new ArgumentException( "Invalid index type" );
		}

		static AssetsCore()
        {
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
				Log.Error( string.Format( "{0} failed to load", "PrefabInitializer" ) );
			}
			if( AssetsCore.loaded )
			{
				AssetsCore.loaded &= MaterialInitializer.Initialize();
			} else
			{
				Log.Error( string.Format( "{0} failed to load", "MeshInitializer" ) );
			}
			if( AssetsCore.loaded )
			{
				AssetsCore.loaded &= ShaderInitializer.Initialize();
			} else
			{
				Log.Error( string.Format( "{0} failed to load", "MaterialInitializer" ) );
			}
			if( AssetsCore.loaded )
			{
				AssetsCore.loaded &= TextureInitializer.Initialize();
			} else
			{
				Log.Error( string.Format( "{0} failed to load", "ShaderInitializer" ) );
			}
			if( !AssetsCore.loaded )
			{
				Log.Error( string.Format( "{0} failed to load", "TextureInitializer" ) );
			}
		}


		internal static Dictionary<Type, Type> indexAssociations = new Dictionary<Type, Type>();

		internal static Dictionary<Type, Type> assetAssociations = new Dictionary<Type, Type>();

		internal static bool MatchAssetIndexType( Type assetType, Type enumType )
		{
			Type type;
			if( AssetsCore.indexAssociations.TryGetValue( assetType, out type ) )
			{
				return enumType == type;
			}
			return false;
		}


		private static MethodInfo canLoadAsset;

		private static Dictionary<Type, AssetsCore.CanLoadAssetDelegate> delegateCache = new Dictionary<Type, AssetsCore.CanLoadAssetDelegate>();

		private static bool ReflectedCanLoadAsset( Type assetType, Enum index )
		{
			AssetsCore.CanLoadAssetDelegate canLoadAssetDelegate = null;
			if( !AssetsCore.delegateCache.TryGetValue( assetType, out canLoadAssetDelegate ) )
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

		private delegate bool CanLoadAssetDelegate( Enum index );
	}
}
