namespace Sniper.Modules
{
    using System;

    using UnityEngine;

    using UnityObject = UnityEngine.Object;

    internal static class AssetModule
    {
        internal static AssetBundle GetSniperAssetBundle()
        {
            if( _sniperAssetBundle == null )
            {
                _sniperAssetBundle = Properties.Tools.LoadAssetBundle( Properties.Resources.sniper );
            }

            return _sniperAssetBundle;
        }
        private static AssetBundle _sniperAssetBundle;

        internal static TAsset LoadAsset<TAsset>(String path)
            where TAsset : UnityObject
            => String.IsNullOrWhiteSpace(path) ? Default<TAsset>.value : GetSniperAssetBundle().LoadAsset<TAsset>(path);
    }

}
