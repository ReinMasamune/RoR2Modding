using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using RoR2.UI;
using Sniper.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Sniper.Modules
{
    internal static class UICreationModule
    {
        internal static GameObject CreateScopeCrosshair()
        {
            // TODO: Create scope crosshair
            var obj = PrefabsCore.ClonePrefab(Resources.Load<GameObject>( "Prefabs/Crosshair/SniperCrosshair"), "Sniper Crosshair", false );

            var control = obj.AddComponent<ScopeUIController>();
            control.HookUpComponents();

            var img = obj.GetComponent<RawImage>();
            img.enabled = false;

            var ind = obj.GetComponent<SniperScopeChargeIndicatorController>();
            control.chargeIndicator = ind.image;
            UnityEngine.Object.Destroy( ind );

            var range = obj.GetComponent<SniperRangeIndicator>();
            control.rangeIndicator = range.label as HGTextMeshProUGUI;
            UnityEngine.Object.Destroy( range );

            var stock = obj.GetComponent<DisplayStock>();
            var stockPar = stock.stockImages[0].rectTransform.parent.gameObject;
            UnityEngine.Object.Destroy( stockPar );
            UnityEngine.Object.Destroy( stock );

            var zoom = obj.transform.Find( "Zoom Amount" );
            control.zoomIndicator = zoom.GetComponent<HGTextMeshProUGUI>();

            return obj;
        }
    }

}
