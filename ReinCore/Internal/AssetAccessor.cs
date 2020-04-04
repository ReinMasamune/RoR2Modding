using System;
using BepInEx;

namespace ReinCore
{
    internal delegate TAsset AssetAccessDelegate<TAsset>();
    internal class AssetAccessor<TAsset> where TAsset : UnityEngine.Object
    {
        internal AssetAccessor( Enum index, AssetAccessDelegate<TAsset> del, params Enum[] dependencies )
        {
            if( !AssetsCore.MatchAssetIndexType( typeof( TAsset ), index.GetType() ) ) throw new ArgumentException( "Incorrect index type" );

            this.index = index;
            this.accessDelegate = del;
            this.dependencies = dependencies;
        }

        internal void RegisterAccessor()
        {
            AssetLibrary<TAsset>.AddAsset( this );
        }


        internal TAsset value
        {
            get
            {
                if( this._cachedValue == null )
                {
                    if( this.CanLoad() != true ) throw new AssetNotLoadableException( this.index );

                    this._cachedValue = this.accessDelegate();
                }
                return this._cachedValue;
            }
        }

        internal Enum index { get; private set; }



        internal Boolean CanLoad()
        {
            var val = true;
            for( Int32 i = 0; i < this.dependencies.Length; ++i )
            {
                val &= AssetsCore.Loadable( this.dependencies[i] );
            }
            return val;
        }


        private TAsset _cachedValue;

        private AssetAccessDelegate<TAsset> accessDelegate;
        private Enum[] dependencies;
    }

}
