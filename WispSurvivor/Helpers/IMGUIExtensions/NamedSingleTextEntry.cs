#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers.IMGUI;

namespace RogueWispPlugin.Helpers
{
    internal class NamedSingleTextEntry
    {
        internal NamedSingleTextEntry( Single startValue, String name, Action<Single> onChanged )
        {
            this.onChanged = onChanged;
            this.cachedValue = startValue;
            this.name = name;
            this.textEntry = this.cachedValue.ToString();
        }
        internal void ChangeValue( Single newValue )
        {
            this.cachedValue = newValue;
            this.textEntry = this.cachedValue.ToString();
        }
        internal void ChangeName( String name )
        {
            this.name = name;
        }
        internal void ChangeAction( Action<Single> onChanged )
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
                    if( Single.TryParse( this.textEntry, out Single temp ) )
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

        private Action<Single> onChanged;
        private Single cachedValue;
        private String textEntry;
        private String name;
    }
}
#endif