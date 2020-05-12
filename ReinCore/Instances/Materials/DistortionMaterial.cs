namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// A material using CloudRemap. Generally for particle effects.
    /// </summary>
    public class DistortionMaterial : MaterialBase
    {
        [Menu( sectionName = "Uncategorized" )]
        
#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Color color
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetColor( "_Colour" );
            set => base.SetColor( "_Colour", value );
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
        public ScaleOffsetTextureData bumpTexture
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning disable IDE1006 // Naming Styles
        private ScaleOffsetTextureData _bumpTexture;
#pragma warning restore IDE1006 // Naming Styles

        [Menu(sectionName = "Uncategorized")]
        
#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
        /// Unknown
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single magnitude
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_Magnitude" );
            set => base.SetSingle( "_Magnitude", value );
        }

        /// <summary>
        /// Creates a cloud remap material.
        /// </summary>
        /// <param name="name">The name of the material</param>
        public DistortionMaterial( String name ) : base( name, ShaderIndex.HGDistortion )
        {

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public DistortionMaterial( Material mat ) : base( mat ) 
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public DistortionMaterial() : base() { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

}
