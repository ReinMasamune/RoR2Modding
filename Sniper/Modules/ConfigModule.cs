namespace Rein.Sniper.Modules
{
    using System;

    using BepInEx.Configuration;

    using RoR2;

    using Rein.Sniper.ScriptableObjects;
    using Rein.Sniper.UI.Components;

    using UnityEngine;

    internal static class ConfigModule
    {
        #region Crosshair
        internal static NibController.NibType topNib { get => _topNib.Value; }
        internal static NibController.NibType leftNib { get => _leftNib.Value; }
        internal static NibController.NibType rightNib { get => _rightNib.Value; }
        internal static Boolean rotate180 { get => _rotate180.Value; }
        internal static Boolean showCenterDot { get => _showCenterDot.Value; }
        internal static Single scale { get => _scale.Value; }


        private static ConfigEntry<NibController.NibType> _topNib;
        private static ConfigEntry<NibController.NibType> _leftNib;
        private static ConfigEntry<NibController.NibType> _rightNib;
        private static ConfigEntry<Boolean> _rotate180;
        private static ConfigEntry<Boolean> _showCenterDot;
        private static ConfigEntry<Single> _scale;

        private static void SetupCrosshairConfig(SniperMain plugin)
        {
            const String section = "Crosshair";
            _topNib = plugin.Config.Bind<NibController.NibType>(section, "Top nib type", NibController.NibType.Bar, "The shape of the upper crosshair nib");
            _leftNib = plugin.Config.Bind<NibController.NibType>(section, "Left nib type", NibController.NibType.Bar, "The shape of the left crosshair nib");
            _rightNib = plugin.Config.Bind<NibController.NibType>(section, "Right nib type", NibController.NibType.Bar, "The shape of the right crosshair nib");
            _rotate180 = plugin.Config.Bind<Boolean>(section, "Flip Vertical", false, "Should the crosshair be inverted vertically?");
            _showCenterDot = plugin.Config.Bind<Boolean>(section, "Dot in center", true, "Should the center dot be shown?");
            _scale = plugin.Config.Bind<Single>(section, "Size", 1.0f, "The overall size of the crosshair");
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
            _sfxVolume = plugin.Config.Bind<Single>(section, "Sound effects volume", 100f, "The volume of the sounds from this mod.");
            _shotsVolume = plugin.Config.Bind<Single>(section, "Fire sound effects volume", 100f, "The volume of the fire sounds from this mod.");
        }
        #endregion


        #region Unlocks
        private static ConfigEntry<Int32> _eclipseLevel;



        #endregion


        private static Boolean loaded = false;
        internal static void CreateAndLoadConfig(SniperMain plugin)
        {
            if(loaded) return;
            SetupCrosshairConfig(plugin);
            SetupScopeConfig(plugin);
            SetupSoundConfig(plugin);


            loaded = true;
        }
    }
}
