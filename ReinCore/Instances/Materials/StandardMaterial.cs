namespace ReinCore
{
    using System;

    using UnityEngine;

    /// <summary>
    /// A material for normal objects in game like enemies.
    /// </summary>
    [Serializable]
    public class StandardMaterial : MaterialBase
    {
        [Menu( sectionName = "Main" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean cutout
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_EnableCutout" ) != 0f;
            set => base.SetSingle( "_EnableCutout", value ? 1.0f : 0.0f );
        }

        [Menu( sectionName = "Main" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Color mainColor
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetColor( "_Color" );
            set => base.SetColor( "_Color", value );
        }

        [Menu( sectionName = "Main" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ScaleOffsetTextureData mainTexture
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
        private ScaleOffsetTextureData _mainTexture;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Main" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single normalStrength
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_NormalStrength" );
            set => base.SetSingle( "_NormalStrength", value );
        }

        [Menu( sectionName = "Main" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ScaleOffsetTextureData normalMap
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
        private ScaleOffsetTextureData _normalMap;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Main" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Color emissionColor
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetColor( "_EmColor" );
            set => base.SetColor( "_EmColor", value );
        }

        [Menu( sectionName = "Main" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TextureData emissionTexture
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
        private TextureData _emissionTexture;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Main" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single emissionPower
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_EmPower" );
            set => base.SetSingle( "_EmPower", value );
        }

        [Menu( sectionName = "Main" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single smoothness
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_Smoothness" );
            set => base.SetSingle( "_Smoothness", value );
        }

        [Menu( sectionName = "Main" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean ignoreDiffuseAlphaForSpecular
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetKeyword( "FORCE_SPEC" );
            set => base.SetKeyword( "FORCE_SPEC", value );
        }

        [Menu( sectionName = "Main" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public RampInfo rampChoice
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => (RampInfo)base.GetSingle( "_RampInfo" );
            set => base.SetSingle( "_RampInfo", (Single)value );
        }

        [Menu( sectionName = "Main" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public DecalLayer decalLayer
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => (DecalLayer)base.GetSingle( "_DecalLayer" );
            set => base.SetSingle( "_DecalLayer", (Single)value );
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public enum DecalLayer
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            Default = 0,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            Environment = 1,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            Character = 2,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            Misc = 3,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }

        [Menu( sectionName = "Main" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single specularStrength
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_SpecularStrength" );
            set => base.SetSingle( "_SpecularStrength", value );
        }

        [Menu( sectionName = "Main" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single specularExponent
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_SpecularExponent" );
            set => base.SetSingle( "_SpecularExponent", value );
        }

        [Menu( sectionName = "Main" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public CullMode cull
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => (CullMode)base.GetSingle( "_Cull" );
            set => base.SetSingle( "_Cull", (Single)value );
        }

        [Menu( sectionName = "Dithering" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean dither
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetKeyword( "DITHER" );
            set => base.SetKeyword( "DITHER", value );
        }

        [Menu( sectionName = "Dithering" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single fadeBias
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_FadeBias" );
            set => base.SetSingle( "_FadeBias", value );
        }

        [Menu( sectionName = "Fresnel Emission" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean fresnelEmission
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetKeyword( "FRESNEL_EMISSION" );
            set => base.SetKeyword( "FRESNEL_EMISSION", value );
        }

        [Menu( sectionName = "Fresnel Emission", isRampTexture = true )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TextureData fresnelRamp
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
        private TextureData _fresnelRamp;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Fresnel Emission" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single fresnelPower
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_FresnelPower" );
            set => base.SetSingle( "_FresnelPower", value );
        }

        [Menu( sectionName = "Fresnel Emission" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TextureData fresnelMask
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
        private TextureData _fresnelMask;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Fresnel Emission" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single fresnelBoost
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_FresnelBoost" );
            set => base.SetSingle( "_FresnelBoost", value );
        }

        [Menu( sectionName = "Printing" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean printingEnabled
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetKeyword( "PRINT_CUTOFF" );
            set => base.SetKeyword( "PRINT_CUTOFF", value );
        }

        [Menu( sectionName = "Printing" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single sliceHeight
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_SliceHeight" );
            set => base.SetSingle( "_SliceHeight", value );
        }

        [Menu( sectionName = "Printing" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single printBandHeight
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_SliceHeight" );
            set => base.SetSingle( "_SliceHeight", value );
        }

        [Menu( sectionName = "Printing" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single printAlphaDepth
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_SliceAlphaDepth" );
            set => base.SetSingle( "_SliceAlphaDepth", value );
        }

        [Menu( sectionName = "Printing" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ScaleOffsetTextureData printAlphaTexture
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
        private ScaleOffsetTextureData _printAlphaTexture;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Printing" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single printColorBoost
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_PrintBoost" );
            set => base.SetSingle( "_PrintBoost", value );
        }

        [Menu( sectionName = "Printing" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single printAlphaBias
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_PrintBias" );
            set => base.SetSingle( "_PrintBias", value );
        }

        [Menu( sectionName = "Printing" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single printEmissionToAlbedoLerp
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_PrintEmissionToAlbedoLerp" );
            set => base.SetSingle( "_PrintEmissionToAlbedoLerp", value );
        }

        [Menu( sectionName = "Printing" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public PrintDirection printDirection
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => (PrintDirection)base.GetSingle( "_PrintDirection" );
            set => base.SetSingle( "_PrintDirection", (Single)value );
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public enum PrintDirection
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            BottomUp = 0,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            TopDown = 1,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            BackToFront = 3,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }

        [Menu( sectionName = "Printing", isRampTexture = true )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TextureData printRampTexture
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
        private TextureData _printRampTexture;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Printing" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public RoR2.EliteIndex eliteIndex
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => (RoR2.EliteIndex)( base.GetSingle( "_EliteIndex" ) - 1 );
            set => base.SetSingle( "_EliteIndex", ( (Single)value ) + 1 );
        }

        [Menu( sectionName = "Printing" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single eliteBrightnessMin
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_EliteBrightnessMin" );
            set => base.SetSingle( "_EliteBrightnessMin", value );
        }

        [Menu( sectionName = "Printing" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single eliteBrightnessMax
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_EliteBrightnessMax" );
            set => base.SetSingle( "_EliteBrightnessMax", value );
        }

        [Menu( sectionName = "Splatmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean splatmapEnabled
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetKeyword( "SPLATMAP" );
            set => base.SetKeyword( "SPLATMAP", value );
        }

        [Menu( sectionName = "Splatmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean useVertexColors
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetKeyword( "USE_VERTEX_COLORS" );
            set => base.SetKeyword( "USE_VERTEX_COLORS", value );
        }

        [Menu( sectionName = "Splatmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single blendDepth
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_Depth" );
            set => base.SetSingle( "_Depth", value );
        }

        [Menu( sectionName = "Splatmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ScaleOffsetTextureData splatmapTexture
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
        private ScaleOffsetTextureData _splatmapTexture;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Splatmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single splatmapTileScale
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_SplatmapTileScale" );
            set => base.SetSingle( "_SplatmapTileScale", value );
        }

        [Menu( sectionName = "Splatmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TextureData greenChannelTexture
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
        private TextureData _greenChannelTexture;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Splatmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TextureData greenChannelNormalmap
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
        private TextureData _greenChannelNormalmap;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Splatmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single greenChannelSmoothness
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_GreenChannelSmoothness" );
            set => base.SetSingle( "_GreenChannelSmoothness", value );
        }

        [Menu( sectionName = "Splatmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single greenChannelBias
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_GreenChannelBias" );
            set => base.SetSingle( "_GreenChannelBias", value );
        }

        [Menu( sectionName = "Splatmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TextureData blueChannelTexture
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
        private TextureData _blueChannelTexture;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Splatmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TextureData blueChannelNormalmap
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
        private TextureData _blueChannelNormalmap;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Splatmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single blueChannelSmoothness
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_BlueChannelSmoothness" );
            set => base.SetSingle( "_BlueChannelSmoothness", value );
        }

        [Menu( sectionName = "Splatmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single blueChannelBias
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_BlueChannelBias" );
            set => base.SetSingle( "_BlueChannelBias", value );
        }

        [Menu( sectionName = "Flowmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean flowmapEnabled
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetKeyword( "FLOWMAP" );
            set => base.SetKeyword( "FLOWMAP", value );
        }

        [Menu( sectionName = "Flowmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TextureData flowmapTexture
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
        private TextureData _flowmapTexture;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Flowmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ScaleOffsetTextureData flowmapHeightmap
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
        private ScaleOffsetTextureData _flowmapHeightmap;
#pragma warning restore IDE1006 // Naming Styles


        /// <summary>
        /// Unknown
        /// </summary>
        [Menu( sectionName = "Flowmap", isRampTexture = true )]
        public ScaleOffsetTextureData flowmapHeightRamp
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
        [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
        private ScaleOffsetTextureData _flowmapHeightRamp;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Flowmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single flowHeightBias
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_FlowHeightBias" );
            set => base.SetSingle( "_FlowHeightBias", value );
        }

        [Menu( sectionName = "Flowmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single flowHeightPower
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_FlowHeightPower" );
            set => base.SetSingle( "_FlowHeightPower", value );
        }

        [Menu( sectionName = "Flowmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single flowHeightEmissionStrength
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_FlowEmissionStrength" );
            set => base.SetSingle( "_FlowEmissionStrength", value );
        }

        [Menu( sectionName = "Flowmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single flowSpeed
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_FlowSpeed" );
            set => base.SetSingle( "_FlowSpeed", value );
        }

        [Menu( sectionName = "Flowmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single flowMaskStrength
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_FlowMaskStrength" );
            set => base.SetSingle( "_FlowMaskStrength", value );
        }

        [Menu( sectionName = "Flowmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single flowNormalStrength
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_FlowNormalStrength" );
            set => base.SetSingle( "_FlowNormalStrength", value );
        }

        [Menu( sectionName = "Flowmap" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single flowTextureScaleFactor
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_FlowTextureScaleFactor" );
            set => base.SetSingle( "_FlowTextureScaleFactor", value );
        }

        [Menu( sectionName = "Limb" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean limbRemovalEnabled
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetKeyword( "LIMBREMOVAL" );
            set => base.SetKeyword( "LIMBREMOVAL", value );
        }

        [Menu( sectionName = "Limb" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single limbPrimeMask
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_LimbPrimeMask" );
            set => base.SetSingle( "_LimbPrimeMask", value );
        }

        [Menu( sectionName = "Limb" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Color flashColor
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetColor( "_FlashColor" );
            set => base.SetColor( "_FlashColor", value );
        }

        /*
        [Menu( sectionName = "Limb" )]
        /// <summary>
        /// Unknown
        /// </summary>
        public Color fadeColor
        {
            get => base.GetColor( "_Fade" );
            set => base.SetColor( "_Fade", value );
        }
        */

        /// <summary>
        /// Creates a standard material.
        /// </summary>
        /// <param name="name">The name of the material</param>
        public StandardMaterial( String name ) : base( name, ShaderIndex.HGStandard )
        {

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public StandardMaterial( Material mat ) : base( mat )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {

        }


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public StandardMaterial() : base() { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    }

}
