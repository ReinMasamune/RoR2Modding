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
    using UnityEngine;
    using System.Collections;

    public partial class Main
    {
        private static ModPrefs serverPrefs;


        private static Dictionary<NetworkConnection,ModList> connectingMods = new Dictionary<NetworkConnection, ModList>();
        private static Dictionary<UInt64, ModList> clientModLists = new Dictionary<UInt64, ModList>();
        private void GameNetworkManager_onServerConnectGlobal( UnityEngine.Networking.NetworkConnection connection )
        {
            base.Logger.LogWarning( "onServerConnectGlobal" );

            ModInfoMessage message = new ModInfoMessage
            {
                clientSteamID = 0,
                mods = this.modList
            };

            connection.SendByChannel( serverToClient, message, QosChannelIndex.defaultReliable.intVal );

            serverPrefs = this.GetModPrefs();
            base.StartCoroutine( this.ModCheck( connection, serverPrefs ) );
        }

        private IEnumerator ModCheck( NetworkConnection conn, ModPrefs prefs )
        {
            yield return new WaitForSeconds( messageTimeout );

            if( connectingMods.ContainsKey( conn ) )
            {
                if( !prefs.Check( connectingMods[conn] ) )
                {
                    Debug.LogWarning( "Mod Mismatch Kick" );
                    GameNetworkManager.singleton.ServerKickClient( conn, GameNetworkManager.KickReason.BadVersion );
                }
                connectingMods.Remove( conn );
            } else
            {
                if( !prefs.allowVanilla )
                {
                    base.Logger.LogWarning( "Kicking vanilla player" );
                    GameNetworkManager.singleton.ServerKickClient( conn, GameNetworkManager.KickReason.BadVersion );
                }
            }

        }

        private static ModList serverModList;
        private void GameNetworkManager_onClientConnectGlobal( UnityEngine.Networking.NetworkConnection connection )
        {
            base.Logger.LogWarning( "onClientConnectGlobal" );
            serverModList = null;

            ModInfoMessage message = new ModInfoMessage
            {
                clientSteamID = Client.Instance.SteamId,
                mods = this.modList
            };

            connection.SendByChannel( clientToServer, message, QosChannelIndex.defaultReliable.intVal );
        }

        [NetworkMessageHandler( client = true, server = false, msgType = serverToClient )]
        private static void HandleModListFromServer( NetworkMessage netMsg )
        {
            ModInfoMessage modInfo = netMsg.ReadMessage<ModInfoMessage>();

            if( modInfo == null )
            {
                return;
            }

            modInfo.mods.LogList();

            serverModList = modInfo.mods;
        }

        [NetworkMessageHandler( client = false, server = true, msgType = clientToServer )]
        private static void HandleModListFromClient( NetworkMessage netMsg )
        {
            ModInfoMessage modInfo = netMsg.ReadMessage<ModInfoMessage>();

            if( modInfo == null )
            {
                return;
            }

            modInfo.mods.LogList();

            clientModLists[modInfo.clientSteamID] = modInfo.mods;
            connectingMods[netMsg.conn] = modInfo.mods;
        }


        private class ModInfoMessage : MessageBase
        {
            public ModList mods;
            public UInt64 clientSteamID;

            public override void Serialize( NetworkWriter writer )
            {
                writer.Write( this.clientSteamID );
                this.mods.Write( writer );
            }

            public override void Deserialize( NetworkReader reader )
            {
                this.clientSteamID = reader.ReadUInt64();
                this.mods = ModList.Read( reader );
            }
        }
    }
}
