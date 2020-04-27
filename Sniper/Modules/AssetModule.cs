using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;

namespace Sniper.Modules
{
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
