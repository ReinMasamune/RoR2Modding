#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using ReinCore;

namespace Rein.RogueWispPlugin.Helpers
{
    internal class ColorWrapper<TMenu> : PropertyWrapper<TMenu>
    {
        private Dictionary<TMenu, ColorContext> contextLookup = new Dictionary<TMenu,ColorContext>();
        private Func<TMenu,Color> getter;
        private Action<TMenu,Color> setter;
        private MenuAttribute settings;
        private String name;
        private TMenu lastInstance;
        private ColorContext lastContext;

        internal ColorWrapper( PropertyInfo property, MenuAttribute settings )
        {
            var getMethod = property.GetGetMethod(true);
            var setMethod = property.GetSetMethod(true);
            if( getMethod == null || setMethod == null )
            {
                Main.LogE( property.Name + " is missing a get or set method" );
                throw new ArgumentException();
            }
            this.getter = base.CompileGetter<Color>( getMethod );
            this.setter = base.CompileSetter<Color>( setMethod );
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

        private ColorContext GetContext( TMenu instance )
        {
            if( !this.contextLookup.ContainsKey(instance) || this.contextLookup[instance] == null )
            {
                this.contextLookup[instance] = this.ChooseContext(instance);
            }

            return this.contextLookup[instance];
        }

        private ColorContext ChooseContext( TMenu instance )
        {
            var inValue = this.getter(instance);
            return new TextEntry( inValue, this.name, ( val ) => this.setter( instance, val ) );
        }



        private abstract class ColorContext
        {
            internal abstract void Draw();
        }

        private class TextEntry : ColorContext
        {
            private NamedColorTextEntry elem;

            internal TextEntry( Color inValue, String name, Action<Color> onChanged )
            {
                this.elem = new NamedColorTextEntry( inValue, name, onChanged );
            }

            internal override void Draw()
            {
                this.elem.Draw();
            }
        }
    }
}
#endif