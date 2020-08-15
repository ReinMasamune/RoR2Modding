namespace Sniper.Modules
{
    using ReinCore;

    using RoR2;

    using Sniper.Components;

    using Unity.Jobs;

    using UnityEngine;
    using UnityEngine.UI;

    internal static class UIModule
    {
        internal static Sprite GetUnfinishedIcon()
        {
            ITextureJob texJob = TexturesCore.GenerateCrossTextureBatch( 512, 512, 64, 16, 2, Color.black, Color.red, Color.white, Color.white, Color.white, Color.white );

            return Sprite.Create(texJob.OutputTextureAndDispose(), new Rect(0f, 0f, 512f, 512f), new Vector2(0.5f, 0.5f));
        }


        internal static Sprite GetStandardAmmoIcon() => AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>(Properties.Resources.icon__StandardAmmoIcon);
        internal static Sprite GetExplosiveAmmoIcon() => AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>(Properties.Resources.icon__ExplosiveAmmoIcon);
        internal static Sprite GetSnipeIcon() => AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>(Properties.Resources.icon__SnipeIcon);
        internal static Sprite GetSnipeReloadIcon() => AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>(Properties.Resources.icon__ReloadIcon);
        internal static Sprite GetSnipeMagIcon() => null;
        internal static Sprite GetSnipeMagReloadIcon() => AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>(Properties.Resources.icon__SnipeMagReload);
        internal static Sprite GetSteadyAimIcon() => AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>(Properties.Resources.icon__SteadyAimIcon);
        internal static Sprite GetCritPassiveIcon() => AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>(Properties.Resources.icon__CritPassiveIcon);
        internal static Sprite GetHeadshotPassiveIcon() => AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>(Properties.Resources.icon__HeadshotIcon);
        internal static Sprite GetBackflipIcon() => AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>(Properties.Resources.icon__BackflipIcon);
        internal static Sprite GetQuickScopeIcon() => AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>(Properties.Resources.icon__QuickscopeIcon);
        internal static Sprite GetKnifeIcon() => null;//AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>( Properties.Resources.KnifeIcon );
        internal static Sprite GetKnifeReactivationIcon() => null;//AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>( Properties.Resources.KnifeReactivationIcon );
        internal static Sprite GetDecoyIcon() => AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>(Properties.Resources.icon__DecoyIcon);
        internal static Sprite GetDecoyReactivationIcon() => AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>(Properties.Resources.icon__DecoyReactivateIcon);
        internal static Sprite GetPlasmaAmmoIcon() => null;//AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>( Properties.Resources.PlasmaAmmoIcon );
        internal static Sprite GetScatterAmmoIcon() => null;// AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>( Properties.Resources.ScatterAmmoIcon );
        internal static Sprite GetShockAmmoIcon() => GetUnfinishedIcon(); //AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>(Properties.Resources.ShockAmmoIcon );









        internal static Color reloadHandleColor { get; } = new Color(1f, 1f, 1f, 1f);

        private static GameObject crosshair;
        internal static GameObject GetCrosshair()
        {
            if(crosshair == null)
            {
                crosshair = AssetModule.GetSniperAssetBundle().LoadAsset<GameObject>(Properties.Resources.prefab__Crosshair);
            }
            return crosshair;
        }


        internal static Texture GetPortraitIcon() => AssetModule.GetSniperAssetBundle().LoadAsset<Texture2D>(Properties.Resources.icon__PortraitIcon);//return null;

        private static GameObject reloadBarPrefab;
        internal static void CreateReloadBarPrefab()
        {
            ITextureJob texBatch = TexturesCore.GenerateBarTextureBatch( 128, 640, true, 64, 16, new Color( 0f, 0f, 0f, 1f ), reloadHandleColor, 4 );
            JobHandle.ScheduleBatchedJobs();

            GameObject obj = PrefabsCore.CreateUIPrefab("ReloadBar", false );
            var objTrans = obj.transform as RectTransform;
            objTrans.sizeDelta = new Vector2(512f, 64f);
            objTrans.anchorMin = new Vector2(0.5f, 0.5f);
            objTrans.anchorMax = new Vector2(0.5f, 0.5f);
            objTrans.pivot = new Vector2(0.5f, 0.5f);
            objTrans.localPosition = new Vector3(0.0f, 0f, 0f);
            //objTrans.localEulerAngles = new Vector3( 0f, 0f, 0f );

            GameObject holder = PrefabsCore.CreateUIPrefab( "BarHolder", false );
            var holderTrans = holder.transform as RectTransform;
            holderTrans.SetParent(objTrans, false);
            holderTrans.sizeDelta = Vector2.zero;
            holderTrans.anchorMax = Vector2.one;
            holderTrans.anchorMin = Vector2.zero;
            holderTrans.pivot = new Vector2(0.5f, 0.5f);
            holderTrans.localPosition = new Vector3(0f, 330f, 0f);

            GameObject background = PrefabsCore.CreateUIPrefab( "Background", false );
            var bgTrans = background.transform as RectTransform;
            bgTrans.SetParent(holderTrans, false);
            bgTrans.localPosition = Vector3.zero;
            bgTrans.sizeDelta = Vector2.zero;
            bgTrans.anchorMin = new Vector2(0f, 0.1f);
            bgTrans.anchorMax = new Vector2(1f, 0.9f);
            bgTrans.pivot = new Vector2(0.5f, 0.5f);
            CanvasRenderer bgRend = background.AddComponent<CanvasRenderer>();
            bgRend.cullTransparentMesh = false;
            Image bgImg = background.AddComponent<Image>();
            bgImg.sprite = null;
            bgImg.color = Color.white;
            bgImg.material = null;
            bgImg.raycastTarget = false;
            bgImg.type = Image.Type.Simple;
            bgImg.useSpriteMesh = false;
            bgImg.preserveAspect = false;


            GameObject slideArea = PrefabsCore.CreateUIPrefab( "Handle Slide Area", false );
            var slideAreaTrans = slideArea.transform as RectTransform;
            slideAreaTrans.SetParent(holderTrans, false);
            slideAreaTrans.localPosition = Vector3.zero;
            slideAreaTrans.sizeDelta = Vector2.zero;
            slideAreaTrans.anchorMin = new Vector2(0f, 0f);
            slideAreaTrans.anchorMax = new Vector2(1f, 1f);
            slideAreaTrans.pivot = new Vector2(0.5f, 0.5f);

            GameObject handle = PrefabsCore.CreateUIPrefab( "Handle", false );
            var handleTrans = handle.transform as RectTransform;
            handleTrans.SetParent(slideAreaTrans, false);
            handleTrans.localPosition = Vector3.zero;
            handleTrans.sizeDelta = new Vector2(16f, 0f);
            handleTrans.pivot = new Vector2(0.5f, 0.5f);
            CanvasRenderer handleRend = handle.AddComponent<CanvasRenderer>();
            handleRend.cullTransparentMesh = false;
            Image handleImg = handle.AddComponent<Image>();
            Texture2D tex = texBatch.OutputTextureAndDispose();
            handleImg.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            handleImg.color = Color.white;
            handleImg.material = null;
            handleImg.raycastTarget = false;
            handleImg.type = Image.Type.Simple;
            handleImg.useSpriteMesh = false;
            handleImg.preserveAspect = false;

            Slider slider = holder.AddComponent<Slider>();
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

            _ = obj.AddComponent<ReloadUIController>();

            reloadBarPrefab = obj;
        }

        internal static GameObject GetRelodBar()
        {
            if(!reloadBarPrefab)
            {
                CreateReloadBarPrefab();
            }
            GameObject bar = UnityEngine.Object.Instantiate<GameObject>( reloadBarPrefab );
            return bar;
        }

        //public static GameObject hudPrefab;
        internal static void EditHudPrefab()
        {
            //hudPrefab = Resources.Load<GameObject>( "Prefabs/HUDSimple" );
        }
    }
}
