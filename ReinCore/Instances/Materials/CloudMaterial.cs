namespace ReinCore
{
    using System;

    using UnityEngine;

    /// <summary>
    /// A material using CloudRemap. Generally for particle effects.
    /// </summary>
    [Serializable]
    public class CloudMaterial : MaterialBase
    {
        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public Single alphaBias
        {
            get => base.GetSingle( "_AlphaBias" );
            set => base.SetSingle( "_AlphaBias", value );
        }


        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public Single alphaBoost
        {
            get => base.GetSingle( "_AlphaBoost" );
            set => base.SetSingle( "_AlphaBoost", value );
        }


        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        [Obsolete]
        public Single brightnessBoost
        {
            get => base.GetSingle( "_Boost" );
            set => base.SetSingle( "_Boost", value );
        }


        /// <summary>
        /// Brightness Boost
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public Single boost
        {
            get => base.GetSingle( "_Boost" );
            set => base.SetSingle( "_Boost", value );
        }


        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public CullMode cull
        {
            get => (CullMode)base.GetSingle( "_Cull" );
            set => base.SetSingle( "_Cull", (Single)value );
        }



        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public Single distortionStrength
        {
            get => base.GetSingle( "_DistortionStrength" );
            set => base.SetSingle( "_DistortionStrength", value );
        }


        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public UnityEngine.Rendering.BlendMode destinationBlend
        {
            get => (UnityEngine.Rendering.BlendMode)base.GetSingle( "_DstBlend" );
            set => base.SetSingle( "_DstBlend", (Single)value );
        }


        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public Single externalAlpha
        {
            get => base.GetSingle( "_ExternalAlpha" );
            set => base.SetSingle( "_ExternalAlpha", value );
        }


        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public Single fadeCloseDistance
        {
            get => base.GetSingle( "_FadeCloseDistance" );
            set => base.SetSingle( "_FadeCloseDistance", value );
        }


        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public Single fresnelPower
        {
            get => base.GetSingle( "_FresnelPower" );
            set => base.SetSingle( "_FresnelPower", value );
        }


        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public Single internalSimpleBlendMode
        {
            get => base.GetSingle( "_InternalSimpleBlendMode" );
            set => base.SetSingle( "_InternalSimpleBlendMode", value );
        }


        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        [Obsolete]
        public Single softFactor
        {
            get => base.GetSingle( "_InvFade" );
            set => base.SetSingle( "_InvFade", value );
        }


        /// <summary>
        /// softFactor
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public Single invFade
        {
            get => base.GetSingle( "_InvFade" );
            set => base.SetSingle( "_InvFade", value );
        }


        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public Single vertexOffsetAmount
        {
            get => base.GetSingle( "_OffsetAmount" );
            set => base.SetSingle( "_OffsetAmount", value );
        }


        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public UnityEngine.Rendering.BlendMode sourceBlend
        {
            get => (UnityEngine.Rendering.BlendMode)base.GetSingle( "_SrcBlend" );
            set => base.SetSingle( "_SrcBlend", (Single)value );
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public Color color
        {
            get => base.GetColor( "_Color" );
            set => base.SetColor( "_Color", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public Vector4 cutoffScrollSpeed
        {
            get => base.GetVector4( "_CutoffScroll" );
            set => base.SetVector4( "_CutoffScroll", value );
        }


        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public Color emissionColor
        {
            get => base.GetColor( "_EmissionColor" );
            set => base.SetColor( "_EmissionColor", value );
        }


        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public Color tintColor
        {
            get => base.GetColor( "_TintColor" );
            set => base.SetColor( "_TintColor", value );
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public ScaleOffsetTextureData cloudTexture1
        {
            get
            {
                if( this._cloudTexture1 == null )
                {
                    this._cloudTexture1 = new ScaleOffsetTextureData( base.material, "_Cloud1Tex" );
                }
                return this._cloudTexture1;
            }
        }
#pragma warning disable IDE1006 // Naming Styles
        private ScaleOffsetTextureData _cloudTexture1;
#pragma warning restore IDE1006 // Naming Styles


        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
        public ScaleOffsetTextureData cloudTexture2
        {
            get
            {
                if( this._cloudTexture2 == null )
                {
                    this._cloudTexture2 = new ScaleOffsetTextureData( base.material, "_Cloud2Tex" );
                }
                return this._cloudTexture2;
            }
        }
#pragma warning disable IDE1006 // Naming Styles
        private ScaleOffsetTextureData _cloudTexture2;
#pragma warning restore IDE1006 // Naming Styles


        /// <summary>
        /// Unknown
        /// </summary>
       //[Menu( sectionName = "Uncategorized" )]
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
        private ScaleOffsetTextureData _mainTexture;


        public ScaleOffsetTextureData remapTexture
        {
            get
            {
                if( this._remapTexture == null )
                {
                    this._remapTexture = new ScaleOffsetTextureData( base.material, "_RemapTex" );
                }
                return this._remapTexture;
            }
        }
        private ScaleOffsetTextureData _remapTexture;

        [Obsolete]
        public Boolean luminanceForTextureAlpha
        {
            get => base.GetKeyword( "CALCTEXTUREALPHA" );
            set => base.SetKeyword( "CALCTEXTUREALPHA", value, "_CalcTexAlphaOn" );
        }
        /// <summary>
        /// Luminance for Texture Alpha
        /// </summary>
        public Boolean calcTexAlpha
        {
            get => base.GetKeyword( "CALCTEXTUREALPHA" );
            set => base.SetKeyword( "CALCTEXTUREALPHA", value, "_CalcTexAlphaOn" );
        }


        public Boolean cloudDistortionOn
        {
            get => base.GetKeyword( "CLOUDOFFSET" );
            set => base.SetKeyword( "CLOUDOFFSET", value, "_CloudOffsetOn" );
        }


        public Boolean cloudRemappingOn
        {
            get => base.GetKeyword( "USE_CLOUDS" );
            set => base.SetKeyword( "USE_CLOUDS", value, "_CloudsOn" );
        }

        public Boolean disableRemapping
        {
            get => base.GetKeyword( "DISABLEREMAP" );
            set => base.SetKeyword( "DISABLEREMAP", value, "_DisableRemapOn" );
        }


        public Boolean emissionOn
        {
            get => base.GetKeyword( "_EMISSION" );
            set => base.SetKeyword( "_EMISSION", value );
        }

       //[Menu( sectionName = "Uncategorized" )]
        public Boolean fadeClose
        {
            get => base.GetKeyword( "FADECLOSE" );
            set => base.SetKeyword( "FADECLOSE", value, "_FadeCloseOn" );
        }


       //[Menu( sectionName = "Uncategorized" )]
        public Boolean fresnelFade
        {
            get => base.GetKeyword( "FRESNEL" );
            set => base.SetKeyword( "FRESNEL", value, "_FresnelOn" );
        }


       //[Menu( sectionName = "Uncategorized" )]
        public Boolean useUV1
        {
            get => base.GetKeyword( "USE_UV1" );
            set => base.SetKeyword( "USE_UV1", value, "_UseUV1On" );
        }


       //[Menu( sectionName = "Uncategorized" )]
        public Boolean vertexAlphaOn
        {
            get => base.GetKeyword( "VERTEXALPHA" );
            set => base.SetKeyword( "VERTEXALPHA", value, "_VertexAlphaOn" );
        }


       //[Menu( sectionName = "Uncategorized" )]
        public Boolean vertexColorOn
        {
            get => base.GetKeyword( "VERTEXCOLOR" );
            set => base.SetKeyword( "VERTEXCOLOR", value, "_VertexColorOn" );
        }


       //[Menu( sectionName = "Uncategorized" )]
        public Boolean vertexOffset
        {
            get => base.GetKeyword( "VERTEXOFFSET" );
            set => base.SetKeyword( "VERTEXOFFSET", value );
        }
        /// <summary>
        /// Creates a cloud remap material.
        /// </summary>
        /// <param name="name">The name of the material</param>
        public CloudMaterial( String name ) : base( name, ShaderIndex.HGCloudRemap )
        {

        }

        public CloudMaterial( Material mat ) : base( mat )
        {

        }

        public CloudMaterial() : base() { }

    }

}
