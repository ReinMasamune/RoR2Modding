#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class Int32Wrapper<TMenu> : PropertyWrapper<TMenu>
    {
        private Dictionary<TMenu, Int32Context> contextLookup = new Dictionary<TMenu,Int32Context>();
        private Func<TMenu,Int32> getter;
        private Action<TMenu,Int32> setter;
        private MenuAttribute settings;
        private String name;
        private TMenu lastInstance;
        private Int32Context lastContext;

        internal Int32Wrapper( PropertyInfo property, MenuAttribute settings )
        {
            var getMethod = property.GetGetMethod(true);
            var setMethod = property.GetSetMethod(true);
            if( getMethod == null || setMethod == null )
            {
                Main.LogE( property.Name + " is missing a get or set method" );
                throw new ArgumentException();
            }
            this.getter = base.CompileGetter<Int32>( getMethod );
            this.setter = base.CompileSetter<Int32>( setMethod );
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

        private Int32Context GetContext( TMenu instance )
        {
            if( !this.contextLookup.ContainsKey(instance) || this.contextLookup[instance] == null )
            {
                this.contextLookup[instance] = this.ChooseContext(instance);
            }

            return this.contextLookup[instance];
        }

        private Int32Context ChooseContext( TMenu instance )
        {
            var inValue = this.getter(instance);
            return new TextEntry( inValue, this.name, ( val ) => this.setter( instance, val ) );
        }



        private abstract class Int32Context
        {
            internal abstract void Draw();
        }

        private class TextEntry : Int32Context
        {
            private NamedInt32TextEntry elem;

            internal TextEntry( Int32 inValue, String name, Action<Int32> onChanged )
            {
                this.elem = new NamedInt32TextEntry( inValue, name, onChanged );
            }

            internal override void Draw()
            {
                this.elem.Draw();
            }
        }
    }
}
#endif