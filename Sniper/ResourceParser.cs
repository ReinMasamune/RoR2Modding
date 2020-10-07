namespace Rein.Sniper.Properties
{
	using System;

	using ReinCore;

	using Sniper.Modules;

	using UnityEngine;

	using Object = System.Object;
	//using Resources = Sniper.Properties.Resources;
	using Random = System.Random;

	using UnityResources = UnityEngine.Resources;
	using UnityObject = UnityEngine.Object;
	using UnityRandom = UnityEngine.Random;

	internal static class Tokens
	{
		#pragma warning disable IDE1006 // Naming Styles
		#pragma warning disable CA1707 // Identifiers should not contain underscores
        internal const String SNIPER_AMMO_BURST_DESC = "SNIPER_AMMO_BURST_DESC";
        internal const String SNIPER_AMMO_BURST_NAME = "SNIPER_AMMO_BURST_NAME";
        internal const String SNIPER_AMMO_EXPLOSIVE_DESC = "SNIPER_AMMO_EXPLOSIVE_DESC";
        internal const String SNIPER_AMMO_EXPLOSIVE_NAME = "SNIPER_AMMO_EXPLOSIVE_NAME";
        internal const String SNIPER_AMMO_PLASMA_DESC = "SNIPER_AMMO_PLASMA_DESC";
        internal const String SNIPER_AMMO_PLASMA_NAME = "SNIPER_AMMO_PLASMA_NAME";
        internal const String SNIPER_AMMO_SHOCK_DESC = "SNIPER_AMMO_SHOCK_DESC";
        internal const String SNIPER_AMMO_SHOCK_NAME = "SNIPER_AMMO_SHOCK_NAME";
        internal const String SNIPER_AMMO_STANDARD_DESC = "SNIPER_AMMO_STANDARD_DESC";
        internal const String SNIPER_AMMO_STANDARD_NAME = "SNIPER_AMMO_STANDARD_NAME";
        internal const String SNIPER_DESC = "SNIPER_DESC";
        internal const String SNIPER_DISPLAY_NAME = "SNIPER_DISPLAY_NAME";
        internal const String SNIPER_KEYWORD_BOOST = "SNIPER_KEYWORD_BOOST";
        internal const String SNIPER_KEYWORD_EXPLOSIVE = "SNIPER_KEYWORD_EXPLOSIVE";
        internal const String SNIPER_KEYWORD_PHASED = "SNIPER_KEYWORD_PHASED";
        internal const String SNIPER_KEYWORD_PIERCING = "SNIPER_KEYWORD_PIERCING";
        internal const String SNIPER_KEYWORD_PRIMARYDMG = "SNIPER_KEYWORD_PRIMARYDMG";
        internal const String SNIPER_KEYWORD_REACTIVATION = "SNIPER_KEYWORD_REACTIVATION";
        internal const String SNIPER_KEYWORD_RELOADS = "SNIPER_KEYWORD_RELOADS";
        internal const String SNIPER_KEYWORD_RICOCHET = "SNIPER_KEYWORD_RICOCHET";
        internal const String SNIPER_KEYWORD_SCOPED = "SNIPER_KEYWORD_SCOPED";
        internal const String SNIPER_NAME = "SNIPER_NAME";
        internal const String SNIPER_OUTRO_FLAVOR = "SNIPER_OUTRO_FLAVOR";
        internal const String SNIPER_PRIMARY_DASH_DESC = "SNIPER_PRIMARY_DASH_DESC";
        internal const String SNIPER_PRIMARY_DASH_NAME = "SNIPER_PRIMARY_DASH_NAME";
        internal const String SNIPER_PRIMARY_DASH_RELOAD_DESC = "SNIPER_PRIMARY_DASH_RELOAD_DESC";
        internal const String SNIPER_PRIMARY_DASH_RELOAD_NAME = "SNIPER_PRIMARY_DASH_RELOAD_NAME";
        internal const String SNIPER_PRIMARY_MAG_DESC = "SNIPER_PRIMARY_MAG_DESC";
        internal const String SNIPER_PRIMARY_MAG_NAME = "SNIPER_PRIMARY_MAG_NAME";
        internal const String SNIPER_PRIMARY_MAG_RELOAD_DESC = "SNIPER_PRIMARY_MAG_RELOAD_DESC";
        internal const String SNIPER_PRIMARY_MAG_RELOAD_NAME = "SNIPER_PRIMARY_MAG_RELOAD_NAME";
        internal const String SNIPER_PRIMARY_SNIPE_DESC = "SNIPER_PRIMARY_SNIPE_DESC";
        internal const String SNIPER_PRIMARY_SNIPE_NAME = "SNIPER_PRIMARY_SNIPE_NAME";
        internal const String SNIPER_PRIMARY_SNIPE_RELOAD_DESC = "SNIPER_PRIMARY_SNIPE_RELOAD_DESC";
        internal const String SNIPER_PRIMARY_SNIPE_RELOAD_NAME = "SNIPER_PRIMARY_SNIPE_RELOAD_NAME";
        internal const String SNIPER_SECONDARY_QUICK_DESC = "SNIPER_SECONDARY_QUICK_DESC";
        internal const String SNIPER_SECONDARY_QUICK_NAME = "SNIPER_SECONDARY_QUICK_NAME";
        internal const String SNIPER_SECONDARY_STEADY_DESC = "SNIPER_SECONDARY_STEADY_DESC";
        internal const String SNIPER_SECONDARY_STEADY_NAME = "SNIPER_SECONDARY_STEADY_NAME";
        internal const String SNIPER_SKIN_ALT1_NAME = "SNIPER_SKIN_ALT1_NAME";
        internal const String SNIPER_SKIN_ALT2_NAME = "SNIPER_SKIN_ALT2_NAME";
        internal const String SNIPER_SKIN_ALT3_NAME = "SNIPER_SKIN_ALT3_NAME";
        internal const String SNIPER_SKIN_ALT4_NAME = "SNIPER_SKIN_ALT4_NAME";
        internal const String SNIPER_SKIN_ALT5_NAME = "SNIPER_SKIN_ALT5_NAME";
        internal const String SNIPER_SKIN_ALT6_NAME = "SNIPER_SKIN_ALT6_NAME";
        internal const String SNIPER_SKIN_DEFAULT_NAME = "SNIPER_SKIN_DEFAULT_NAME";
        internal const String SNIPER_SPECIAL_DECOY_DESC = "SNIPER_SPECIAL_DECOY_DESC";
        internal const String SNIPER_SPECIAL_DECOY_NAME = "SNIPER_SPECIAL_DECOY_NAME";
        internal const String SNIPER_SPECIAL_KNIFE_DESC = "SNIPER_SPECIAL_KNIFE_DESC";
        internal const String SNIPER_SPECIAL_KNIFE_NAME = "SNIPER_SPECIAL_KNIFE_NAME";
        internal const String SNIPER_SUBTITLE = "SNIPER_SUBTITLE";
        internal const String SNIPER_UTILITY_BACKFLIP_DESC = "SNIPER_UTILITY_BACKFLIP_DESC";
        internal const String SNIPER_UTILITY_BACKFLIP_NAME = "SNIPER_UTILITY_BACKFLIP_NAME";
		#pragma warning restore IDE1006 // Naming Styles
		#pragma warning restore CA1707 // Identifiers should not contain underscores
	}

	internal static class ComputeShaders
	{
        private static ComputeShader _ScopeOverlay;
        internal static ComputeShader ScopeOverlay => _ScopeOverlay ??= AssetModule.LoadAsset<ComputeShader>(Resources.cshader__ScopeOverlay);
	}

	internal static class Prefabs
	{
        private static GameObject _Crosshair;
        internal static GameObject Crosshair => _Crosshair ??= AssetModule.LoadAsset<GameObject>(Resources.prefab__Crosshair);
        private static GameObject _KnifeGhostPrefab;
        internal static GameObject KnifeGhostPrefab => _KnifeGhostPrefab ??= AssetModule.LoadAsset<GameObject>(Resources.prefab__KnifeGhostPrefab);
        private static GameObject _SniperPrefab;
        internal static GameObject SniperPrefab => _SniperPrefab ??= AssetModule.LoadAsset<GameObject>(Resources.prefab__SniperPrefab);
	}

	internal static class Icons
	{
        private static Sprite _BackflipIcon;
        internal static Sprite BackflipIcon => _BackflipIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__BackflipIcon);
        private static Sprite _BurstAmmoIcon;
        internal static Sprite BurstAmmoIcon => _BurstAmmoIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__BurstAmmoIcon);
        private static Sprite _CritPassiveIcon;
        internal static Sprite CritPassiveIcon => _CritPassiveIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__CritPassiveIcon);
        private static Sprite _CrosshairCenter;
        internal static Sprite CrosshairCenter => _CrosshairCenter ??= AssetModule.LoadAsset<Sprite>(Resources.icon__CrosshairCenter);
        private static Sprite _DecoyIcon;
        internal static Sprite DecoyIcon => _DecoyIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__DecoyIcon);
        private static Sprite _DecoyReactivateIcon;
        internal static Sprite DecoyReactivateIcon => _DecoyReactivateIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__DecoyReactivateIcon);
        private static Sprite _ExplosiveAmmoIcon;
        internal static Sprite ExplosiveAmmoIcon => _ExplosiveAmmoIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__ExplosiveAmmoIcon);
        private static Sprite _HeadshotIcon;
        internal static Sprite HeadshotIcon => _HeadshotIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__HeadshotIcon);
        private static Sprite _KnifeIcon;
        internal static Sprite KnifeIcon => _KnifeIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__KnifeIcon);
        private static Sprite _KnifeReactivateIcon;
        internal static Sprite KnifeReactivateIcon => _KnifeReactivateIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__KnifeReactivateIcon);
        private static Sprite _PlasmaAmmoIcon;
        internal static Sprite PlasmaAmmoIcon => _PlasmaAmmoIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__PlasmaAmmoIcon);
        private static Sprite _PortraitIcon;
        internal static Sprite PortraitIcon => _PortraitIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__PortraitIcon);
        private static Sprite _QuickscopeIcon;
        internal static Sprite QuickscopeIcon => _QuickscopeIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__QuickscopeIcon);
        private static Sprite _ReloadIcon;
        internal static Sprite ReloadIcon => _ReloadIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__ReloadIcon);
        private static Sprite _ShockAmmoIcon;
        internal static Sprite ShockAmmoIcon => _ShockAmmoIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__ShockAmmoIcon);
        private static Sprite _SlideIcon;
        internal static Sprite SlideIcon => _SlideIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__SlideIcon);
        private static Sprite _SlideReloadIcon;
        internal static Sprite SlideReloadIcon => _SlideReloadIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__SlideReloadIcon);
        private static Sprite _SnipeIcon;
        internal static Sprite SnipeIcon => _SnipeIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__SnipeIcon);
        private static Sprite _SnipeMag;
        internal static Sprite SnipeMag => _SnipeMag ??= AssetModule.LoadAsset<Sprite>(Resources.icon__SnipeMag);
        private static Sprite _SnipeMagReload;
        internal static Sprite SnipeMagReload => _SnipeMagReload ??= AssetModule.LoadAsset<Sprite>(Resources.icon__SnipeMagReload);
        private static Sprite _StandardAmmoIcon;
        internal static Sprite StandardAmmoIcon => _StandardAmmoIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__StandardAmmoIcon);
        private static Sprite _SteadyAimIcon;
        internal static Sprite SteadyAimIcon => _SteadyAimIcon ??= AssetModule.LoadAsset<Sprite>(Resources.icon__SteadyAimIcon);
	}

	internal static class Textures
	{
        private static GameObject _ScopeOverlayMask;
        internal static GameObject ScopeOverlayMask => _ScopeOverlayMask ??= AssetModule.LoadAsset<GameObject>(Resources.texture__ScopeOverlayMask);
	}

	namespace Achievements
	{
	}
}