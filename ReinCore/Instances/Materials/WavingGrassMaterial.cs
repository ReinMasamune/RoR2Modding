namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// A material using CloudRemap. Generally for particle effects.
    /// </summary>
    public class WavingGrassMaterial : MaterialBase
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Color mainColor
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetColor( "_Color" );
            set => base.SetColor( "_Color", value );
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ScaleOffsetTextureData mainTexture
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning disable IDE1006 // Naming Styles
        private ScaleOffsetTextureData _mainTexture;
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Vector4 scrollSpeedDistortionNoise
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetVector4( "_Scroll" );
            set => base.SetVector4( "_Scroll", value );
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single cutoffAlpha
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_Cutoff" );
            set => base.SetSingle( "_Cutoff", value );
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single vertexOffsetStrength
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_VertexOffsetStrength" );
            set => base.SetSingle( "_VertexOffsetStrength", value );
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Vector4 windOffset
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetVector4( "_WindVector" );
            set => base.SetVector4( "_WindVector", value );
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single smoothness
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_Smoothness" );
            set => base.SetSingle( "_Smoothness", value );
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public RampInfo rampInfo
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => (RampInfo)base.GetSingle( "_RampInfo" );
            set => base.SetSingle( "_RampInfo", (Single)value );
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single specularStrength
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_SpecularStrength" );
            set => base.SetSingle( "_SpecularStrength", value );
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single specularExponent
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => base.GetSingle( "_SpecularExponent" );
            set => base.SetSingle( "_SpecularExponent", value );
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public WavingGrassMaterial( String name ) : base( name, ShaderIndex.HGCloudRemap )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public WavingGrassMaterial( Material mat ) : base( mat )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public WavingGrassMaterial() : base() { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

}
