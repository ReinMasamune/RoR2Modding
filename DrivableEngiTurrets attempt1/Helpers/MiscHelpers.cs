using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using UnityEngine;

namespace RoR2Plugin
{
    public static class MiscHelpers
    {
        public static void DebugMaterialInfo( this Material m )
        {
            Debug.Log( "Material name: " + m.name );
            string[] s = m.shaderKeywords;
            Debug.Log( "Shader keywords" );
            for( int i = 0; i < s.Length; i++ )
            {
                Debug.Log( s[i] );
            }

            Debug.Log( "Shader name: " + m.shader.name );

            Debug.Log( "Texture Properties" );
            string[] s2 = m.GetTexturePropertyNames();
            for( int i = 0; i < s2.Length; i++ )
            {
                Debug.Log( s2[i] + " : " + m.GetTexture( s2[i] ) );
            }
        }
    }
}
