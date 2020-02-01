using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static class ShaderLibrary
    {
        private static HashSet<Shader> mappedShaders = new HashSet<Shader>();
        private static Dictionary<ShaderIndex, GenericAccessor<Shader>> vanilla = new Dictionary<ShaderIndex, GenericAccessor<Shader>>();
        private static Dictionary<String, GenericAccessor<Shader>> custom = new Dictionary<String, GenericAccessor<Shader>>();


        internal static Boolean HasShader( Shader shader )
        {
            return mappedShaders.Contains( shader );
        }

        internal static Shader GetShaderRaw( String key )
        {
            if( !custom.ContainsKey( key ) )
            {
                Main.LogE( "Invalid key: " + key );
                return null;
            }
            var shader = custom[key].value;
            mappedShaders.Add( shader );
            return shader;
        }

        internal static Shader GetShaderRaw( ShaderIndex key )
        {
            if( !vanilla.ContainsKey( key ) )
            {
                Main.LogE( "Invalid key: " + key.ToString() );
                return null;
            }
            var shader = vanilla[key].value;
            mappedShaders.Add( shader );
            return shader;
        }

        internal static Shader GetShaderClone( String key )
        {
            return UnityEngine.Object.Instantiate<Shader>( GetShaderRaw( key ) );
        }

        internal static Shader GetShaderClone( ShaderIndex key )
        {
            return UnityEngine.Object.Instantiate<Shader>( GetShaderRaw( key ) );
        }

        internal static void LogAll()
        {
            Main.LogI( "Materials::" );
            foreach( var v in mappedShaders )
            {
                Main.LogI( v?.name );
            }
        }
    }
}