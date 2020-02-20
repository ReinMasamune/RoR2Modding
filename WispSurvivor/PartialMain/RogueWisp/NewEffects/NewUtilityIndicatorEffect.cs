#if ROGUEWISP
using R2API;
using RogueWispPlugin.Helpers;
using RoR2;
using System;
using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{

    internal partial class Main
    {
        partial void RW_NewUtilityIndicatorEffect()
        {
            this.Load += this.RW_CreateNewUtilityIndicator;
        }

        private void RW_CreateNewUtilityIndicator()
        {
            var obj = new GameObject().InstantiateClone("UtilityAim", false );

            var effComp = obj.AddComponent<EffectComponent>();
            effComp.positionAtReferencedTransform = false;
            effComp.parentToReferencedTransform = false;
            effComp.applyScale = true;
            effComp.soundName = "";

            var skin = obj.AddComponent<WispSkinnedEffect>();

            var vfxAtrib = obj.AddComponent<VFXAttributes>();
            vfxAtrib.vfxPriority = VFXAttributes.VFXPriority.Always;
            vfxAtrib.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            var destroyOnEnd = obj.AddComponent<DestroyOnEffectTimer>();
            destroyOnEnd.effectComp = effComp;

            EffectHelper.AddMeshIndicator( obj, skin, MaterialType.AreaIndicator, MeshIndex.Sphere, false );


            utilityIndicator = obj;
            RegisterEffect( utilityIndicator );
        }
        /*
partial void RW_UtilityIndicatorEffects() => this.Load += this.RW_CreateUtilityIndicatorEffects;

private void RW_CreateUtilityIndicatorEffects()
{
GameObject baseFX = GameObject.CreatePrimitive(PrimitiveType.Sphere);

for( Int32 i = 0; i < 8; i++ )
{
utilityIndicators[i] = CreateUtilityIndicator( baseFX, i );
}
}

private static GameObject CreateUtilityIndicator( GameObject baseFX, Int32 skinIndex )
{
GameObject obj = baseFX.InstantiateClone("WispUtilityIndicator", false);

MeshFilter mesh = obj.GetComponent<MeshFilter>();
MeshRenderer renderer = obj.GetComponent<MeshRenderer>();

return obj;
}
*/
    }

}
#endif