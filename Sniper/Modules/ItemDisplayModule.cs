using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;
using Sniper.Properties;

namespace Sniper.Modules
{
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
        private static ItemDisplayRuleSet _sniperItemDisplay;


        private static ItemDisplayRuleSet CreateSniperItemDisplay()
        {
            // TODO: Create Sniper item display ruleset
            return null;
        }
    }
}
