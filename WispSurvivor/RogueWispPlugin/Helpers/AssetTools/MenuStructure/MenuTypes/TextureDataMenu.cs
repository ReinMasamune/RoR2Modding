#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    /*
    internal static class TextureDataMenu
    {
        internal static MaterialBase.TextureData Draw( MaterialBase.TextureData inValue, MenuRenderer renderer, MenuAttribute settings )
        {
            if( renderer.context == null ) renderer.context = new TextureDataContext( inValue, settings.name );
            var con = renderer.context as TextureDataContext;
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

        internal class TextureDataContext : MenuRenderer.MenuContext
        {
            internal TextureDataContext( MaterialBase.TextureData inValue, String name )
            {
                this.data = inValue;
                this._enabled = false;
                this.name = name;
                this.showPreview = false;
                this.windowObjectInstance = this.CreateWindowObject();
            }

            internal Boolean showPreview;

            internal Boolean enabled
            {
                get
                {
                    return this._enabled;
                }
                set
                {
                    if( value )
                    {
                        activeWindowContext = this;
                    } else
                    {
                        activeWindowContext = null;
                    }
                }
            }

            private Boolean _enabled;
            private MaterialBase.TextureData data;
            private GameObject windowObjectInstance;
            private TexSelect selector;
            private String name;

            private GameObject CreateWindowObject()
            {
                var obj = new GameObject();
                this.selector = obj.AddComponent<TexSelect>();
                this.selector.Init( this, this.data, this.name );
                return obj;
            }

            private static TextureDataContext _activeWindowContext;
            private static TextureDataContext activeWindowContext
            {
                set
                {
                    if( _activeWindowContext == value )
                    {
                        return;
                    }
                    if( _activeWindowContext != null )
                    {
                        _activeWindowContext.selector.Off();
                        _activeWindowContext._enabled = false;
                        _activeWindowContext = null;
                    }
                    if( value != null )
                    {
                        _activeWindowContext = value;
                        _activeWindowContext.selector.On();
                        _activeWindowContext._enabled = true;
                    }
                }
            }


        }
    }
    */
}
#endif