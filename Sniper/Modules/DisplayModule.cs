namespace Sniper.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using BepInEx.Logging;
    using ReinCore;
    using RoR2;
    using Sniper.Components;
    using UnityEngine;

    internal static class DisplayModule
    {
        internal static void CreateDisplayPrefab()
        {
            GameObject display = SniperMain.sniperBodyPrefab.GetComponent<ModelLocator>().modelBaseTransform.gameObject.ClonePrefab( "SniperDisplay", false );
            _ = display.AddComponent<DisplayAnimationPlayer>();
            SniperMain.sniperDisplayPrefab = display;
        }
    }

}
