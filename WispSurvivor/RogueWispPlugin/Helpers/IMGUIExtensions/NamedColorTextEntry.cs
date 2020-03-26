#if MATEDITOR
using RogueWispPlugin.Helpers.IMGUI;
using System;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal class NamedColorTextEntry
    {
        internal NamedColorTextEntry( Color startValue, String name, Action<Color> onChanged )
        {
            this.onChanged = onChanged;
            this.cachedValue = startValue;
            this.name = name;
            this.textEntryR = this.cachedValue.r.ToString();
            this.textEntryG = this.cachedValue.g.ToString();
            this.textEntryB = this.cachedValue.b.ToString();
            this.textEntryA = this.cachedValue.a.ToString();
        }
        internal void ChangeValue( Color newValue )
        {
            this.cachedValue = newValue;
            this.textEntryR = this.cachedValue.r.ToString();
            this.textEntryG = this.cachedValue.g.ToString();
            this.textEntryB = this.cachedValue.b.ToString();
            this.textEntryA = this.cachedValue.a.ToString();
        }
        internal void ChangeName( String name )
        {
            this.name = name;
        }
        internal void ChangeAction( Action<Color> onChanged )
        {
            this.onChanged = onChanged;
        }


        internal void Draw()
        {
            GUILayout.BeginHorizontal();
            {
                if( !String.IsNullOrEmpty( this.name ) )
                {
                    GUILayout.Label( this.name, GUILayout.Width( this.name.Length * Settings.widthPerChar ) );
                    GUILayout.FlexibleSpace();
                }

                GUILayout.Label( "R", GUILayout.Width( 2f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );
                this.textEntryR = GUILayout.TextField( this.textEntryR, GUILayout.Width( 6f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );

                GUILayout.Label( "G", GUILayout.Width( 2f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );
                this.textEntryG = GUILayout.TextField( this.textEntryG, GUILayout.Width( 6f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );

                GUILayout.Label( "B", GUILayout.Width( 2f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );
                this.textEntryB = GUILayout.TextField( this.textEntryB, GUILayout.Width( 6f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );

                GUILayout.Label( "A", GUILayout.Width( 2f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );
                this.textEntryA = GUILayout.TextField( this.textEntryA, GUILayout.Width( 6f * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );

                if( GUILayout.Button( "set", GUILayout.Width( 4f * Settings.widthPerChar ) ) )
                {
                    if( Single.TryParse( this.textEntryR, out Single tempR ) && Single.TryParse( this.textEntryG, out Single tempG ) && Single.TryParse( this.textEntryB, out Single tempB ) && Single.TryParse( this.textEntryA, out Single tempA ) )
                    {
                        var temp = new Color( tempR, tempG, tempB, tempA );
                        if( temp != this.cachedValue )
                        {
                            this.cachedValue = temp;
                            this.onChanged?.Invoke( this.cachedValue );
                            this.textEntryR = this.cachedValue.r.ToString();
                            this.textEntryG = this.cachedValue.g.ToString();
                            this.textEntryB = this.cachedValue.b.ToString();
                            this.textEntryA = this.cachedValue.a.ToString();
                        }
                    } else
                    {
                        Main.LogE( "Could not parse" );
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private Action<Color> onChanged;
        private Color cachedValue;
        private String textEntryR;
        private String textEntryG;
        private String textEntryB;
        private String textEntryA;
        private String name;
    }
}
#endif