using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReinCore
{
    /// <summary>
    /// Unknown
    /// </summary>
    public class IntersectionCloudMaterial : MaterialBase
    {
        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        public UnityEngine.Rendering.BlendMode sourceBlend
        {
            get => (UnityEngine.Rendering.BlendMode)base.GetSingle( "_SrcBlendFloat" );
            set => base.SetSingle( "_SrcBlendFloat", (Single)value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        public UnityEngine.Rendering.BlendMode destinationBlend
        {
            get => (UnityEngine.Rendering.BlendMode)base.GetSingle( "_DstBlendFloat" );
            set => base.SetSingle( "_DstBlendFloat", (Single)value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        public Color tintColor
        {
            get => base.GetColor( "_TintColor" );
            set => base.SetColor( "_TintColor", value );
        }

        [Menu( sectionName = "Uncategorized" )]
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
        private ScaleOffsetTextureData _mainTexture;

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
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
        private ScaleOffsetTextureData _cloudTexture1;

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
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
        private ScaleOffsetTextureData _cloudTexture2;

        [Menu( sectionName = "Uncategorized", isRampTexture = true )]
        /// <summary>
        /// Unknown
        /// </summary>
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

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        public Vector4 cutoffScrollSpeed
        {
            get => base.GetVector4( "_CutoffScroll" );
            set => base.SetVector4( "_CutoffScroll", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        public Single softFactor
        {
            get => base.GetSingle( "_InvFade" );
            set => base.SetSingle( "_InvFade", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        public Single softPower
        {
            get => base.GetSingle( "_SoftPower" );
            set => base.SetSingle( "_SoftPower", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        public Single brightnessBoost
        {
            get => base.GetSingle( "_Boost" );
            set => base.SetSingle( "_Boost", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        public Single rimPower
        {
            get => base.GetSingle( "_RimPower" );
            set => base.SetSingle( "_RimPower", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        public Single rimStrength
        {
            get => base.GetSingle( "_RimStrength" );
            set => base.SetSingle( "_RimStrength", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        public Single alphaBoost
        {
            get => base.GetSingle( "_AlphaBoost" );
            set => base.SetSingle( "_AlphaBoost", value );
        }
        
        [Menu( sectionName = "Uncategorized")]
        /// <summary>
        /// Unknown
        /// </summary>
        public Single intersectionStrength
        {
            get => base.GetSingle( "_IntersectionStrength" );
            set => base.SetSingle( "_IntersectionStrength", value );
        }

        [Menu(sectionName = "Uncategorized")]
        /// <summary>
        /// Unknown
        /// </summary>
        public CullMode cull
        {
            get => (CullMode)base.GetSingle( "_Cull" );
            set => base.SetSingle( "_Cull", (Single)value );
        }

        [Menu(sectionName = "Uncategorized")]
        /// <summary>
        /// Unknown
        /// </summary>
        public Single externalAlpha
        {
            get => base.GetSingle( "_ExternalAlpha" );
            set => base.SetSingle( "_ExternalAlpha", value );
        }

        [Menu(sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        public Boolean vertexColorsOn
        {
            get => base.GetKeyword( "IGNORE_VERTEX_COLORS" );
            set => base.SetKeyword( "IGNORE_VERTEX_COLORS", value );
        }

        [Menu(sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        public Boolean triplanarOn
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

        public IntersectionCloudMaterial( Material m ) : base( m )
        {

        }
    }

}
