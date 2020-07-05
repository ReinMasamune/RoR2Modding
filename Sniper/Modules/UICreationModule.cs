namespace Sniper.Modules
{
    using System;
    using ReinCore;
    using RoR2;
    using RoR2.UI;

    using Sniper.Components;

    using UnityEngine;
    using UnityEngine.UI;

    internal static class UICreationModule
    {
        //internal static GameObject CreateScopeCrosshair()
        //{
        //    // TODO: Create scope crosshair
        //    GameObject obj = PrefabsCore.ClonePrefab(Resources.Load<GameObject>( "Prefabs/Crosshair/SniperCrosshair"), "Sniper Crosshair", false );

        //    ScopeUIController control = obj.AddComponent<ScopeUIController>();
        //    control.HookUpComponents();

        //    RawImage img = obj.GetComponent<RawImage>();
        //    img.enabled = false;

        //    SniperScopeChargeIndicatorController ind = obj.GetComponent<SniperScopeChargeIndicatorController>();
        //    control.chargeIndicator = ind.image;
        //    UnityEngine.Object.Destroy( ind );

        //    SniperRangeIndicator range = obj.GetComponent<SniperRangeIndicator>();
        //    control.rangeIndicator = range.label as HGTextMeshProUGUI;
        //    UnityEngine.Object.Destroy( range );

        //    DisplayStock stock = obj.GetComponent<DisplayStock>();
        //    GameObject stockPar = stock.stockImages[0].rectTransform.parent.gameObject;
        //    UnityEngine.Object.Destroy( stockPar );
        //    UnityEngine.Object.Destroy( stock );

        //    Transform zoom = obj.transform.Find( "Zoom Amount" );
        //    control.zoomIndicator = zoom.GetComponent<HGTextMeshProUGUI>();

        //    return obj;
        //}

        internal static GameObject CreateStandardCrosshair()
        {
            var obj = Resources.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").GetComponent<CharacterBody>().crosshairPrefab.ClonePrefab( "SniperCrosshair", false );
            //var tex = Log.CallProf<Texture2D>( "GenTexture", TextureModule.GetCrosshairTexture );

            UnityEngine.Object.Destroy( obj.GetComponent<RawImage>() );

            foreach( Transform child in obj.transform )
            {
                UnityEngine.Object.Destroy( child.gameObject );
            }

            var controller = obj.GetComponent<CrosshairController>();
            controller.spriteSpreadPositions = Array.Empty<CrosshairController.SpritePosition>();

            var imgObj = new GameObject( "Image", typeof(RectTransform) );

            var imgTrans = imgObj.transform as RectTransform;
            imgTrans.localPosition = new Vector2( 0.0f, 22f );
            imgTrans.localScale = new Vector2( 0.5f, 0.5f );

            imgObj.transform.SetParent( obj.transform );

            var img = imgObj.AddComponent<Image>();

            img.sprite = TextureModule.GetCrosshairSprite();



            //var img = obj.GetComponent<RawImage>();
            //img.texture = tex;



            return obj;
        }
    }

}
