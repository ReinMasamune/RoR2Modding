#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers.IMGUI;

namespace Rein.RogueWispPlugin.Helpers
{
    internal class NamedVector4TextEntry
    {
        internal NamedVector4TextEntry( Vector4 startValue, String name, Action<Vector4> onChanged )
        {
            this.onChanged = onChanged;
            this.cachedValue = startValue;
            this.name = name;
            this.textEntryX = this.cachedValue.x.ToString();
            this.textEntryY = this.cachedValue.y.ToString();
            this.textEntryZ = this.cachedValue.z.ToString();
            this.textEntryW = this.cachedValue.w.ToString();
        }
        internal void ChangeValue( Vector4 newValue )
        {
            this.cachedValue = newValue;
            this.textEntryX = this.cachedValue.x.ToString();
            this.textEntryY = this.cachedValue.y.ToString();
            this.textEntryZ = this.cachedValue.z.ToString();
            this.textEntryW = this.cachedValue.w.ToString();
        }
        internal void ChangeName( String name )
        {
            this.name = name;
        }
        internal void ChangeAction( Action<Vector4> onChanged )
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

                GUILayout.Label( "W", GUILayout.Width( 2f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );
                this.textEntryW = GUILayout.TextField( this.textEntryW, GUILayout.Width( 6f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );

                if( GUILayout.Button( "set", GUILayout.Width( 4f * Settings.widthPerChar ) ) )
                {
                    if( Single.TryParse( this.textEntryX, out Single tempX ) && Single.TryParse( this.textEntryY, out Single tempY ) && Single.TryParse( this.textEntryZ, out Single tempZ ) && Single.TryParse( this.textEntryW, out Single tempW ) )
                    {
                        var temp = new Vector4( tempX, tempY, tempZ, tempW );
                        if( temp != this.cachedValue )
                        {
                            this.cachedValue = temp;
                            this.onChanged?.Invoke( this.cachedValue );
                            this.textEntryX = this.cachedValue.x.ToString();
                            this.textEntryY = this.cachedValue.y.ToString();
                            this.textEntryZ = this.cachedValue.z.ToString();
                            this.textEntryW = this.cachedValue.w.ToString();
                        }
                    } else
                    {
                        Main.LogE( "Could not parse" );
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private Action<Vector4> onChanged;
        private Vector4 cachedValue;
        private String textEntryX;
        private String textEntryY;
        private String textEntryZ;
        private String textEntryW;
        private String name;
    }
}
#endif