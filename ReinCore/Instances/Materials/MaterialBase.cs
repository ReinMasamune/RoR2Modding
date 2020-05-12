namespace ReinCore
{
    using System;

    using UnityEngine;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class MaterialBase
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// The name of the material
        /// </summary>
        public String name
        {
            get => this.material?.name;
            set
            {
                if( this.material )
                {
                    this.material.name = value;
                }
            }
        }

        /// <summary>
        /// The shader to use
        /// </summary>
        public ShaderIndex shader { get; set; }

        /// <summary>
        /// The Material that is being created
        /// </summary>
        public Material material { get; private set; }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void Init( Material mat, String name )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            this.material = mat;
            this.name = name;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public MaterialBase( String name, ShaderIndex index )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            this.shader = index;
            this.material = new Material( AssetLibrary<Shader>.GetAsset( index ) );
            this.name = name;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public MaterialBase( Material mat )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            this.material = mat;
            this.name = mat.name;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public MaterialBase() { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public enum CullMode
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            Off = 0,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            Front = 1,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            Back = 2
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public enum RampInfo
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            TwoTone = 0,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            SmoothedTwoTone = 1,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            Unlitish = 3,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            SubSurface = 4,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void SetSingle( String name, Single value ) => this.material.SetFloat( name, value );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single GetSingle( String name ) => this.material.GetFloat( name );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void SetColor( String name, Color value ) => this.material.SetColor( name, value );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Color GetColor( String name ) => this.material.GetColor( name );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void SetKeyword( String keyword, Boolean value, params String[] toggleNames )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( value )
            {
                this.material.EnableKeyword( keyword );
            } else
            {
                this.material.DisableKeyword( keyword );
            }
            for( Int32 i = 0; i < toggleNames.Length; ++i )
            {
                this.material.SetFloat( toggleNames[i], value ? 1f : 0f );
            }
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean GetKeyword( String keyword ) => this.material.IsKeywordEnabled( keyword );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Vector4 GetVector4( String name ) => this.material.GetVector( name );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void SetVector4( String name, Vector4 value ) => this.material.SetVector( name, value );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        [Serializable]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public class TextureData
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public TextureData( Material mat, String propName )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            {
                this.mat = mat;
                this.propName = propName;
                this._texture = this.mat.GetTexture( this.propName );
            }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public Texture texture
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            {
                get => this._texture;
                set
                {
                    if( value != this._texture )
                    {
                        this._texture = value;
                        this.mat.SetTexture( this.propName, this._texture );
                    }
                }
            }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public Material mat;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public String propName;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

            [SerializeField]
#pragma warning disable IDE1006 // Naming Styles
            private Texture _texture;
#pragma warning restore IDE1006 // Naming Styles
        }

        /// <summary>
        /// Data for a texture with scale and offset settings
        /// </summary>
        [Serializable]
        public class ScaleOffsetTextureData : TextureData
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public ScaleOffsetTextureData( Material mat, String propName ) : base( mat, propName )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            {

                this._tiling = base.mat.GetTextureScale( base.propName );
                this._offset = base.mat.GetTextureOffset( base.propName );
            }
            /// <summary>
            /// The scale vector
            /// </summary>
            public Vector2 tiling
            {
                get => this._tiling;
                set
                {
                    if( value != this._tiling )
                    {
                        this._tiling = value;
                        base.mat.SetTextureScale( base.propName, this._tiling );
                    }
                }
            }
            /// <summary>
            /// The offset vector
            /// </summary>
            public Vector2 offset
            {
                get => this._offset;
                set
                {
                    if( value != this._offset )
                    {
                        this._offset = value;
                        base.mat.SetTextureOffset( base.propName, this._offset );
                    }
                }
            }

#pragma warning disable IDE1006 // Naming Styles
            [SerializeField]
            private Vector2 _tiling;
            [SerializeField]
            private Vector2 _offset;
#pragma warning restore IDE1006 // Naming Styles
        }
    }

}
