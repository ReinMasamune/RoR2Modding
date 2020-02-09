#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers.IMGUI;

namespace RogueWispPlugin.Helpers
{
    internal class RangedSingleSlider
    {
        private Single cachedValue;
        private Single minValue;
        private Single maxValue;
        private Action<Single> onChanged;
        private String name;
        private Single gap;

        internal RangedSingleSlider( Single startValue, Single minValue, Single maxValue, String name, Action<Single> onChanged, Single gap = 50f )
        {
            if( minValue >= maxValue )
            {
                throw new ArgumentException( "Min value should be less than max value" );
            }

            this.onChanged = onChanged;
            this.cachedValue = startValue;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.name = name;
            this.gap = gap;
        }
        internal void ChangeValue( Single newValue )
        {
            this.cachedValue = newValue;
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
                if( !String.IsNullOrEmpty( this.name ) )
                {
                    GUILayout.Label( this.name, GUILayout.Width( this.name.Length * Settings.widthPerChar ) );
                    if( this.gap < 0f ) GUILayout.FlexibleSpace(); else GUILayout.Space( this.gap );
                }

                var temp = GUILayout.HorizontalSlider( this.cachedValue, this.minValue, this.maxValue );
                
                if( GUI.changed && temp != this.cachedValue )
                {
                    this.cachedValue = temp;
                    this.onChanged?.Invoke( this.cachedValue );
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
#endif