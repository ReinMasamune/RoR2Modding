using System;
using System.Collections.Generic;
using System.Text;

namespace ReinCore
{
    public enum PrefabIndex : UInt64
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        refWillOWispExplosion,
        refNullifierDeathExplosion,
        refNullifierPreBombGhost,
        refLockedMage,
        refFireTornadoGhost,
        refTitanRechargeRocksEffect,
        refBoostJumpEffect,
        refProcStealthkit,
        refRoboBallBossDelayKnockupEffect,
        refFirePillarEffect,
        refLightningStakeNova,
        refMercSwordFinisherSlash,
        refMageUnlockPreExplosion,
        refTeleporterBeaconEffect,
        refSprintActivate,
        refLemurianBiteTrail,
        refHippoRezEffect,
        refExplosionDroneDeath,
        refWaterFootstep,
        refBootIsReady,
        refExplosionEngiTurretDeath,
        refTeleportOutBoom,
        refFruitHealEffect,
        refImpBossDeathEffect,
        refPoisonNovaProc,
        refSonicBoomEffect,
        refMageLightningbombExplosion,
        refMageFlamethrowerEffect,
        refCleanseEffect,
        refLaserTurbineBombExplosion,
        refGravekeeperMaskDeath,
        refLevelUpEffect,
        refClayBossMulcher,
        refElectricWormBurrow,
        refAmmoPackPickupEffect,
        refBubbleShieldEndEffect,
        refPickupTriTip,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
// TODO: Switch to UInt32 for indexing to avoid hash collisions in dictionary.
