using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;
using Sniper.SkillDefs;
using UnityEngine.Networking;

namespace Sniper.Components
{
    internal interface IRuntimePrefabComponent
    {
        void InitializePrefab();
    }
}
