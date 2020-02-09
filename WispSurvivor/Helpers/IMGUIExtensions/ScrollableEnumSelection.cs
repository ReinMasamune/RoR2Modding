#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers.IMGUI;

namespace RogueWispPlugin.Helpers
{
    internal class ScrollableEnumSelection<TEnum> where TEnum : struct, Enum
    {
        private static String[] names;
        private static TEnum[] values;
        private static Dictionary<TEnum,Int32> baseValues;

        private Action<TEnum> onChanged;
        private TEnum cachedValue;
        private Vector2 scroll;
        private Int32 curSelected;
        private String name;

        static ScrollableEnumSelection()
        {
            names = Enum.GetNames( typeof( TEnum ) );
            var count = names.Length;
            values = new TEnum[count];
            baseValues = new Dictionary<TEnum, Int32>();

            for( Int32 i = 0; i < count; ++i )
            {
                var cName = names[i];
                TEnum value;
                if( Enum.TryParse<TEnum>( cName, out value ) )
                {
                    values[i] = value;
                    baseValues[value] = i;
                }
            }
        }

        internal ScrollableEnumSelection( TEnum startValue, String name, Action<TEnum> onChanged )
        {
            this.onChanged = onChanged;
            this.cachedValue = startValue;
            this.name = name;
            this.curSelected = baseValues[startValue];
        }
        internal void ChangeValue( TEnum newValue )
        {
            this.cachedValue = newValue;
            this.curSelected = baseValues[newValue];
        }
        internal void ChangeName( String name )
        {
            this.name = name;
        }
        internal void ChangeAction( Action<TEnum> onChanged )
        {
            this.onChanged = onChanged;
        }

        internal void Draw()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label( this.name, GUILayout.Width( this.name.Length * Settings.widthPerChar ) );
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                this.scroll = GUILayout.BeginScrollView( this.scroll );
                {
                    var selection = GUILayout.SelectionGrid( this.curSelected, names, 1 );
                    if( GUI.changed && selection != this.curSelected )
                    {
                        this.curSelected = selection;
                        this.cachedValue = values[this.curSelected];
                        this.onChanged?.Invoke( this.cachedValue );
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }
    }
}
#endif