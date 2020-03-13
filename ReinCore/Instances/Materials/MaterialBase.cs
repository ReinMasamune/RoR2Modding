using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReinCore
{
    public abstract class MaterialBase
    {
        /// <summary>
        /// The name of the material
        /// </summary>
        public String name
        {
            get
            {
                return this.material?.name;
            }
            set
            {
                if( this.material ) this.material.name = value;
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

        public MaterialBase( String name, ShaderIndex index )
        {
            this.shader = index;
            this.material = new Material( AssetLibrary<Shader>.GetAsset(index) );
            this.name = name;
        }

        public MaterialBase( Material mat )
        {
            this.material = mat;
            this.name = mat.name;
        }

        public enum CullMode
        {
            Off = 0,
            Front = 1,
            Back = 2
        }

        public enum RampInfo
        {
            TwoTone = 0,
            SmoothedTwoTone = 1,
            Unlitish = 3,
            SubSurface = 4,
        }

        public void SetSingle( String name, Single value )
        {
            this.material.SetFloat( name, value );
        }
        public Single GetSingle( String name )
        {
            return this.material.GetFloat( name );
        }
        public void SetColor( String name, Color value )
        {
            this.material.SetColor( name, value );
        }
        public Color GetColor( String name )
        {
            return this.material.GetColor( name );
        }
        public void SetKeyword( String keyword, Boolean value )
        {
            if( value )
            {
                this.material.EnableKeyword( keyword );
            } else
            {
                this.material.DisableKeyword( keyword );
            }
        }
        public Boolean GetKeyword( String keyword )
        {
            return this.material.IsKeywordEnabled( keyword );
        }
        public Vector4 GetVector4( String name )
        {
            return this.material.GetVector( name );
        }
        public void SetVector4( String name, Vector4 value )
        {
            this.material.SetVector( name, value );
        }


        public class TextureData
        {
            public TextureData( Material mat, String propName )
            {
                this.mat = mat;
                this.propName = propName;
                this._texture = this.mat.GetTexture( this.propName );
            }

            public Texture texture
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

            public Material mat;
            public String propName;

            private Texture _texture;
        }

        public class ScaleOffsetTextureData : TextureData
        {
            public ScaleOffsetTextureData( Material mat, String propName ) : base( mat, propName )
            {

                this._tiling = base.mat.GetTextureScale( base.propName );
                this._offset = base.mat.GetTextureOffset( base.propName );
            }
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

            private Vector2 _tiling;
            private Vector2 _offset;
        }
    }

}
