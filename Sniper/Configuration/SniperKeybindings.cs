namespace Sniper.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using BepInEx.Logging;
    using ReinCore;
    using RoR2;
    using UnityEngine;
    using UnityEngine.Networking;

    internal static class SniperKeybindings
    {
        private static readonly Dictionary<SniperAction,SniperKey> actionMap = new Dictionary<SniperAction, SniperKey>();
        private static readonly Dictionary<SniperKey,SniperAction> keyMap = new Dictionary<SniperKey, SniperAction>();
        private static readonly Dictionary<SniperKey,SniperKeyType> keyTypeMap = new Dictionary<SniperKey, SniperKeyType>();
    }
}
