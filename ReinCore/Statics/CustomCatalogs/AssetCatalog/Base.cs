namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using UnityEngine;

    public abstract class AssetCatalog<TSelf, TAsset, TAssetDef> : Catalog<TSelf, TAssetDef>
        where TSelf : AssetCatalog<TSelf, TAsset, TAssetDef>, new()
        where TAsset : UnityEngine.Object
        where TAssetDef : IAssetDef<TAssetDef, TSelf, TAsset>
    {
    }

    public interface IAssetDef<TSelf, TCatalog, TAsset> : AssetCatalog<TCatalog, TAsset, TSelf>.ICatalogDef
        where TSelf : IAssetDef<TSelf, TCatalog, TAsset>, AssetCatalog<TCatalog, TAsset, TSelf>.ICatalogDef
        where TCatalog : AssetCatalog<TCatalog, TAsset, TSelf>, new()
        where TAsset : UnityEngine.Object
    {

    }


}
