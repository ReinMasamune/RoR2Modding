using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class AssetLibrary<TAsset> : BaseAssetLibrary
    {
        private static HashSet<TAsset> mappedAssets = new HashSet<TAsset>();
        private static HashSet<GenericAccessor<TAsset>> uncachedAccessors = new HashSet<GenericAccessor<TAsset>>();
        private static HashSet<GenericAccessor<TAsset>> cachedAccessors = new HashSet<GenericAccessor<TAsset>>();
        private static Dictionary<UInt64,GenericAccessor<TAsset>> assets = new Dictionary<UInt64, GenericAccessor<TAsset>>();

        #region Singleton Stuff
        private static AssetLibrary<TAsset> instance;
        static AssetLibrary()
        {
            if( instance == null ) Init();
        }
        private static void Init()
        {
            instance = new AssetLibrary<TAsset>();
        }
        internal static AssetLibrary<TAsset> i
        {
            get
            {
                if( instance == null )
                {
                    Init();
                }
                return instance;
            }
        }
        #endregion

        #region Interface
        internal TAsset this[Enum enumKey]
        {
            get
            {
                return GetAssetRaw( enumKey );
            }
        }
        internal static void AddAssetAccess( GenericAccessor<TAsset> accessor )
        {
            var index = accessor.index;
            if( assets.ContainsKey( index ) )
            {
                Main.LogE( "Duplicate index: " + index + " for type: " + typeof( TAsset ).ToString() );
                return;
            }
            assets[index] = accessor;
            if( accessor.shouldAutoCache )
            {
                uncachedAccessors.Add( accessor );
            }
        }
        internal static void TryOnAll( Action<TAsset> action )
        {
            foreach( var access in assets )
            {
                try
                {
                    //Main.LogI( access.Key );
                    var val = access.Value.value;
                    action?.Invoke( val );
                } catch( Exception e )
                {
                    Main.LogE( access.Key + "   " + e );
                    continue;
                }
            }
        }
        internal static Boolean HasAsset( TAsset asset )
        {
            return mappedAssets.Contains( asset );
        }
        #endregion

        #region Internals
        private static TAsset GetAssetRaw( UInt64 key )
        {
            if( !assets.ContainsKey( key ) )
            {
                throw new ArgumentException( "Key: " + key + " is not registered to an asset" );
            }
            var access = assets[key];
            if( access == null )
            {
                throw new NullReferenceException( "Accessor with key: " + key + " is null" );
            }
            if( access.minState > Main.state )
            {
                throw new ArgumentException( "Accessor with key: " + key + " cannot be accesed before state: " + access.minState.ToString() );
            }
            var asset = access.value;
            mappedAssets.Add( asset );
            if( uncachedAccessors.Contains( access ) )
            {
                cachedAccessors.Add( access );
                uncachedAccessors.Remove( access );
            }

            return asset;
        }
        private static TAsset GetAssetRaw( Enum enumKey )
        {
            var key = (UInt64)Convert.ChangeType( enumKey, enumKey.GetType() );
            return GetAssetRaw( key );
        }

        internal override Main.ExecutionState GetMinState( UInt64 ind )
        {
            if( !assets.ContainsKey( ind ) )
            {
                Main.LogE( "Index: " + ind + " does not exist in library for type: " + typeof( TAsset ).ToString() );
                return Main.ExecutionState.PreLoad;
            }
            return assets[ind].minState;
        }
        #endregion
    }
}