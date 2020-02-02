
using RogueWispPlugin.Helpers;
using System;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void CreateMaterialAccessors()
        {
            #region Reference Materials
            #region NullifierDeathExplosion
            new GenericAccessor<Material>( MaterialIndex.refMatTracerBright,                        () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refNullifierDeathExplosion].transform;
                var rend = trans.Find( "Vacuum Stars, Trails" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[1];
            }, false, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatDebris1,                             () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refNullifierDeathExplosion].transform;
                var rend = trans.Find( "Dirt" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatInverseDistortion,                   () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refNullifierDeathExplosion].transform;
                var rend = trans.Find( "Vacuum Stars, Distortion" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatOpagueDustSpeckledLarge,             () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refNullifierDeathExplosion].transform;
                var rend = trans.Find( "Goo, Medium" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatNullifierStarParticle,               () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refNullifierDeathExplosion].transform;
                var rend = trans.Find( "Vacuum Stars" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatNullifierStarTrail,                  () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refNullifierDeathExplosion].transform;
                var rend = trans.Find( "Vacuum Stars, Trails" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatNullifierStarPortalEdge,             () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refNullifierDeathExplosion].transform;
                var rend = trans.Find( "Vacuum Radial" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatNullifierExplosionAreaIndicatorSoft, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refNullifierDeathExplosion].transform;
                var rend = trans.Find( "AreaIndicator" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatNullifierExplosionAreaIndicatorHard, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refNullifierDeathExplosion].transform;
                var rend = trans.Find( "AreaIndicator (1)" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatNullifierGemPortal,                  () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refNullifierDeathExplosion].transform;
                var rend = trans.Find( "Sphere" ).GetComponent<MeshRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();
            #endregion

            #region Nullifier Pre-Bomb Ghost
            new GenericAccessor<Material>( MaterialIndex.refMatNullBombAreaIndicator,               () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refNullifierDeathExplosion].transform;
                ParticleSystemRenderer rend = null;
                var pastFirst = false;
                for( Int32 i = 0; i < trans.childCount; ++i )
                {
                    var child = trans.GetChild( i );
                    if( child.name == "AreaIndicator" )
                    {
                        if( pastFirst )
                        {
                            rend = child.GetComponent<ParticleSystemRenderer>();
                            break;
                        } else pastFirst = true;
                    }
                }
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refNullifierPreBombGhost ).RegisterAccessor();
            #endregion

            #region Will-O-Wisp Explosion
            new GenericAccessor<Material>( MaterialIndex.refMatTracer,                              () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refWillOWispExplosion].transform;
                var rend = trans.Find( "Sparks" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refWillOWispExplosion ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatDistortionFaded,                     () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refWillOWispExplosion].transform;
                var rend = trans.Find( "Distortion" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refWillOWispExplosion ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatWillowispSpiral,                     () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refWillOWispExplosion].transform;
                var rend = trans.Find( "Flames, Tube" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refWillOWispExplosion ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatWillowispRadial,                     () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refWillOWispExplosion].transform;
                var rend = trans.Find( "Flames, Radial" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refWillOWispExplosion ).RegisterAccessor();
            #endregion
            #endregion

















        }
    }
}