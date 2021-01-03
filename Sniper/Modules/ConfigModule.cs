namespace Rein.Sniper.Modules
{
    using System;

    using BepInEx.Configuration;

    using RoR2;

    using Rein.Sniper.ScriptableObjects;
    using Rein.Sniper.UI.Components;

    using UnityEngine;

    using ReinCore;

    internal static class ConfigModule
    {
        #region Crosshair
        internal static NibController.NibType topNib { get => _topNib.Value; }
        internal static NibController.NibType leftNib { get => _leftNib.Value; }
        internal static NibController.NibType rightNib { get => _rightNib.Value; }
        internal static Boolean rotate180 { get => _rotate180.Value; }
        internal static Boolean showCenterDot { get => _showCenterDot.Value; }
        internal static Single scale { get => _scale.Value; }
        internal static Single emptyAlpha => _emptyAlpha.Value;
        internal static Single badAlpha => _badAlpha.Value;
        internal static Single goodAlpha => _goodAlpha.Value;
        internal static Single perfectAlpha => _perfectAlpha.Value;
        internal static Single reloadIndScale => _reloadIndScale.Value;
        internal static Single reloadIndXOffset => _reloadIndXOffset.Value;
        internal static Single reloadIndYOffset => _reloadIndYOffset.Value;



        private static ConfigEntry<NibController.NibType> _topNib;
        private static ConfigEntry<NibController.NibType> _leftNib;
        private static ConfigEntry<NibController.NibType> _rightNib;
        private static ConfigEntry<Boolean> _rotate180;
        private static ConfigEntry<Boolean> _showCenterDot;
        private static ConfigEntry<Single> _scale;
        private static ConfigEntry<Single> _emptyAlpha;
        private static ConfigEntry<Single> _badAlpha;
        private static ConfigEntry<Single> _goodAlpha;
        private static ConfigEntry<Single> _perfectAlpha;
        private static ConfigEntry<Single> _reloadIndScale;
        private static ConfigEntry<Single> _reloadIndXOffset;
        private static ConfigEntry<Single> _reloadIndYOffset;

        private static void SetupCrosshairConfig(SniperMain plugin)
        {
            const String section = "Crosshair";
            _topNib = plugin.Config.Bind<NibController.NibType>(section, "Top nib type", NibController.NibType.Bar, "The shape of the upper crosshair nib");
            _leftNib = plugin.Config.Bind<NibController.NibType>(section, "Left nib type", NibController.NibType.Bar, "The shape of the left crosshair nib");
            _rightNib = plugin.Config.Bind<NibController.NibType>(section, "Right nib type", NibController.NibType.Bar, "The shape of the right crosshair nib");
            _rotate180 = plugin.Config.Bind<Boolean>(section, "Flip Vertical", false, "Should the crosshair be inverted vertically?");
            _showCenterDot = plugin.Config.Bind<Boolean>(section, "Dot in center", true, "Should the center dot be shown?");
            _scale = plugin.Config.Bind<Single>(section, "Size", 1.0f, "The overall size of the crosshair");
            _emptyAlpha = plugin.Config.Bind<Single>(section, "Empty Opacity", 1.0f, "0.0 - 1.0, the opacity of the empty reload indicator");
            _badAlpha = plugin.Config.Bind<Single>(section, "Bad Opacity", 1.0f, "0.0 - 1.0, the opacity of the bad reload indicator");
            _goodAlpha = plugin.Config.Bind<Single>(section, "Good Opacity", 1.0f, "0.0 - 1.0, the opacity of the good reload indicator");
            _perfectAlpha = plugin.Config.Bind<Single>(section, "Perfect Opacity", 1.0f, "0.0 - 1.0, the opacity of the perfect reload indicator");
            _reloadIndScale = plugin.Config.Bind<Single>(section, "Reload Indicator size", 1.0f, "The overall size of the reload indicator");
            _reloadIndXOffset = plugin.Config.Bind<Single>(section, "Reload Indicator X offset", 0.0f, "The position of the reload indicator on the X axis");
            _reloadIndYOffset = plugin.Config.Bind<Single>(section, "Reload Indicator Y offset", 45.0f, "The position of the reload indicator on the Y axis");
        }
        #endregion

        #region Scope
        internal static Single zoomSpeed { get => _zoomSpeed.Value; }
        internal static Boolean zoomNoPersist => _zoomNoPersist.Value;

        private static ConfigEntry<Single> _zoomSpeed;
        private static ConfigEntry<Boolean> _zoomNoPersist;

        private static void SetupScopeConfig(SniperMain plugin)
        {
            const String section = "Scope";
            _zoomSpeed = plugin.Config.Bind<Single>(section, "Zoom speed", 1f, "The speed that zoom changes");
            _zoomNoPersist = plugin.Config.Bind<Boolean>(section, "Reset zoom on scope", true, "Should scope reset to default zoom on each use?");
        }
        #endregion


        #region Sound
        internal static Single sfxVolume { get => _sfxVolume.Value; }
        internal static Single shotsVolume { get => _shotsVolume.Value; }


        private static ConfigEntry<Single> _sfxVolume;
        private static ConfigEntry<Single> _shotsVolume;

        private static void SetupSoundConfig(SniperMain plugin)
        {
            const String section = "Sounds";
            _sfxVolume = plugin.Config.Bind<Single>(section, "Sound effects volume", 80f, "The volume of the sounds from this mod.");
            _shotsVolume = plugin.Config.Bind<Single>(section, "Fire sound effects volume", 80f, "The volume of the fire sounds from this mod.");
        }
        #endregion


        #region Unlocks
        internal static ConfigEntry<EclipseLevel> _eclipseLevel { get; private set; }
        
        private static void SetupUnlocksConfig(SniperMain plugin)
        {
            const String section = "Unlocks";
            _eclipseLevel = plugin.Config.Bind<EclipseLevel>(section, "Current Eclipse level", EclipseLevel.One, "The current eclipse level. If your eclipse level is lower than this setting on game launch, you will automatically unlock up to the level of this setting. As you unlock more eclipse levels in game, this setting will update automatically as a sort of save file.");
        }
        #endregion

        #region Performance
        internal static Single sporeScanResolution => _sporeScanResolution.Value;
        internal static Single sporeParticleMultiplier => _sporeParticleMultiplier.Value;
        internal static Boolean showSporeRangeIndicator => _showSporeRangeIndicator.Value;


        private static ConfigEntry<Single> _sporeScanResolution;
        private static ConfigEntry<Single> _sporeParticleMultiplier;
        private static ConfigEntry<Boolean> _showSporeRangeIndicator;

        private static void SetupPerformanceConfig(SniperMain plugin)
        {
            const String section = "Performance";
            _sporeScanResolution = plugin.Config.Bind<Single>(section, "Spore scan frequency", 3f, "How often does spore ammo check for targets inside the radius. This does not impact the tick rate, just how accurate entering and leaving the zone is.");
            _sporeParticleMultiplier = plugin.Config.Bind<Single>(section, "Spore particle multiplier", 1f, "Multiplier applied to the number of particles emitted by the spore ammo zone.");
            _showSporeRangeIndicator = plugin.Config.Bind<Boolean>(section, "Show spore range indicator", true, "Should a range indicator be shown for spore ammo zones in addition to the particles?");
        }
        #endregion


        private static Boolean loaded = false;
        internal static void CreateAndLoadConfig(SniperMain plugin)
        {
            if(loaded) return;
            SetupCrosshairConfig(plugin);
            SetupScopeConfig(plugin);
            SetupSoundConfig(plugin);
            SetupUnlocksConfig(plugin);
            SetupPerformanceConfig(plugin);


            loaded = true;
        }
    }


}
