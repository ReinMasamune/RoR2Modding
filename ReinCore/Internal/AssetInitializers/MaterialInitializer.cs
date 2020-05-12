namespace ReinCore
{
    using System;

    using UnityEngine;

    internal static class MaterialInitializer
    {
        private static readonly Boolean completedProperly = false;
        internal static Boolean Initialize() => completedProperly;


        static MaterialInitializer()
        {
            #region Reference Materials
            #region Resources
            new AssetAccessor<Material>( MaterialIndex.refMatOnHelfire, () => Resources.Load<Material>( "Materials/MatOnHelfire" ) ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatShatteredGlass, () => Resources.Load<Material>( "Materials/MatShatteredGlass" ) ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatTPInOut, () => Resources.Load<Material>( "Materials/MatTPInOut" ) ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatVagrantEnergized, () => Resources.Load<Material>( "Materials/MatVagrantEnergized" ) ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatElitePoisonParticleSystemReplacement, () => Resources.Load<Material>( "Materials/MatElitePoisonParticleReplacement" ) ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatMercEnergized, () => Resources.Load<Material>( "Materials/MatMercEnergized" ) ).RegisterAccessor();
            #endregion

            #region NullifierDeathExplosion
            new AssetAccessor<Material>( MaterialIndex.refMatTracerBright, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                ParticleSystemRenderer rend = trans.Find( "Vacuum Stars, Trails" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatDebris1, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                ParticleSystemRenderer rend = trans.Find( "Dirt" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatInverseDistortion, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                ParticleSystemRenderer rend = trans.Find( "Vacuum Stars, Distortion" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatOpagueDustSpeckledLarge, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                ParticleSystemRenderer rend = trans.Find( "Goo, Medium" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatNullifierStarParticle, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                ParticleSystemRenderer rend = trans.Find( "Vacuum Stars" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatNullifierStarTrail, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                ParticleSystemRenderer rend = trans.Find( "Vacuum Stars, Trails" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[1];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatNullifierStarPortalEdge, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                ParticleSystemRenderer rend = trans.Find( "Vacuum Radial" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatNullifierExplosionAreaIndicatorSoft, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                ParticleSystemRenderer rend = trans.Find( "AreaIndicator" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatNullifierExplosionAreaIndicatorHard, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                ParticleSystemRenderer rend = trans.Find( "AreaIndicator (1)" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();


            new AssetAccessor<Material>( MaterialIndex.refMatNullifierGemPortal, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierDeathExplosion).transform;
                MeshRenderer rend = trans.Find( "Sphere" ).GetComponent<MeshRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierDeathExplosion ).RegisterAccessor();
            #endregion

            #region Nullifier Pre-Bomb Ghost
            new AssetAccessor<Material>( MaterialIndex.refMatNullBombAreaIndicator, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refNullifierPreBombGhost).transform;
                ParticleSystemRenderer rend = null;
                Boolean pastFirst = true;
                for( Int32 i = 0; i < trans.childCount; ++i )
                {
                    Transform child = trans.GetChild( i );
                    if( child.name == "AreaIndicator" )
                    {
                        if( pastFirst )
                        {
                            rend = child.GetComponent<ParticleSystemRenderer>();
                            break;
                        } else
                        {
                            pastFirst = true;
                        }
                    }
                }
                return rend.sharedMaterials[0];
            }, PrefabIndex.refNullifierPreBombGhost ).RegisterAccessor();
            #endregion

            #region Will-O-Wisp Explosion
            new AssetAccessor<Material>( MaterialIndex.refMatTracer, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refWillOWispExplosion).transform;
                ParticleSystemRenderer rend = trans.Find( "Sparks" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refWillOWispExplosion ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatDistortionFaded, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refWillOWispExplosion).transform;
                ParticleSystemRenderer rend = trans.Find( "Distortion" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refWillOWispExplosion ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatWillowispSpiral, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refWillOWispExplosion).transform;
                ParticleSystemRenderer rend = trans.Find( "Flames, Tube" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refWillOWispExplosion ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatWillowispRadial, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refWillOWispExplosion).transform;
                ParticleSystemRenderer rend = trans.Find( "Flames, Radial" ).GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refWillOWispExplosion ).RegisterAccessor();
            #endregion

            #region Locked Mage
            new AssetAccessor<Material>( MaterialIndex.refMatBazaarIceCore, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLockedMage).transform.Find( "ModelBase/IceMesh" );
                MeshRenderer rend = trans.GetComponent<MeshRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLockedMage ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatBazaarIceDistortion, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLockedMage).transform.Find( "ModelBase/IceMesh" );
                MeshRenderer rend = trans.GetComponent<MeshRenderer>();
                return rend.sharedMaterials[1];
            }, PrefabIndex.refLockedMage ).RegisterAccessor();
            #endregion

            #region Fire Tornado Ghost
            new AssetAccessor<Material>( MaterialIndex.refMatGenericFlash, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFireTornadoGhost).transform.Find( "Flash, Red" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refFireTornadoGhost ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatDustDirectionalDark, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFireTornadoGhost).transform.Find( "Smoke" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refFireTornadoGhost ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatFireRingRunes, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFireTornadoGhost).transform.Find( "InitialBurst/RuneRings" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refFireTornadoGhost ).RegisterAccessor();

            #endregion

            #region Titan Recharge Rocks Effect
            new AssetAccessor<Material>( MaterialIndex.refMatGolemExplosion, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refTitanRechargeRocksEffect).transform.Find( "3DDebris" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[1];
            }, PrefabIndex.refTitanRechargeRocksEffect ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatTitanBeam, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refTitanRechargeRocksEffect).transform.Find( "Sparks, Trail" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refTitanRechargeRocksEffect ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatArcaneCircle1, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refTitanRechargeRocksEffect).transform.Find( "Glow" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refTitanRechargeRocksEffect ).RegisterAccessor();
            new AssetAccessor<Material>( MaterialIndex.refMatDistortion, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refTitanRechargeRocksEffect).transform.Find( "Distortion" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refTitanRechargeRocksEffect ).RegisterAccessor();
            #endregion

            #region BoostJumpEffect
            new AssetAccessor<Material>( MaterialIndex.refMatAngelFeather, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refBoostJumpEffect).transform.Find( "Feather, Directional" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refBoostJumpEffect ).RegisterAccessor();
            #endregion

            #region ProcStealthKit
            new AssetAccessor<Material>( MaterialIndex.refMatStealthkitSparks, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refProcStealthkit).transform.Find( "Sparks" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refProcStealthkit ).RegisterAccessor();
            #endregion

            #region RoboBallBossDelayKnockupEffect
            new AssetAccessor<Material>( MaterialIndex.refMatRoboBallParticleRingHuge, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refRoboBallBossDelayKnockupEffect).transform.Find( "Sphere" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refRoboBallBossDelayKnockupEffect ).RegisterAccessor();
            #endregion

            #region FirePillarEffect
            new AssetAccessor<Material>( MaterialIndex.refMatFireStaticLarge, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFirePillarEffect).transform.Find( "FX/Fire" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refFirePillarEffect ).RegisterAccessor();
            #endregion

            #region LightningStakeNova
            new AssetAccessor<Material>( MaterialIndex.refMatOmniRing2Generic, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLightningStakeNova).transform.Find( "AreaIndicatorRing, Billboard" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLightningStakeNova ).RegisterAccessor();
            #endregion

            #region MercSwordFinisherSlash
            new AssetAccessor<Material>( MaterialIndex.refMatMercSwipe2, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refMercSwordFinisherSlash).transform.Find( "SwingTrail" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refMercSwordFinisherSlash ).RegisterAccessor();
            #endregion

            #region MageUnlockPreExplosion
            new AssetAccessor<Material>( MaterialIndex.refMatSuspendedInTime, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refMageUnlockPreExplosion).transform.Find( "Running Particles/Flames, Radial" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refMageUnlockPreExplosion ).RegisterAccessor();
            #endregion

            #region TeleporterBeaconEffect
            new AssetAccessor<Material>( MaterialIndex.refMatTPShockwave, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refTeleporterBeaconEffect).transform.Find( "InitialBurst/Ring" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refTeleporterBeaconEffect ).RegisterAccessor();
            #endregion

            #region SprintActivate
            new AssetAccessor<Material>( MaterialIndex.refMatOpagueDustTrail, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refSprintActivate).transform.Find( "SwingTrail" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refSprintActivate ).RegisterAccessor();
            #endregion

            #region LemurianBiteTrail
            new AssetAccessor<Material>( MaterialIndex.refMatLizardBiteTrail, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLemurianBiteTrail).transform.Find( "SwingTrail" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLemurianBiteTrail ).RegisterAccessor();
            #endregion

            #region HippoRezEffect
            new AssetAccessor<Material>( MaterialIndex.refMatAngelEffect, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refHippoRezEffect).transform.Find( "Ring" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refHippoRezEffect ).RegisterAccessor();
            #endregion

            #region ExplosionDroneDeath
            new AssetAccessor<Material>( MaterialIndex.refMatCutExplosion, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refExplosionDroneDeath).transform.Find( "Particles/InitialBurst/Flames" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refExplosionDroneDeath ).RegisterAccessor();
            #endregion

            #region WaterFootstep
            new AssetAccessor<Material>( MaterialIndex.refMatOpagueWaterFoam, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refWaterFootstep).transform.Find( "FoamBilllboard" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refWaterFootstep ).RegisterAccessor();
            #endregion

            #region BootIsReady
            new AssetAccessor<Material>( MaterialIndex.refMatBootWaveEnergy, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refBootIsReady).transform.Find( "BlueRing" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refBootIsReady ).RegisterAccessor();
            #endregion

            #region ExplosionEngiTurretDeath
            new AssetAccessor<Material>( MaterialIndex.refMatEngiTrail, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refExplosionEngiTurretDeath).transform.Find( "InitialBurst/Ring" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refExplosionEngiTurretDeath ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatGenericFire, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refExplosionEngiTurretDeath).transform.Find( "InitialBurst/Flames" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refExplosionEngiTurretDeath ).RegisterAccessor();
            #endregion

            #region TeleportOutBoom
            new AssetAccessor<Material>( MaterialIndex.refMatTeleportOut, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refTeleportOutBoom).transform.Find( "CenterPoof" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refTeleportOutBoom ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatDustSoft, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refTeleportOutBoom).transform.Find( "Dust" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refTeleportOutBoom ).RegisterAccessor();
            #endregion

            #region FruitHealEffect
            new AssetAccessor<Material>( MaterialIndex.refMatHealingCross, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFruitHealEffect).transform.Find( "Crosses" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refFruitHealEffect ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatJellyfishChunks, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFruitHealEffect).transform.Find( "Goo Slash" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refFruitHealEffect ).RegisterAccessor();
            #endregion

            #region ImpBossDeathEffect
            new AssetAccessor<Material>( MaterialIndex.refMatImpSwipe, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refImpBossDeathEffect).transform.Find( "DashRings" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refImpBossDeathEffect ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatImpBossPortal, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refImpBossDeathEffect).transform.Find( "Ring" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refImpBossDeathEffect ).RegisterAccessor();
            #endregion

            #region PoisonNovaProc
            new AssetAccessor<Material>( MaterialIndex.refMatHauntedAura, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refPoisonNovaProc).transform.Find( "Particles/Ring, Procced" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refPoisonNovaProc ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatSkullFire, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refPoisonNovaProc).transform.Find( "Particles/Flames" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refPoisonNovaProc ).RegisterAccessor();
            #endregion

            #region SonicBoomEffect
            new AssetAccessor<Material>( MaterialIndex.refMatSonicBoomGroundDust, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refSonicBoomEffect).transform.Find( "DustColliders/Dust" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refSonicBoomEffect ).RegisterAccessor();
            #endregion

            #region MageLightningBombExplosion
            new AssetAccessor<Material>( MaterialIndex.refMatMageMatrixDirectionalLightning, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refMageLightningbombExplosion).transform.Find( "Matrix, Directional" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refMageLightningbombExplosion ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatMageMatrixLightning, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refMageLightningbombExplosion).transform.Find( "Matrix, Billboard" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refMageLightningbombExplosion ).RegisterAccessor();
            #endregion

            #region MageFlamethrowerEffect
            new AssetAccessor<Material>( MaterialIndex.refMatMatrixTriFire, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refMageFlamethrowerEffect).transform.Find( "IcoCharge" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refMageFlamethrowerEffect ).RegisterAccessor();
            #endregion

            #region CleanseEffect
            new AssetAccessor<Material>( MaterialIndex.refMatCleanseCore, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refCleanseEffect).transform.Find( "Ring" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refCleanseEffect ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatCleanseWater, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refCleanseEffect).transform.Find( "Splash" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refCleanseEffect ).RegisterAccessor();
            #endregion

            #region LaserTurbineBombExplosion
            new AssetAccessor<Material>( MaterialIndex.refMatLaserTurbineTargetingLaser, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLaserTurbineBombExplosion).transform.Find( "Slashes" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLaserTurbineBombExplosion ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatOmniRadialSlash1Merc, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLaserTurbineBombExplosion).transform.Find( "BillboardSlashes" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLaserTurbineBombExplosion ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatOmniHitspark2Merc, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLaserTurbineBombExplosion).transform.Find( "SharpSlashes" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLaserTurbineBombExplosion ).RegisterAccessor();
            #endregion

            #region GravekeeperMaskDeath
            new AssetAccessor<Material>( MaterialIndex.refMatOmniHitspark1, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refGravekeeperMaskDeath).transform.Find( "OmniExplosionVFXArchWisp/ScaledHitsparks 1" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refGravekeeperMaskDeath ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatOpagueDust, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refGravekeeperMaskDeath).transform.Find( "OmniExplosionVFXArchWisp/ScaledSmoke, Billboard" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refGravekeeperMaskDeath ).RegisterAccessor();
            #endregion

            #region LevelUpEffect
            new AssetAccessor<Material>( MaterialIndex.refMatOmniRing1Generic, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLevelUpEffect).transform.Find( "Ring" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLevelUpEffect ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatOmniHitspark4Merc, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refLevelUpEffect).transform.Find( "BrightFlash, Lines" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refLevelUpEffect ).RegisterAccessor();
            #endregion

            #region ClayBossMulcher
            new AssetAccessor<Material>( MaterialIndex.refMatBloodClayLarge, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refClayBossMulcher).transform.Find( "Goo" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refClayBossMulcher ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatClayGooFizzle, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refClayBossMulcher).transform.Find( "Trail" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refClayBossMulcher ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatClayBubbleBillboard, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refClayBossMulcher).transform.Find( "Bubbles, 2D" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refClayBossMulcher ).RegisterAccessor();
            #endregion

            #region ElectricWormBurrow
            new AssetAccessor<Material>( MaterialIndex.refMatOpagueDustLargeDirectional, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refElectricWormBurrow).transform.Find( "ParticleLoop/Dust, Directional" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refElectricWormBurrow ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatOpagueDustLarge, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refElectricWormBurrow).transform.Find( "ParticleLoop/Dust, Billboard" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refElectricWormBurrow ).RegisterAccessor();
            #endregion

            #region AmmoPackPickupEffect
            new AssetAccessor<Material>( MaterialIndex.refMatGenericLaser, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refAmmoPackPickupEffect).transform.Find( "Ring" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refAmmoPackPickupEffect ).RegisterAccessor();
            #endregion

            #region BubbleShieldEndEffect
            new AssetAccessor<Material>( MaterialIndex.refMatEngiShieldShards, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refBubbleShieldEndEffect).transform.Find( "Quads" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refBubbleShieldEndEffect ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatOmniExplosion1, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refBubbleShieldEndEffect).transform.Find( "OmniExplosionVFXEngiTurretDeath/Unscaled Flames" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refBubbleShieldEndEffect ).RegisterAccessor();

            new AssetAccessor<Material>( MaterialIndex.refMatRoboChunks, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refBubbleShieldEndEffect).transform.Find( "OmniExplosionVFXEngiTurretDeath/Chunks, Billboards" );
                ParticleSystemRenderer rend = trans.GetComponent<ParticleSystemRenderer>();
                return rend.sharedMaterials[0];
            }, PrefabIndex.refBubbleShieldEndEffect ).RegisterAccessor();
            #endregion





            #endregion









            completedProperly = true;
        }
    }
}
