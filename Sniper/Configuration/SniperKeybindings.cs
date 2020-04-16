using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace Sniper.Configuration
{
    internal static class SniperKeybindings
    {
        private static Dictionary<SniperAction,SniperKey> actionMap = new Dictionary<SniperAction, SniperKey>();
        private static Dictionary<SniperKey,SniperAction> keyMap = new Dictionary<SniperKey, SniperAction>();
        private static Dictionary<SniperKey,SniperKeyType> keyTypeMap = new Dictionary<SniperKey, SniperKeyType>();
    }
}
