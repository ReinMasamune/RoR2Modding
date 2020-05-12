namespace ReinCore
{
    using System;

    using UnityEngine;

    /// <summary>
    /// Unknown
    /// </summary>
    public class OpaqueCloudMaterial : MaterialBase
    {
        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Color tintColor
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetColor( "_TintColor" );
            set => base.SetColor( "_TintColor", value );
        }

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Color emission
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetColor( "_EmissionColor" );
            set => base.SetColor( "_EmissionColor", value );
        }

        [Menu( sectionName = "Uncategorized" )]

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
#pragma warning disable IDE1006 // Naming Styles
        private ScaleOffsetTextureData _mainTexture;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Uncategorized" )]

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

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TextureData normalMap
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get
            {
                if( this._normalMap == null )
                {
                    this._normalMap = new TextureData( base.material, "_NormalTex" );
                }
                return this._normalMap;
            }
        }
#pragma warning disable IDE1006 // Naming Styles
        private TextureData _normalMap;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ScaleOffsetTextureData cloudTexture1
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ScaleOffsetTextureData cloudTexture2
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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

        [Menu( sectionName = "Uncategorized", isRampTexture = true )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TextureData remapTexture
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get
            {
                if( this._remapTexture == null )
                {
                    this._remapTexture = new TextureData( base.material, "_RemapTex" );
                }
                return this._remapTexture;
            }
        }
#pragma warning disable IDE1006 // Naming Styles
        private TextureData _remapTexture;
#pragma warning restore IDE1006 // Naming Styles

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Vector4 cutoffScrollSpeed
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetVector4( "_CutoffScroll" );
            set => base.SetVector4( "_CutoffScroll", value );
        }

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single softFactor
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_InvFade" );
            set => base.SetSingle( "_InvFade", value );
        }

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single alphaBoost
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_AlphaBoost" );
            set => base.SetSingle( "_AlphaBoost", value );
        }

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single alphaCutoff
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_Cutoff" );
            set => base.SetSingle( "_Cutoff", value );
        }

        [Menu( sectionName = "Uncategorized" )]

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

        [Menu( sectionName = "Uncategorized" )]

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

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single extrusionStrength
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_ExtrusionStrength" );
            set => base.SetSingle( "_ExtrusionStrength", value );
        }

        [Menu( sectionName = "Uncategorized" )]

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

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean emissionFromAlbedo
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetKeyword( "EMISSIONFROMALBEDO" );
            set => base.SetKeyword( "EMISSIONFROMALBEDO", value );
        }

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean normalAsCloud
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetKeyword( "CLOUDNORMAL" );
            set => base.SetKeyword( "COULDNORMAL", value );
        }

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean luminanceForVertexAlpha
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetKeyword( "VERTEXALPHA" );
            set => base.SetKeyword( "VERTEXALPHA", value );
        }

        [Menu( sectionName = "Uncategorized" )]

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

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single externalAlpha
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_ExternalAlpha" );
            set => base.SetSingle( "_ExternalAlpha", value );
        }



        /// <summary>
        /// Creates an opaque cloud material.
        /// </summary>
        /// <param name="name">The name of the material</param>
        public OpaqueCloudMaterial( String name ) : base( name, ShaderIndex.HGOpaqueCloudRemap )
        {

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public OpaqueCloudMaterial( Material m ) : base( m )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public OpaqueCloudMaterial() : base() { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

}
