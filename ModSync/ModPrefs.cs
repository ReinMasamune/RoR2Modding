namespace ModSync
{
    using R2API;
    using System;
    using System.Collections.Generic;

    public partial class Main
    {
        internal class ModPrefs
        {
            public Header header;
            public PrefList requiredMods;
            public PrefList bannedMods;
            public PrefList approvedMods;

            public class Header
            {
                public Boolean vanillaAllowed;
                public Boolean moddedAllowed;
                public Boolean enforceRequiredMods;
                public Boolean enforceBannedMods;
                public Boolean enforceApprovedMods;
            }

            internal class PrefList
            {
                public List<PrefEntry> mods = new List<PrefEntry>();
                internal class PrefEntry
                {
                    public String guid { get; private set; }
                    public Boolean enforceConfig { get; private set; }
                    public Version minVersion { get; private set; }
                    public Version maxVersion { get; private set; }

                    public Boolean useMinVersion
                    {
                        get
                        {
                            return this.minVersion != null;
                        }
                    }

                    public Boolean useMaxVersion
                    {
                        get
                        {
                            return this.maxVersion != null;
                        }
                    }

                    public Boolean Check( ModListAPI.ModInfo mod )
                    {
                        if( mod == null ) return false;
                        if( this.guid != mod.guid.ToLower() ) return false;
                        if( this.useMinVersion && mod.version < this.minVersion ) return false;
                        if( this.useMaxVersion && mod.version > this.maxVersion ) return false;
                        return true;
                    }

                    internal PrefEntry( String guid, String enforceConfig, String minVersion, String maxVersion )
                    {
                        this.guid = guid;
                        this.enforceConfig = (enforceConfig == "true");

                        Version minVer = null;
                        try
                        {
                            minVer = new Version( minVersion );
                        } catch { }
                        this.minVersion = minVer;

                        Version maxVer = null;
                        try
                        {
                            maxVer = new Version( maxVersion );
                        } catch { }
                        this.maxVersion = maxVer;
                    }
                }
            }
        }
    }
}
