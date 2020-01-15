namespace ModSync
{
    using BepInEx.Configuration;
    using R2API;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public partial class Main
    {
        const String header = "Mod enforcement settings";
        internal ConfigEntry<Boolean> cfgAllowVanillaPlayers;
        internal ConfigEntry<Boolean> cfgAllowModdedPlayers;
        internal ConfigEntry<Boolean> cfgEnforceRequiredMods;
        internal ConfigEntry<Boolean> cfgEnforceBannedMods;
        internal ConfigEntry<Boolean> cfgEnforceApprovedMods;

        internal String basePath = "\\ModSyncLists";
        internal String requiredPath = "\\RequiredMods.txt";
        internal String bannedPath = "\\BannedMods.txt";
        internal String approvedPath = "\\ApprovedMods.txt";

        const String infoEnd = "----------";
        internal String[] requiredModsInfo =  new String[]
        {
            "Required Mods",
            "If enabled in the standard config, any connecting players must have all mods on this list installed.",
            "Format is GUID | Enforce Config | Min Version | Max Version",
            "Example: com.testMods.RandomMod | false | 0.0.0 | 2.1.0",
            "GUID = the name of the mod (Might not match thunderstore, need to ask the developer or decompile and check",
            "Enforce Config = does nothing at the moment, true or false",
            "Min Version = The lowest version number accepted. Leave blank for no minimum.",
            "Max Version = The highest version number accepted. Leave blank for no maximum.",
            "Recommended settings for min and max are minimum set to version installed on server, and max left blank.",
            infoEnd
        };

        internal String[] bannedModsInfo =  new String[]
        {
            "Banned Mods",
            "If enabled in the standard config, players will be unable to connect with any of the mods on this list installed.",
            "Format is GUID | Enforce Config | Min Version | Max Version",
            "Example: com.testMods.RandomMod | false | 0.0.0 | 2.1.0",
            "GUID = the name of the mod (Might not match thunderstore, need to ask the developer or decompile and check",
            "Enforce Config = does nothing at the moment, true or false",
            "Min Version = The lowest version number accepted. Leave blank for no minimum.",
            "Max Version = The highest version number accepted. Leave blank for no maximum.",
            infoEnd
        };

        internal String[] approvedModsInfo =  new String[]
        {
            "Approved Mods",
            "If enabled in the standard config, any players with mods that are not on this list (Or required list) will be unable to connect.",
            "Format is GUID | Enforce Config | Min Version | Max Version",
            "Example: com.testMods.RandomMod | false | 0.0.0 | 2.1.0",
            "GUID = the name of the mod (Might not match thunderstore, need to ask the developer or decompile and check",
            "Enforce Config = does nothing at the moment, true or false",
            "Min Version = The lowest version number accepted. Leave blank for no minimum.",
            "Max Version = The highest version number accepted. Leave blank for no maximum.",
            infoEnd
        };

        const String requiredLine2 = "If enabled in the standard config, any connecting players must have all mods on this list installed.";
        const String requiredLine3 = "Min Version ";
        const String requiredLine4 = "#Required Mods";
        const String requiredLine5 = "#Required Mods";
        const String requiredLine6 = "#Required Mods";

        private List<ModEntry> requiredEntries = new List<ModEntry>();
        private List<ModEntry> bannedEntries = new List<ModEntry>();
        private List<ModEntry> approvedEntries = new List<ModEntry>();

        internal String baseDirectoryPath
        {
            get
            {
                return base.Config.ConfigFilePath.Replace( base.Info.Metadata.GUID + ".cfg", this.basePath );
            }
        }

        internal String fullRequiredPath
        {
            get
            {
                return this.baseDirectoryPath + this.requiredPath;
            }
        }

        internal String fullBannedPath
        {
            get
            {
                return this.baseDirectoryPath + this.bannedPath;
            }
        }

        internal String fullApprovedPath
        {
            get
            {
                return this.baseDirectoryPath + this.approvedPath;
            }
        }

        internal void BuildConfig()
        {
            this.cfgAllowModdedPlayers = base.Config.Bind<Boolean>( header, "Allow Modded Players", true,
                "Should players with mods be allowed to connect?" );
            this.cfgAllowVanillaPlayers = base.Config.Bind<Boolean>( header, "Allow Vanilla Players", false,
                "Should players without mods be allowed to connect?" );
            this.cfgEnforceRequiredMods = base.Config.Bind<Boolean>( header, "Enforce Required Mods", false,
                "Should a list of required mods be enforced for connecting players?" );
            this.cfgEnforceBannedMods = base.Config.Bind<Boolean>( header, "Enforce Banned Mods", false,
                "Should a list of banned mods be enforced for connecting players? (Blacklist)" );
            this.cfgEnforceApprovedMods = base.Config.Bind<Boolean>( header, "Enforce Approved Mods", false,
                "Should a list of allowed mods be enforced for connecting players? (Whitelist)" );

            if( !Directory.Exists( this.baseDirectoryPath ) )
            {
                Directory.CreateDirectory( this.baseDirectoryPath );
            }

            if( !File.Exists( this.fullRequiredPath ) )
            {
                base.Logger.LogInfo( "Required Mods file not found, creating new." );

                File.AppendAllLines( this.fullRequiredPath, this.requiredModsInfo );
            } else
            {
                var infoList = new List<String>();
                var modsList = new List<String>();

                var infoDone = false;
                foreach( String s in File.ReadAllLines(this.fullRequiredPath ) )
                {
                    if( infoDone )
                    {
                        modsList.Add( s );
                    } else
                    {
                        if( s == infoEnd )
                        {
                            infoDone = true;
                        } else
                        {
                            infoList.Add( s );
                        }
                    }
                }

                var writeNewInfo = false;
                if( infoList.Count != this.requiredModsInfo.Length )
                {
                    writeNewInfo = true;
                } else
                {
                    for( Int32 i = 0; i < infoList.Count; ++i )
                    {
                        if( infoList[i] != this.requiredModsInfo[i] ) writeNewInfo = true;
                    }
                }

                if( writeNewInfo )
                {
                    File.WriteAllLines( this.fullRequiredPath, this.requiredModsInfo );
                    File.AppendAllLines( this.fullRequiredPath, modsList );
                }

                foreach( String s in modsList )
                {
                    if( s == "" )
                    {
                        continue;
                    }
                    try
                    {
                        var en = new ModEntry( s );
                        this.requiredEntries.Add( en );
                    } catch
                    {
                        base.Logger.LogError( "Invalid Line: " + s + " in required mods list, skipping." );
                        continue;
                    }
                }
            }

            if( !File.Exists( this.fullBannedPath ) )
            {
                base.Logger.LogInfo( "Banned Mods file not found, creating new." );

                File.AppendAllLines( this.fullBannedPath, this.bannedModsInfo );
            } else
            {
                var infoList = new List<String>();
                var modsList = new List<String>();

                var infoDone = false;
                foreach( String s in File.ReadAllLines( this.fullBannedPath ) )
                {
                    if( infoDone )
                    {
                        modsList.Add( s );
                    } else
                    {
                        if( s == infoEnd )
                        {
                            infoDone = true;
                        } else
                        {
                            infoList.Add( s );
                        }
                    }
                }

                var writeNewInfo = false;
                if( infoList.Count != this.bannedModsInfo.Length )
                {
                    writeNewInfo = true;
                } else
                {
                    for( Int32 i = 0; i < infoList.Count; ++i )
                    {
                        if( infoList[i] != this.bannedModsInfo[i] ) writeNewInfo = true;
                    }
                }

                if( writeNewInfo )
                {
                    File.WriteAllLines( this.fullBannedPath, this.bannedModsInfo );
                    File.AppendAllLines( this.fullBannedPath, modsList );
                }

                foreach( String s in modsList )
                {
                    if( s == "" )
                    {
                        continue;
                    }
                    try
                    {
                        var en = new ModEntry( s );
                        this.bannedEntries.Add( en );
                    } catch
                    {
                        base.Logger.LogError( "Invalid Line: " + s + " in banned mods list, skipping." );
                        continue;
                    }
                }
            }

            if( !File.Exists( this.fullApprovedPath ) )
            {
                base.Logger.LogInfo( "Approved Mods file not found, creating new." );

                File.AppendAllLines( this.fullApprovedPath, this.approvedModsInfo );
            } else
            {
                var infoList = new List<String>();
                var modsList = new List<String>();

                var infoDone = false;
                foreach( String s in File.ReadAllLines( this.fullApprovedPath ) )
                {
                    if( infoDone )
                    {
                        modsList.Add( s );
                    } else
                    {
                        if( s == infoEnd )
                        {
                            infoDone = true;
                        } else
                        {
                            infoList.Add( s );
                        }
                    }
                }

                var writeNewInfo = false;
                if( infoList.Count != this.approvedModsInfo.Length )
                {
                    writeNewInfo = true;
                } else
                {
                    for( Int32 i = 0; i < infoList.Count; ++i )
                    {
                        if( infoList[i] != this.approvedModsInfo[i] ) writeNewInfo = true;
                    }
                }

                if( writeNewInfo )
                {
                    File.WriteAllLines( this.fullApprovedPath, this.approvedModsInfo );
                    File.AppendAllLines( this.fullApprovedPath, modsList );
                }

                foreach( String s in modsList )
                {
                    if( s == "" )
                    {
                        continue;
                    }
                    try
                    {
                        var en = new ModEntry( s );
                        this.approvedEntries.Add( en );
                    } catch
                    {
                        base.Logger.LogError( "Invalid Line: " + s + " in approved mods list, skipping." );
                        continue;
                    }
                }
            }

        }

        private struct ModEntry
        {
            public String guid;
            public String enforceConfig;
            public String minVersion;
            public String maxVersion;


            //FORMAT:
            //GUID|Enforce Config|Min Version|Max Version
            //com.example.thing | false | 1.0.0 | 2.0.0
            public ModEntry( String text )
            {
                var splits = text.Split('|');

                this.guid = splits[0].Trim().ToLower();
                this.enforceConfig = splits[1].Trim().ToLower();
                this.minVersion = splits[2].Trim().ToLower();
                this.maxVersion = splits[3].Trim().ToLower();
            }

            public ModPrefs.PrefList.PrefEntry prefEntry
            {
                get
                {
                    return new ModPrefs.PrefList.PrefEntry( this.guid, this.enforceConfig, this.minVersion, this.maxVersion );
                }
            }
        }
    }
}
