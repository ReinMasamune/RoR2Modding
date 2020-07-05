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
        private static AssetBundle _sniperAssetBundle;


    }

}
