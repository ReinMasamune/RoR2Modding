using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class AssetLibrary<TAsset> where TAsset : UnityEngine.Object
    {
        private static AssetLibrary<TAsset> instance;

        private static HashSet<TAsset> mappedAssets = new HashSet<TAsset>();
        private static HashSet<GenericAccessor<TAsset>> uncachedAccessors = new HashSet<GenericAccessor<TAsset>>();
        private static HashSet<GenericAccessor<TAsset>> cachedAccessors = new HashSet<GenericAccessor<TAsset>>();
        private static Dictionary<UInt64,GenericAccessor<TAsset>> assets = new Dictionary<UInt64, GenericAccessor<TAsset>>();

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

        internal TAsset this[UInt64 key]
        {
            get
            {
                return GetAssetRaw( key );
            }
        }

        internal TAsset this[Enum enumKey]
        {
            get
            {
                return GetAssetRaw( enumKey );
            }
        }

        internal static void SetAssetCached( TAsset asset )
        {
            mappedAssets.Add( asset );
        }

        internal static Boolean HasAsset( TAsset asset )
        {
            return mappedAssets.Contains( asset );
        }

        internal static TAsset GetAssetRaw( UInt64 key )
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
        internal static TAsset GetAssetRaw( Enum enumKey )
        {
            var key = (UInt64)Convert.ChangeType( enumKey, enumKey.GetType() );
            return GetAssetRaw( key );
        }
        internal static Main.ExecutionState GetLastState( params UInt64[] inds )
        {
            var max = Main.ExecutionState.PreLoad;
            foreach( var i in inds )
            {
                var temp = GetMinState( i );
                if( temp > max ) max = temp;
            }

            return max;
        }
        internal static Main.ExecutionState GetLastState( params System.Enum[] inds )
        {
            var max  = Main.ExecutionState.PreLoad;
            foreach( var ind in inds )
            {
                var i = (UInt64)Convert.ChangeType( ind, ind.GetType() ); ;
                var temp = GetMinState( i );
                if( temp > max ) max = temp;
            }

            return max;
        }

        private static Main.ExecutionState GetMinState( UInt64 ind )
        {
            if( !assets.ContainsKey( ind ) )
            {
                Main.LogE( "Index: " + ind + " does not exist in library for type: " + typeof( TAsset ).ToString() );
                return Main.ExecutionState.PreLoad;
            }

            return assets[ind].minState;
        }

        private enum GenericIndex : UInt64
        { }
    }
}