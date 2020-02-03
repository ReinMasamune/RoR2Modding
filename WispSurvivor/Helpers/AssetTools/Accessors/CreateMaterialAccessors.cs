
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
            #region Resources
            new GenericAccessor<Material>( MaterialIndex.refMatOnHelfire,                           () =>
            {
                return Resources.Load<Material>( "Materials/MatOnHelfire" );
            }, false, ExecutionState.Awake ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatShatteredGlass,                      () =>
            {
                return Resources.Load<Material>( "Materials/MatShatteredGlass" );
            }, false, ExecutionState.Awake ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatTPInOut,                             () =>
            {
                return Resources.Load<Material>( "Materials/MatTPInOut" );
            }, false, ExecutionState.Awake ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatVagrantEnergized,                    () =>
            {
                return Resources.Load<Material>( "Materials/MatVagrantEnergized" );
            }, false, ExecutionState.Awake ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatElitePoisonParticleSystemReplacement,() =>
            {
                return Resources.Load<Material>( "Materials/MatElitePoisonParticleReplacement" );
            }, false, ExecutionState.Awake ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatMercEnergized,                       () =>
            {
                return Resources.Load<Material>( "Materials/MatMercEnergized" );
            }, false, ExecutionState.Awake ).RegisterAccessor();
            #endregion

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
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refNullifierPreBombGhost].transform;
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

            #region Locked Mage
            new GenericAccessor<Material>( MaterialIndex.refMatBazaarIceCore,                       () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refLockedMage].transform.Find( "ModelBase/IceMesh" );
                var rend = trans.GetComponent<MeshRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refLockedMage ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatBazaarIceDistortion,                 () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refLockedMage].transform.Find( "ModelBase/IceMesh" );
                var rend = trans.GetComponent<MeshRenderer>();
                return rend.sharedMaterials[1];
            }, false, PrefabIndex.refLockedMage ).RegisterAccessor();
            #endregion

            #region Fire Tornado Ghost
            new GenericAccessor<Material>( MaterialIndex.refMatGenericFlash, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refFireTornadoGhost].transform.Find( "Flash, Red" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refFireTornadoGhost ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatDustDirectionalDark, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refFireTornadoGhost].transform.Find( "Smoke" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refFireTornadoGhost ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatFireRingRunes, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refFireTornadoGhost].transform.Find( "InitialBurst/RuneRings" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refFireTornadoGhost ).RegisterAccessor();

            #endregion

            #region Titan Recharge Rocks Effect
            new GenericAccessor<Material>( MaterialIndex.refMatGolemExplosion,                         () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refTitanRechargeRocksEffect].transform.Find( "3DDebris" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[1];
            }, false, PrefabIndex.refTitanRechargeRocksEffect ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatTitanBeam,                              () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refTitanRechargeRocksEffect].transform.Find( "Sparks, Trail" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refTitanRechargeRocksEffect ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatArcaneCircle1,                          () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refTitanRechargeRocksEffect].transform.Find( "Glow" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refTitanRechargeRocksEffect ).RegisterAccessor();
            new GenericAccessor<Material>( MaterialIndex.refMatDistortion,                             () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refTitanRechargeRocksEffect].transform.Find( "Distortion" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refTitanRechargeRocksEffect ).RegisterAccessor();
            #endregion
            #endregion







        }
    }
}