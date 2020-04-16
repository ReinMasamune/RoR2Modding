using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using Sniper.Components;
using UnityEngine;

namespace Sniper.Modules
{
    internal static class DisplayModule
    {
        internal static void CreateDisplayPrefab()
        {
            var display = SniperMain.sniperBodyPrefab.GetComponent<ModelLocator>().modelBaseTransform.gameObject.ClonePrefab( "SniperDisplay", false );
            display.AddComponent<DisplayAnimationPlayer>();
            SniperMain.sniperDisplayPrefab = display;
        }
    }

}
