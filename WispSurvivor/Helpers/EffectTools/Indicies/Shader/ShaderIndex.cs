using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal enum ShaderIndex : ulong
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
        HGOpagueCloudRemap,
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
