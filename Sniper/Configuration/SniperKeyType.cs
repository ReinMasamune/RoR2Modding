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
    internal enum SniperKeyType : UInt32
    {
        Button,
        Axis,
        Scroll,
    }
}
