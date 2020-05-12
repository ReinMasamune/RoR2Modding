namespace Sniper.Modules
{
    using ReinCore;

    using RoR2.UI;

    using Sniper.Components;

    using UnityEngine;
    using UnityEngine.UI;

    internal static class UICreationModule
    {
        internal static GameObject CreateScopeCrosshair()
        {
            // TODO: Create scope crosshair
            GameObject obj = PrefabsCore.ClonePrefab(Resources.Load<GameObject>( "Prefabs/Crosshair/SniperCrosshair"), "Sniper Crosshair", false );

            ScopeUIController control = obj.AddComponent<ScopeUIController>();
            control.HookUpComponents();

            RawImage img = obj.GetComponent<RawImage>();
            img.enabled = false;

            SniperScopeChargeIndicatorController ind = obj.GetComponent<SniperScopeChargeIndicatorController>();
            control.chargeIndicator = ind.image;
            UnityEngine.Object.Destroy( ind );

            SniperRangeIndicator range = obj.GetComponent<SniperRangeIndicator>();
            control.rangeIndicator = range.label as HGTextMeshProUGUI;
            UnityEngine.Object.Destroy( range );

            DisplayStock stock = obj.GetComponent<DisplayStock>();
            GameObject stockPar = stock.stockImages[0].rectTransform.parent.gameObject;
            UnityEngine.Object.Destroy( stockPar );
            UnityEngine.Object.Destroy( stock );

            Transform zoom = obj.transform.Find( "Zoom Amount" );
            control.zoomIndicator = zoom.GetComponent<HGTextMeshProUGUI>();

            return obj;
        }
    }

}
