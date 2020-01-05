using System;
using UnityEngine;

namespace RoR2Plugin
{
    internal partial class Main
    {
        public static class MiscHelpers
        {
            public static void DebugMaterialInfo( Material m )
            {
                Debug.Log( "Material name: " + m.name );
                String[] s = m.shaderKeywords;
                Debug.Log( "Shader keywords" );
                for( Int32 i = 0; i < s.Length; i++ )
                {
                    Debug.Log( s[i] );
                }

                Debug.Log( "Shader name: " + m.shader.name );

                Debug.Log( "Texture Properties" );
                String[] s2 = m.GetTexturePropertyNames();
                for( Int32 i = 0; i < s2.Length; i++ )
                {
                    Debug.Log( s2[i] + " : " + m.GetTexture( s2[i] ) );
                }
            }
        }
    }
}
