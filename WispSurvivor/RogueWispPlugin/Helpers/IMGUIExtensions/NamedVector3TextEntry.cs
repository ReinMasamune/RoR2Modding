#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers.IMGUI;

namespace RogueWispPlugin.Helpers
{
    internal class NamedVector3TextEntry
    {
        internal NamedVector3TextEntry( Vector3 startValue, String name, Action<Vector3> onChanged )
        {
            this.onChanged = onChanged;
            this.cachedValue = startValue;
            this.name = name;
            this.textEntryX = this.cachedValue.x.ToString();
            this.textEntryY = this.cachedValue.y.ToString();
            this.textEntryZ = this.cachedValue.z.ToString();
        }
        internal void ChangeValue( Vector3 newValue )
        {
            this.cachedValue = newValue;
            this.textEntryX = this.cachedValue.x.ToString();
            this.textEntryY = this.cachedValue.y.ToString();
            this.textEntryZ = this.cachedValue.z.ToString();
        }
        internal void ChangeName( String name )
        {
            this.name = name;
        }
        internal void ChangeAction( Action<Vector3> onChanged )
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

                GUILayout.Label( "Z", GUILayout.Width( 2f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );
                this.textEntryZ = GUILayout.TextField( this.textEntryZ, GUILayout.Width( 6f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );

                if( GUILayout.Button( "set", GUILayout.Width( 4f * Settings.widthPerChar ) ) )
                {
                    if( Single.TryParse( this.textEntryX, out Single tempX ) && Single.TryParse( this.textEntryY, out Single tempY ) && Single.TryParse( this.textEntryZ, out Single tempZ ) )
                    {
                        var temp = new Vector3( tempX, tempY, tempZ );
                        if( temp != this.cachedValue )
                        {
                            this.cachedValue = temp;
                            this.onChanged?.Invoke( this.cachedValue );
                            this.textEntryX = this.cachedValue.x.ToString();
                            this.textEntryY = this.cachedValue.y.ToString();
                            this.textEntryZ = this.cachedValue.z.ToString();
                        }
                    } else
                    {
                        Main.LogE( "Could not parse" );
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private Action<Vector3> onChanged;
        private Vector3 cachedValue;
        private String textEntryX;
        private String textEntryY;
        private String textEntryZ;
        private String name;
    }
}
#endif