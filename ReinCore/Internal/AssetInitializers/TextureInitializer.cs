using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReinCore
{
    internal static class TextureInitializer
    {
        private static Boolean completedProperly = false;
        internal static Boolean Initialize()
        {
            return completedProperly;
        }


        static TextureInitializer()
        {
            new AssetAccessor<Texture2D>( Texture2DIndex.None, () =>
            {
                return null;
            } ).RegisterAccessor();

            #region Reference Textures
            #region Cloud Textures
            new AssetAccessor<Texture2D>( Texture2DIndex.refTexCloudDifferenceBW1, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatOnHelfire ) );
            }, MaterialIndex.refMatOnHelfire ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexCloudDifferenceBW2, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatNullBombAreaIndicator ) );
            }, MaterialIndex.refMatNullBombAreaIndicator ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexCloudIce, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatFireRingRunes ) );
            }, MaterialIndex.refMatFireRingRunes ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexCloudCrackedIce, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatBazaarIceCore ) );
            }, MaterialIndex.refMatBazaarIceCore ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexCloudOrganic1, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatElitePoisonParticleSystemReplacement ) );
            }, MaterialIndex.refMatElitePoisonParticleSystemReplacement ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexCloudOrganic2, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatVagrantEnergized ) );
            }, MaterialIndex.refMatVagrantEnergized ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexCloudPixel2, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatTPInOut ) );
            }, MaterialIndex.refMatTPInOut ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexCloudLightning1, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatVagrantEnergized ) );
            }, MaterialIndex.refMatVagrantEnergized ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexMagmaCloud, () =>
            {
                return GetCloud2Tex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatNullifierExplosionAreaIndicatorHard ) );
            }, MaterialIndex.refMatNullifierExplosionAreaIndicatorHard ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexCloudColor1, () =>
            {
                return GetCloud2Tex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatNullBombAreaIndicator ) );
            }, MaterialIndex.refMatNullBombAreaIndicator ).RegisterAccessor();


            #endregion
            #region Mask Textures
            new AssetAccessor<Texture2D>( Texture2DIndex.refTexBasicMask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatElitePoisonParticleSystemReplacement ) );
            }, MaterialIndex.refMatElitePoisonParticleSystemReplacement ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexBehemothTileMask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatNullifierStarTrail ) );
            }, MaterialIndex.refMatNullifierStarTrail ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexGalaxy1Mask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatNullifierStarParticle ) );
            }, MaterialIndex.refMatNullifierStarParticle ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexParticleDust1Mask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatTracerBright ) );
            }, MaterialIndex.refMatTracerBright ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexParticleDust2Mask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatGenericFlash ) );
            }, MaterialIndex.refMatGenericFlash ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexLightning2Mask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatNullifierStarPortalEdge ) );
            }, MaterialIndex.refMatNullifierStarPortalEdge ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexGenericStarburstMaskSkewed, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatWillowispRadial ) );
            }, MaterialIndex.refMatWillowispRadial ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexGlowSoftCenterMask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatDustDirectionalDark ) );
            }, MaterialIndex.refMatDustDirectionalDark ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexRingSetRuneMask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatFireRingRunes ) );
            }, MaterialIndex.refMatFireRingRunes ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexBanditExplosionMask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatGolemExplosion ) );
            }, MaterialIndex.refMatGolemExplosion ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexArcaneCircle1Mask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatArcaneCircle1 ) );
            }, MaterialIndex.refMatArcaneCircle1 ).RegisterAccessor();


            #endregion
            #region Assorted Textures
            new AssetAccessor<Texture2D>( Texture2DIndex.refTexAlphaGradient2, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatShatteredGlass ) );
            }, MaterialIndex.refMatShatteredGlass ).RegisterAccessor();



            new AssetAccessor<Texture2D>( Texture2DIndex.refTexEngiShield, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatMercEnergized ) );
            }, MaterialIndex.refMatMercEnergized ).RegisterAccessor();



            new AssetAccessor<Texture2D>( Texture2DIndex.refTexEngiShieldBlurred, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatMercEnergized ) );
            }, MaterialIndex.refMatMercEnergized ).RegisterAccessor();



            new AssetAccessor<Texture2D>( Texture2DIndex.refTexDebris1, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatDebris1 ) );
            }, MaterialIndex.refMatDebris1 ).RegisterAccessor();



            new AssetAccessor<Texture2D>( Texture2DIndex.refTexSmokePuffSPeckledDisplacement, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatOpagueDustSpeckledLarge ) );
            }, MaterialIndex.refMatOpagueDustSpeckledLarge ).RegisterAccessor();



            new AssetAccessor<Texture2D>( Texture2DIndex.refTexNullifierSky, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatNullifierExplosionAreaIndicatorSoft ) );
            }, MaterialIndex.refMatNullifierExplosionAreaIndicatorSoft ).RegisterAccessor();



            new AssetAccessor<Texture2D>( Texture2DIndex.refTexNullifierSky2, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatNullifierGemPortal ) );
            }, MaterialIndex.refMatNullifierGemPortal ).RegisterAccessor();



            new AssetAccessor<Texture2D>( Texture2DIndex.refTexWillowispSpiral, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatWillowispSpiral ) );
            }, MaterialIndex.refMatWillowispSpiral ).RegisterAccessor();



            new AssetAccessor<Texture2D>( Texture2DIndex.refCaustics, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatNullifierExplosionAreaIndicatorHard ) );
            }, MaterialIndex.refMatNullifierExplosionAreaIndicatorHard ).RegisterAccessor();



            new AssetAccessor<Texture2D>( Texture2DIndex.refDistTex, () =>
            {
                return GetMainTex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatTracer ) );
            }, MaterialIndex.refMatTracer ).RegisterAccessor();




            #endregion
            #region Normal Textures
            new AssetAccessor<Texture2D>( Texture2DIndex.refWaves_N, () =>
            {
                return GetBumpMap( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatBazaarIceDistortion ) );
            }, MaterialIndex.refMatBazaarIceDistortion ).RegisterAccessor();



            new AssetAccessor<Texture2D>( Texture2DIndex.refTexNormalSphere, () =>
            {
                return GetBumpMap( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatDistortion ) );
            }, MaterialIndex.refMatDistortion ).RegisterAccessor();



            new AssetAccessor<Texture2D>( Texture2DIndex.refTexNormalSphereFaded, () =>
            {
                return GetBumpMap( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatDistortionFaded ) );
            }, MaterialIndex.refMatDistortionFaded ).RegisterAccessor();



            new AssetAccessor<Texture2D>( Texture2DIndex.refTexCloudOrganicNormal, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatNullifierStarPortalEdge ) );
            }, MaterialIndex.refMatNullifierStarPortalEdge ).RegisterAccessor();



            new AssetAccessor<Texture2D>( Texture2DIndex.refTexSmokePuffSpeckledNormal, () =>
            {
                return GetNormalMap( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatOpagueDustSpeckledLarge ) );
            }, MaterialIndex.refMatOpagueDustSpeckledLarge ).RegisterAccessor();



            new AssetAccessor<Texture2D>( Texture2DIndex.refTexNormalInvertConeFade, () =>
            {
                return GetBumpMap( AssetLibrary<Material>.GetAsset( MaterialIndex.refMatInverseDistortion ) );
            }, MaterialIndex.refMatInverseDistortion ).RegisterAccessor();
            #endregion

            #region Unsorted
            new AssetAccessor<Texture2D>( Texture2DIndex.refTexFeatherMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatAngelFeather);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatAngelFeather ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexGlowSquareMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatStealthkitSparks);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatStealthkitSparks ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexShard03Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatRoboBallParticleRingHuge);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatRoboBallParticleRingHuge ).RegisterAccessor();

            ////////////////////////////////////////////////////////////////////////////////////////////////
            new AssetAccessor<Texture2D>( Texture2DIndex.refTexFireMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatFireStaticLarge);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatFireStaticLarge ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexOmniShockwave2Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatOmniRing2Generic);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatOmniRing2Generic ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexMercSwipeMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatMercSwipe2);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatMercSwipe2 ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexChainTrailMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatSuspendedInTime);
                //return GetMainTex( m );
                return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatSuspendedInTime ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexVagrantTentacleMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatTPShockwave);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatTPShockwave ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexBehemothTileNormal, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatOpagueDustTrail);
                //return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                return GetNormalMap( m );
            }, MaterialIndex.refMatOpagueDustTrail ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexLemurianSlash, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatLizardBiteTrail);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatLizardBiteTrail ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexAngelWingMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatAngelEffect);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatAngelEffect ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexVFXExplosionMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatCutExplosion);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatCutExplosion ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexShockwaveRing2Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatOpagueWaterFoam);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatOpagueWaterFoam ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexItemPickupEffectParticleMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatBootWaveEnergy);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatBootWaveEnergy ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexEngiTrail, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatEngiTrail);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatEngiTrail ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexGlowPaintMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatGenericFire);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatGenericFire ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexAlphaGradient3Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatTeleportOut);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatTeleportOut ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexAlphaGradient2Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatDustSoft);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatDustSoft ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexHealingCrossMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatHealingCross);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatHealingCross ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexFluidSpraySmall, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatJellyfishChunks);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatJellyfishChunks ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexImpSwipeMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatImpSwipe);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatImpSwipe ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexCloudWaterRipples, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatImpBossPortal);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatImpBossPortal ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexCloudSkulls, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatHauntedAura);
                //return GetMainTex( m );
                //return GetCloud1Tex( m );
                return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatHauntedAura ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexGlowSkullMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatSkullFire);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatSkullFire ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexFluffyCloud2Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatSonicBoomGroundDust);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatSonicBoomGroundDust ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexFluffyCloud3Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatSonicBoomGroundDust);
                //return GetMainTex( m );
                return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatSonicBoomGroundDust ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexMageMatrixMaskDirectional, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatMageMatrixDirectionalLightning);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatMageMatrixDirectionalLightning ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexMageMatrixMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatMageMatrixLightning);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatMageMatrixLightning ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexMageMatrixTri, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatMatrixTriFire);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatMatrixTriFire ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexShockwaveMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatCleanseCore);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatCleanseCore ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexBlackHoleMask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatCleanseCore);
                //return GetMainTex( m );
                return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatCleanseCore ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexCloudOrganic3, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatCleanseWater);
                //return GetMainTex( m );
                //return GetCloud1Tex( m );
                return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatCleanseWater ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexAlphaGradient4Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatLaserTurbineTargetingLaser);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatLaserTurbineTargetingLaser ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexOmniRadialSlash1Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatOmniRadialSlash1Merc);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatOmniRadialSlash1Merc ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexOmniHitspark2Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatOmniHitspark2Merc);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatOmniHitspark2Merc ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexOmniHitspark1Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatOmniHitspark1);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatOmniHitspark1 ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexSmokePuffSmallDirectionalDisplacement, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatOpagueDust);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatOpagueDust ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexSmokePuffSmallDirectionalNormal, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatOpagueDust);
                //return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                return GetNormalMap( m );
            }, MaterialIndex.refMatOpagueDust ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexOmniShockwave1Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatOmniRing1Generic);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatOmniRing1Generic ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexOmniHitspark4Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatOmniHitspark4Merc);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatOmniHitspark4Merc ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexFluidSprayLarge, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatBloodClayLarge);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatBloodClayLarge ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexWhiteRadialGradient256, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatClayGooFizzle);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatClayGooFizzle ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexParticleDust3Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatClayBubbleBillboard);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatClayBubbleBillboard ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexSmokePuffDirectionalDisplacement, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatOpagueDustLargeDirectional);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatOpagueDustLargeDirectional ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexSmokePuffDirectionalNormal, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatOpagueDustLargeDirectional);
                //return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                return GetNormalMap( m );
            }, MaterialIndex.refMatOpagueDustLargeDirectional ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexSmokePuffSmallDisplacement, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatOpagueDustLarge);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatOpagueDustLarge ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexSmokePuffSmallNormal, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatOpagueDustLarge);
                //return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                return GetNormalMap( m );
            }, MaterialIndex.refMatOpagueDustLarge ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexLightning3Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatGenericLaser);
                //return GetMainTex( m );
                return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatGenericLaser ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexShard02Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatEngiShieldShards);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatEngiShieldShards ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexOmniExplosion2Mask, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatOmniExplosion1);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatOmniExplosion1 ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexRoboChunksDiffuse, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatRoboChunks);
                return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                //return GetNormalMap( m );
            }, MaterialIndex.refMatRoboChunks ).RegisterAccessor();


            new AssetAccessor<Texture2D>( Texture2DIndex.refTexRoboChunksNormal, () =>
            {
                var m = AssetLibrary<Material>.GetAsset(MaterialIndex.refMatRoboChunks);
                //return GetMainTex( m );
                //return GetCloud1Tex( m );
                //return GetCloud2Tex( m );
                //return GetBumpMap( m );
                return GetNormalMap( m );
            }, MaterialIndex.refMatRoboChunks ).RegisterAccessor();

            #endregion
            #endregion

            completedProperly = true;
        }





        private static Texture2D GetMainTex( Material mat )
        {
            return (Texture2D)mat.GetTexture( "_MainTex" );
        }
        private static Texture2D GetCloud1Tex( Material mat )
        {
            return (Texture2D)mat.GetTexture( "_Cloud1Tex" );
        }
        private static Texture2D GetCloud2Tex( Material mat )
        {
            return (Texture2D)mat.GetTexture( "_Cloud2Tex" );
        }
        private static Texture2D GetBumpMap( Material mat )
        {
            return (Texture2D)mat.GetTexture( "_BumpMap" );
        }
        private static Texture2D GetNormalMap( Material mat )
        {
            return (Texture2D)mat.GetTexture( "_NormalTex" );
        }
    }
}
