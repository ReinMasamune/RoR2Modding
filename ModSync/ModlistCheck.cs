namespace ModSync
{
    using R2API;
    using System;

    public partial class Main
    {
        internal Boolean CheckList( ModListAPI.ModList clientList, ModPrefs modPrefs )
        {
            //Check vanilla
            var vanillaCheck = true;
            if( !modPrefs.header.vanillaAllowed )
            {
                if( clientList.isVanilla )
                {
                    base.Logger.LogWarning( "Kicking player due to disallowed vanilla players" );
                    vanillaCheck = false;
                }
            }
            //Check modded
            var moddedCheck = true;
            if( !modPrefs.header.moddedAllowed )
            {
                if( !clientList.isVanilla )
                {
                    base.Logger.LogWarning( "Kicking player due to disallowed modded players" );
                    moddedCheck = false;
                }
            }

            //Check required mods
            var requiredCheck = true;
            if( modPrefs.header.enforceRequiredMods )
            {
                foreach( ModPrefs.PrefList.PrefEntry entry in modPrefs.requiredMods.mods )
                {
                    var tempCheck = false;
                    foreach( ModListAPI.ModInfo mod in clientList.mods )
                    {
                        if( entry.Check( mod ) )
                        {
                            tempCheck = true;
                        }
                    }
                    if( !tempCheck )
                    {
                        base.Logger.LogWarning( "Kicking player due to missing required mod: " + entry.guid );
                        requiredCheck = false;
                    }
                }
            }

            //Check banned
            var bannedCheck = true;
            if( modPrefs.header.enforceBannedMods )
            {
                foreach( ModListAPI.ModInfo mod in clientList.mods )
                {
                    if( !modPrefs.bannedMods.mods.TrueForAll( (x) => !x.Check( mod ) ) )
                    {
                        base.Logger.LogWarning( "Kicking player due to banned mod: " + mod.guid );
                        bannedCheck = false;
                    }
                }
            }

            //Check approved
            var allowedCheck = true;
            if( modPrefs.header.enforceApprovedMods )
            {
                foreach( ModListAPI.ModInfo mod in clientList.mods )
                {
                    var tempCheck = false;
                    foreach( ModPrefs.PrefList.PrefEntry entry in modPrefs.approvedMods.mods )
                    {
                        if( entry.Check( mod ) )
                        {
                            tempCheck = true;
                        }
                    }
                    if( modPrefs.header.enforceApprovedMods )
                    {
                        foreach( ModPrefs.PrefList.PrefEntry entry in modPrefs.requiredMods.mods )
                        {
                            if( entry.Check( mod ) )
                            {
                                tempCheck = true;
                            }
                        }
                    }

                    if( !tempCheck )
                    {
                        base.Logger.LogWarning( "Kicking player due to unapproved mod: " + mod.guid );
                    }
                }
            }

            return vanillaCheck && moddedCheck && requiredCheck && bannedCheck && allowedCheck;
        }
    }
}
