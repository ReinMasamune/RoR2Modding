using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReinCore
{
    /// <summary>
    /// A material using CloudRemap. Generally for particle effects.
    /// </summary>
    public class WavingGrassMaterial : MaterialBase
    {
        public Color mainColor
        {
            get => base.GetColor( "_Color" );
            set => base.SetColor( "_Color", value );
        }

        public ScaleOffsetTextureData mainTexture
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

        public Vector4 scrollSpeedDistortionNoise
        {
            get => base.GetVector4( "_Scroll" );
            set => base.SetVector4( "_Scroll", value );
        }

        public Single cutoffAlpha
        {
            get => base.GetSingle( "_Cutoff" );
            set => base.SetSingle( "_Cutoff", value );
        }

        public Single vertexOffsetStrength
        {
            get => base.GetSingle( "_VertexOffsetStrength" );
            set => base.SetSingle( "_VertexOffsetStrength", value );
        }

        public Vector4 windOffset
        {
            get => base.GetVector4( "_WindVector" );
            set => base.SetVector4( "_WindVector", value );
        }

        public Single smoothness
        {
            get => base.GetSingle( "_Smoothness" );
            set => base.SetSingle( "_Smoothness", value );
        }

        public RampInfo rampInfo
        {
            get => (RampInfo)base.GetSingle( "_RampInfo" );
            set => base.SetSingle( "_RampInfo", (Single)value );
        }

        public Single specularStrength
        {
            get => base.GetSingle( "_SpecularStrength" );
            set => base.SetSingle( "_SpecularStrength", value );
        }

        public Single specularExponent
        {
            get => base.GetSingle( "_SpecularExponent" );
            set => base.SetSingle( "_SpecularExponent", value );
        }

        public WavingGrassMaterial( String name ) : base( name, ShaderIndex.HGCloudRemap )
        {

        }

        public WavingGrassMaterial( Material mat ) : base( mat )
        {

        }
    }

}
