using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    /// <summary>
    /// A material using CloudRemap. Generally for particle effects.
    /// </summary>
    internal class WavingGrassMaterial : MaterialBase
    {
        internal Color mainColor
        {
            get => base.GetColor( "_Color" );
            set => base.SetColor( "_Color", value );
        }

        internal ScaleOffsetTextureData mainTexture
        {
            get
            {
                if( this._mainTexture == null )
                {
                    this._mainTexture = new ScaleOffsetTextureData( base.material, "_MainTex " );
                }
                return this._mainTexture;
            }
        }
        private ScaleOffsetTextureData _mainTexture;

        internal Vector4 scrollSpeedDistortionNoise
        {
            get => base.GetVector4( "_Scroll" );
            set => base.SetVector4( "_Scroll", value );
        }

        internal Single cutoffAlpha
        {
            get => base.GetSingle( "_Cutoff" );
            set => base.SetSingle( "_Cutoff", value );
        }

        internal Single vertexOffsetStrength
        {
            get => base.GetSingle( "_VertexOffsetStrength" );
            set => base.SetSingle( "_VertexOffsetStrength", value );
        }

        internal Vector4 windOffset
        {
            get => base.GetVector4( "_WindVector" );
            set => base.SetVector4( "_WindVector", value );
        }

        internal Single smoothness
        {
            get => base.GetSingle( "_Smoothness" );
            set => base.SetSingle( "_Smoothness", value );
        }

        internal RampInfo rampInfo
        {
            get => (RampInfo)base.GetSingle( "_RampInfo" );
            set => base.SetSingle( "_RampInfo", (Single)value );
        }

        internal Single specularStrength
        {
            get => base.GetSingle( "_SpecularStrength" );
            set => base.SetSingle( "_SpecularStrength", value );
        }

        internal Single specularExponent
        {
            get => base.GetSingle( "_SpecularExponent" );
            set => base.SetSingle( "_SpecularExponent", value );
        }

        internal WavingGrassMaterial( String name ) : base( name, ShaderIndex.HGCloudRemap )
        {

        }

        internal WavingGrassMaterial( Material mat ) : base( mat )
        {

        }
    }

}
