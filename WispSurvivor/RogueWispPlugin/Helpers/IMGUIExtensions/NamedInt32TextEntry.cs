#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers.IMGUI;

namespace RogueWispPlugin.Helpers
{
    internal class NamedInt32TextEntry
    {
        internal NamedInt32TextEntry( Int32 startValue, String name, Action<Int32> onChanged )
        {
            this.onChanged = onChanged;
            this.cachedValue = startValue;
            this.name = name;
            this.textEntry = this.cachedValue.ToString();
        }
        internal void ChangeValue( Int32 newValue )
        {
            this.cachedValue = newValue;
            this.textEntry = this.cachedValue.ToString();
        }
        internal void ChangeName( String name )
        {
            this.name = name;
        }
        internal void ChangeAction( Action<Int32> onChanged )
        {
            this.onChanged = onChanged;
        }


        internal void Draw()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label( this.name, GUILayout.Width( this.name.Length * Settings.widthPerChar ) );
                GUILayout.FlexibleSpace();

                this.textEntry = GUILayout.TextField( this.textEntry, GUILayout.Width( 10f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );

                if( GUILayout.Button( "set", GUILayout.Width( 4f * Settings.widthPerChar ) ) )
                {
                    if( Int32.TryParse( this.textEntry, out Int32 temp ) )
                    {
                        if( temp != this.cachedValue )
                        {
                            this.cachedValue = temp;
                            this.onChanged?.Invoke( this.cachedValue );
                            this.textEntry = this.cachedValue.ToString();
                        }
                    } else
                    {
                        Main.LogE( "Could not parse" );
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private Action<Int32> onChanged;
        private Int32 cachedValue;
        private String textEntry;
        private String name;
    }
}
#endif