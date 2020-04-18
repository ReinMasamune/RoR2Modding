using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using RoR2.Networking;
using UnityEngine;
using KinematicCharacterController;
using EntityStates;
using RoR2.Skills;
using RoR2.UI;
using UnityEngine.UI;
using Sniper.Components;

namespace Sniper.Modules
{
    internal static class UIModule
    {
        internal static Color reloadHandleColor { get; } = new Color( 1f, 1f, 1f, 1f );

        internal static GameObject GetDefaultDrosshair()
        {
            // TODO: Create default crosshair
            return null;
        }

        internal static GameObject GetScopeCrosshair()
        {
            // TODO: Create scope crosshair
            return null;
        }

        internal static Texture GetPortraitIcon()
        {
            // TODO: Portrait Icon
            return Properties.Tools.LoadTexture2D(Properties.Resources.unknown__11_);
            //return null;
        }

        internal static GameObject GetQuickScope()
        {
            // TODO: Quick scope UI
            return null;
        }

        internal static GameObject GetChargeScope()
        {
            // TODO: Charge scope UI
            return null;
        }

        internal static GameObject GetRelodBar()
        {
            var obj = PrefabsCore.CreatePrefab("ReloadBar", false, true);
            var objTrans = obj.transform as RectTransform;
            objTrans.sizeDelta = new Vector2( 640f, 80f );
            objTrans.anchorMin = new Vector2( 0.5f, 0.5f );
            objTrans.anchorMax = new Vector2( 0.5f, 0.5f );
            objTrans.pivot = new Vector2( 0.5f, 0.5f );
            objTrans.localPosition = Vector3.zero;

            var background = PrefabsCore.CreatePrefab( "Background", false, true );
            var bgTrans = background.transform as RectTransform;
            bgTrans.parent = objTrans;
            bgTrans.localPosition = Vector3.zero;
            bgTrans.sizeDelta = Vector2.zero;
            bgTrans.anchorMin = new Vector2( 0f, 0.1f );
            bgTrans.anchorMax = new Vector2( 1f, 0.9f );
            bgTrans.pivot = new Vector2( 0.5f, 0.5f );
            var bgRend = background.AddComponent<CanvasRenderer>();
            bgRend.cullTransparentMesh = false;
            var bgImg = background.AddComponent<Image>();
            bgImg.sprite = null;
            bgImg.color = Color.white;
            bgImg.material = null;
            bgImg.raycastTarget = false;
            bgImg.type = Image.Type.Simple;
            bgImg.useSpriteMesh = false;
            bgImg.preserveAspect = false;


            var slideArea = PrefabsCore.CreatePrefab( "Handle Slide Area", false, true );
            var slideAreaTrans = slideArea.transform as RectTransform;
            slideAreaTrans.parent = objTrans;
            slideAreaTrans.localPosition = Vector3.zero;
            slideAreaTrans.sizeDelta = Vector2.zero;
            slideAreaTrans.anchorMin = new Vector2( 0f, 0f );
            slideAreaTrans.anchorMax = new Vector2( 1f, 1f );
            slideAreaTrans.pivot = new Vector2( 0.5f, 0.5f );

            var handle = PrefabsCore.CreatePrefab( "Handle", false, true );
            var handleTrans = handle.transform as RectTransform;
            handleTrans.parent = slideAreaTrans;
            handleTrans.localPosition = Vector3.zero;
            handleTrans.sizeDelta = new Vector2( 16f, 0f );
            handleTrans.pivot = new Vector2( 0.5f, 0.5f );
            var handleRend = handle.AddComponent<CanvasRenderer>();
            handleRend.cullTransparentMesh = false;
            var handleImg = handle.AddComponent<Image>();
            var tex = TexturesCore.GenerateBarTexture(128, 640, true, 64, 16, Color.black, reloadHandleColor, 4 );
            handleImg.sprite = Sprite.Create( tex, new Rect( 0f, 0f, tex.width, tex.height ), new Vector2( 0.5f, 0.5f ) );
            handleImg.color = Color.white;
            handleImg.material = null;
            handleImg.raycastTarget = false;
            handleImg.type = Image.Type.Simple;
            handleImg.useSpriteMesh = false;
            handleImg.preserveAspect = false;

            var slider = obj.AddComponent<Slider>();
            slider.interactable = false;
            slider.transition = Selectable.Transition.None;
            slider.navigation = new Navigation { mode = Navigation.Mode.None };
            slider.fillRect = null;
            slider.handleRect = handleTrans;
            slider.direction = Slider.Direction.LeftToRight;
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.wholeNumbers = false;
            slider.value = 0f;

            obj.AddComponent<ReloadUIController>();


            return obj;
        }
    }
}
