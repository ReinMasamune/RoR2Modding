namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    internal static class PrefabInitializer
    {
        private static readonly Boolean completedProperly = false;
        internal static Boolean Initialize() => completedProperly;


        static PrefabInitializer()
        {
            #region Reference Prefabs
            new AssetAccessor<GameObject>( PrefabIndex.refWillOWispExplosion, () => Resources.Load<GameObject>( "Prefabs/Effects/WillOWispExplosion" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refNullifierDeathExplosion, () => Resources.Load<GameObject>( "Prefabs/Effects/NullifierDeathExplosion" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refNullifierPreBombGhost, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/ProjectileGhosts/NullifierPreBombGhost" ); ;
            }).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refLockedMage, () => Resources.Load<GameObject>( "Prefabs/NetworkedObjects/LockedMage" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refFireTornadoGhost, () => Resources.Load<GameObject>( "Prefabs/ProjectileGhosts/FireTornadoGhost" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refTitanRechargeRocksEffect, () => Resources.Load<GameObject>( "Prefabs/Effects/TitanRechargeRocksEffect" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refBoostJumpEffect, () => Resources.Load<GameObject>( "Prefabs/Effects/BoostJumpEffect" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refProcStealthkit, () => Resources.Load<GameObject>( "Prefabs/Effects/ProcStealthkit" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refRoboBallBossDelayKnockupEffect, () => Resources.Load<GameObject>( "Prefabs/Effects/RoboBallBossDelayKnockupEffect" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refFirePillarEffect, () => Resources.Load<GameObject>( "Prefabs/Effects/FirePillarEffect" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refLightningStakeNova, () => Resources.Load<GameObject>( "Prefabs/Effects/LightningStakeNova" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refMercSwordFinisherSlash, () => Resources.Load<GameObject>( "Prefabs/Effects/MercSwordFinisherSlash" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refMageUnlockPreExplosion, () => Resources.Load<GameObject>( "Prefabs/Effects/MageUnlockPreExplosion" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refTeleporterBeaconEffect, () => Resources.Load<GameObject>( "Prefabs/Effects/TeleporterBeaconEffect" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refSprintActivate, () => Resources.Load<GameObject>( "Prefabs/Effects/SprintActivate" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refLemurianBiteTrail, () => Resources.Load<GameObject>( "Prefabs/Effects/LemurianBiteTrail" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refHippoRezEffect, () => Resources.Load<GameObject>( "Prefabs/Effects/HippoRezEffect" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refExplosionDroneDeath, () => Resources.Load<GameObject>( "Prefabs/Effects/ExplosionDroneDeath" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refWaterFootstep, () => Resources.Load<GameObject>( "Prefabs/Effects/WaterFootstep" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refBootIsReady, () => Resources.Load<GameObject>( "Prefabs/Effects/BootIsReady" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refExplosionEngiTurretDeath, () => Resources.Load<GameObject>( "Prefabs/Effects/ExplosionEngiTurretDeath" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refTeleportOutBoom, () => Resources.Load<GameObject>( "Prefabs/Effects/TeleportOutBoom" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refFruitHealEffect, () => Resources.Load<GameObject>( "Prefabs/Effects/FruitHealEffect" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refImpBossDeathEffect, () => Resources.Load<GameObject>( "Prefabs/Effects/ImpBossDeathEffect" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refPoisonNovaProc, () => Resources.Load<GameObject>( "Prefabs/Effects/PoisonNovaProc" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refSonicBoomEffect, () => Resources.Load<GameObject>( "Prefabs/Effects/SonicBoomEffect" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refMageLightningbombExplosion, () => Resources.Load<GameObject>( "Prefabs/Effects/MageLightningBombExplosion" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refMageFlamethrowerEffect, () => Resources.Load<GameObject>( "Prefabs/Effects/MageFlamethrowerEffect" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refCleanseEffect, () => Resources.Load<GameObject>( "Prefabs/Effects/CleanseEffect" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refLaserTurbineBombExplosion, () => Resources.Load<GameObject>( "Prefabs/Effects/LaserTurbineBombExplosion" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refGravekeeperMaskDeath, () => Resources.Load<GameObject>( "Prefabs/Effects/GravekeeperMaskDeath" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refLevelUpEffect, () => Resources.Load<GameObject>( "Prefabs/Effects/LevelUpEffect" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refClayBossMulcher, () => Resources.Load<GameObject>( "Prefabs/Effects/ClayBossMulcher" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refElectricWormBurrow, () => Resources.Load<GameObject>( "Prefabs/Effects/ElectricWurmBurrow" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refAmmoPackPickupEffect, () => Resources.Load<GameObject>( "Prefabs/Effects/AmmoPackPickupEffect" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refBubbleShieldEndEffect, () => Resources.Load<GameObject>( "Prefabs/Effects/BubbleShieldEndEffect" ) ).RegisterAccessor();


            new AssetAccessor<GameObject>( PrefabIndex.refPickupTriTip, () => Resources.Load<GameObject>( "Prefabs/PickupModels/PickupTriTip" ) ).RegisterAccessor();



            #endregion


            completedProperly = true;
        }
    }
}
