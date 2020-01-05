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

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void RW_EditModel();
        partial void RW_CreateModelSkins();
        partial void RW_EditModelMesh();
        partial void RW_SetupChildLocator();
        partial void RW_SetupIDRS();
        partial void RW_SetupHurtBoxes();

        partial void RW_Model()
        {
            this.RW_EditModel();
            this.RW_CreateModelSkins();
            this.RW_EditModelMesh();
            this.RW_SetupChildLocator();
            this.RW_SetupIDRS();
            this.RW_SetupHurtBoxes();
        }
    }

}
