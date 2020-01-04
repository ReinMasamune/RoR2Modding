namespace ModSync
{
    using BepInEx.Configuration;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public partial class Main
    {
        const String mainCategory = "Mod Enforcement";
        const String requiredCategory = mainCategory + " . Required Mods List";
        const String bannedCategory = mainCategory + " . Banned Mods List";
        const String allowedCategory = mainCategory + " . Allowed Mods List";
        private ConfigEntry<Boolean> allowVanilla;
        private ConfigEntry<Boolean> allowModded;
        private ConfigEntry<Boolean> forceExactMatch;
        private ConfigEntry<Boolean> enforceConfigMatching;
        private ConfigEntry<Boolean> enforceRequiredMods;
        private ConfigEntry<Boolean> enforceBannedMods;
        private ConfigEntry<Boolean> enforceAllowedMods;
        private ConfigEntry<UInt32> requiredModsLength;
        private ConfigEntry<UInt32> bannedModsLength;
        private ConfigEntry<UInt32> allowedModsLength;
        private List<ConfigModSettings> requiredMods = new List<ConfigModSettings>();
        private List<ConfigModSettings> bannedMods = new List<ConfigModSettings>();
        private List<ConfigModSettings> allowedMods = new List<ConfigModSettings>();
        partial void CreateConfig()
        {
            var converter = new BepInEx.Configuration.TypeConverter
            {
                ConvertToString = ( obj, type ) => ((ModSettingsData)obj).String(),
                ConvertToObject = ( str, type ) => new ModSettingsData( str )
            };

            TomlTypeConverter.AddConverter( typeof( ModSettingsData ), converter );

            this.allowVanilla = base.Config.Bind<Boolean>( mainCategory, "AllowVanillaUsers", true, "Should users without any mods be able to connect?" );
            this.allowModded = base.Config.Bind<Boolean>( mainCategory, "AllowModdedUsers", false, "Should users with mods be able to connect?" );
            this.forceExactMatch = base.Config.Bind<Boolean>( mainCategory, "ForceExactModMatch", false, "Block all connections from users who do not have identical modlists." );
            this.enforceConfigMatching = base.Config.Bind<Boolean>( mainCategory, "ForceConfigMatch", false, "Block connections from users who do not have identical configs for required mods" );
            this.enforceRequiredMods = base.Config.Bind<Boolean>( mainCategory, "EnforceRequriredMods", true, "Block connection of users that do not have required mods." );
            this.requiredModsLength = base.Config.Bind<UInt32>( mainCategory, "RequiredModCount", 0, "How many mods are on the list of required mods?" );
            this.enforceBannedMods = base.Config.Bind<Boolean>( mainCategory, "EnforceBannedMods", false, "Block connection of users who have any of the mods on the Banned Mods list." );
            this.bannedModsLength = base.Config.Bind<UInt32>( mainCategory, "BannedModCount", 0, "How many mods are on the list of banned mods?" );
            this.enforceAllowedMods = base.Config.Bind<Boolean>( mainCategory, "EnforceAllowedMods", false, "Block connection of users who have any mods other than those on allowed and required lists" );
            this.allowedModsLength = base.Config.Bind<UInt32>( mainCategory, "AllowedModCount", 0, "How many mods are on the list of allowed mods?" );
            for( UInt32 i = 0; i < this.requiredModsLength.Value; i++ )
            {
                this.requiredMods.Add( new ConfigModSettings( base.Config, requiredCategory, i + 1 ) );
            }
            for( UInt32 i = 0; i < this.bannedModsLength.Value; i++ )
            {
                this.bannedMods.Add( new ConfigModSettings( base.Config, bannedCategory, i + 1 ) );
            }
            for( UInt32 i = 0; i < this.allowedModsLength.Value; i++ )
            {
                this.allowedMods.Add( new ConfigModSettings( base.Config, allowedCategory, i + 1 ) );
            }
        }
        private class ConfigModSettings
        {
            public ConfigEntry<ModSettingsData> data;
            public UInt32 indexInCategory { get; private set; }

            public ConfigModSettings( ConfigFile cfg, String category, UInt32 index )
            {
                this.data = cfg.Bind<ModSettingsData>( category, "Mod" + index, new ModSettingsData( false ) );
                this.indexInCategory = index;
            }
        }

        public struct ModSettingsData
        {
            public readonly String modGUID;
            public readonly String minVersion;
            public readonly String maxVersion;

            public String String()
            {
                var s = "GUID(" + modGUID + "), Min Version(" + minVersion + "), Max Version(" + maxVersion + ")";
                return s;
            }

            public ModSettingsData( String data )
            {
                var split = data.Split( ',' );
                this.modGUID = split[0].Split( '(', ')' )[1].Trim();
                this.minVersion = split[1].Split( '(', ')' )[1].Trim();
                this.maxVersion = split[2].Split( '(', ')' )[1].Trim();
            }

            public ModSettingsData( String modGUID, String minVersion, String maxVersion )
            {
                this.modGUID = modGUID;
                this.minVersion = minVersion;
                this.maxVersion = maxVersion;
            }

            public ModSettingsData( Boolean a )
            {
                this.modGUID = "";
                this.minVersion = "0.0.0";
                this.maxVersion = "10.10.10";
            }
        }

        public struct ParsedModSettings
        {
            public Boolean valid { get; private set; }
            public String guid { get; private set; }
            public Boolean minVerValid { get; private set; }
            public Version minVersion { get; private set; }
            public Boolean maxVerValid { get; private set; }
            public Version maxVersion { get; private set; }

            public ParsedModSettings( ModSettingsData data )
            {
                var dataGuid = data.modGUID;
                this.valid = dataGuid != "";
                this.guid = dataGuid;

                try 
                {
                    this.minVersion = new Version( data.minVersion );
                    this.minVerValid = true;
                }
                catch 
                {
                    this.minVersion = null;
                    this.minVerValid = false; 
                }

                try 
                { 
                    this.maxVersion = new Version( data.maxVersion );
                    this.maxVerValid = true;
                }
                catch 
                {
                    this.maxVersion = null;
                    this.maxVerValid = false;
                }
            }
        }

    }
}
