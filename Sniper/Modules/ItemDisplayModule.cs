namespace Sniper.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using BepInEx.Logging;
    using ReinCore;
    using RoR2;
    using UnityEngine;
    using Sniper.Properties;

    internal static class ItemDisplayModule
    {
        internal static ItemDisplayRuleSet GetSniperItemDisplay()
        {
            if( _sniperItemDisplay == null )
            {
                _sniperItemDisplay = CreateSniperItemDisplay();
            }

            return _sniperItemDisplay;
        }
#pragma warning disable IDE1006 // Naming Styles
        private static ItemDisplayRuleSet _sniperItemDisplay;
#pragma warning restore IDE1006 // Naming Styles


        private static ItemDisplayRuleSet CreateSniperItemDisplay() =>
            // TODO: Create Sniper item display ruleset
            null;
    }
}
