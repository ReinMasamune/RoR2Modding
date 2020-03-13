using System;
using System.Collections.Generic;
using System.Text;

namespace ReinCore
{
    public enum ShaderIndex : UInt64
    {
        #region Hopoo Deferred
        HGStandard,
        HGSnowTopped,
        HGTriPlanarTerrainBlend,
        HGWavyCloth,
        #endregion
        #region Hopoo Environment
        HGDistantWater,
        HGGrass,
        HGWaterfall,
        #endregion
        #region Hopoo FX
        HGCloudRemap,
        HGDamageNumber,
        HGDistortion,
        HGIntersectionCloudRemap,
        HGOpaqueCloudRemap,
        HGSolidParallax,
        #endregion
        #region Hopoo PostProcess
        HGOutlineHighlight,
        HGScopeShader,
        HGScreenDamage,
        HGSobelBuffer,
        #endregion
        #region Hopoo UI
        HGUIAnimateAlpha,
        HGUIBarRemap,
        HGUIBlur,
        HGUIIgnoreZ,
        HGUIOverBrighten,
        #endregion
    }
}
