
//using RogueWispPlugin.Helpers;
//using UnityEngine;

//namespace RogueWispPlugin
//{
//    internal partial class Main
//    {
//        partial void CreateTextureAccessors()
//        {
//            new GenericAccessor<Texture>( TextureIndex.None, () =>
//            {
//                return null;
//            }, false, ExecutionState.Constructor ).RegisterAccessor();

//            #region Reference Textures
//            #region Cloud Textures
//            new GenericAccessor<Texture>( TextureIndex.refTexCloudDifferenceBW1, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatOnHelfire] );
//            }, false, MaterialIndex.refMatOnHelfire ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexCloudDifferenceBW2, () =>
//            {
//                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatNullBombAreaIndicator] );
//            }, false, MaterialIndex.refMatNullBombAreaIndicator ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexCloudIce, () =>
//            {
//                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatFireRingRunes] );
//            }, false, MaterialIndex.refMatFireRingRunes ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexCloudCrackedIce, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatBazaarIceCore] );
//            }, false, MaterialIndex.refMatBazaarIceCore ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexCloudOrganic1, () =>
//            {
//                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatElitePoisonParticleSystemReplacement] );
//            }, false, MaterialIndex.refMatElitePoisonParticleSystemReplacement ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexCloudOrganic2, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatVagrantEnergized] );
//            }, false, MaterialIndex.refMatVagrantEnergized ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexCloudPixel2, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatTPInOut] );
//            }, false, MaterialIndex.refMatTPInOut ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexCloudLightning1, () =>
//            {
//                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatVagrantEnergized] );
//            }, false, MaterialIndex.refMatVagrantEnergized ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexMagmaCloud, () =>
//            {
//                return GetCloud2Tex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierExplosionAreaIndicatorHard] );
//            }, false, MaterialIndex.refMatNullifierExplosionAreaIndicatorHard ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexCloudColor1, () =>
//            {
//                return GetCloud2Tex( AssetLibrary<Material>.i[MaterialIndex.refMatNullBombAreaIndicator] );
//            }, false, MaterialIndex.refMatNullBombAreaIndicator ).RegisterAccessor();


//            #endregion
//            #region Mask Textures
//            new GenericAccessor<Texture>( TextureIndex.refTexBasicMask, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatElitePoisonParticleSystemReplacement] );
//            }, false, MaterialIndex.refMatElitePoisonParticleSystemReplacement ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexBehemothTileMask, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierStarTrail] );
//            }, false, MaterialIndex.refMatNullifierStarTrail ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexGalaxy1Mask, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierStarParticle] );
//            }, false, MaterialIndex.refMatNullifierStarParticle ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexParticleDust1Mask, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatTracerBright] );
//            }, false, MaterialIndex.refMatTracerBright ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexParticleDust2Mask, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatGenericFlash] );
//            }, false, MaterialIndex.refMatGenericFlash ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexLightning2Mask, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierStarPortalEdge] );
//            }, false, MaterialIndex.refMatNullifierStarPortalEdge ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexGenericStarburstMaskSkewed, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatWillowispRadial] );
//            }, false, MaterialIndex.refMatWillowispRadial ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexGlowSoftCenterMask, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatDustDirectionalDark] );
//            }, false, MaterialIndex.refMatDustDirectionalDark ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexRingSetRuneMask, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatFireRingRunes] );
//            }, false, MaterialIndex.refMatFireRingRunes ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexBanditExplosionMask, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatGolemExplosion] );
//            }, false, MaterialIndex.refMatGolemExplosion ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexArcaneCircle1Mask, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatArcaneCircle1] );
//            }, false, MaterialIndex.refMatArcaneCircle1 ).RegisterAccessor();


//            #endregion
//            #region Assorted Textures
//            new GenericAccessor<Texture>( TextureIndex.refTexAlphaGradient2, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatShatteredGlass] );
//            }, false, MaterialIndex.refMatShatteredGlass ).RegisterAccessor();



//            new GenericAccessor<Texture>( TextureIndex.refTexEngiShield, () =>
//            {
//                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatMercEnergized] );
//            }, false, MaterialIndex.refMatMercEnergized ).RegisterAccessor();



//            new GenericAccessor<Texture>( TextureIndex.refTexEngiShieldBlurred, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatMercEnergized] );
//            }, false, MaterialIndex.refMatMercEnergized ).RegisterAccessor();



//            new GenericAccessor<Texture>( TextureIndex.refTexDebris1, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatDebris1] );
//            }, false, MaterialIndex.refMatDebris1 ).RegisterAccessor();



//            new GenericAccessor<Texture>( TextureIndex.refTexSmokePuffSPeckledDisplacement, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatOpagueDustSpeckledLarge] );
//            }, false, MaterialIndex.refMatOpagueDustSpeckledLarge ).RegisterAccessor();



//            new GenericAccessor<Texture>( TextureIndex.refTexNullifierSky, () =>
//            {
//                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierExplosionAreaIndicatorSoft] );
//            }, false, MaterialIndex.refMatNullifierExplosionAreaIndicatorSoft ).RegisterAccessor();



//            new GenericAccessor<Texture>( TextureIndex.refTexNullifierSky2, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierGemPortal] );
//            }, false, MaterialIndex.refMatNullifierGemPortal ).RegisterAccessor();



//            new GenericAccessor<Texture>( TextureIndex.refTexWillowispSpiral, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatWillowispSpiral] );
//            }, false, MaterialIndex.refMatWillowispSpiral ).RegisterAccessor();



//            new GenericAccessor<Texture>( TextureIndex.refCaustics, () =>
//            {
//                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierExplosionAreaIndicatorHard] );
//            }, false, MaterialIndex.refMatNullifierExplosionAreaIndicatorHard ).RegisterAccessor();



//            new GenericAccessor<Texture>( TextureIndex.refDistTex, () =>
//            {
//                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatTracer] );
//            }, false, MaterialIndex.refMatTracer ).RegisterAccessor();




//            #endregion
//            #region Normal Textures
//            new GenericAccessor<Texture>( TextureIndex.refWaves_N, () =>
//            {
//                return GetBumpMap( AssetLibrary<Material>.i[MaterialIndex.refMatBazaarIceDistortion] );
//            }, false, MaterialIndex.refMatBazaarIceDistortion ).RegisterAccessor();



//            new GenericAccessor<Texture>( TextureIndex.refTexNormalSphere, () =>
//            {
//                return GetBumpMap( AssetLibrary<Material>.i[MaterialIndex.refMatDistortion] );
//            }, false, MaterialIndex.refMatDistortion ).RegisterAccessor();



//            new GenericAccessor<Texture>( TextureIndex.refTexNormalSphereFaded, () =>
//            {
//                return GetBumpMap( AssetLibrary<Material>.i[MaterialIndex.refMatDistortionFaded] );
//            }, false, MaterialIndex.refMatDistortionFaded ).RegisterAccessor();



//            new GenericAccessor<Texture>( TextureIndex.refTexCloudOrganicNormal, () =>
//            {
//                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierStarPortalEdge] );
//            }, false, MaterialIndex.refMatNullifierStarPortalEdge ).RegisterAccessor();



//            new GenericAccessor<Texture>( TextureIndex.refTexSmokePuffSpeckledNormal, () =>
//            {
//                return GetNormalMap( AssetLibrary<Material>.i[MaterialIndex.refMatOpagueDustSpeckledLarge] );
//            }, false, MaterialIndex.refMatOpagueDustSpeckledLarge ).RegisterAccessor();



//            new GenericAccessor<Texture>( TextureIndex.refTexNormalInvertConeFade, () =>
//            {
//                return GetBumpMap( AssetLibrary<Material>.i[MaterialIndex.refMatInverseDistortion] );
//            }, false, MaterialIndex.refMatInverseDistortion ).RegisterAccessor();
//            #endregion

//            #region Unsorted
//            new GenericAccessor<Texture>( TextureIndex.refTexFeatherMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatAngelFeather];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatAngelFeather ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexGlowSquareMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatStealthkitSparks];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatStealthkitSparks ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexShard03Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatRoboBallParticleRingHuge];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatRoboBallParticleRingHuge ).RegisterAccessor();

//            ////////////////////////////////////////////////////////////////////////////////////////////////
//            new GenericAccessor<Texture>( TextureIndex.refTexFireMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatFireStaticLarge];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatFireStaticLarge ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexOmniShockwave2Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatOmniRing2Generic];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatOmniRing2Generic ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexMercSwipeMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatMercSwipe2];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatMercSwipe2 ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexChainTrailMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatSuspendedInTime];
//                //return GetMainTex( m );
//                return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatSuspendedInTime ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexVagrantTentacleMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatTPShockwave];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatTPShockwave ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexBehemothTileNormal, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatOpagueDustTrail];
//                //return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                return GetNormalMap( m );
//            }, false, MaterialIndex.refMatOpagueDustTrail ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexLemurianSlash, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatLizardBiteTrail];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatLizardBiteTrail ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexAngelWingMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatAngelEffect];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatAngelEffect ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexVFXExplosionMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatCutExplosion];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatCutExplosion ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexShockwaveRing2Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatOpagueWaterFoam];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatOpagueWaterFoam ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexItemPickupEffectParticleMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatBootWaveEnergy];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatBootWaveEnergy ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexEngiTrail, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatEngiTrail];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatEngiTrail ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexGlowPaintMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatGenericFire];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatGenericFire ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexAlphaGradient3Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatTeleportOut];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatTeleportOut ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexAlphaGradient2Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatDustSoft];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatDustSoft ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexHealingCrossMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatHealingCross];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatHealingCross ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexFluidSpraySmall, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatJellyfishChunks];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatJellyfishChunks ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexImpSwipeMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatImpSwipe];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatImpSwipe ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexCloudWaterRipples, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatImpBossPortal];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatImpBossPortal ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexCloudSkulls, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatHauntedAura];
//                //return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatHauntedAura ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexGlowSkullMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatSkullFire];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatSkullFire ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexFluffyCloud2Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatSonicBoomGroundDust];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatSonicBoomGroundDust ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexFluffyCloud3Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatSonicBoomGroundDust];
//                //return GetMainTex( m );
//                return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatSonicBoomGroundDust ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexMageMatrixMaskDirectional, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatMageMatrixDirectionalLightning];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatMageMatrixDirectionalLightning ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexMageMatrixMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatMageMatrixLightning];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatMageMatrixLightning ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexMageMatrixTri, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatMatrixTriFire];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatMatrixTriFire ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexShockwaveMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatCleanseCore];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatCleanseCore ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexBlackHoleMask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatCleanseCore];
//                //return GetMainTex( m );
//                return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatCleanseCore ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexCloudOrganic3, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatCleanseWater];
//                //return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatCleanseWater ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexAlphaGradient4Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatLaserTurbineTargetingLaser];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatLaserTurbineTargetingLaser ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexOmniRadialSlash1Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatOmniRadialSlash1Merc];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatOmniRadialSlash1Merc ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexOmniHitspark2Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatOmniHitspark2Merc];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatOmniHitspark2Merc ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexOmniHitspark1Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatOmniHitspark1];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatOmniHitspark1 ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexSmokePuffSmallDirectionalDisplacement, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatOpagueDust];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatOpagueDust ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexSmokePuffSmallDirectionalNormal, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatOpagueDust];
//                //return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                return GetNormalMap( m );
//            }, false, MaterialIndex.refMatOpagueDust ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexOmniShockwave1Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatOmniRing1Generic];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatOmniRing1Generic ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexOmniHitspark4Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatOmniHitspark4Merc];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatOmniHitspark4Merc ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexFluidSprayLarge, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatBloodClayLarge];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatBloodClayLarge ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexWhiteRadialGradient256, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatClayGooFizzle];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatClayGooFizzle ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexParticleDust3Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatClayBubbleBillboard];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatClayBubbleBillboard ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexSmokePuffDirectionalDisplacement, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatOpagueDustLargeDirectional];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatOpagueDustLargeDirectional ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexSmokePuffDirectionalNormal, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatOpagueDustLargeDirectional];
//                //return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                return GetNormalMap( m );
//            }, false, MaterialIndex.refMatOpagueDustLargeDirectional ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexSmokePuffSmallDisplacement, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatOpagueDustLarge];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatOpagueDustLarge ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexSmokePuffSmallNormal, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatOpagueDustLarge];
//                //return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                return GetNormalMap( m );
//            }, false, MaterialIndex.refMatOpagueDustLarge ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexLightning3Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatGenericLaser];
//                //return GetMainTex( m );
//                return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatGenericLaser ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexShard02Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatEngiShieldShards];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatEngiShieldShards ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexOmniExplosion2Mask, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatOmniExplosion1];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatOmniExplosion1 ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexRoboChunksDiffuse, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatRoboChunks];
//                return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                //return GetNormalMap( m );
//            }, false, MaterialIndex.refMatRoboChunks ).RegisterAccessor();


//            new GenericAccessor<Texture>( TextureIndex.refTexRoboChunksNormal, () =>
//            {
//                var m = AssetLibrary<Material>.i[MaterialIndex.refMatRoboChunks];
//                //return GetMainTex( m );
//                //return GetCloud1Tex( m );
//                //return GetCloud2Tex( m );
//                //return GetBumpMap( m );
//                return GetNormalMap( m );
//            }, false, MaterialIndex.refMatRoboChunks ).RegisterAccessor();

//            #endregion
//            #endregion
//        }


//        private static Texture GetMainTex( Material mat )
//        {
//            return mat.GetTexture( "_MainTex" );
//        }
//        private static Texture GetCloud1Tex( Material mat )
//        {
//            return mat.GetTexture( "_Cloud1Tex" );
//        }
//        private static Texture GetCloud2Tex( Material mat )
//        {
//            return mat.GetTexture( "_Cloud2Tex" );
//        }
//        private static Texture GetBumpMap( Material mat )
//        {
//            return mat.GetTexture( "_BumpMap" );
//        }
//        private static Texture GetNormalMap( Material mat )
//        {
//            return mat.GetTexture( "_NormalTex" );
//        }

//    }
//}


///*
//BoostJumpEffect/
//	TexFeatherMask                              //100%      Mat
//procstealthkit/
//	texglowsquaremask                           //100%
//roboballbossdelayknockupeffect/
//	texshard03mask                              //100%
//firePillarEffect/
//	texFireMask                                 //100%
//lightningstakenova/
//	texomnishockwave2mask                       //100%
//mercswordfinisherslash/
//	texmercswipemask                            //100%
//mageunlockpreexplosion/
//	texchaintrailmask                           //100%	
//teleporterbeaconeffect/
//	texvagranttentaclemask                      //100%
//sprintactivate/
//	texbehemothtilenormal                       //100%
//lemurianbitetrail/
//	texLemurianSlash                            //100%
//hippoRezEffect/
//	texAngelWingMask                            //100%
//explosionDroneDeath/
//	texVFXExplosionMask                         //100%
//waterfootstep/
//	texshockwavering2mask                       //100%
//BootIsReady/
//	texItemPickupEffectParticleMask             //100%
//explosionEngiTurretDeath/
//	texEngiTrail                                //100%
//	texGlowPaintmask                            //100%	
//teleportoutboom/
//	texalphagradient3mask                       //100%
//	texalphagradient2mask	                    //100%
//fruitHealEffect
//	texHealingCrossMask                         //100%
//	texFluidSpraySmall                          //100%
//impbossdeatheffect
//	texImpSwipeMask                             //100%
//	texCloudWaterRipples                        //100%
//poisonNovaProc
//	TexCloudSkulls                              //100%
//	texglowskullmask                            //100%
//sonicboomeffect
//	texfluffycloud2mask                         //100%
//	texfluffycloud3mask                         //100%
//mageLightningbombExplosion
//	texMageMatrixMaskDirectional                //100%
//	texMageMatrixMask                           //100%
//mageflamethrowereffect
//	texmagematrixtri                            //100%
//cleanseeffect
//	texShockwaveMask                            //100%
//	texBlackHoleMask                            //100%
//	texCloudOrganic3                            //100%
//laserTurbineBombExplosion
//	texAlphaGradient4Mask                       //100%
//	texOmniRadialSlash1Mask                     //100%
//	texOmniHitspark2Mask                        //100%
//gravekeeperMaskDeath
//	texOmniHitspark1Mask                        //100%
//	texSmokePuffSmallDirectionalDisplacement    //100%
//	texSmokePuffSmallDirectionalNormal          //100%
//levelupeffect
//	texOmniShockwave1Mask                       //100%
//	texomnihitspark4mask                        //100%
//clayBossMulcher
//	texParticleDust3Mask                        //100%
//	texWhiteRadialGradient256                   //100%
//	texFluidSprayLarge                          //100%
//electricwormburrow
//	texSmokePuffDirectionalDisplacement         //100%
//	texSmokePuffDirectionalNormal               //100%  
//	texSmokePuffSmallDisplacement               //100%
//	texSmokePuffSmallNormal                     //100%
//AmmoPackPickupEffect
//	TexLightning3Mask                           //100%	
//bubbleShieldendEffect
//	texShard02Mask                              //100%
//	texOmniExplosion2Mask                       //100%
//	texRoboChunksDiffuse                        //100%
//	texRoboChunksNormal                         //100%
//    */
