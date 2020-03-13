
using RogueWispPlugin.Helpers;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void CreatePrefabAccessors()
        {
            #region Reference Prefabs
            new GenericAccessor<GameObject>( PrefabIndex.refWillOWispExplosion,                 () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/WillOWispExplosion" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refNullifierDeathExplosion,            () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/NullifierDeathExplosion" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refNullifierPreBombGhost,              () =>
            {
                return Resources.Load<GameObject>( "Prefabs/ProjectileGhosts/NullifierPreBombGhost" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refLockedMage,                         () =>
            {
                return Resources.Load<GameObject>( "Prefabs/NetworkedObjects/LockedMage" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refFireTornadoGhost,                   () =>
            {
                return Resources.Load<GameObject>( "Prefabs/ProjectileGhosts/FireTornadoGhost" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refTitanRechargeRocksEffect,           () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/TitanRechargeRocksEffect" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refBoostJumpEffect,                    () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/BoostJumpEffect" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refProcStealthkit,                     () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/ProcStealthkit" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refRoboBallBossDelayKnockupEffect,     () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/RoboBallBossDelayKnockupEffect" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refFirePillarEffect,                   () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/FirePillarEffect" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refLightningStakeNova,                 () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/LightningStakeNova" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refMercSwordFinisherSlash,             () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/MercSwordFinisherSlash" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refMageUnlockPreExplosion,             () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/MageUnlockPreExplosion" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refTeleporterBeaconEffect,             () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/TeleporterBeaconEffect" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refSprintActivate,                     () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/SprintActivate" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refLemurianBiteTrail,                  () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/LemurianBiteTrail" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refHippoRezEffect,                     () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/HippoRezEffect" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refExplosionDroneDeath,                () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/ExplosionDroneDeath" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refWaterFootstep,                      () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/WaterFootstep" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refBootIsReady,                        () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/BootIsReady" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refExplosionEngiTurretDeath,           () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/ExplosionEngiTurretDeath" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refTeleportOutBoom,                    () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/TeleportOutBoom" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refFruitHealEffect,                    () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/FruitHealEffect" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refImpBossDeathEffect,                 () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/ImpBossDeathEffect" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refPoisonNovaProc,                     () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/PoisonNovaProc" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refSonicBoomEffect,                    () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/SonicBoomEffect" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refMageLightningbombExplosion,         () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/MageLightningBombExplosion" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refMageFlamethrowerEffect,             () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/MageFlamethrowerEffect" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refCleanseEffect,                      () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/CleanseEffect" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refLaserTurbineBombExplosion,          () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/LaserTurbineBombExplosion" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refGravekeeperMaskDeath,               () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/GravekeeperMaskDeath" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refLevelUpEffect,                      () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/LevelUpEffect" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refClayBossMulcher,                    () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/ClayBossMulcher" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refElectricWormBurrow,                 () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/ElectricWurmBurrow" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refAmmoPackPickupEffect,               () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/AmmoPackPickupEffect" );
            }, false, ExecutionState.Awake ).RegisterAccessor();


            new GenericAccessor<GameObject>( PrefabIndex.refBubbleShieldEndEffect,              () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/BubbleShieldEndEffect" );
            }, false, ExecutionState.Awake ).RegisterAccessor();
            #endregion
        }
    }
}
