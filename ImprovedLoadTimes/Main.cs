using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using R2API;
using R2API.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using EntityStates;

namespace ImprovedLoadTimes
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ImprovedLoadTime", "ImprovedLoadTime", "1.0.0")]
    public class Main : BaseUnityPlugin
    {

    }
}
