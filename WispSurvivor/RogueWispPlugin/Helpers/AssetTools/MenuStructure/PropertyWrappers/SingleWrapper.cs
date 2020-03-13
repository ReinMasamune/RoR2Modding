#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class SingleWrapper<TMenu> : PropertyWrapper<TMenu>
    {
        private Dictionary<TMenu, SingleContext> contextLookup = new Dictionary<TMenu,SingleContext>();
        private Func<TMenu,Single> getter;
        private Action<TMenu,Single> setter;
        private MenuAttribute settings;
        private String name;
        private TMenu lastInstance;
        private SingleContext lastContext;

        internal SingleWrapper( PropertyInfo property, MenuAttribute settings )
        {
            var getMethod = property.GetGetMethod(true);
            var setMethod = property.GetSetMethod(true);
            if( getMethod == null || setMethod == null )
            {
                Main.LogE( property.Name + " is missing a get or set method" );
                throw new ArgumentException();
            }
            this.getter = base.CompileGetter<Single>( getMethod );
            this.setter = base.CompileSetter<Single>( setMethod );
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

        private SingleContext GetContext( TMenu instance )
        {
            if( !this.contextLookup.ContainsKey(instance) || this.contextLookup[instance] == null )
            {
                this.contextLookup[instance] = this.ChooseContext(instance);
            }

            return this.contextLookup[instance];
        }

        private SingleContext ChooseContext( TMenu instance )
        {
            var inValue = this.getter(instance);
            return new TextEntry( inValue, this.name, ( val ) => this.setter( instance, val ) );
        }



        private abstract class SingleContext
        {
            internal abstract void Draw();
        }

        private class TextEntry : SingleContext
        {
            private NamedSingleTextEntry elem;

            internal TextEntry( Single inValue, String name, Action<Single> onChanged )
            {
                this.elem = new NamedSingleTextEntry( inValue, name, onChanged );
            }

            internal override void Draw()
            {
                this.elem.Draw();
            }
        }
    }
}
#endif