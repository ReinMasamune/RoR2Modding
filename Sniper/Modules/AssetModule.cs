namespace Sniper.Modules
{
    using UnityEngine;

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
#pragma warning disable IDE1006 // Naming Styles
        private static AssetBundle _sniperAssetBundle;
#pragma warning restore IDE1006 // Naming Styles


    }

}
