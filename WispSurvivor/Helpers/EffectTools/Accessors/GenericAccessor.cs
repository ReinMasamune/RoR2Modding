
using System;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class GenericAccessor<TAsset> where TAsset : UnityEngine.Object
    {
        internal String name { get; private set; }
        internal UInt64 index { get; private set; }
        internal Main.ExecutionState minState { get; private set; }
        internal Boolean shouldAutoCache { get; private set; }
        internal TAsset value
        {
            get
            {
                if( this.val != null ) return this.val;

                if( Main.state >= this.minState )
                {
                    if( this.access != null )
                    {
                        this.val = this.access();
                        return this.val;
                    } else
                    {
                        throw new NullReferenceException( "Access null for " + this.name );
                    }
                } else
                {
                    throw new NullReferenceException( "Too early in execution for " + this.name );
                }
            }
        }


        private TAsset val;
        private Func<TAsset> access;

        internal void RegisterAccessor()
        {
            AssetLibrary<TAsset>.AddAssetAccess( this );
        }

        internal GenericAccessor( Enum index, Func<TAsset> access, Boolean shouldCache = false, Main.ExecutionState minState = Main.ExecutionState.Awake )
        {
            var ind = Convert.ChangeType( index, index.GetType() );
            this.name = ind.ToString();
            this.index = (UInt64)ind;
            this.access = access;
            this.minState = minState;
            this.shouldAutoCache = shouldCache;
        }

        private enum GenericIndex { }
    }
}