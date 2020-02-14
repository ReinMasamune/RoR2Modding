
using RogueWispPlugin.Helpers;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void CreateTextureAccessors()
        {
            new GenericAccessor<Texture>( TextureIndex.None, () =>
            {
                return null;
            }, false, ExecutionState.Constructor ).RegisterAccessor();

            #region Reference Textures
            #region Cloud Textures
            new GenericAccessor<Texture>( TextureIndex.refTexCloudDifferenceBW1, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatOnHelfire] );
            }, false, MaterialIndex.refMatOnHelfire ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexCloudDifferenceBW2, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatNullBombAreaIndicator] );
            }, false, MaterialIndex.refMatNullBombAreaIndicator ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexCloudIce, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatFireRingRunes] );
            }, false, MaterialIndex.refMatFireRingRunes ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexCloudCrackedIce, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatBazaarIceCore] );
            }, false, MaterialIndex.refMatBazaarIceCore ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexCloudOrganic1, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatElitePoisonParticleSystemReplacement] );
            }, false, MaterialIndex.refMatElitePoisonParticleSystemReplacement ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexCloudOrganic2, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatVagrantEnergized] );
            }, false, MaterialIndex.refMatVagrantEnergized ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexCloudPixel2, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatTPInOut] );
            }, false, MaterialIndex.refMatTPInOut ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexCloudLightning1, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatVagrantEnergized] );
            }, false, MaterialIndex.refMatVagrantEnergized ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexMagmaCloud, () =>
            {
                return GetCloud2Tex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierExplosionAreaIndicatorHard] );
            }, false, MaterialIndex.refMatNullifierExplosionAreaIndicatorHard ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexCloudColor1, () =>
            {
                return GetCloud2Tex( AssetLibrary<Material>.i[MaterialIndex.refMatNullBombAreaIndicator] );
            }, false, MaterialIndex.refMatNullBombAreaIndicator ).RegisterAccessor();


            #endregion
            #region Mask Textures
            new GenericAccessor<Texture>( TextureIndex.refTexBasicMask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatElitePoisonParticleSystemReplacement] );
            }, false, MaterialIndex.refMatElitePoisonParticleSystemReplacement ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexBehemothTileMask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierStarTrail] );
            }, false, MaterialIndex.refMatNullifierStarTrail ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexGalaxy1Mask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierStarParticle] );
            }, false, MaterialIndex.refMatNullifierStarParticle ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexParticleDust1Mask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatTracerBright] );
            }, false, MaterialIndex.refMatTracerBright ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexParticleDust2Mask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatGenericFlash] );
            }, false, MaterialIndex.refMatGenericFlash ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexLightning2Mask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierStarPortalEdge] );
            }, false, MaterialIndex.refMatNullifierStarPortalEdge ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexGenericStarburstMaskSkewed, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatWillowispRadial] );
            }, false, MaterialIndex.refMatWillowispRadial ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexGlowSoftCenterMask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatDustDirectionalDark] );
            }, false, MaterialIndex.refMatDustDirectionalDark ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexRingSetRuneMask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatFireRingRunes] );
            }, false, MaterialIndex.refMatFireRingRunes ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexBanditExplosionMask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatGolemExplosion] );
            }, false, MaterialIndex.refMatGolemExplosion ).RegisterAccessor();


            new GenericAccessor<Texture>( TextureIndex.refTexArcaneCircle1Mask, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatArcaneCircle1] );
            }, false, MaterialIndex.refMatArcaneCircle1 ).RegisterAccessor();


            #endregion
            #region Assorted Textures
            new GenericAccessor<Texture>( TextureIndex.refTexAlphaGradient2, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatShatteredGlass] );
            }, false, MaterialIndex.refMatShatteredGlass ).RegisterAccessor();



            new GenericAccessor<Texture>( TextureIndex.refTexEngiShield, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatMercEnergized] );
            }, false, MaterialIndex.refMatMercEnergized ).RegisterAccessor();



            new GenericAccessor<Texture>( TextureIndex.refTexEngiShieldBlurred, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatMercEnergized] );
            }, false, MaterialIndex.refMatMercEnergized ).RegisterAccessor();



            new GenericAccessor<Texture>( TextureIndex.refTexDebris1, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatDebris1] );
            }, false, MaterialIndex.refMatDebris1 ).RegisterAccessor();



            new GenericAccessor<Texture>( TextureIndex.refTexSmokePuffSPeckledDisplacement, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatOpagueDustSpeckledLarge] );
            }, false, MaterialIndex.refMatOpagueDustSpeckledLarge ).RegisterAccessor();



            new GenericAccessor<Texture>( TextureIndex.refTexNullifierSky, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierExplosionAreaIndicatorSoft] );
            }, false, MaterialIndex.refMatNullifierExplosionAreaIndicatorSoft ).RegisterAccessor();



            new GenericAccessor<Texture>( TextureIndex.refTexNullifierSky2, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierGemPortal] );
            }, false, MaterialIndex.refMatNullifierGemPortal ).RegisterAccessor();



            new GenericAccessor<Texture>( TextureIndex.refTexWillowispSpiral, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatWillowispSpiral] );
            }, false, MaterialIndex.refMatWillowispSpiral ).RegisterAccessor();



            new GenericAccessor<Texture>( TextureIndex.refCaustics, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierExplosionAreaIndicatorHard] );
            }, false, MaterialIndex.refMatNullifierExplosionAreaIndicatorHard ).RegisterAccessor();



            new GenericAccessor<Texture>( TextureIndex.refDistTex, () =>
            {
                return GetMainTex( AssetLibrary<Material>.i[MaterialIndex.refMatTracer] );
            }, false, MaterialIndex.refMatTracer ).RegisterAccessor();




            #endregion
            #region Normal Textures
            new GenericAccessor<Texture>( TextureIndex.refWaves_N, () =>
            {
                return GetBumpMap( AssetLibrary<Material>.i[MaterialIndex.refMatBazaarIceDistortion] );
            }, false, MaterialIndex.refMatBazaarIceDistortion ).RegisterAccessor();



            new GenericAccessor<Texture>( TextureIndex.refTexNormalSphere, () =>
            {
                return GetBumpMap( AssetLibrary<Material>.i[MaterialIndex.refMatDistortion] );
            }, false, MaterialIndex.refMatDistortion ).RegisterAccessor();



            new GenericAccessor<Texture>( TextureIndex.refTexNormalSphereFaded, () =>
            {
                return GetBumpMap( AssetLibrary<Material>.i[MaterialIndex.refMatDistortionFaded] );
            }, false, MaterialIndex.refMatDistortionFaded ).RegisterAccessor();



            new GenericAccessor<Texture>( TextureIndex.refTexCloudOrganicNormal, () =>
            {
                return GetCloud1Tex( AssetLibrary<Material>.i[MaterialIndex.refMatNullifierStarPortalEdge] );
            }, false, MaterialIndex.refMatNullifierStarPortalEdge ).RegisterAccessor();



            new GenericAccessor<Texture>( TextureIndex.refTexSmokePuffSpeckledNormal, () =>
            {
                return GetNormalMap( AssetLibrary<Material>.i[MaterialIndex.refMatOpagueDustSpeckledLarge] );
            }, false, MaterialIndex.refMatOpagueDustSpeckledLarge ).RegisterAccessor();



            new GenericAccessor<Texture>( TextureIndex.refTexNormalInvertConeFade, () =>
            {
                return GetBumpMap( AssetLibrary<Material>.i[MaterialIndex.refMatInverseDistortion] );
            }, false, MaterialIndex.refMatInverseDistortion ).RegisterAccessor();
            #endregion
            #endregion
        }


        private static Texture GetMainTex( Material mat )
        {
            return mat.GetTexture( "_MainTex" );
        }
        private static Texture GetCloud1Tex( Material mat )
        {
            return mat.GetTexture( "_Cloud1Tex" );
        }
        private static Texture GetCloud2Tex( Material mat )
        {
            return mat.GetTexture( "_Cloud2Tex" );
        }
        private static Texture GetBumpMap( Material mat )
        {
            return mat.GetTexture( "_BumpMap" );
        }
        private static Texture GetNormalMap( Material mat )
        {
            return mat.GetTexture( "_NormalTex" );
        }

    }
}


/*
BoostJumpEffect
	TexFeatherMask                              //100%
procstealthkit
	texglowsquaremask                           //100%
roboballbossdelayknockupeffect
	texshard03mask                              //100%
firePillarEffect
	texFireMask                                 //100%
lightningstakenova
	texomnishockwave2mask                       //100%
mercswordfinisherslash
	texmercswipemask                            //100%
mageunlockpreexplosion
	texchaintrailmask                           //100%	
teleporterbeaconeffect
	texvagranttentaclemask                      //100%
sprintactivate
	texbehemothtilenormal                       //100%
lemurianbitetrail
	texLemurianSlash                            //100%
hippoRezEffect
	texAngelWingMask                            //100%
explosionDroneDeath
	texVFXExplosionMask                         //100%
waterfootstep
	texshockwavering2mask                       //100%
BootIsReady
	texItemPickupEffectParticleMask             //100%
explosionEngiTurretDeath
	texEngiTrail                                //100%
	texGlowPaintmask                            //100%	
teleportoutboom
	texalphagradient3mask                       //100%
	texalphagradient2mask	                    //100%
fruitHealEffect
	texHealingCrossMask                         //100%
	texFluidSpraySmall                          //100%
impbossdeatheffect
	texImpSwipeMask                             //100%
	texCloudWaterRipples                        //100%
poisonNovaProc
	TexCloudSkulls                              //100%
	texglowskullmask                            //100%
sonicboomeffect
	texfluffycloud2mask                         //100%
	texfluffycloud3mask                         //100%
mageLightningbombExplosion
	texMageMatrixMaskDirectional                //100%
	texMageMatrixMask                           //100%
mageflamethrowereffect
	texmagematrixtri                            //100%
cleanseeffect
	texShockwaveMask                            //100%
	texBlackHoleMask                            //100%
	texCloudOrganic3                            //100%
laserTurbineBombExplosion
	texAlphaGradient4Mask                       //100%
	texOmniRadialSlash1Mask                     //100%
	texOmniHitspark2Mask                        //100%
gravekeeperMaskDeath
	texOmniHitspark1Mask                        //100%
	texSmokePuffSmallDirectionalDisplacement    //100%
	texSmokePuffSmallDirectionalNormal          //100%
levelupeffect
	texOmniShockwave1Mask                       //100%
	texomnihitspark4mask                        //100%
clayBossMulcher
	texParticleDust3Mask                        //100%
	texWhiteRadialGradient256                   //100%
	texFluidSprayLarge                          //100%
electricwormburrow
	texSmokePuffDirectionalDisplacement         //100%
	texSmokePuffDirectionalNormal               //100%  
	texSmokePuffSmallDisplacement               //100%
	texSmokePuffSmallNormal                     //100%
AmmoPackPickupEffect
	TexLightning3Mask                           //100%	
bubbleShieldendEffect
	texShard02Mask                              //100%
	texOmniExplosion2Mask                       //100%
	texRoboChunksDiffuse                        //100%
	texRoboChunksNormal                         //100%
    */
