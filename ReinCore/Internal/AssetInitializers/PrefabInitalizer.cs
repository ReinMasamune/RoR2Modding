using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReinCore
{
    internal static class PrefabInitializer
    {
        private static Boolean completedProperly = false;
        internal static Boolean Initialize()
        {
            return completedProperly;
        }


        static PrefabInitializer()
        {
            #region Reference Prefabs
            new AssetAccessor<GameObject>( PrefabIndex.refWillOWispExplosion, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/WillOWispExplosion" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refNullifierDeathExplosion, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/NullifierDeathExplosion" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refNullifierPreBombGhost, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/ProjectileGhosts/NullifierPreBombGhost" ); ;
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refLockedMage, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/NetworkedObjects/LockedMage" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refFireTornadoGhost, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/ProjectileGhosts/FireTornadoGhost" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refTitanRechargeRocksEffect, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/TitanRechargeRocksEffect" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refBoostJumpEffect, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/BoostJumpEffect" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refProcStealthkit, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/ProcStealthkit" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refRoboBallBossDelayKnockupEffect, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/RoboBallBossDelayKnockupEffect" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refFirePillarEffect, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/FirePillarEffect" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refLightningStakeNova, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/LightningStakeNova" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refMercSwordFinisherSlash, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/MercSwordFinisherSlash" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refMageUnlockPreExplosion, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/MageUnlockPreExplosion" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refTeleporterBeaconEffect, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/TeleporterBeaconEffect" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refSprintActivate, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/SprintActivate" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refLemurianBiteTrail, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/LemurianBiteTrail" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refHippoRezEffect, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/HippoRezEffect" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refExplosionDroneDeath, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/ExplosionDroneDeath" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refWaterFootstep, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/WaterFootstep" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refBootIsReady, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/BootIsReady" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refExplosionEngiTurretDeath, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/ExplosionEngiTurretDeath" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refTeleportOutBoom, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/TeleportOutBoom" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refFruitHealEffect, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/FruitHealEffect" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refImpBossDeathEffect, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/ImpBossDeathEffect" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refPoisonNovaProc, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/PoisonNovaProc" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refSonicBoomEffect, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/SonicBoomEffect" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refMageLightningbombExplosion, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/MageLightningBombExplosion" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refMageFlamethrowerEffect, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/MageFlamethrowerEffect" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refCleanseEffect, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/CleanseEffect" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refLaserTurbineBombExplosion, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/LaserTurbineBombExplosion" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refGravekeeperMaskDeath, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/GravekeeperMaskDeath" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refLevelUpEffect, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/LevelUpEffect" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refClayBossMulcher, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/ClayBossMulcher" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refElectricWormBurrow, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/ElectricWurmBurrow" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refAmmoPackPickupEffect, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/AmmoPackPickupEffect" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refBubbleShieldEndEffect, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/BubbleShieldEndEffect" );
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refPickupTriTip, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/PickupModels/PickupTriTip" );
            } ).RegisterAccessor();



            #endregion


            completedProperly = true;
        }
    }
}
