using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static class TextureLibrary
    {
        private static Dictionary<TextureIndex, GenericAccessor<Texture>> vanilla = new Dictionary<TextureIndex, GenericAccessor<Texture>>();
        private static Dictionary<String, GenericAccessor<Texture>> custom = new Dictionary<String, GenericAccessor<Texture>>();

        internal static void AddAccessor( GenericAccessor<Texture> accessor )
        {
            if( accessor.isVanilla )
            {
                var key = (TextureIndex)accessor.index;
                if( vanilla.ContainsKey( key ) )
                {
                    Main.LogE( "Duplicate key: " + key.ToString() );
                    return;
                }

                vanilla[key] = accessor;
            } else
            {
                var key = accessor.name;
                if( custom.ContainsKey( key ) )
                {
                    Main.LogE( "Duplicate key: " + key );
                    return;
                }

                custom[key] = accessor;
            }
        }

        internal static Texture GetTexture( String key )
        {
            if( !custom.ContainsKey( key ) )
            {
                Main.LogE( "Invalid key: " + key );
                return null;
            }
            return custom[key].value;
        }

        internal static Texture GetTexture( TextureIndex key )
        {
            if( !vanilla.ContainsKey( key ) )
            {
                Main.LogE( "Invalid key: " + key.ToString() );
                return null;
            }
            return vanilla[key].value;
        }
    }
}