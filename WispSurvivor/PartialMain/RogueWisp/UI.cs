using BepInEx;
using R2API.Utils;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers;
using RogueWispPlugin.Modules;
using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        private GameObject RW_crosshair;
        partial void RW_UI()
        {
            this.Load += this.RW_CreateCrosshair;
        }

        private void RW_CreateCrosshair()
        {
            GameObject baseUI = Resources.Load<GameObject>("Prefabs/Crosshair/HuntressSnipeCrosshair").InstantiateClone("WispCrosshair", false);
            this.RW_crosshair = baseUI;
        }
    }

}
