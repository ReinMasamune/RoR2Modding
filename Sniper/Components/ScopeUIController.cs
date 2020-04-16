using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace Sniper.Components
{
    internal class ScopeUIController : NetworkBehaviour
    {
        // TODO: Add all functionality

        internal static ScopeUIController Create( GameObject prefab, CharacterBody body )
        {
            // TODO: Implement
            return null;
        }

        internal Single zoom { private get; set; }

        internal void SetActivity( Boolean active )
        {
            // TODO: Implement
        }
    }
}
