using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal enum MaterialIndex : ulong
    {
        #region Reference Materials
        #region Direct Load
        refMatShatteredGlass,
        refMatVagrantEnergized,
        refMatTPInOut,
        refMatMercEnergized,
        refMatElitePoisonParticleSystemReplacement,
        refMatOnHelfire,
        #endregion

        #region Prefab Dependencies
        refMatTracer,
        refMatTracerBright,
        refMatDebris1,
        refMatDistortionFaded,
        refMatInverseDistortion,
        refMatOpagueDustSpeckledLarge,

        refMatNullifierStarParticle,
        refMatNullifierStarTrail,
        refMatNullifierStarPortalEdge,
        refMatNullifierExplosionAreaIndicatorSoft,
        refMatNullifierExplosionAreaIndicatorHard,
        refMatNullBombAreaIndicator,
        refMatNullifierGemPortal,

        refMatWillowispSpiral,
        refMatWillowispRadial,

        refMatBazaarIceCore,
        refMatBazaarIceDistortion,

        refMatGenericFlash,
        refMatDustDirectionalDark,
        refMatFireRingRunes,
        
        refMatGolemExplosion,
        refMatTitanBeam,
        refMatArcaneCircle1,
        refMatDistortion,



        refMatAngelFeather,	

		refMatStealthkitSparks, 

		refMatRoboBallParticleRingHuge, 

		refMatFireStaticLarge, 

		refMatOmniRing2Generic, 

		refMatMercSwipe2, 

		refMatSuspendedInTime,

		refMatTPShockwave, 

		refMatOpagueDustTrail,

		refMatLizardBiteTrail, 

		refMatAngelEffect, 

		refMatCutExplosion,

		refMatOpagueWaterFoam,
		
		refMatBootWaveEnergy, 

		refMatEngiTrail, //
		refMatGenericFire, //

		refMatTeleportOut, //
		refMatDustSoft, //

		refMatHealingCross, //
		refMatJellyfishChunks, //

		refMatImpSwipe,	//
		refMatImpBossPortal, //

		refMatHauntedAura, //
		refMatSkullFire, //

		refMatSonicBoomGroundDust, //

		refMatMageMatrixDirectionalLightning, //
		refMatMageMatrixLightning, //

		refMatMatrixTriFire, //

		refMatCleanseCore, //
		refMatCleanseWater, //

		refMatLaserTurbineTargetingLaser, //
		refMatOmniRadialSlash1Merc, //
		refMatOmniHitspark2Merc, //

		refMatOmniHitspark1, //
		refMatOpagueDust, //

		refMatOmniRing1Generic, //
		refMatOmniHitspark4Merc, //

		refMatBloodClayLarge, //
		refMatClayGooFizzle, //
		refMatClayBubbleBillboard, //

		refMatOpagueDustLargeDirectional, //TexSmokePuffDirectionalDisplacement TexSmokePuffDirectionalNormal
		refMatOpagueDustLarge, //TexSmokePuffSmallDisplacement TexSmokePuffSmallNormal

		refMatGenericLaser, //TexLightning3Mask

		refMatEngiShieldShards, //TexShard02Mask
		refMatOmniExplosion1, //TexOmniExplosion2Mask
		refMatRoboChunks, //TexRoboChunksDiffuse TexRoboChunksNormal
        #endregion
        #endregion
    }
}

/*
bubbleShieldendEffect
	texShard02Mask                              //100%
	texOmniExplosion2Mask                       //100%
	texRoboChunksDiffuse                        //100%
	texRoboChunksNormal                         //100%
    */
