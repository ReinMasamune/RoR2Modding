using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal enum TextureIndex : ulong
    {
        None = 0,
        #region Reference Textures
        refTexCloudDifferenceBW1,
        refTexCloudDifferenceBW2,
        refTexCloudIce,
        refTexCloudCrackedIce,
        refTexCloudOrganic1,
        refTexCloudOrganic2,
        refTexCloudPixel2,
        refTexCloudLightning1,
        refTexMagmaCloud,
        refTexCloudColor1,


        refTexBasicMask,
        refTexBehemothTileMask,
        refTexGalaxy1Mask,
        refTexParticleDust1Mask,
        refTexParticleDust2Mask,
        refTexLightning2Mask,
        refTexGenericStarburstMaskSkewed,
        refTexGlowSoftCenterMask,
        refTexRingSetRuneMask,
        refTexBanditExplosionMask,
        refTexArcaneCircle1Mask,


        refTexAlphaGradient2,
        refTexEngiShield,
        refTexEngiShieldBlurred,
        refTexDebris1,
        refTexSmokePuffSPeckledDisplacement,
        refTexNullifierSky,
        refTexNullifierSky2,
        refTexWillowispSpiral,
        refCaustics,
        refDistTex,


        refWaves_N,
        refTexNormalSphere,
        refTexNormalSphereFaded,
        refTexCloudOrganicNormal,
        refTexSmokePuffSpeckledNormal,
        refTexNormalInvertConeFade,
        #endregion

    }
}