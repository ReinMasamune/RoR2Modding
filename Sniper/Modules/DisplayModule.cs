namespace Sniper.Modules
{
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
