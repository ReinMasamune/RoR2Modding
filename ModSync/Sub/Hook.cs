namespace ModSync
{
    using BepInEx;
    using Facepunch.Steamworks;
    using R2API.Utils;
    using RoR2.Networking;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking;

    public partial class Main
    {
        partial void Hook()
        {
            this.Enable += this.AddHooks;
            this.Disable += this.RemoveHooks;
        }
        private void RemoveHooks()
        {
            GameNetworkManager.onClientConnectGlobal -= this.GameNetworkManager_onClientConnectGlobal; // In Networking
            GameNetworkManager.onServerConnectGlobal -= this.GameNetworkManager_onServerConnectGlobal; // In Networking
        }
        private void AddHooks()
        {
            GameNetworkManager.onClientConnectGlobal += this.GameNetworkManager_onClientConnectGlobal; // In Networking
            GameNetworkManager.onServerConnectGlobal += this.GameNetworkManager_onServerConnectGlobal; // In Networking
        }
    }
}
