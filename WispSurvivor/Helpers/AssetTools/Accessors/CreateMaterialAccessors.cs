
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
                return rend.sharedMaterials[0];
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
                return rend.sharedMaterials[1];
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

            #region BoostJumpEffect
            new GenericAccessor<Material>( MaterialIndex.refMatAngelFeather, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refBoostJumpEffect].transform.Find( "Feather, Directional" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refBoostJumpEffect ).RegisterAccessor();
            #endregion

            #region ProcStealthKit
            new GenericAccessor<Material>( MaterialIndex.refMatStealthkitSparks, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refProcStealthkit].transform.Find( "Sparks" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refProcStealthkit).RegisterAccessor();
            #endregion

            #region RoboBallBossDelayKnockupEffect
            new GenericAccessor<Material>( MaterialIndex.refMatRoboBallParticleRingHuge, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refRoboBallBossDelayKnockupEffect].transform.Find( "Sphere" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refRoboBallBossDelayKnockupEffect ).RegisterAccessor();
            #endregion

            #region FirePillarEffect
            new GenericAccessor<Material>( MaterialIndex.refMatFireStaticLarge, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refFirePillarEffect].transform.Find( "FX/Fire" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refFirePillarEffect ).RegisterAccessor();
            #endregion

            #region LightningStakeNova
            new GenericAccessor<Material>( MaterialIndex.refMatOmniRing2Generic, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refLightningStakeNova].transform.Find( "AreaIndicatorRing, Billboard" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refLightningStakeNova ).RegisterAccessor();
            #endregion

            #region MercSwordFinisherSlash
            new GenericAccessor<Material>( MaterialIndex.refMatMercSwipe2, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refMercSwordFinisherSlash].transform.Find( "SwingTrail" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refMercSwordFinisherSlash ).RegisterAccessor();
            #endregion

            #region MageUnlockPreExplosion
            new GenericAccessor<Material>( MaterialIndex.refMatSuspendedInTime, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refMageUnlockPreExplosion].transform.Find( "Running Particles/Flames, Radial" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refMageUnlockPreExplosion ).RegisterAccessor();
            #endregion

            #region TeleporterBeaconEffect
            new GenericAccessor<Material>( MaterialIndex.refMatTPShockwave, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refTeleporterBeaconEffect].transform.Find( "InitialBurst/Ring" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refTeleporterBeaconEffect ).RegisterAccessor();
            #endregion

            #region SprintActivate
            new GenericAccessor<Material>( MaterialIndex.refMatOpagueDustTrail, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refSprintActivate].transform.Find( "SwingTrail" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refSprintActivate ).RegisterAccessor();
            #endregion

            #region LemurianBiteTrail
            new GenericAccessor<Material>( MaterialIndex.refMatLizardBiteTrail, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refLemurianBiteTrail].transform.Find( "SwingTrail" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refLemurianBiteTrail ).RegisterAccessor();
            #endregion

            #region HippoRezEffect
            new GenericAccessor<Material>( MaterialIndex.refMatAngelEffect, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refHippoRezEffect].transform.Find( "Ring" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refHippoRezEffect ).RegisterAccessor();
            #endregion

            #region ExplosionDroneDeath
            new GenericAccessor<Material>( MaterialIndex.refMatCutExplosion, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refExplosionDroneDeath].transform.Find( "Particles/InitialBurst/Flames" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refExplosionDroneDeath ).RegisterAccessor();
            #endregion

            #region WaterFootstep
            new GenericAccessor<Material>( MaterialIndex.refMatOpagueWaterFoam, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refWaterFootstep].transform.Find( "FoamBilllboard" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refWaterFootstep ).RegisterAccessor();
            #endregion

            #region BootIsReady
            new GenericAccessor<Material>( MaterialIndex.refMatBootWaveEnergy, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refBootIsReady].transform.Find( "BlueRing" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refBootIsReady ).RegisterAccessor();
            #endregion

            #region ExplosionEngiTurretDeath
            new GenericAccessor<Material>( MaterialIndex.refMatEngiTrail, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refExplosionEngiTurretDeath].transform.Find( "InitialBurst/Ring" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refExplosionEngiTurretDeath ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatGenericFire, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refExplosionEngiTurretDeath].transform.Find( "InitialBurst/Flames" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refExplosionEngiTurretDeath ).RegisterAccessor();
            #endregion

            #region TeleportOutBoom
            new GenericAccessor<Material>( MaterialIndex.refMatTeleportOut, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refTeleportOutBoom].transform.Find( "CenterPoof" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refTeleportOutBoom ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatDustSoft, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refTeleportOutBoom].transform.Find( "Dust" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refTeleportOutBoom ).RegisterAccessor();
            #endregion

            #region FruitHealEffect
            new GenericAccessor<Material>( MaterialIndex.refMatHealingCross, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refFruitHealEffect].transform.Find( "Crosses" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refFruitHealEffect ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatJellyfishChunks, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refFruitHealEffect].transform.Find( "Goo Slash" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refFruitHealEffect ).RegisterAccessor();
            #endregion

            #region ImpBossDeathEffect
            new GenericAccessor<Material>( MaterialIndex.refMatImpSwipe, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refImpBossDeathEffect].transform.Find( "DashRings" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refImpBossDeathEffect ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatImpBossPortal, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refImpBossDeathEffect].transform.Find( "Ring" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refImpBossDeathEffect ).RegisterAccessor();
            #endregion

            #region PoisonNovaProc
            new GenericAccessor<Material>( MaterialIndex.refMatHauntedAura, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refPoisonNovaProc].transform.Find( "Particles/Ring, Procced" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refPoisonNovaProc ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatSkullFire, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refPoisonNovaProc].transform.Find( "Particles/Flames" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refPoisonNovaProc ).RegisterAccessor();
            #endregion

            #region SonicBoomEffect
            new GenericAccessor<Material>( MaterialIndex.refMatSonicBoomGroundDust, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refSonicBoomEffect].transform.Find( "DustColliders/Dust" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refSonicBoomEffect ).RegisterAccessor();
            #endregion

            #region MageLightningBombExplosion
            new GenericAccessor<Material>( MaterialIndex.refMatMageMatrixDirectionalLightning, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refMageLightningbombExplosion].transform.Find( "Matrix, Directional" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refMageLightningbombExplosion ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatMageMatrixLightning, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refMageLightningbombExplosion].transform.Find( "Matrix, Billboard" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refMageLightningbombExplosion ).RegisterAccessor();
            #endregion

            #region MageFlamethrowerEffect
            new GenericAccessor<Material>( MaterialIndex.refMatMatrixTriFire, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refMageFlamethrowerEffect].transform.Find( "IcoCharge" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refMageFlamethrowerEffect ).RegisterAccessor();
            #endregion

            #region CleanseEffect
            new GenericAccessor<Material>( MaterialIndex.refMatCleanseCore, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refCleanseEffect].transform.Find( "Ring" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refCleanseEffect ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatCleanseWater, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refCleanseEffect].transform.Find( "Splash" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refCleanseEffect ).RegisterAccessor();
            #endregion

            #region LaserTurbineBombExplosion
            new GenericAccessor<Material>( MaterialIndex.refMatLaserTurbineTargetingLaser, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refLaserTurbineBombExplosion].transform.Find( "Slashes" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refLaserTurbineBombExplosion ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatOmniRadialSlash1Merc, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refLaserTurbineBombExplosion].transform.Find( "BillboardSlashes" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refLaserTurbineBombExplosion ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatOmniHitspark2Merc, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refLaserTurbineBombExplosion].transform.Find( "SharpSlashes" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refLaserTurbineBombExplosion ).RegisterAccessor();
            #endregion

            #region GravekeeperMaskDeath
            new GenericAccessor<Material>( MaterialIndex.refMatOmniHitspark1, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refGravekeeperMaskDeath].transform.Find( "OmniExplosionVFXArchWisp/ScaledHitsparks 1" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refGravekeeperMaskDeath ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatOpagueDust, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refGravekeeperMaskDeath].transform.Find( "OmniExplosionVFXArchWisp/ScaledSmoke, Billboard" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refGravekeeperMaskDeath ).RegisterAccessor();
            #endregion

            #region LevelUpEffect
            new GenericAccessor<Material>( MaterialIndex.refMatOmniRing1Generic, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refLevelUpEffect].transform.Find( "Ring" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refLevelUpEffect ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatOmniHitspark4Merc, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refLevelUpEffect].transform.Find( "BrightFlash, Lines" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refLevelUpEffect ).RegisterAccessor();
            #endregion

            #region ClayBossMulcher
            new GenericAccessor<Material>( MaterialIndex.refMatBloodClayLarge, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refClayBossMulcher].transform.Find( "Goo" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refClayBossMulcher ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatClayGooFizzle, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refClayBossMulcher].transform.Find( "Trail" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refClayBossMulcher ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatClayBubbleBillboard, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refClayBossMulcher].transform.Find( "Bubbles, 2D" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refClayBossMulcher ).RegisterAccessor();
            #endregion

            #region ElectricWormBurrow
            new GenericAccessor<Material>( MaterialIndex.refMatOpagueDustLargeDirectional, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refElectricWormBurrow].transform.Find( "ParticleLoop/Dust, Directional" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refElectricWormBurrow ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatOpagueDustLarge, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refElectricWormBurrow].transform.Find( "ParticleLoop/Dust, Billboard" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refElectricWormBurrow ).RegisterAccessor();
            #endregion

            #region AmmoPackPickupEffect
            new GenericAccessor<Material>( MaterialIndex.refMatGenericLaser, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refAmmoPackPickupEffect].transform.Find( "Ring" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refAmmoPackPickupEffect ).RegisterAccessor();
            #endregion

            #region BubbleShieldEndEffect
            new GenericAccessor<Material>( MaterialIndex.refMatEngiShieldShards, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refBubbleShieldEndEffect].transform.Find( "Quads" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refBubbleShieldEndEffect ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatOmniExplosion1, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refBubbleShieldEndEffect].transform.Find( "OmniExplosionVFXEngiTurretDeath/Unscaled Flames" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refBubbleShieldEndEffect ).RegisterAccessor();

            new GenericAccessor<Material>( MaterialIndex.refMatRoboChunks, () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refBubbleShieldEndEffect].transform.Find( "OmniExplosionVFXEngiTurretDeath/Chunks, Billboards" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, false, PrefabIndex.refBubbleShieldEndEffect ).RegisterAccessor();
            #endregion





            #endregion







        }
    }
}