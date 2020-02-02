using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    /// <summary>
    /// A material for normal objects in game like enemies.
    /// </summary>
    internal class StandardMaterial : MaterialBase
    {
        #region Textures
        /// <summary>
        /// Unknown and unimplemented
        /// </summary>
        internal Texture blueNormalTexture { get; set; }

        /// <summary>
        /// Unknown and unimplemented
        /// </summary>
        internal Texture blueTexture { get; set; }

        /// <summary>
        /// Unknown and unimplemented
        /// </summary>
        internal Texture greenNormalTexture { get; set; }

        /// <summary>
        /// Unknown and unimplemented
        /// </summary>
        internal Texture greenTexture { get; set; }

        /// <summary>
        /// Unknown and unimplemented
        /// </summary>
        internal Texture emTexture { get; set; }

        /// <summary>
        /// A ramp texture for the flowmap
        /// Unimplemented
        /// </summary>
        internal Texture flowHeightRamp { get; set; }

        /// <summary>
        /// A heightmap texture for the flowmap
        /// Unimplemented
        /// </summary>
        internal Texture flowHeightMap { get; set; }

        /// <summary>
        /// A texture for the flowmap
        /// Unimplemented
        /// </summary>
        internal Texture flowTexture { get; set; }

        /// <summary>
        /// Unknown and unimplemented
        /// </summary>
        internal Texture fresnelRamp { get; set; }

        /// <summary>
        /// Unknown and unimplemented
        /// </summary>
        internal Texture mainTexture { get; set; }

        /// <summary>
        /// Unknown and unimplemented
        /// </summary>
        internal Texture normalTexture { get; set; }

        /// <summary>
        /// Unknown and unimplemented
        /// </summary>
        internal Texture printRamp { get; set; }

        /// <summary>
        /// unknownn and unimplemented
        /// </summary>
        internal Texture sliceAlphaTexture { get; set; }

        /// <summary>
        /// Unknown and unimplemented
        /// </summary>
        internal Texture splatMapTexture { get; set; }
        #endregion
        #region Values
        internal Single blueChannelBias { get; set; }
        internal Single blueChannelSmoothness { get; set; }
        internal Single bumpScale { get; set; }
        internal Single colorsOn { get; set; }
        internal Single cull { get; set; }
        internal Single cutoff { get; set; }
        internal Single decalLayer { get; set; }
        internal Single depth { get; set; }
        internal Single detailNormalmapScale { get; set; }
        internal Single ditherOn { get; set; }
        internal Single dstBlend { get; set; }
        internal Single eliteBrightnessMax { get; set; }
        internal Single eliteBrightnessMin { get; set; }
        internal Single eliteIndex { get; set; }
        internal Single emPower { get; set; }
        internal Single enableCutout { get; set; }
        internal Single feon { get; set; }
        internal Single fade { get; set; }
        internal Single fadeBias { get; set; }
        internal Single flowDiffuseStrength { get; set; }
        internal Single flowEmissionStrength { get; set; }
        internal Single flowHeightBias { get; set; }
        internal Single flowHeightPower { get; set; }
        internal Single flowMaskStrength { get; set; }
        internal Single flowNormalStrength { get; set; }
        internal Single flowSpeed { get; set; }
        internal Single flowTextureScaleFactor { get; set; }
        internal Single flowmapOn { get; set; }
        internal Single forceSpecOn { get; set; }
        internal Single fresnelBoost { get; set; }
        internal Single fresnelPower { get; set; }
        internal Single glossmapScale { get; set; }
        internal Single glossiness { get; set; }
        internal Single glossyReflection { get; set; }
        internal Single greenChannelBias { get; set; }
        internal Single greenChannelSmoothness { get; set; }
        internal Single limbPrimeMask { get; set; }
        internal Single limbRemovalOn { get; set; }
        internal Single metallic { get; set; }
        internal Single mode { get; set; }
        internal Single normalStrength { get; set; }
        internal Single occlusionStrength { get; set; }
        internal Single parallax { get; set; }
        internal Single printBias { get; set; }
        internal Single printBoost { get; set; }
        internal Single printDirection { get; set; }
        internal Single printEmissionToAlbedoLerp { get; set; }
        internal Single printOn { get; set; }
        internal Single rampInfo { get; set; }
        internal Single sliceAlphaDepth { get; set; }
        internal Single sliceBandHeight { get; set; }
        internal Single sliceHeight { get; set; }
        internal Single smoothness { get; set; }
        internal Single smoothnessTextureChannel { get; set; }
        internal Single specularExponent { get; set; }
        internal Single specularHighlights { get; set; }
        internal Single specularStrength { get; set; }
        internal Single splatmapOn { get; set; }
        internal Single splatmapTileScale { get; set; }
        internal Single srcBlend { get; set; }
        internal Single uvSec { get; set; }
        internal Single zWrite { get; set; }
        #endregion
        #region Colors
        internal Color color { get; set; }
        internal Color emColor { get; set; }
        internal Color emissionColor { get; set; }
        internal Color flashColor { get; set; }
        #endregion

        /// <summary>
        /// Creates a standard material.
        /// </summary>
        /// <param name="name">The name of the material</param>
        internal StandardMaterial( String name ) : base( name, ShaderIndex.HGStandard )
        {

        }
    }

}
