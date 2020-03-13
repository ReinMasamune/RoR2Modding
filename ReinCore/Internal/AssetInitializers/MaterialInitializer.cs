using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReinCore
{
    internal static class MaterialInitializer
    {
        private static Boolean completedProperly = false;
        internal static Boolean Initialize()
        {
            return completedProperly;
        }


        static MaterialInitializer()
        {
            #region Reference Materials
            #region Resources
            new AssetAccessor<Material>( MaterialIndex.refMatOnHelfire, () =>
            {
                return Resources.Load<Material>( "Materials/MatOnHelfire" );
            } ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatShatteredGlass, () =>
            {
                return Resources.Load<Material>( "Materials/MatShatteredGlass" );
            } ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatTPInOut, () =>
            {
                return Resources.Load<Material>( "Materials/MatTPInOut" );
            } ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatVagrantEnergized, () =>
            {
                return Resources.Load<Material>( "Materials/MatVagrantEnergized" );
            } ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatElitePoisonParticleSystemReplacement, () =>
            {
                return Resources.Load<Material>( "Materials/MatElitePoisonParticleReplacement" );
            } ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatMercEnergized, () =>
            {
                return Resources.Load<Material>( "Materials/MatMercEnergized" );
            } ).RegisterAccessor();
            #endregion

            #region NullifierDeathExplosion
            new AssetAccessor<Material>( MaterialIndex.refMatTracerBright, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                var rend = trans.Find( "Vacuum Stars, Trails" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatDebris1, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                var rend = trans.Find( "Dirt" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatInverseDistortion, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                var rend = trans.Find( "Vacuum Stars, Distortion" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatOpagueDustSpeckledLarge, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                var rend = trans.Find( "Goo, Medium" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatNullifierStarParticle, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                var rend = trans.Find( "Vacuum Stars" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatNullifierStarTrail, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                var rend = trans.Find( "Vacuum Stars, Trails" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[1];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatNullifierStarPortalEdge, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                var rend = trans.Find( "Vacuum Radial" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatNullifierExplosionAreaIndicatorSoft, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                var rend = trans.Find( "AreaIndicator" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatNullifierExplosionAreaIndicatorHard, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                var rend = trans.Find( "AreaIndicator (1)" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatNullifierGemPortal, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                var rend = trans.Find( "Sphere" ).GetComponent<MeshRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();
            #endregion

            #region Nullifier Pre-Bomb Ghost
            new AssetAccessor<Material>( MaterialIndex.refMatNullBombAreaIndicator, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierPreBombGhost).transform;
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
            }, PrefabIndex.refNullifierPreBombGhost ).RegisterAccessor();
            #endregion

            #region Will-O-Wisp Explosion
            new AssetAccessor<Material>( MaterialIndex.refMatTracer, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refWillOWispExplosion).transform;
                var rend = trans.Find( "Sparks" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refWillOWispExplosion ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatDistortionFaded, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refWillOWispExplosion).transform;
                var rend = trans.Find( "Distortion" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refWillOWispExplosion ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatWillowispSpiral, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refWillOWispExplosion).transform;
                var rend = trans.Find( "Flames, Tube" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refWillOWispExplosion ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatWillowispRadial, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refWillOWispExplosion).transform;
                var rend = trans.Find( "Flames, Radial" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refWillOWispExplosion ).RegisterAccessor();
            #endregion

            #region Locked Mage
            new AssetAccessor<Material>( MaterialIndex.refMatBazaarIceCore, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLockedMage).transform.Find( "ModelBase/IceMesh" );
                var rend = trans.GetComponent<MeshRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLockedMage ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatBazaarIceDistortion, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLockedMage).transform.Find( "ModelBase/IceMesh" );
                var rend = trans.GetComponent<MeshRenderer>();
                return rend.sharedMaterials[1];
            }, PrefabIndex.refLockedMage ).RegisterAccessor();
            #endregion

            #region Fire Tornado Ghost
            new AssetAccessor<Material>( MaterialIndex.refMatGenericFlash, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFireTornadoGhost).transform.Find( "Flash, Red" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refFireTornadoGhost ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatDustDirectionalDark, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFireTornadoGhost).transform.Find( "Smoke" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refFireTornadoGhost ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatFireRingRunes, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFireTornadoGhost).transform.Find( "InitialBurst/RuneRings" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refFireTornadoGhost ).RegisterAccessor();

            #endregion

            #region Titan Recharge Rocks Effect
            new AssetAccessor<Material>( MaterialIndex.refMatGolemExplosion, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refTitanRechargeRocksEffect).transform.Find( "3DDebris" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[1];
            }, PrefabIndex.refTitanRechargeRocksEffect ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatTitanBeam, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refTitanRechargeRocksEffect).transform.Find( "Sparks, Trail" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refTitanRechargeRocksEffect ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatArcaneCircle1, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refTitanRechargeRocksEffect).transform.Find( "Glow" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refTitanRechargeRocksEffect ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatDistortion, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refTitanRechargeRocksEffect).transform.Find( "Distortion" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refTitanRechargeRocksEffect ).RegisterAccessor();
            #endregion

            #region BoostJumpEffect
            new AssetAccessor<Material>( MaterialIndex.refMatAngelFeather, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refBoostJumpEffect).transform.Find( "Feather, Directional" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refBoostJumpEffect ).RegisterAccessor();
            #endregion

            #region ProcStealthKit
            new AssetAccessor<Material>( MaterialIndex.refMatStealthkitSparks, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refProcStealthkit).transform.Find( "Sparks" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refProcStealthkit ).RegisterAccessor();
            #endregion

            #region RoboBallBossDelayKnockupEffect
            new AssetAccessor<Material>( MaterialIndex.refMatRoboBallParticleRingHuge, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refRoboBallBossDelayKnockupEffect).transform.Find( "Sphere" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refRoboBallBossDelayKnockupEffect ).RegisterAccessor();
            #endregion

            #region FirePillarEffect
            new AssetAccessor<Material>( MaterialIndex.refMatFireStaticLarge, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFirePillarEffect).transform.Find( "FX/Fire" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refFirePillarEffect ).RegisterAccessor();
            #endregion

            #region LightningStakeNova
            new AssetAccessor<Material>( MaterialIndex.refMatOmniRing2Generic, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLightningStakeNova).transform.Find( "AreaIndicatorRing, Billboard" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLightningStakeNova ).RegisterAccessor();
            #endregion

            #region MercSwordFinisherSlash
            new AssetAccessor<Material>( MaterialIndex.refMatMercSwipe2, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refMercSwordFinisherSlash).transform.Find( "SwingTrail" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refMercSwordFinisherSlash ).RegisterAccessor();
            #endregion

            #region MageUnlockPreExplosion
            new AssetAccessor<Material>( MaterialIndex.refMatSuspendedInTime, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refMageUnlockPreExplosion).transform.Find( "Running Particles/Flames, Radial" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refMageUnlockPreExplosion ).RegisterAccessor();
            #endregion

            #region TeleporterBeaconEffect
            new AssetAccessor<Material>( MaterialIndex.refMatTPShockwave, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refTeleporterBeaconEffect).transform.Find( "InitialBurst/Ring" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refTeleporterBeaconEffect ).RegisterAccessor();
            #endregion

            #region SprintActivate
            new AssetAccessor<Material>( MaterialIndex.refMatOpagueDustTrail, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refSprintActivate).transform.Find( "SwingTrail" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refSprintActivate ).RegisterAccessor();
            #endregion

            #region LemurianBiteTrail
            new AssetAccessor<Material>( MaterialIndex.refMatLizardBiteTrail, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLemurianBiteTrail).transform.Find( "SwingTrail" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLemurianBiteTrail ).RegisterAccessor();
            #endregion

            #region HippoRezEffect
            new AssetAccessor<Material>( MaterialIndex.refMatAngelEffect, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refHippoRezEffect).transform.Find( "Ring" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refHippoRezEffect ).RegisterAccessor();
            #endregion

            #region ExplosionDroneDeath
            new AssetAccessor<Material>( MaterialIndex.refMatCutExplosion, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refExplosionDroneDeath).transform.Find( "Particles/InitialBurst/Flames" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refExplosionDroneDeath ).RegisterAccessor();
            #endregion

            #region WaterFootstep
            new AssetAccessor<Material>( MaterialIndex.refMatOpagueWaterFoam, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refWaterFootstep).transform.Find( "FoamBilllboard" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refWaterFootstep ).RegisterAccessor();
            #endregion

            #region BootIsReady
            new AssetAccessor<Material>( MaterialIndex.refMatBootWaveEnergy, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refBootIsReady).transform.Find( "BlueRing" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refBootIsReady ).RegisterAccessor();
            #endregion

            #region ExplosionEngiTurretDeath
            new AssetAccessor<Material>( MaterialIndex.refMatEngiTrail, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refExplosionEngiTurretDeath).transform.Find( "InitialBurst/Ring" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refExplosionEngiTurretDeath ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatGenericFire, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refExplosionEngiTurretDeath).transform.Find( "InitialBurst/Flames" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refExplosionEngiTurretDeath ).RegisterAccessor();
            #endregion

            #region TeleportOutBoom
            new AssetAccessor<Material>( MaterialIndex.refMatTeleportOut, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refTeleportOutBoom).transform.Find( "CenterPoof" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refTeleportOutBoom ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatDustSoft, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refTeleportOutBoom).transform.Find( "Dust" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refTeleportOutBoom ).RegisterAccessor();
            #endregion

            #region FruitHealEffect
            new AssetAccessor<Material>( MaterialIndex.refMatHealingCross, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFruitHealEffect).transform.Find( "Crosses" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refFruitHealEffect ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatJellyfishChunks, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFruitHealEffect).transform.Find( "Goo Slash" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refFruitHealEffect ).RegisterAccessor();
            #endregion

            #region ImpBossDeathEffect
            new AssetAccessor<Material>( MaterialIndex.refMatImpSwipe, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refImpBossDeathEffect).transform.Find( "DashRings" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refImpBossDeathEffect ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatImpBossPortal, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refImpBossDeathEffect).transform.Find( "Ring" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refImpBossDeathEffect ).RegisterAccessor();
            #endregion

            #region PoisonNovaProc
            new AssetAccessor<Material>( MaterialIndex.refMatHauntedAura, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refPoisonNovaProc).transform.Find( "Particles/Ring, Procced" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refPoisonNovaProc ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatSkullFire, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refPoisonNovaProc).transform.Find( "Particles/Flames" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refPoisonNovaProc ).RegisterAccessor();
            #endregion

            #region SonicBoomEffect
            new AssetAccessor<Material>( MaterialIndex.refMatSonicBoomGroundDust, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refSonicBoomEffect).transform.Find( "DustColliders/Dust" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refSonicBoomEffect ).RegisterAccessor();
            #endregion

            #region MageLightningBombExplosion
            new AssetAccessor<Material>( MaterialIndex.refMatMageMatrixDirectionalLightning, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refMageLightningbombExplosion).transform.Find( "Matrix, Directional" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refMageLightningbombExplosion ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatMageMatrixLightning, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refMageLightningbombExplosion).transform.Find( "Matrix, Billboard" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refMageLightningbombExplosion ).RegisterAccessor();
            #endregion

            #region MageFlamethrowerEffect
            new AssetAccessor<Material>( MaterialIndex.refMatMatrixTriFire, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refMageFlamethrowerEffect).transform.Find( "IcoCharge" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refMageFlamethrowerEffect ).RegisterAccessor();
            #endregion

            #region CleanseEffect
            new AssetAccessor<Material>( MaterialIndex.refMatCleanseCore, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refCleanseEffect).transform.Find( "Ring" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refCleanseEffect ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatCleanseWater, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refCleanseEffect).transform.Find( "Splash" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refCleanseEffect ).RegisterAccessor();
            #endregion

            #region LaserTurbineBombExplosion
            new AssetAccessor<Material>( MaterialIndex.refMatLaserTurbineTargetingLaser, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLaserTurbineBombExplosion).transform.Find( "Slashes" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLaserTurbineBombExplosion ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatOmniRadialSlash1Merc, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLaserTurbineBombExplosion).transform.Find( "BillboardSlashes" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLaserTurbineBombExplosion ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatOmniHitspark2Merc, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLaserTurbineBombExplosion).transform.Find( "SharpSlashes" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLaserTurbineBombExplosion ).RegisterAccessor();
            #endregion

            #region GravekeeperMaskDeath
            new AssetAccessor<Material>( MaterialIndex.refMatOmniHitspark1, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refGravekeeperMaskDeath).transform.Find( "OmniExplosionVFXArchWisp/ScaledHitsparks 1" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refGravekeeperMaskDeath ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatOpagueDust, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refGravekeeperMaskDeath).transform.Find( "OmniExplosionVFXArchWisp/ScaledSmoke, Billboard" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refGravekeeperMaskDeath ).RegisterAccessor();
            #endregion

            #region LevelUpEffect
            new AssetAccessor<Material>( MaterialIndex.refMatOmniRing1Generic, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLevelUpEffect).transform.Find( "Ring" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLevelUpEffect ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatOmniHitspark4Merc, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLevelUpEffect).transform.Find( "BrightFlash, Lines" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLevelUpEffect ).RegisterAccessor();
            #endregion

            #region ClayBossMulcher
            new AssetAccessor<Material>( MaterialIndex.refMatBloodClayLarge, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refClayBossMulcher).transform.Find( "Goo" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refClayBossMulcher ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatClayGooFizzle, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refClayBossMulcher).transform.Find( "Trail" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refClayBossMulcher ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatClayBubbleBillboard, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refClayBossMulcher).transform.Find( "Bubbles, 2D" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refClayBossMulcher ).RegisterAccessor();
            #endregion

            #region ElectricWormBurrow
            new AssetAccessor<Material>( MaterialIndex.refMatOpagueDustLargeDirectional, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refElectricWormBurrow).transform.Find( "ParticleLoop/Dust, Directional" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refElectricWormBurrow ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatOpagueDustLarge, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refElectricWormBurrow).transform.Find( "ParticleLoop/Dust, Billboard" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refElectricWormBurrow ).RegisterAccessor();
            #endregion

            #region AmmoPackPickupEffect
            new AssetAccessor<Material>( MaterialIndex.refMatGenericLaser, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refAmmoPackPickupEffect).transform.Find( "Ring" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refAmmoPackPickupEffect ).RegisterAccessor();
            #endregion

            #region BubbleShieldEndEffect
            new AssetAccessor<Material>( MaterialIndex.refMatEngiShieldShards, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refBubbleShieldEndEffect).transform.Find( "Quads" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refBubbleShieldEndEffect ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatOmniExplosion1, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refBubbleShieldEndEffect).transform.Find( "OmniExplosionVFXEngiTurretDeath/Unscaled Flames" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refBubbleShieldEndEffect ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatRoboChunks, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refBubbleShieldEndEffect).transform.Find( "OmniExplosionVFXEngiTurretDeath/Chunks, Billboards" );
                var rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refBubbleShieldEndEffect ).RegisterAccessor();
            #endregion





            #endregion









            completedProperly = true;
        }
    }
}
