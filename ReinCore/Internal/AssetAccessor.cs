namespace ReinCore
{
    using System;
    using BepInEx;

    internal delegate TAsset AssetAccessDelegate<TAsset>();
    internal class AssetAccessor<TAsset> where TAsset : UnityEngine.Object
    {
        internal AssetAccessor( Enum index, AssetAccessDelegate<TAsset> del, params Enum[] dependencies )
        {
            if( !AssetsCore.MatchAssetIndexType( typeof( TAsset ), index.GetType() ) )
            {
                throw new ArgumentException( "Incorrect index type" );
            }

            this.index = index;
            this.accessDelegate = del;
            this.dependencies = dependencies;
        }

        internal void RegisterAccessor() => AssetLibrary<TAsset>.AddAsset( this );

        internal TAsset value
        {
            get
            {
                if( this._cachedValue == null )
                {
                    if( this.CanLoad() != true )
                    {
                        throw new AssetNotLoadableException( this.index );
                    }

                    this._cachedValue = this.accessDelegate();
                }
                return this._cachedValue;
            }
        }

        internal Enum index { get; private set; }

        internal Boolean CanLoad()
        {
            Boolean val = true;
            for( Int32 i = 0; i < this.dependencies.Length; ++i )
            {
                val &= AssetsCore.Loadable( this.dependencies[i] );
            }
            return val;
        }


#pragma warning disable IDE1006 // Naming Styles
        private TAsset _cachedValue;
#pragma warning restore IDE1006 // Naming Styles

        private readonly AssetAccessDelegate<TAsset> accessDelegate;
        private readonly Enum[] dependencies;
    }

}
