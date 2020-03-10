using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    /// <summary>
    /// A material using CloudRemap. Generally for particle effects.
    /// </summary>
    internal class DistortionMaterial : MaterialBase
    {
        [Menu( sectionName = "Uncategorized" )]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Color color
        {
            get => base.GetColor( "_Colour" );
            set => base.SetColor( "_Colour", value );
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
        internal ScaleOffsetTextureData bumpTexture
        {
            get
            {
                if( this._bumpTexture == null )
                {
                    this._bumpTexture = new ScaleOffsetTextureData( base.material, "_BumpMap" );
                }
                return this._bumpTexture;
            }
        }
        private ScaleOffsetTextureData _bumpTexture;

        [Menu(sectionName = "Uncategorized")]
        /// <summary>
        /// Unknown
        /// </summary>
        internal Single magnitude
        {
            get => base.GetSingle( "_Magnitude" );
            set => base.SetSingle( "_Magnitude", value );
        }

        /// <summary>
        /// Creates a cloud remap material.
        /// </summary>
        /// <param name="name">The name of the material</param>
        internal DistortionMaterial( String name ) : base( name, ShaderIndex.HGDistortion )
        {

        }

        internal DistortionMaterial( Material mat ) : base( mat )
        {

        }
    }

}
