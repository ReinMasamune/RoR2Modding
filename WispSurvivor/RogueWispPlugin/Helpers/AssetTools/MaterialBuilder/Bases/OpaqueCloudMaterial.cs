using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    /// <summary>
    /// Unknown
    /// </summary>
    internal class OpaqueCloudMaterial : MaterialBase
    {
        [Menu(sectionName = "Uncategorized")]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Color tintColor
        {
            get => base.GetColor( "_TintColor" );
            set => base.SetColor( "_TintColor", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Color emission
        {
            get => base.GetColor( "_EmissionColor" );
            set => base.SetColor( "_EmissionColor", value );
        }

        [Menu( sectionName = "Uncategorized" )]
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

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Single normalStrength
        {
            get => base.GetSingle( "_NormalStrength" );
            set => base.SetSingle( "_NormalStrength", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal TextureData normalMap
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
        private TextureData _normalMap;

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

        [Menu(sectionName = "Uncategorized", isRampTexture = true)]
        /// <summary>
        /// Unknown
        /// </summary>
        internal TextureData remapTexture
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
        private TextureData _remapTexture;

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
        internal Single softFactor
        {
            get => base.GetSingle( "_InvFade" );
            set => base.SetSingle( "_InvFade", value );
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
        internal Single alphaCutoff
        {
            get => base.GetSingle( "_Cutoff" );
            set => base.SetSingle( "_Cutoff", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Single specularStrength
        {
            get => base.GetSingle( "_SpecularStrength" );
            set => base.SetSingle( "_SpecularStrength", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Single specularExponent
        {
            get => base.GetSingle( "_SpecularExponent" );
            set => base.SetSingle( "_SpecularExponent", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Single extrusionStrength
        {
            get => base.GetSingle( "_ExtrusionStrength" );
            set => base.SetSingle( "_ExtrusionStrength", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal RampInfo rampChoice
        {
            get => (RampInfo)base.GetSingle( "_RampInfo" );
            set => base.SetSingle( "_RampInfo", (Single)value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean emissionFromAlbedo
        {
            get => base.GetKeyword( "EMISSIONFROMALBEDO" );
            set => base.SetKeyword( "EMISSIONFROMALBEDO", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean normalAsCloud
        {
            get => base.GetKeyword( "CLOUDNORMAL" );
            set => base.SetKeyword( "COULDNORMAL", value );
        }

        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean luminanceForVertexAlpha
        {
            get => base.GetKeyword( "VERTEXALPHA" );
            set => base.SetKeyword( "VERTEXALPHA", value );
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
        internal Single externalAlpha
        {
            get => base.GetSingle( "_ExternalAlpha" );
            set => base.SetSingle( "_ExternalAlpha", value );
        }



        /// <summary>
        /// Creates an opaque cloud material.
        /// </summary>
        /// <param name="name">The name of the material</param>
        internal OpaqueCloudMaterial( String name ) : base( name, ShaderIndex.HGOpaqueCloudRemap )
        {

        }

        internal OpaqueCloudMaterial( Material m ) : base( m )
        {

        }
    }

}
