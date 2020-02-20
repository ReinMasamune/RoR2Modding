using R2API;
using System;
using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        partial void RW_UtilityIndicatorEffects() => this.Load += this.RW_CreateUtilityIndicatorEffects;

        private void RW_CreateUtilityIndicatorEffects()
        {
            GameObject baseFX = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            for( Int32 i = 0; i < 8; i++ )
            {
                //utilityIndicators[i] = CreateUtilityIndicator( baseFX, i );
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
#endif
}
