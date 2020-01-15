namespace ModSync
{
    using BepInEx;
    using R2API.Utils;
    using System;
    using UnityEngine;
    using UnityEngine.Networking;
    using R2API;
    using RoR2;
    using RoR2.Networking;
    using System.Collections.Generic;

    //[R2APISubmoduleDependency()]
    [BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.ReinThings.ModSync", "Rein-ModSync", "1.0.0" )]
    public partial class Main : BaseUnityPlugin
    {
        private void Awake()
        {
            this.BuildConfig();
            ModListAPI.modlistRecievedFromClient += this.ModListAPI_modlistRecievedFromClient;
            ModListAPI.modlistRecievedFromServer += this.ModListAPI_modlistRecievedFromServer;
        }

        private void ModListAPI_modlistRecievedFromServer( NetworkConnection connection, ModListAPI.ModList list )
        {
            base.Logger.LogWarning( "Modlist recieved from server" );
            foreach( ModListAPI.ModInfo mod in list.mods )
            {
                base.Logger.LogWarning( mod.guid + " : " + mod.version );
            }
        }

        private void ModListAPI_modlistRecievedFromClient( NetworkConnection connection, ModListAPI.ModList list, CSteamID steamID )
        {
            base.Logger.LogWarning( "Modlist recieved from client with steamID: " + steamID.value );

            foreach( ModListAPI.ModInfo mod in list.mods )
            {
                base.Logger.LogWarning( mod.guid + " : " + mod.version );
            }

            if( !this.CheckList(list, this.GetModPrefs() ) )
            {
                GameNetworkManager.singleton.ServerKickClient( connection, GameNetworkManager.KickReason.BadVersion );
            }
        }
    }
}
