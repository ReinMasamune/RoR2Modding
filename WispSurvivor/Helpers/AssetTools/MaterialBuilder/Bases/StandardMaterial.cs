using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    /// <summary>
    /// A material for normal objects in game like enemies.
    /// </summary>
    internal class StandardMaterial : MaterialBase
    {
        /// <summary>
        /// Unknown
        /// </summary>
        internal Boolean cutout
        {
            get => base.GetSingle( "_EnableCutout" ) != 0f;
            set => base.SetSingle( "_EnableCutout", (value ? 1.0f : 0.0f) );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal Color mainColor
        {
            get => base.GetColor( "_Color" );
            set => base.SetColor( "_Color", value );
        }

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

        /// <summary>
        /// Unknown
        /// </summary>
        internal Single normalStrength
        {
            get => base.GetSingle( "_NormalStrength" );
            set => base.SetSingle( "_NormalStrength", value );
        }

        /// <summary>
        /// Unknown
        /// </summary>
        internal ScaleOffsetTextureData normalMap
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
        private ScaleOffsetTextureData _normalMap;

        /// <summary>
        /// Unknown
        /// </summary>
        internal Color emissionColor
        {
            get => base.GetColor( "_EmColor" );
            set => base.SetColor( "_EmColor", value );
        }


































        /// <summary>
        /// Creates a standard material.
        /// </summary>
        /// <param name="name">The name of the material</param>
        internal StandardMaterial( String name ) : base( name, ShaderIndex.HGStandard )
        {

        }

        internal StandardMaterial( Material mat ) : base( mat )
        {

        }
    }

}
