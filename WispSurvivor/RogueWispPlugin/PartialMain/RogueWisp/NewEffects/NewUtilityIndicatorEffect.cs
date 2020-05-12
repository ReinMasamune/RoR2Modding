#if ROGUEWISP
using Rein.RogueWispPlugin.Helpers;

using ReinCore;

using RoR2;

using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        partial void RW_NewUtilityIndicatorEffect()
        {
            this.Load += this.RW_CreateNewUtilityIndicator;
        }

        private void RW_CreateNewUtilityIndicator()
        {
            var obj = new GameObject().ClonePrefab("UtilityAim", false );

            obj.layer = LayerIndex.entityPrecise.intVal;

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
            EffectHelper.AddMeshIndicator( obj, skin, MaterialType.AreaIndicator2, MeshIndex.Sphere, false );

            //var tornado = EffectHelper.AddFlameTornado( obj, skin, MaterialType.FlameTornado, 1f, 10f, 7.5f, 7.5f );
            //var tornadoMain = tornado.main;
            //tornadoMain.startSizeX = 7.5f;
            //tornadoMain.startSizeY = 7.5f;
            //var tornadoSOL = tornado.sizeOverLifetime;
            //tornadoSOL.enabled = true;
            //tornadoSOL.separateAxes = true;
            //tornadoSOL.x = new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.Linear( 0f, 1f, 1f, 0f ) );
            //tornadoSOL.y = new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.Linear( 0f, 1f, 1f, 0f ) );
            //tornadoSOL.z = new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.EaseInOut( 0f, 0.9f, 1f, 1f ) );

            var fire = EffectHelper.AddFire( obj, skin, MaterialType.Flames, 3f, 1.5f, 30f, 0f, -1f, true );
            var fireMain = fire.main;
            fireMain.scalingMode = ParticleSystemScalingMode.Local;

            //var col = obj.AddComponent<SphereCollider>();
            //col.isTrigger = true;
            //col.center = Vector3.zero;
            //col.radius = 0.5f;
            //col.material = null;

            //var slow = obj.AddComponent<SlowDownProjectiles>();
            //slow.maxVelocityMagnitude = 20f;
            //slow.antiGravity = 0f;


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