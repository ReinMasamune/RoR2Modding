#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers.IMGUI;

namespace RogueWispPlugin.Helpers
{
    internal class NamedVector2TextEntry
    {
        internal NamedVector2TextEntry( Vector2 startValue, String name, Action<Vector2> onChanged )
        {
            this.onChanged = onChanged;
            this.cachedValue = startValue;
            this.name = name;
            this.textEntryX = this.cachedValue.x.ToString();
            this.textEntryY = this.cachedValue.y.ToString();
        }
        internal void ChangeValue( Vector2 newValue )
        {
            this.cachedValue = newValue;
            this.textEntryX = this.cachedValue.x.ToString();
            this.textEntryY = this.cachedValue.y.ToString();
        }
        internal void ChangeName( String name )
        {
            this.name = name;
        }
        internal void ChangeAction( Action<Vector2> onChanged )
        {
            this.onChanged = onChanged;
        }


        internal void Draw()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label( this.name, GUILayout.Width( this.name.Length * Settings.widthPerChar ) );
                GUILayout.FlexibleSpace();

                GUILayout.Label( "X", GUILayout.Width( 2f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );
                this.textEntryX = GUILayout.TextField( this.textEntryX, GUILayout.Width( 6f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );

                GUILayout.Label( "Y", GUILayout.Width( 2f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );
                this.textEntryY = GUILayout.TextField( this.textEntryY, GUILayout.Width( 6f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );

                if( GUILayout.Button( "set", GUILayout.Width( 4f * Settings.widthPerChar ) ) )
                {
                    if( Single.TryParse( this.textEntryX, out Single tempX ) && Single.TryParse( this.textEntryY, out Single tempY ) )
                    {
                        var temp = new Vector2( tempX, tempY );
                        if( temp != this.cachedValue )
                        {
                            this.cachedValue = temp;
                            this.onChanged?.Invoke( this.cachedValue );
                            this.textEntryX = this.cachedValue.x.ToString();
                            this.textEntryY = this.cachedValue.y.ToString();
                        }
                    } else
                    {
                        Main.LogE( "Could not parse" );
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private Action<Vector2> onChanged;
        private Vector2 cachedValue;
        private String textEntryX;
        private String textEntryY;
        private String name;
    }
}
#endif