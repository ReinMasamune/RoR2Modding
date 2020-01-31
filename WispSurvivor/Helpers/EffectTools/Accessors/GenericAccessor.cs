
using System;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class GenericAccessor<TValue>
    {
        internal Boolean isVanilla { get; private set; }
        internal String name { get; private set; }
        internal UInt64 index { get; private set; }
        internal TValue value
        {
            get
            {
                if( this.val != null ) return this.val;

                if( Main.state >= this.minState )
                {
                    if( this.access != null )
                    {
                        this.val = this.access();
                        return this.val;
                    } else
                    {
                        throw new NullReferenceException( "Access null for " + this.indexName );
                    }
                } else
                {
                    throw new NullReferenceException( "Too early in execution for " + this.indexName );
                }
            }
        }


        private AssetType assetType;
        private Main.ExecutionState minState;
        private TValue val;
        private Func<TValue> access;
        private String indexName
        {
            get
            {
                if( this.isVanilla )
                {
                    String s = "index: ";
                    switch( this.assetType )
                    {
                        case AssetType.Material:
                            return s + ((MaterialIndex)this.index).ToString();
                        case AssetType.Mesh:
                            return s + ((MeshIndex)this.index).ToString();
                        case AssetType.Shader:
                            return s + ((ShaderIndex)this.index).ToString();
                        case AssetType.Texture:
                            return s + ((TextureIndex)this.index).ToString();
                        default:
                            return "Invalid index type " + this.index;
                    }
                } else
                {
                    return "name: " + this.name;
                }
            }
        }
        private enum AssetType
        {
            Material,
            Mesh,
            Texture,
            Shader,
            Count
        }
        private String validTypes
        {
            get
            {
                String s = "";
                for( Int32 i = 0; i < (Int32)AssetType.Count; ++i )
                {
                    s += ((AssetType)i).ToString() + ", ";
                }
                return s;
            }
        }
        internal void RegisterAccessor()
        {
            switch( this.assetType )
            {
                case AssetType.Material:
                    MaterialLibrary.AddAccessor( this as GenericAccessor<Material> );
                    break;
                case AssetType.Mesh:
                    MeshLibrary.AddAccessor( this as GenericAccessor<Mesh> );
                    break;
                case AssetType.Shader:
                    ShaderLibrary.AddAccessor( this as GenericAccessor<Shader> );
                    break;
                case AssetType.Texture:
                    TextureLibrary.AddAccessor( this as GenericAccessor<Texture> );
                    break;
            }
        }
        internal GenericAccessor( String name, Func<TValue> access, Main.ExecutionState minState = Main.ExecutionState.Awake )
        {
            var tv = typeof(TValue);
            if( tv == typeof(Material) )
            {
                this.assetType = AssetType.Material;
            } else if( tv == typeof(Shader) )
            {
                this.assetType = AssetType.Shader;
            } else if( tv == typeof( Mesh ) )
            {
                this.assetType = AssetType.Mesh;
            } else if( tv == typeof( Texture ) )
            {
                this.assetType = AssetType.Texture;
            } else
            {
                throw new ArgumentException( "Bad type for genericaccessor with name: " + name + ", Type must be: " + this.validTypes );
            }

            this.isVanilla = false;
            this.name = name;
            this.access = access;
            this.minState = minState;

            this.RegisterAccessor();
        }
        internal GenericAccessor( UInt64 index, Func<TValue> access, Main.ExecutionState minState = Main.ExecutionState.Awake )
        {
            var tv = typeof(TValue);
            if( tv == typeof( Material ) )
            {
                this.assetType = AssetType.Material;
            } else if( tv == typeof( Shader ) )
            {
                this.assetType = AssetType.Shader;
            } else if( tv == typeof( Mesh ) )
            {
                this.assetType = AssetType.Mesh;
            } else if( tv == typeof( Texture ) )
            {
                this.assetType = AssetType.Texture;
            } else
            {
                throw new ArgumentException( "Bad type for genericaccessor with name: " + name + ", Type must be: " + this.validTypes );
            }

            this.isVanilla = true;
            this.index = index;
            this.access = access;
            this.minState = minState;
        }

        internal GenericAccessor( MaterialIndex index, Func<TValue> access, Main.ExecutionState minState = Main.ExecutionState.Awake )
        {
            var tv = typeof(TValue);
            if( tv == typeof( Material ) )
            {
                this.assetType = AssetType.Material;
            } else if( tv == typeof( Shader ) )
            {
                this.assetType = AssetType.Shader;
            } else if( tv == typeof( Mesh ) )
            {
                this.assetType = AssetType.Mesh;
            } else if( tv == typeof( Texture ) )
            {
                this.assetType = AssetType.Texture;
            } else
            {
                throw new ArgumentException( "Bad type for genericaccessor with name: " + name + ", Type must be: " + this.validTypes );
            }

            this.isVanilla = true;
            this.index = (UInt64)index;
            this.access = access;
            this.minState = minState;
        }

        internal GenericAccessor( TextureIndex index, Func<TValue> access, Main.ExecutionState minState = Main.ExecutionState.Awake )
        {
            var tv = typeof(TValue);
            if( tv == typeof( Material ) )
            {
                this.assetType = AssetType.Material;
            } else if( tv == typeof( Shader ) )
            {
                this.assetType = AssetType.Shader;
            } else if( tv == typeof( Mesh ) )
            {
                this.assetType = AssetType.Mesh;
            } else if( tv == typeof( Texture ) )
            {
                this.assetType = AssetType.Texture;
            } else
            {
                throw new ArgumentException( "Bad type for genericaccessor with name: " + name + ", Type must be: " + this.validTypes );
            }

            this.isVanilla = true;
            this.index = (UInt64)index;
            this.access = access;
            this.minState = minState;
        }

        internal GenericAccessor( MeshIndex index, Func<TValue> access, Main.ExecutionState minState = Main.ExecutionState.Awake )
        {
            var tv = typeof(TValue);
            if( tv == typeof( Material ) )
            {
                this.assetType = AssetType.Material;
            } else if( tv == typeof( Shader ) )
            {
                this.assetType = AssetType.Shader;
            } else if( tv == typeof( Mesh ) )
            {
                this.assetType = AssetType.Mesh;
            } else if( tv == typeof( Texture ) )
            {
                this.assetType = AssetType.Texture;
            } else
            {
                throw new ArgumentException( "Bad type for genericaccessor with name: " + name + ", Type must be: " + this.validTypes );
            }

            this.isVanilla = true;
            this.index = (UInt64)index;
            this.access = access;
            this.minState = minState;
        }

        internal GenericAccessor( ShaderIndex index, Func<TValue> access, Main.ExecutionState minState = Main.ExecutionState.Awake )
        {
            var tv = typeof(TValue);
            if( tv == typeof( Material ) )
            {
                this.assetType = AssetType.Material;
            } else if( tv == typeof( Shader ) )
            {
                this.assetType = AssetType.Shader;
            } else if( tv == typeof( Mesh ) )
            {
                this.assetType = AssetType.Mesh;
            } else if( tv == typeof( Texture ) )
            {
                this.assetType = AssetType.Texture;
            } else
            {
                throw new ArgumentException( "Bad type for genericaccessor with name: " + name + ", Type must be: " + this.validTypes );
            }

            this.isVanilla = true;
            this.index = (UInt64)index;
            this.access = access;
            this.minState = minState;
        }
    }
}