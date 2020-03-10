#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers.IMGUI;

namespace RogueWispPlugin.Helpers
{
    internal class NamedCheckbox
    {
        internal NamedCheckbox( Boolean startValue, String name, Action<Boolean> onChanged )
        {
            this.onChanged = onChanged;
            this.cachedValue = startValue;
            this.name = name;
        }
        internal void ChangeValue( Boolean newValue )
        {
            this.cachedValue = newValue;
        }
        internal void ChangeName( String name )
        {
            this.name = name;
        }
        internal void ChangeAction( Action<Boolean> onChanged )
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
                var res = GUILayout.Toggle( this.cachedValue, "Toggle" );
                if( GUI.changed && res != this.cachedValue )
                {
                    this.cachedValue = res;
                    this.onChanged?.Invoke( this.cachedValue );
                }
            }
            GUILayout.EndHorizontal();
        }

        private Action<Boolean> onChanged;
        private Boolean cachedValue;
        private String name;
    }
}
#endif