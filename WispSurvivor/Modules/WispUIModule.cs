using System;
using System.Collections.Generic;
using UnityEngine;
using WispSurvivor.Helpers;
using static WispSurvivor.Helpers.APIInterface;

namespace WispSurvivor.Modules
{
    public static class WispUIModule
    {
        public static GameObject crosshair;

        public static void DoModule( GameObject body, Dictionary<Type, Component> dic ) => CreateCrosshair();

        public static void CreateCrosshair()
        {
            GameObject baseUI = Resources.Load<GameObject>("Prefabs/Crosshair/HuntressSnipeCrosshair").InstantiateClone("WispCrosshair", false);
            crosshair = baseUI;
        }
    }
}
