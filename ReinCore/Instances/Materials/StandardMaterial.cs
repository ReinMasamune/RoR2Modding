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


        /// <summary>
        /// Unknown
        /// </summary>

        public Boolean cutout


        {
            get => base.GetSingle( "_EnableCutout" ) != 0f;
            set => base.SetSingle( "_EnableCutout", value ? 1.0f : 0.0f );
        }

        [Menu( sectionName = "Main" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Color mainColor


        {
            get => base.GetColor( "_Color" );
            set => base.SetColor( "_Color", value );
        }

        [Menu( sectionName = "Main" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public ScaleOffsetTextureData mainTexture


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

        private ScaleOffsetTextureData _mainTexture;


        [Menu( sectionName = "Main" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single normalStrength


        {
            get => base.GetSingle( "_NormalStrength" );
            set => base.SetSingle( "_NormalStrength", value );
        }

        [Menu( sectionName = "Main" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public ScaleOffsetTextureData normalMap


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

        private ScaleOffsetTextureData _normalMap;


        [Menu( sectionName = "Main" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Color emissionColor


        {
            get => base.GetColor( "_EmColor" );
            set => base.SetColor( "_EmColor", value );
        }

        [Menu( sectionName = "Main" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public TextureData emissionTexture


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

        private TextureData _emissionTexture;


        [Menu( sectionName = "Main" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single emissionPower


        {
            get => base.GetSingle( "_EmPower" );
            set => base.SetSingle( "_EmPower", value );
        }

        [Menu( sectionName = "Main" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single smoothness


        {
            get => base.GetSingle( "_Smoothness" );
            set => base.SetSingle( "_Smoothness", value );
        }

        [Menu( sectionName = "Main" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Boolean ignoreDiffuseAlphaForSpecular


        {
            get => base.GetKeyword( "FORCE_SPEC" );
            set => base.SetKeyword( "FORCE_SPEC", value );
        }

        [Menu( sectionName = "Main" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public RampInfo rampChoice


        {
            get => (RampInfo)base.GetSingle( "_RampInfo" );
            set => base.SetSingle( "_RampInfo", (Single)value );
        }

        [Menu( sectionName = "Main" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public DecalLayer decalLayer


        {
            get => (DecalLayer)base.GetSingle( "_DecalLayer" );
            set => base.SetSingle( "_DecalLayer", (Single)value );
        }

        public enum DecalLayer

        {

            Default = 0,


            Environment = 1,


            Character = 2,


            Misc = 3,

        }

        [Menu( sectionName = "Main" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single specularStrength


        {
            get => base.GetSingle( "_SpecularStrength" );
            set => base.SetSingle( "_SpecularStrength", value );
        }

        [Menu( sectionName = "Main" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single specularExponent


        {
            get => base.GetSingle( "_SpecularExponent" );
            set => base.SetSingle( "_SpecularExponent", value );
        }

        [Menu( sectionName = "Main" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public CullMode cull


        {
            get => (CullMode)base.GetSingle( "_Cull" );
            set => base.SetSingle( "_Cull", (Single)value );
        }

        [Menu( sectionName = "Dithering" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Boolean dither


        {
            get => base.GetKeyword( "DITHER" );
            set => base.SetKeyword( "DITHER", value );
        }

        [Menu( sectionName = "Dithering" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single fadeBias


        {
            get => base.GetSingle( "_FadeBias" );
            set => base.SetSingle( "_FadeBias", value );
        }

        [Menu( sectionName = "Fresnel Emission" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Boolean fresnelEmission


        {
            get => base.GetKeyword( "FRESNEL_EMISSION" );
            set => base.SetKeyword( "FRESNEL_EMISSION", value );
        }

        [Menu( sectionName = "Fresnel Emission", isRampTexture = true )]


        /// <summary>
        /// Unknown
        /// </summary>

        public TextureData fresnelRamp


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

        private TextureData _fresnelRamp;


        [Menu( sectionName = "Fresnel Emission" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single fresnelPower
        {
            get => base.GetSingle( "_FresnelPower" );
            set => base.SetSingle( "_FresnelPower", value );
        }

        [Menu( sectionName = "Fresnel Emission" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public TextureData fresnelMask


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

        private TextureData _fresnelMask;


        [Menu( sectionName = "Fresnel Emission" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single fresnelBoost


        {
            get => base.GetSingle( "_FresnelBoost" );
            set => base.SetSingle( "_FresnelBoost", value );
        }

        [Menu( sectionName = "Printing" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Boolean printingEnabled


        {
            get => base.GetKeyword( "PRINT_CUTOFF" );
            set => base.SetKeyword( "PRINT_CUTOFF", value );
        }

        [Menu( sectionName = "Printing" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single sliceHeight


        {
            get => base.GetSingle( "_SliceHeight" );
            set => base.SetSingle( "_SliceHeight", value );
        }

        [Menu( sectionName = "Printing" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single printBandHeight


        {
            get => base.GetSingle( "_SliceHeight" );
            set => base.SetSingle( "_SliceHeight", value );
        }

        [Menu( sectionName = "Printing" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single printAlphaDepth


        {
            get => base.GetSingle( "_SliceAlphaDepth" );
            set => base.SetSingle( "_SliceAlphaDepth", value );
        }

        [Menu( sectionName = "Printing" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public ScaleOffsetTextureData printAlphaTexture


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

        private ScaleOffsetTextureData _printAlphaTexture;


        [Menu( sectionName = "Printing" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single printColorBoost


        {
            get => base.GetSingle( "_PrintBoost" );
            set => base.SetSingle( "_PrintBoost", value );
        }

        [Menu( sectionName = "Printing" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single printAlphaBias


        {
            get => base.GetSingle( "_PrintBias" );
            set => base.SetSingle( "_PrintBias", value );
        }

        [Menu( sectionName = "Printing" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single printEmissionToAlbedoLerp


        {
            get => base.GetSingle( "_PrintEmissionToAlbedoLerp" );
            set => base.SetSingle( "_PrintEmissionToAlbedoLerp", value );
        }

        [Menu( sectionName = "Printing" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public PrintDirection printDirection


        {
            get => (PrintDirection)base.GetSingle( "_PrintDirection" );
            set => base.SetSingle( "_PrintDirection", (Single)value );
        }

        public enum PrintDirection

        {

            BottomUp = 0,


            TopDown = 1,


            BackToFront = 3,

        }

        [Menu( sectionName = "Printing", isRampTexture = true )]


        /// <summary>
        /// Unknown
        /// </summary>

        public TextureData printRampTexture


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

        private TextureData _printRampTexture;


        [Menu( sectionName = "Printing" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public RoR2.EliteIndex eliteIndex


        {
            get => (RoR2.EliteIndex)( base.GetSingle( "_EliteIndex" ) - 1 );
            set => base.SetSingle( "_EliteIndex", ( (Single)value ) + 1 );
        }

        [Menu( sectionName = "Printing" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single eliteBrightnessMin


        {
            get => base.GetSingle( "_EliteBrightnessMin" );
            set => base.SetSingle( "_EliteBrightnessMin", value );
        }

        [Menu( sectionName = "Printing" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single eliteBrightnessMax


        {
            get => base.GetSingle( "_EliteBrightnessMax" );
            set => base.SetSingle( "_EliteBrightnessMax", value );
        }

        [Menu( sectionName = "Splatmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Boolean splatmapEnabled


        {
            get => base.GetKeyword( "SPLATMAP" );
            set => base.SetKeyword( "SPLATMAP", value );
        }

        [Menu( sectionName = "Splatmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Boolean useVertexColors


        {
            get => base.GetKeyword( "USE_VERTEX_COLORS" );
            set => base.SetKeyword( "USE_VERTEX_COLORS", value );
        }

        [Menu( sectionName = "Splatmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single blendDepth


        {
            get => base.GetSingle( "_Depth" );
            set => base.SetSingle( "_Depth", value );
        }

        [Menu( sectionName = "Splatmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public ScaleOffsetTextureData splatmapTexture


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

        private ScaleOffsetTextureData _splatmapTexture;


        [Menu( sectionName = "Splatmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single splatmapTileScale


        {
            get => base.GetSingle( "_SplatmapTileScale" );
            set => base.SetSingle( "_SplatmapTileScale", value );
        }

        [Menu( sectionName = "Splatmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public TextureData greenChannelTexture


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

        private TextureData _greenChannelTexture;


        [Menu( sectionName = "Splatmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public TextureData greenChannelNormalmap


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

        private TextureData _greenChannelNormalmap;


        [Menu( sectionName = "Splatmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single greenChannelSmoothness


        {
            get => base.GetSingle( "_GreenChannelSmoothness" );
            set => base.SetSingle( "_GreenChannelSmoothness", value );
        }

        [Menu( sectionName = "Splatmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single greenChannelBias


        {
            get => base.GetSingle( "_GreenChannelBias" );
            set => base.SetSingle( "_GreenChannelBias", value );
        }

        [Menu( sectionName = "Splatmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public TextureData blueChannelTexture


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

        private TextureData _blueChannelTexture;


        [Menu( sectionName = "Splatmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public TextureData blueChannelNormalmap


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

        private TextureData _blueChannelNormalmap;


        [Menu( sectionName = "Splatmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single blueChannelSmoothness


        {
            get => base.GetSingle( "_BlueChannelSmoothness" );
            set => base.SetSingle( "_BlueChannelSmoothness", value );
        }

        [Menu( sectionName = "Splatmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single blueChannelBias


        {
            get => base.GetSingle( "_BlueChannelBias" );
            set => base.SetSingle( "_BlueChannelBias", value );
        }

        [Menu( sectionName = "Flowmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Boolean flowmapEnabled


        {
            get => base.GetKeyword( "FLOWMAP" );
            set => base.SetKeyword( "FLOWMAP", value );
        }

        [Menu( sectionName = "Flowmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public TextureData flowmapTexture


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

        private TextureData _flowmapTexture;


        [Menu( sectionName = "Flowmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public ScaleOffsetTextureData flowmapHeightmap


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

        private ScaleOffsetTextureData _flowmapHeightmap;



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

        private ScaleOffsetTextureData _flowmapHeightRamp;


        [Menu( sectionName = "Flowmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single flowHeightBias


        {
            get => base.GetSingle( "_FlowHeightBias" );
            set => base.SetSingle( "_FlowHeightBias", value );
        }

        [Menu( sectionName = "Flowmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single flowHeightPower


        {
            get => base.GetSingle( "_FlowHeightPower" );
            set => base.SetSingle( "_FlowHeightPower", value );
        }

        [Menu( sectionName = "Flowmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single flowHeightEmissionStrength


        {
            get => base.GetSingle( "_FlowEmissionStrength" );
            set => base.SetSingle( "_FlowEmissionStrength", value );
        }

        [Menu( sectionName = "Flowmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single flowSpeed


        {
            get => base.GetSingle( "_FlowSpeed" );
            set => base.SetSingle( "_FlowSpeed", value );
        }

        [Menu( sectionName = "Flowmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single flowMaskStrength


        {
            get => base.GetSingle( "_FlowMaskStrength" );
            set => base.SetSingle( "_FlowMaskStrength", value );
        }

        [Menu( sectionName = "Flowmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single flowNormalStrength


        {
            get => base.GetSingle( "_FlowNormalStrength" );
            set => base.SetSingle( "_FlowNormalStrength", value );
        }

        [Menu( sectionName = "Flowmap" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single flowTextureScaleFactor


        {
            get => base.GetSingle( "_FlowTextureScaleFactor" );
            set => base.SetSingle( "_FlowTextureScaleFactor", value );
        }

        [Menu( sectionName = "Limb" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Boolean limbRemovalEnabled


        {
            get => base.GetKeyword( "LIMBREMOVAL" );
            set => base.SetKeyword( "LIMBREMOVAL", value );
        }

        [Menu( sectionName = "Limb" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Single limbPrimeMask


        {
            get => base.GetSingle( "_LimbPrimeMask" );
            set => base.SetSingle( "_LimbPrimeMask", value );
        }

        [Menu( sectionName = "Limb" )]


        /// <summary>
        /// Unknown
        /// </summary>

        public Color flashColor


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
            //this._mainTexture = new(base.material, "_MainTex");
            //this._normalMap = new(base.material, "_NormalTex");
            //this._emissionTexture = new(base.material, "_EmTex");
            //this._fresnelRamp = new(base.material, "_FresnelRamp");
            //this._fresnelMask = new(base.material, "_FresnelMask");
            //this._printAlphaTexture = new(base.material, "_SliceAlphaTex");
            //this._printRampTexture = new(base.material, "_PrintRamp");
            //this._splatmapTexture = new(base.material, "_SplatmapTex");
            //this._greenChannelTexture = new(base.material, "_GreenChannelTex");
            //this._greenChannelNormalmap = new(base.material, "_GreenChannelNormalTex");
            //this._blueChannelTexture = new(base.material, "_BlueChannelTex");
            //this._blueChannelNormalmap = new(base.material, "_BlueChannelNormalTex");
            //this._flowmapTexture = new(base.material, "_FlowTex");
            //this._flowmapHeightmap = new(base.material, "_FlowHeightmap");
            //this._flowmapHeightRamp = new(base.material, "_FlowHeightRamp")
        }


        public StandardMaterial( Material mat ) : base( mat )

        {

        }



        public StandardMaterial() : base() { }


    }

}
