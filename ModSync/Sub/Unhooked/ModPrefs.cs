namespace ModSync
{
    using BepInEx;
    using R2API.Utils;
    using System;
    using System.Reflection;
    using RoR2.Networking;
    using System.Collections.Generic;
    using UnityEngine.Networking;
    using Facepunch.Steamworks;

    public partial class Main
    {
        private ModPrefs GetModPrefs()
        {
            var requiredList = new HashSet<ParsedModSettings>();
            var bannedList = new HashSet<ParsedModSettings>();
            var allowedList = new HashSet<ParsedModSettings>();
            foreach( ConfigModSettings settings in this.requiredMods )
            {
                var parsed = new ParsedModSettings( settings.data.Value );
                if( parsed.valid ) requiredList.Add( parsed );
            }
            foreach( ConfigModSettings settings in this.bannedMods )
            {
                var parsed = new ParsedModSettings( settings.data.Value );
                if( parsed.valid ) bannedList.Add( parsed );
            }
            foreach( ConfigModSettings settings in this.allowedMods )
            {
                var parsed = new ParsedModSettings( settings.data.Value );
                if( parsed.valid ) allowedList.Add( parsed );
            }

            return new ModPrefs
            {
                allowVanilla = this.allowVanilla.Value,
                allowModded = this.allowModded.Value,
                forceExactMatch = this.forceExactMatch.Value,
                enforceConfig = this.enforceConfigMatching.Value,
                enforceRequired = this.enforceRequiredMods.Value,
                enforceBanned = this.enforceBannedMods.Value,
                enforceAllowed = this.enforceAllowedMods.Value,
                serverMods = this.modList,
                allowedMods = allowedList,
                bannedMods = bannedList,
                requiredMods = requiredList
            };

        }

        private struct ModPrefs
        {
            public Boolean allowVanilla;
            public Boolean allowModded;
            public Boolean forceExactMatch;
            public Boolean enforceConfig;
            public Boolean enforceRequired;
            public Boolean enforceBanned;
            public Boolean enforceAllowed;

            public HashSet<String> exactMatchIgnores;
            public HashSet<ParsedModSettings> allowedMods;
            public HashSet<ParsedModSettings> bannedMods;
            public HashSet<ParsedModSettings> requiredMods;

            public ModList serverMods;

            public Boolean Check( ModList list )
            {
                if( !this.allowModded ) return false;

                if( this.forceExactMatch )
                {
                    Int32 modCounter = 0;
                    Int32 ignoreCounter = 0;
                    Int32 overlapCounter = 0;
                    foreach( ModInfo mod in this.serverMods.mods )
                    {
                        var check = false;
                        if( this.exactMatchIgnores.Contains( mod.guid ) )
                        {
                            check = true;
                            ignoreCounter++;
                        }
                        foreach( ModInfo mod2 in list.mods )
                        {
                            if( mod.guid == mod2.guid && mod.version == mod2.version )
                            {
                                if( check ) overlapCounter++; else modCounter++;
                                check = true;
                                break;
                            }
                        }
                        if( !check ) return false;
                    }
                    var clientTotal = list.mods.Count - overlapCounter;
                    var serverTotal = this.serverMods.mods.Count - ignoreCounter;
                    if( clientTotal != serverTotal ) return false;      
                }

                if( this.enforceRequired )
                {
                    foreach( ParsedModSettings settings in this.requiredMods )
                    {
                        var check = false;
                        foreach( ModInfo mod in list.mods )
                        {
                            if( mod.guid == settings.guid )
                            {
                                var minCheck = settings.minVerValid || settings.minVersion <= mod.version;
                                var maxCheck = settings.maxVerValid || mod.version <= settings.maxVersion;
                                if( minCheck && maxCheck )
                                {
                                    check = true;
                                    break;
                                }
                            }
                        }

                        if( !check ) return false;
                    }
                }

                if( this.enforceBanned )
                {
                    foreach( ParsedModSettings settings in this.bannedMods )
                    {
                        foreach( ModInfo mod in list.mods )
                        {
                            if( mod.guid == settings.guid )
                            {
                                var minCheck = settings.minVerValid || settings.minVersion <= mod.version;
                                var maxCheck = settings.maxVerValid || mod.version <= settings.maxVersion;
                                if( minCheck && maxCheck ) return false;
                            }
                        }
                    }
                }

                if( this.enforceAllowed )
                {
                    foreach( ModInfo mod in list.mods )
                    {
                        var check = false;
                        foreach( ParsedModSettings settings in this.requiredMods )
                        {
                            if( mod.guid == settings.guid )
                            {
                                var minCheck = settings.minVerValid || settings.minVersion <= mod.version;
                                var maxCheck = settings.maxVerValid || mod.version <= settings.maxVersion;
                                if( minCheck && maxCheck ) check = true;
                            }
                        }
                        foreach( ParsedModSettings settings in this.allowedMods )
                        {
                            if( mod.guid == settings.guid )
                            {
                                var minCheck = settings.minVerValid || settings.minVersion <= mod.version;
                                var maxCheck = settings.maxVerValid || mod.version <= settings.maxVersion;
                                if( minCheck && maxCheck ) check = true;
                            }
                        }
                        if( !check ) return false;
                    }
                }
                return true;
            }
        }
    }
}
