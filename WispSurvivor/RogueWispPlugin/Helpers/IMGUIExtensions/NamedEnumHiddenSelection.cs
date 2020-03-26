#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers.IMGUI;

namespace Rein.RogueWispPlugin.Helpers
{
    internal class NamedEnumHiddenSelection<TEnum> where TEnum : struct, Enum
    {
        private static String[] names;
        private static TEnum[] values;
        private static Dictionary<TEnum,Int32> baseValues;

        private Action<TEnum> onChanged;
        private TEnum cachedValue;
        private Boolean showMenu;
        private Int32 curSelected;
        private Int32 numCols;
        private String name;

        static NamedEnumHiddenSelection()
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

        internal NamedEnumHiddenSelection( TEnum startValue, String name, Int32 numCols, Action<TEnum> onChanged )
        {
            this.onChanged = onChanged;
            this.cachedValue = startValue;
            this.name = name;
            this.curSelected = baseValues[startValue];
            this.numCols = numCols;
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
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label( this.name, GUILayout.Width( this.name.Length * Settings.widthPerChar ) );
                GUILayout.FlexibleSpace();

                GUILayout.Label( this.cachedValue.ToString(), GUILayout.Width( Settings.widthPerChar * 20f ) );
                GUILayout.Space( Settings.defaultMinSpace );
                this.showMenu = GUILayout.Toggle( this.showMenu, "Show selection", GUILayout.Width( 15f * Settings.widthPerChar ) );
            }
            GUILayout.EndHorizontal();
            if( this.showMenu )
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        var selection = GUILayout.SelectionGrid( this.curSelected, names, this.numCols );
                        if( GUI.changed && selection != this.curSelected )
                        {
                            this.curSelected = selection;
                            this.cachedValue = values[this.curSelected];
                            this.onChanged?.Invoke( this.cachedValue );
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
        }
    }
}
#endif