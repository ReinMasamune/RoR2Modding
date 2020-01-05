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
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void RW_UtilityIndicatorEffects()
        {
            this.Load += this.RW_CreateUtilityIndicatorEffects;
        }

        private void RW_CreateUtilityIndicatorEffects()
        {
            GameObject baseFX = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            for( Int32 i = 0; i < 8; i++ )
            {
                utilityIndicator[i] = CreateUtilityIndicator( baseFX, i );
            }
        }

        private static GameObject CreateUtilityIndicator( GameObject baseFX, Int32 skinIndex )
        {
            GameObject obj = baseFX.InstantiateClone("WispUtilityIndicator", false);

            MeshFilter mesh = obj.GetComponent<MeshFilter>();
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();

            return obj;
        }
    }

}
