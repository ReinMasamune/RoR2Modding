namespace ReinCore
{
    using System;

    using UnityEngine;

    /// <summary>
    /// Unknown
    /// </summary>
    public class IntersectionCloudMaterial : MaterialBase
    {
        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public UnityEngine.Rendering.BlendMode sourceBlend
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => (UnityEngine.Rendering.BlendMode)base.GetSingle( "_SrcBlendFloat" );
            set => base.SetSingle( "_SrcBlendFloat", (Single)value );
        }

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public UnityEngine.Rendering.BlendMode destinationBlend
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => (UnityEngine.Rendering.BlendMode)base.GetSingle( "_DstBlendFloat" );
            set => base.SetSingle( "_DstBlendFloat", (Single)value );
        }

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
        public ScaleOffsetTextureData remapTexture
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning disable IDE1006 // Naming Styles
        private ScaleOffsetTextureData _remapTexture;
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
        public Single softPower
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_SoftPower" );
            set => base.SetSingle( "_SoftPower", value );
        }

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single brightnessBoost
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_Boost" );
            set => base.SetSingle( "_Boost", value );
        }

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single rimPower
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_RimPower" );
            set => base.SetSingle( "_RimPower", value );
        }

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single rimStrength
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_RimStrength" );
            set => base.SetSingle( "_RimStrength", value );
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
        public Single intersectionStrength
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_IntersectionStrength" );
            set => base.SetSingle( "_IntersectionStrength", value );
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

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean vertexColorsOn
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetKeyword( "IGNORE_VERTEX_COLORS" );
            set => base.SetKeyword( "IGNORE_VERTEX_COLORS", value );
        }

        [Menu( sectionName = "Uncategorized" )]

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean triplanarOn
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetKeyword( "TRIPLANAR" );
            set => base.SetKeyword( "TRIPLANAR", value );
        }

        /// <summary>
        /// Creates a standard material.
        /// </summary>
        /// <param name="name">The name of the material</param>
        public IntersectionCloudMaterial( String name ) : base( name, ShaderIndex.HGIntersectionCloudRemap )
        {

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IntersectionCloudMaterial( Material m ) : base( m )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IntersectionCloudMaterial() : base() { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

}
