namespace ModSync
{
    using R2API;
    using System;
    using System.Collections.Generic;

    public partial class Main
    {
        private static ModPrefs prefs;
        internal ModPrefs GetModPrefs()
        {
            if( prefs == null )
            {
                this.BuildModPrefs();
            }

            return prefs;
        }

        private void BuildModPrefs()
        {
            var tempPrefs = new ModPrefs();
            tempPrefs.header = new ModPrefs.Header();
            tempPrefs.header.vanillaAllowed = this.cfgAllowVanillaPlayers.Value;
            tempPrefs.header.moddedAllowed = this.cfgAllowModdedPlayers.Value;
            tempPrefs.header.enforceRequiredMods = this.cfgEnforceRequiredMods.Value;
            tempPrefs.header.enforceBannedMods = this.cfgEnforceBannedMods.Value;
            tempPrefs.header.enforceApprovedMods = this.cfgEnforceApprovedMods.Value;

            tempPrefs.requiredMods = new ModPrefs.PrefList();
            tempPrefs.requiredMods.mods = new List<ModPrefs.PrefList.PrefEntry>();
            foreach( ModEntry entry in this.requiredEntries )
            {
                tempPrefs.requiredMods.mods.Add( entry.prefEntry );
            }

            tempPrefs.bannedMods = new ModPrefs.PrefList();
            tempPrefs.bannedMods.mods = new List<ModPrefs.PrefList.PrefEntry>();
            foreach( ModEntry entry in this.bannedEntries )
            {
                tempPrefs.bannedMods.mods.Add( entry.prefEntry );
            }

            tempPrefs.approvedMods = new ModPrefs.PrefList();
            tempPrefs.approvedMods.mods = new List<ModPrefs.PrefList.PrefEntry>();
            foreach( ModEntry entry in this.approvedEntries )
            {
                tempPrefs.approvedMods.mods.Add( entry.prefEntry );
            }

            prefs = tempPrefs;
        }
    }
}
