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
        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean cutout
        {
            get => base.GetSingle( "_EnableCutout" ) != 0f;
            set => base.SetSingle( "_EnableCutout", (value ? 1.0f : 0.0f) );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Color mainColor
        {
            get => base.GetColor( "_Color" );
            set => base.SetColor( "_Color", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal ScaleOffsetTextureData mainTexture
        {
            get
            {
                if( this._mainTexture == null )
                {
                    this._mainTexture = new ScaleOffsetTextureData( base.material, "_MainTex" );
                }
                return this._mainTexture;
            }
        }
        private ScaleOffsetTextureData _mainTexture;

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single normalStrength
        {
            get => base.GetSingle( "_NormalStrength" );
            set => base.SetSingle( "_NormalStrength", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal ScaleOffsetTextureData normalMap
        {
            get
            {
                if( this._normalMap == null )
                {
                    this._normalMap = new ScaleOffsetTextureData( base.material, "_NormalTex" );
                }
                return this._normalMap;
            }
        }
        private ScaleOffsetTextureData _normalMap;

        /// <summary>
        /// Unknown
        /// </summary>
        internal Color emissionColor
        {
            get => base.GetColor( "_EmColor" );
            set => base.SetColor( "_EmColor", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal TextureData emissionTexture
        {
            get
            {
                if( this._emissionTexture == null )
                {
                    this._emissionTexture = new TextureData( base.material, "_EmTex" );
                }
                return this._emissionTexture;
            }
        }
        private TextureData _emissionTexture;

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single emissionPower
        {
            get => base.GetSingle( "_EmPower" );
            set => base.SetSingle( "_EmPower", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single smoothness
        {
            get => base.GetSingle( "_Smoothness" );
            set => base.SetSingle( "_Smoothness", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean ignoreDiffuseAlphaForSpecular
        {
            get => base.GetKeyword( "FORCE_SPEC" );
            set => base.SetKeyword( "FORCE_SPEC", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal RampInfo rampChoice
        {
            get => (RampInfo)base.GetSingle( "_RampInfo" );
            set => base.SetSingle( "_RampInfo", (Single)value );
        }
        internal enum RampInfo
        {
            TwoTone = 0,
            SmoothedTwoTone = 1,
            Unlitish = 3,
            SubSurface = 4,
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal DecalLayer decalLayer
        {
            get => (DecalLayer)base.GetSingle( "_DecalLayer" );
            set => base.SetSingle( "_DecalLayer", (Single)value );
        }
        internal enum DecalLayer
        {
            Default = 0,
            Environment = 1,
            Character = 2,
            Misc = 3,
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single specularStrength
        {
            get => base.GetSingle( "_SpecularStrength" );
            set => base.SetSingle( "_SpecularStrength", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single specularExponent
        {
            get => base.GetSingle( "_SpecularExponent" );
            set => base.SetSingle( "_SpecularExponent", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal CullMode cull
        {
            get => (CullMode)base.GetSingle( "_Cull" );
            set => base.SetSingle( "_Cull", (Single)value );
        }
        internal enum CullMode
        {
            Off = 0,
            Front = 1,
            Back = 2
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean dither
        {
            get => base.GetKeyword( "DITHER" );
            set => base.SetKeyword( "DITHER", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single fadeBias
        {
            get => base.GetSingle( "_FadeBias" );
            set => base.SetSingle( "_FadeBias", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean fresnelEmission
        {
            get => base.GetKeyword( "FRESNEL_EMISSION" );
            set => base.SetKeyword( "FRESNEL_EMISSION", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal TextureData fresnelRamp
        {
            get
            {
                if( this._fresnelRamp == null )
                {
                    this._fresnelRamp = new TextureData( base.material, "_FresnelRamp" );
                }
                return this._fresnelRamp;
            }
        }
        private TextureData _fresnelRamp;

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single fresnelPower
        {
            get => base.GetSingle( "_FresnelPower" );
            set => base.SetSingle( "_FresnelPower", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal TextureData fresnelMask
        {
            get
            {
                if( this._fresnelMask == null )
                {
                    this._fresnelMask = new TextureData( base.material, "_FresnelMask" );
                }
                return this._fresnelMask;
            }
        }
        private TextureData _fresnelMask;

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single fresnelBoost
        {
            get => base.GetSingle( "_FresnelBoost" );
            set => base.SetSingle( "_FresnelBoost", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean printingEnabled
        {
            get => base.GetKeyword( "PRINT_CUTOFF" );
            set => base.SetKeyword( "PRINT_CUTOFF", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single sliceHeight
        {
            get => base.GetSingle( "_SliceHeight" );
            set => base.SetSingle( "_SliceHeight", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single printBandHeight
        {
            get => base.GetSingle( "_SliceHeight" );
            set => base.SetSingle( "_SliceHeight", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single printAlphaDepth
        {
            get => base.GetSingle( "_SliceAlphaDepth" );
            set => base.SetSingle( "_SliceAlphaDepth", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal ScaleOffsetTextureData printAlphaTexture
        {
            get
            {
                if( this._printAlphaTexture == null )
                {
                    this._printAlphaTexture = new ScaleOffsetTextureData( base.material, "_SliceAlphaTex" );
                }
                return this._printAlphaTexture;
            }
        }
        private ScaleOffsetTextureData _printAlphaTexture;

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single printColorBoost
        {
            get => base.GetSingle( "_PrintBoost" );
            set => base.SetSingle( "_PrintBoost", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single printAlphaBias
        {
            get => base.GetSingle( "_PrintBias" );
            set => base.SetSingle( "_PrintBias", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single printEmissionToAlbedoLerp
        {
            get => base.GetSingle( "_PrintEmissionToAlbedoLerp" );
            set => base.SetSingle( "_PrintEmissionToAlbedoLerp", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal PrintDirection printDirection
        {
            get => (PrintDirection)base.GetSingle( "_PrintDirection" );
            set => base.SetSingle( "_PrintDirection", (Single)value );
        }
        internal enum PrintDirection
        {
            BottomUp = 0,
            TopDown = 1,
            BackToFront = 3,
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal TextureData printRampTexture
        {
            get
            {
                if( this._printRampTexture == null )
                {
                    this._printRampTexture = new TextureData( base.material, "_PrintRamp" );
                }
                return this._printRampTexture;
            }
        }
        private TextureData _printRampTexture;

        /// <summary>
        /// Unknown
        /// </summary>
        internal RoR2.EliteIndex eliteIndex
        {
            get => (RoR2.EliteIndex)base.GetSingle( "_EliteIndex" );
            set => base.SetSingle( "_EliteIndex", (Single)value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single eliteBrightnessMin
        {
            get => base.GetSingle( "_EliteBrightnessMin" );
            set => base.SetSingle( "_EliteBrightnessMin", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single eliteBrightnessMax
        {
            get => base.GetSingle( "_EliteBrightnessMax" );
            set => base.SetSingle( "_EliteBrightnessMax", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean splatmapEnabled
        {
            get => base.GetKeyword( "SPLATMAP" );
            set => base.SetKeyword( "SPLATMAP", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean useVertexColors
        {
            get => base.GetKeyword( "USE_VERTEX_COLORS" );
            set => base.SetKeyword( "USE_VERTEX_COLORS", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single blendDepth
        {
            get => base.GetSingle( "_Depth" );
            set => base.SetSingle( "_Depth", value );
        }
        
        /// <summary>
        /// Unknown
        /// </summary>
        internal ScaleOffsetTextureData splatmapTexture
        {
            get
            {
                if( this._splatmapTexture == null )
                {
                    this._splatmapTexture = new ScaleOffsetTextureData( base.material, "_SplatmapTex" );
                }
                return this._splatmapTexture;
            }
        }
        private ScaleOffsetTextureData _splatmapTexture;

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single splatmapTileScale
        {
            get => base.GetSingle( "_SplatmapTileScale" );
            set => base.SetSingle( "_SplatmapTileScale", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal TextureData greenChannelTexture
        {
            get
            {
                if( this._greenChannelTexture == null )
                {
                    this._greenChannelTexture = new TextureData( base.material, "_GreenChannelTex" );
                }
                return this._greenChannelTexture;
            }
        }
        private TextureData _greenChannelTexture;

        /// <summary>
        /// Unknown
        /// </summary>
        internal TextureData greenChannelNormalmap
        {
            get
            {
                if( this._greenChannelNormalmap == null )
                {
                    this._greenChannelNormalmap = new TextureData( base.material, "_GreenChannelNormalTex" );
                }
                return this._greenChannelNormalmap;
            }
        }
        private TextureData _greenChannelNormalmap;

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single greenChannelSmoothness
        {
            get => base.GetSingle( "_GreenChannelSmoothness" );
            set => base.SetSingle( "_GreenChannelSmoothness", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single greenChannelBias
        {
            get => base.GetSingle( "_GreenChannelBias" );
            set => base.SetSingle( "_GreenChannelBias", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal TextureData blueChannelTexture
        {
            get
            {
                if( this._blueChannelTexture == null )
                {
                    this._blueChannelTexture = new TextureData( base.material, "_BlueChannelTex" );
                }
                return this._blueChannelTexture;
            }
        }
        private TextureData _blueChannelTexture;

        /// <summary>
        /// Unknown
        /// </summary>
        internal TextureData blueChannelNormalmap
        {
            get
            {
                if( this._blueChannelNormalmap == null )
                {
                    this._blueChannelNormalmap = new TextureData( base.material, "_BlueChannelNormalTex" );
                }
                return this._blueChannelNormalmap;
            }
        }
        private TextureData _blueChannelNormalmap;

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single blueChannelSmoothness
        {
            get => base.GetSingle( "_BlueChannelSmoothness" );
            set => base.SetSingle( "_BlueChannelSmoothness", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single blueChannelBias
        {
            get => base.GetSingle( "_BlueChannelBias" );
            set => base.SetSingle( "_BlueChannelBias", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean flowmapEnabled
        {
            get => base.GetKeyword( "FLOWMAP" );
            set => base.SetKeyword( "FLOWMAP", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal TextureData flowmapTexture
        {
            get
            {
                if( this._flowmapTexture == null )
                {
                    this._flowmapTexture = new TextureData( base.material, "_FlowTex" );
                }
                return this._flowmapTexture;
            }
        }
        private TextureData _flowmapTexture;

        /// <summary>
        /// Unknown
        /// </summary>
        internal ScaleOffsetTextureData flowmapHeightmap
        {
            get
            {
                if( this._flowmapHeightmap == null )
                {
                    this._flowmapHeightmap = new ScaleOffsetTextureData( base.material, "_FlowHeightmap" );
                }
                return this._flowmapHeightmap;
            }
        }
        private ScaleOffsetTextureData _flowmapHeightmap;

        /// <summary>
        /// Unknown
        /// </summary>
        internal ScaleOffsetTextureData flowmapHeightRamp
        {
            get
            {
                if( this._flowmapHeightRamp == null )
                {
                    this._flowmapHeightRamp = new ScaleOffsetTextureData( base.material, "_FlowHeightRamp" );
                }
                return this._flowmapHeightRamp;
            }
        }
        private ScaleOffsetTextureData _flowmapHeightRamp;

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single flowHeightBias
        {
            get => base.GetSingle( "_FlowHeightBias" );
            set => base.SetSingle( "_FlowHeightBias", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single flowHeightPower
        {
            get => base.GetSingle( "_FlowHeightPower" );
            set => base.SetSingle( "_FlowHeightPower", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single flowHeightEmissionStrength
        {
            get => base.GetSingle( "_FlowEmissionStrength" );
            set => base.SetSingle( "_FlowEmissionStrength", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single flowSpeed
        {
            get => base.GetSingle( "_FlowSpeed" );
            set => base.SetSingle( "_FlowSpeed", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single flowMaskStrength
        {
            get => base.GetSingle( "_FlowMaskStrength" );
            set => base.SetSingle( "_FlowMaskStrength", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single flowNormalStrength
        {
            get => base.GetSingle( "_FlowNormalStrength" );
            set => base.SetSingle( "_FlowNormalStrength", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single flowTextureScaleFactor
        {
            get => base.GetSingle( "_FlowTextureScaleFactor" );
            set => base.SetSingle( "_FlowTextureScaleFactor", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean limbRemovalEnabled
        {
            get => base.GetKeyword( "LIMBREMOVAL" );
            set => base.SetKeyword( "LIMBREMOVAL", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single limbPrimeMask
        {
            get => base.GetSingle( "_LimbPrimeMask" );
            set => base.SetSingle( "_LimbPrimeMask", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Color flashColor
        {
            get => base.GetColor( "_FlashColor" );
            set => base.SetColor( "_FlashColor", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Color fadeColor
        {
            get => base.GetColor( "_Fade" );
            set => base.SetColor( "_Fade", value );
        }


        /// <summary>
        /// Creates a standard material.
        /// </summary>
        /// <param name="name">The name of the material</param>
        internal StandardMaterial( String name ) : base( name, ShaderIndex.HGStandard )
        {

        }

        internal StandardMaterial( Material mat ) : base( mat )
        {

        }
    }

}
