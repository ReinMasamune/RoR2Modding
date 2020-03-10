using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    /// <summary>
    /// A material using CloudRemap. Generally for particle effects.
    /// </summary>
    internal class CloudMaterial : MaterialBase
    {
        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal UnityEngine.Rendering.BlendMode sourceBlend
        {
            get => (UnityEngine.Rendering.BlendMode)base.GetSingle( "_SrcBlend" );
            set => base.SetSingle( "_SrcBlend", (Single)value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal UnityEngine.Rendering.BlendMode destinationBlend
        {
            get => (UnityEngine.Rendering.BlendMode)base.GetSingle( "_DstBlend" );
            set => base.SetSingle( "_DstBlend", (Single)value );
        }

        [Menu(sectionName = "Uncategorized")]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Single internalSimpleBlendMode
        {
            get => base.GetSingle( "_InternalSimpleBlendMode" );
            set => base.SetSingle( "_InternalSimpleBlendMode", value );
        }

        [Menu(sectionName = "Uncategorized")]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Color tintColor
        {
            get => base.GetColor( "_TintColor" );
            set => base.SetColor( "_TintColor", value );
        }

        [Menu(sectionName = "Uncategorized")]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean disableRemapping
        {
            get => base.GetKeyword( "DISABLEREMAP" );
            set => base.SetKeyword( "DISABLEREMAP", value );
        }

        [Menu(sectionName = "Uncategorized" )]
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

        [Menu(sectionName = "Uncategorized", isRampTexture = true )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal ScaleOffsetTextureData remapTexture
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

        [Menu(sectionName = "Uncategorized")]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Single softFactor
        {
            get => base.GetSingle( "_InvFade" );
            set => base.SetSingle( "_InvFade", value );
        }

        [Menu( sectionName = "Uncategorized")]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Single brightnessBoost
        {
            get => base.GetSingle( "_Boost" );
            set => base.SetSingle( "_Boost", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Single alphaBoost
        {
            get => base.GetSingle( "_AlphaBoost" );
            set => base.SetSingle( "_AlphaBoost", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Single alphaBias
        {
            get => base.GetSingle( "_AlphaBias" );
            set => base.SetSingle( "_AlphaBias", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean useUV1
        {
            get => base.GetKeyword( "USE_UV1" );
            set => base.SetKeyword( "USE_UV1", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean fadeClose
        {
            get => base.GetKeyword( "FADECLOSE" );
            set => base.SetKeyword( "FADECLOSE", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Single fadeCloseDistance
        {
            get => base.GetSingle( "_FadeCloseDistance" );
            set => base.SetSingle( "_FadeCloseDistance", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal CullMode cull
        {
            get => (CullMode)base.GetSingle( "_Cull" );
            set => base.SetSingle( "_Cull", (Single)value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean cloudRemappingOn
        {
            get => base.GetKeyword( "USE_CLOUDS" );
            set => base.SetKeyword( "USE_CLOUDS", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean cloudDistortionOn
        {
            get => base.GetKeyword( "CLOUDOFFSET" );
            set => base.SetKeyword( "CLOUDOFFSET", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Single distortionStrength
        {
            get => base.GetSingle( "_DistortionStrength" );
            set => base.SetSingle( "_DistortionStrength", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal ScaleOffsetTextureData cloudTexture1
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
        internal ScaleOffsetTextureData cloudTexture2
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

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Vector4 cutoffScrollSpeed
        {
            get => base.GetVector4( "_CutoffScroll" );
            set => base.SetVector4( "_CutoffScroll", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean vertexColorOn
        {
            get => base.GetKeyword( "VERTEXCOLOR" );
            set => base.SetKeyword( "VERTEXCOLOR", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean vertexAlphaOn
        {
            get => base.GetKeyword( "VERTEXALPHA" );
            set => base.SetKeyword( "VERTEXALPHA", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean luminanceForTextureAlpha
        {
            get => base.GetKeyword( "CALCTEXTUREALPHA" );
            set => base.SetKeyword( "CALCTEXTUREALPHA", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean vertexOffset
        {
            get => base.GetKeyword( "VERTEXOFFSET" );
            set => base.SetKeyword( "VERTEXOFFSET", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean fresnelFade
        {
            get => base.GetKeyword( "FRESNEL" );
            set => base.SetKeyword( "FRESNEL", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Single fresnelPower
        {
            get => base.GetSingle( "_FresnelPower" );
            set => base.SetSingle( "_FresnelPower", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Single vertexOffsetAmount
        {
            get => base.GetSingle( "_OffsetAmount" );
            set => base.SetSingle( "_OffsetAmount", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Single externalAlpha
        {
            get => base.GetSingle( "_ExternalAlpha" );
            set => base.SetSingle( "_ExternalAlpha", value );
        }









        /// <summary>
        /// Creates a cloud remap material.
        /// </summary>
        /// <param name="name">The name of the material</param>
        internal CloudMaterial( String name ) : base( name, ShaderIndex.HGCloudRemap )
        {

        }

        internal CloudMaterial( Material mat ) : base( mat )
        {

        }
    }

}
