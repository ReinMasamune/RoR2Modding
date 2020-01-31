using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static class ShaderLibrary
    {
        private static Dictionary<ShaderIndex, GenericAccessor<Shader>> vanilla = new Dictionary<ShaderIndex, GenericAccessor<Shader>>();
        private static Dictionary<String, GenericAccessor<Shader>> custom = new Dictionary<String, GenericAccessor<Shader>>();

        internal static void AddAccessor( GenericAccessor<Shader> accessor )
        {
            if( accessor.isVanilla )
            {
                var key = (ShaderIndex)accessor.index;
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

        internal static Shader GetShader( String key )
        {
            if( !custom.ContainsKey( key ) )
            {
                Main.LogE( "Invalid key: " + key );
                return null;
            }
            return custom[key].value;
        }

        internal static Shader GetShader( ShaderIndex key )
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