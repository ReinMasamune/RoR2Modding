namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public enum ShaderIndex : UInt64
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

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

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
