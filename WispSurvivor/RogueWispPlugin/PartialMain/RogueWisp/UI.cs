#if ROGUEWISP
using System;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;
using Rein.RogueWispPlugin.Helpers;
using ReinCore;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        private GameObject RW_crosshair;
        partial void RW_UI() => this.Load += this.RW_CreateCrosshair;

        private void RW_CreateCrosshair()
        {
            GameObject baseUI = Resources.Load<GameObject>("Prefabs/Crosshair/CrocoCrosshair").ClonePrefab("WispCrosshair", false);
            //var sprite = Resources.Load<GameObject>("Prefabs/Crosshair/HuntressSnipeCrosshair").transform.Find( "Center Circle" ).GetComponent<Image>().sprite;
            var tex = TextureGenerator.GenerateCircleTexture( new Color32( 0, 0, 0, 0 ), new Color32( 0, 0, 0, 255 ), new Color32( 255, 255, 255, 255), new Vector2( 0f, -1f ), 1f, 0.95f, 0.75f, 0.7f, 512, 512 );
            var sprite = Sprite.Create
            (
                texture: tex,
                rect: new Rect( 0f, 0f, 512f, 256f ),
                pivot: new Vector2( 0.5f, 0.5f ),
                pixelsPerUnit: 100f,
                extrude: 0u,
                meshType: SpriteMeshType.FullRect,
                border: new Vector4( 0f, 0f, 0f, 0f ),
                generateFallbackPhysicsShape: false
            );
            //Main.debugTexture = tex;
            
            
            var temp = baseUI.transform.Find("Image");

            var bar = new GameObject().ClonePrefab("WispHudChunk", false );


            var bar1 = Instantiate<GameObject>( temp.gameObject, bar.transform );
            wispCrossBar1 = bar1;
            Destroy( bar1.GetComponent<Image>() );
            var bar1Rect = bar1.GetComponent<RectTransform>();
            var bar1BG = Instantiate<GameObject>( temp.gameObject, bar1.transform );
            var bar1FG = Instantiate<GameObject>( temp.gameObject, bar1.transform );
            var bar1BGImg = bar1BG.GetComponent<Image>();
            bar1BGImg.sprite = sprite;
            bar1BGImg.type = Image.Type.Filled;
            bar1BGImg.fillMethod = Image.FillMethod.Radial180;
            bar1BGImg.fillOrigin = 0;
            bar1BGImg.fillAmount = 1f;
            bar1BGImg.fillClockwise = true;
            var bar1FGImg = bar1FG.GetComponent<Image>();
            bar1FGImg.sprite = sprite;
            bar1FGImg.type = Image.Type.Filled;
            bar1FGImg.fillMethod = Image.FillMethod.Radial180;
            bar1FGImg.fillOrigin = 0;
            bar1FGImg.fillAmount = 0f;
            bar1FGImg.fillClockwise = false;
            var bar1BGRect = bar1BG.GetComponent<RectTransform>();
            var bar1FGRect = bar1FG.GetComponent<RectTransform>();
            bar1BGRect.anchoredPosition = bar1FGRect.anchoredPosition = Vector2.zero;
            bar1BGRect.sizeDelta = bar1FGRect.sizeDelta = new Vector2( 256f, 128f );
            bar1BGRect.anchorMin = bar1FGRect.anchorMin = Vector2.zero;
            bar1BGRect.anchorMax = bar1FGRect.anchorMax = Vector2.one;
            bar1BGRect.pivot = bar1FGRect.pivot = new Vector2( 0.5f, 0.5f );
            bar1BGRect.localEulerAngles = bar1FGRect.localEulerAngles = Vector3.zero;
            bar1BGRect.localScale = bar1FGRect.localScale = Vector3.one;
            var bar1Controller = bar1.AddComponent<WispCrosshairChargeController>();
            bar1Controller.bgImage = bar1BGImg;
            bar1Controller.fgImage = bar1FGImg;


            var bar2 = Instantiate<GameObject>( temp.gameObject, bar.transform );
            wispCrossBar2 = bar2;
            Destroy( bar2.GetComponent<Image>() );
            var bar2Rect = bar2.GetComponent<RectTransform>();
            var bar2BG = Instantiate<GameObject>( temp.gameObject, bar2.transform );
            var bar2FG = Instantiate<GameObject>( temp.gameObject, bar2.transform );
            var bar2BGImg = bar2BG.GetComponent<Image>();
            bar2BGImg.sprite = sprite;
            bar2BGImg.type = Image.Type.Filled;
            bar2BGImg.fillMethod = Image.FillMethod.Radial180;
            bar2BGImg.fillOrigin = 0;
            bar2BGImg.fillAmount = 1f;
            bar2BGImg.fillClockwise = false;
            var bar2FGImg = bar2FG.GetComponent<Image>();
            bar2FGImg.sprite = sprite;
            bar2FGImg.type = Image.Type.Filled;
            bar2FGImg.fillMethod = Image.FillMethod.Radial180;
            bar2FGImg.fillOrigin = 0;
            bar2FGImg.fillAmount = 0f;
            bar2FGImg.fillClockwise = true;
            var bar2BGRect = bar2BG.GetComponent<RectTransform>();
            var bar2FGRect = bar2FG.GetComponent<RectTransform>();
            bar2BGRect.anchoredPosition = bar2FGRect.anchoredPosition = Vector2.zero;
            bar2BGRect.sizeDelta = bar2FGRect.sizeDelta = new Vector2( 256f, 128f );
            bar2BGRect.anchorMin = bar2FGRect.anchorMin = Vector2.zero;
            bar2BGRect.anchorMax = bar2FGRect.anchorMax = Vector2.one;
            bar2BGRect.pivot = bar2FGRect.pivot = new Vector2( 0.5f, 0.5f );
            bar2BGRect.localEulerAngles = bar2FGRect.localEulerAngles = Vector3.zero;
            bar2BGRect.localScale = bar2FGRect.localScale = Vector3.one;
            var bar2Controller = bar2.AddComponent<WispCrosshairChargeController>();
            bar2Controller.bgImage = bar2BGImg;
            bar2Controller.fgImage = bar2FGImg;




            bar1Rect.anchoredPosition = new Vector2( 96f, 0f );
            bar2Rect.anchoredPosition = new Vector2( -96f, 0f );

            bar1Rect.sizeDelta = new Vector2( 256f, 128f );
            bar2Rect.sizeDelta = new Vector2( 256f, 128f );

            bar1Rect.anchorMin = new Vector2( 0.5f, 0.5f );
            bar2Rect.anchorMin = new Vector2( 0.5f, 0.5f );

            bar1Rect.anchorMax = new Vector2( 0.5f, 0.5f );
            bar2Rect.anchorMax = new Vector2( 0.5f, 0.5f );

            bar1Rect.pivot = new Vector2( 0.5f, 0.5f );
            bar2Rect.pivot = new Vector2( 0.5f, 0.5f );

            bar1Rect.localEulerAngles = new Vector3( 0f, 0f, -90f );
            bar2Rect.localEulerAngles = new Vector3( 0f, 0f, 90f );

            bar1Rect.localScale = Vector3.one * 0.125f;
            bar2Rect.localScale = Vector3.one * 0.125f;

            this.RW_crosshair = baseUI;






            var barTex = TextureGenerator.GenerateBarTexture( new Color32( 0, 0, 0, 255 ), new Color32( 255, 255, 255, 255 ), 16, true, 1025, 101 );
            //Main.debugTexture = barTex;
            var barSprite = Sprite.Create
            (
                texture: barTex,
                rect: new Rect( 0f, 0f, barTex.width, barTex.height ),
                pivot: new Vector2( 0.5f, 0.5f ),
                pixelsPerUnit: 100f,
                extrude: 0u,
                meshType: SpriteMeshType.FullRect,
                border: new Vector4( 0f, 0f, 0f, 0f ),
                generateFallbackPhysicsShape: false
            );



            temp = Resources.Load<GameObject>("Prefabs/HudSimple").transform.Find( "MainContainer/MainUIArea/BottomRightCluster/Scaler/Outline" );

            var bar3 = Instantiate<GameObject>( temp.gameObject, bar.transform );
            bar3.SetActive( true );
            bar3.name = "BAR";
            var bar3Rect = bar3.GetComponent<RectTransform>();
            Destroy( bar3.GetComponent<Image>() );

            var bar3BG = Instantiate<GameObject>( temp.gameObject, bar3.transform );
            bar3BG.SetActive( true );
            var bar3FG = Instantiate<GameObject>( temp.gameObject, bar3.transform );
            bar3FG.SetActive( true );

            var bar3BGImg = bar3BG.GetComponent<Image>();
            bar3BGImg.sprite = barSprite;
            bar3BGImg.type = Image.Type.Filled;
            bar3BGImg.fillMethod = Image.FillMethod.Horizontal;
            bar3BGImg.fillOrigin = 1;
            bar3BGImg.fillAmount = 1f;
            bar3BGImg.color = Color.white;
            bar3BGImg.preserveAspect = true;

            var bar3FGImg = bar3FG.GetComponent<Image>();
            bar3FGImg.sprite = barSprite;
            bar3FGImg.type = Image.Type.Filled;
            bar3FGImg.fillMethod = Image.FillMethod.Horizontal;
            bar3FGImg.fillOrigin = 0;
            bar3FGImg.fillAmount = 0f;
            bar3FGImg.color = Color.white;
            bar3FGImg.preserveAspect = true;

            var bar3BGRect = bar3BG.GetComponent<RectTransform>();
            var bar3FGRect = bar3FG.GetComponent<RectTransform>();
            bar3BGRect.anchoredPosition = bar3FGRect.anchoredPosition = Vector2.zero;
            //bar3BGRect.sizeDelta = bar3FGRect.sizeDelta = new Vector2( 1024f, 64f );
            bar3BGRect.anchorMin = bar3FGRect.anchorMin = Vector2.zero;
            bar3BGRect.anchorMax = bar3FGRect.anchorMax = Vector2.one;
            bar3BGRect.pivot = bar3FGRect.pivot = new Vector2( 1, 0f );
            bar3BGRect.localEulerAngles = bar3FGRect.localEulerAngles = Vector3.zero;
            bar3BGRect.localScale = bar3FGRect.localScale = Vector3.one;

            //bar3Rect.anchoredPosition = new Vector2( 170f, 100f );
            //bar3Rect.sizeDelta = new Vector2( 515f, 32f );
            //bar3Rect.anchorMin = new Vector2( 0.5f, 0.5f );  
            //bar3Rect.anchorMax = new Vector2( 0.5f, 0.5f );
            //bar3Rect.pivot = new Vector2( 0.5f, 0.5f );
            //bar3Rect.localEulerAngles = Vector3.zero;
            //bar3Rect.localScale = Vector3.one;

            var control = bar3.AddComponent<WispCrosshairChargeController>();
            control.bgImage = bar3BGImg;
            control.fgImage = bar3FGImg;








            wispHudPrefab = bar3;
        }

        private static GameObject wispHudPrefab;
        private static GameObject wispCrossBar1;
        private static GameObject wispCrossBar2;
    }
}
#endif