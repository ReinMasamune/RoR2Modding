namespace ModSync
{
    using BepInEx;
    using R2API.Utils;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking;

    public partial class Main
    {
        public ModList modList { get; private set; }

        partial void GetMods()
        {
            base.Logger.LogInfo( "Stuff1111" );
            this.FirstFrame += this.GetModList;
        }

        private List<ModInfo> mods = new List<ModInfo>();
        public class ModInfo
        {
            public String guid { get; private set; }
            public System.Version version { get; private set; }
            public ModInfo( PluginInfo plugin )
            {
                this.guid = plugin.Metadata.GUID;
                this.version = plugin.Metadata.Version;
            }
            private ModInfo( String guid, System.Version version )
            {
                this.guid = guid;
                this.version = version;
            }
            public void Write( NetworkWriter writer )
            {
                writer.Write( this.guid );
                writer.Write( this.version.ToString() );
            }
            public static ModInfo Read( NetworkReader reader )
            {
                var guid = reader.ReadString();
                var version = new System.Version(reader.ReadString());
                return new ModInfo( guid, version );
            }
        }

        public class ModList
        {
            public List<ModInfo> mods;
            public ModList( List<ModInfo> mods )
            {
                this.mods = mods;
            }
            public void Write( NetworkWriter writer )
            {
                writer.Write( this.mods.Count );
                foreach( ModInfo mod in this.mods )
                {
                    mod.Write( writer );
                }
            }
            public static ModList Read( NetworkReader reader )
            {
                var count = reader.ReadInt32();
                var mods = new List<ModInfo>(count);
                for( Int32 i = 0; i < count; i++ )
                {
                    mods.Add( ModInfo.Read( reader ) );
                }
                return new ModList( mods );
            }
            public void LogList()
            {
                foreach( ModInfo mod in this.mods )
                {
                    Debug.LogWarning( mod.guid + " " + mod.version );
                }
            }
        }

        private void GetModList()
        {
            base.Logger.LogWarning( "Stuff" );
            foreach( KeyValuePair<String, PluginInfo> kv in BepInEx.Bootstrap.Chainloader.PluginInfos )
            {
                this.mods.Add( new ModInfo( kv.Value ) );
            }
            this.modList = new ModList( this.mods );
            this.modList.LogList();
        }
    }
}
