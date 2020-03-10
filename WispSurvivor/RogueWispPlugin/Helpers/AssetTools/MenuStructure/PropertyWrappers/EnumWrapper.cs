#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class EnumWrapper<TMenu,TEnum> : PropertyWrapper<TMenu> where TEnum : struct, Enum
    {
        private Dictionary<TMenu, EnumContext> contextLookup = new Dictionary<TMenu,EnumContext>();
        private Func<TMenu,TEnum> getter;
        private Action<TMenu,TEnum> setter;
        private MenuAttribute settings;
        private String name;
        private TMenu lastInstance;
        private EnumContext lastContext;

        internal EnumWrapper( PropertyInfo property, MenuAttribute settings )
        {
            var getMethod = property.GetGetMethod(true);
            var setMethod = property.GetSetMethod(true);
            if( getMethod == null || setMethod == null )
            {
                throw new ArgumentException( property.Name + " is missing a get or set method" );
            }
            this.getter = base.CompileGetter<TEnum>( getMethod );
            this.setter = base.CompileSetter<TEnum>( setMethod );
            this.settings = settings;
            if( String.IsNullOrEmpty( this.settings.name ) )
            {
                this.name = property.Name;
            } else
            {
                this.name = this.settings.name;
            }
        }

        internal override void Draw( TMenu instance )
        {
            if( this.lastInstance == null || this.lastContext == null || !this.lastInstance.Equals( instance ) )
            {
                this.lastContext = this.GetContext( instance );
            }

            this.lastContext.Draw();
        }

        private EnumContext GetContext( TMenu instance )
        {
            if( !this.contextLookup.ContainsKey(instance) || this.contextLookup[instance] == null )
            {
                this.contextLookup[instance] = this.ChooseContext(instance);
            }

            return this.contextLookup[instance];
        }

        private EnumContext ChooseContext( TMenu instance )
        {
            var inValue = this.getter(instance);
            return new HiddenSelection<TEnum>( inValue, this.name, ( val ) => this.setter( instance, val ) );
        }



        private abstract class EnumContext
        {
            internal abstract void Draw();
        }

        private class HiddenSelection<TEnum> : EnumContext where TEnum : struct, Enum
        {
            private NamedEnumHiddenSelection<TEnum> elem;

            internal HiddenSelection( TEnum inValue, String name, Action<TEnum> onChanged )
            {
                this.elem = new NamedEnumHiddenSelection<TEnum>( inValue, name, 4, onChanged );
            }

            internal override void Draw()
            {
                this.elem.Draw();
            }
        }
    }
}
#endif