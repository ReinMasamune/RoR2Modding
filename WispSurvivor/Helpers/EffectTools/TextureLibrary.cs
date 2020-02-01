using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static class TextureLibrary
    {
        private static HashSet<Texture> mappedTextures = new HashSet<Texture>();
        private static Dictionary<TextureIndex, GenericAccessor<Texture>> vanilla = new Dictionary<TextureIndex, GenericAccessor<Texture>>();
        private static Dictionary<String, GenericAccessor<Texture>> custom = new Dictionary<String, GenericAccessor<Texture>>();



        internal static Boolean HasTexture( Texture tex )
        {
            return mappedTextures.Contains( tex );
        }

        internal static Texture GetTextureRaw( String key )
        {
            if( !custom.ContainsKey( key ) )
            {
                Main.LogE( "Invalid key: " + key );
                return null;
            }
            var tex = custom[key].value;
            mappedTextures.Add( tex );
            return tex;
        }

        internal static Texture GetTextureRaw( TextureIndex key )
        {
            if( !vanilla.ContainsKey( key ) )
            {
                Main.LogE( "Invalid key: " + key.ToString() );
                return null;
            }
            var tex = vanilla[key].value;
            mappedTextures.Add( tex );
            return tex;
        }

        internal static Texture GetTextureClone( String key )
        {
            return UnityEngine.Object.Instantiate<Texture>( GetTextureRaw( key ) );
        }

        internal static Texture GetTextureClone( TextureIndex key )
        {
            return UnityEngine.Object.Instantiate<Texture>( GetTextureRaw( key ) );
        }

        internal static void LogAll()
        {
            Main.LogI( "Materials::" );
            foreach( var v in mappedTextures )
            {
                Main.LogI( v?.name );
            }
        }
    }
}