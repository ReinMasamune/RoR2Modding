#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    /*
    internal static class ScaleOffsetTextureDataMenu
    {
        internal static MaterialBase.ScaleOffsetTextureData Draw( MaterialBase.ScaleOffsetTextureData inValue, MenuRenderer renderer, MenuAttribute settings )
        {
            if( renderer.context == null ) renderer.context = new ScaleOffsetTextureDataContext( inValue, settings.name );
            var con = renderer.context as ScaleOffsetTextureDataContext;
            GUILayout.BeginHorizontal();
            var res = inValue;
            GUILayout.Label( settings.name, GUILayout.Width( settings.name.Length * MenuStructure.widthPerChar ) );
            GUILayout.FlexibleSpace();

            var labelString = inValue.texture != null ? inValue.texture.name : "No Texture";
            GUILayout.Label( labelString, GUILayout.Width( labelString.Length * MenuStructure.widthPerChar ) );
            GUILayout.FlexibleSpace();


            if( GUILayout.Button( "Show Texture Selection", GUILayout.Width( MenuStructure.widthPerChar * 15f ) ) )
            {
                if( con.enabled )
                {
                    con.enabled = false;
                } else
                {
                    con.enabled = true;
                }
            }

            GUILayout.EndHorizontal();
            return res;
        }

        internal class ScaleOffsetTextureDataContext : TextureDataMenu.TextureDataContext
        {
            internal ScaleOffsetTextureDataContext( MaterialBase.ScaleOffsetTextureData inValue, String name ) : base( inValue, name ) { }
        }
    }
    */
}
#endif