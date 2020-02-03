using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal abstract class BaseAssetLibrary
    {
        private static Dictionary<Type,BaseAssetLibrary> typeLookup = new Dictionary<Type, BaseAssetLibrary>();

        internal abstract Main.ExecutionState GetMinState( UInt64 ind );

        internal static void AddAssociation<TAsset>( Type indexType )
        {
            var lib = AssetLibrary<TAsset>.i;
            if( typeLookup.ContainsKey( indexType ) )
            {
                if( typeLookup[indexType] == lib )
                {
                    return;
                } else
                {
                    throw new ArgumentException( indexType.ToString() + " already has an association." );
                }
            }

            typeLookup[indexType] = lib;
        }

        internal static Main.ExecutionState GetLastState( params Enum[] indicies )
        {
            var maxState = Main.ExecutionState.Broken;
            foreach( var ind in indicies )
            {
                var val = (UInt64)Convert.ChangeType( ind, ind.GetType() );
                var state = FindLibraryForIndex( ind ).GetMinState( val );
                if( state > maxState ) maxState = state;
            }

            if( maxState == Main.ExecutionState.Broken ) maxState = Main.ExecutionState.Constructor;
            return maxState;
        }

        private static BaseAssetLibrary FindLibraryForIndex( Enum indexType )
        {
            var type = indexType.GetType();

            if( typeLookup.ContainsKey( type ) )
            {
                return typeLookup[type];
            } else
            {
                throw new KeyNotFoundException( type.ToString() + " does not have an associated library." );
            }
        }
    }
}