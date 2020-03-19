#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using ReinCore;

namespace RogueWispPlugin.Helpers
{
    internal class Vector2Wrapper<TMenu> : PropertyWrapper<TMenu>
    {
        private Dictionary<TMenu, Vector2Context> contextLookup = new Dictionary<TMenu,Vector2Context>();
        private Func<TMenu,Vector2> getter;
        private Action<TMenu,Vector2> setter;
        private MenuAttribute settings;
        private String name;
        private TMenu lastInstance;
        private Vector2Context lastContext;

        internal Vector2Wrapper( PropertyInfo property, MenuAttribute settings )
        {
            var getMethod = property.GetGetMethod(true);
            var setMethod = property.GetSetMethod(true);
            if( getMethod == null || setMethod == null )
            {
                Main.LogE( property.Name + " is missing a get or set method" );
                throw new ArgumentException();
            }
            this.getter = base.CompileGetter<Vector2>( getMethod );
            this.setter = base.CompileSetter<Vector2>( setMethod );
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

        private Vector2Context GetContext( TMenu instance )
        {
            if( !this.contextLookup.ContainsKey(instance) || this.contextLookup[instance] == null )
            {
                this.contextLookup[instance] = this.ChooseContext(instance);
            }

            return this.contextLookup[instance];
        }

        private Vector2Context ChooseContext( TMenu instance )
        {
            var inValue = this.getter(instance);
            return new TextEntry( inValue, this.name, ( val ) => this.setter( instance, val ) );
        }



        private abstract class Vector2Context
        {
            internal abstract void Draw();
        }

        private class TextEntry : Vector2Context
        {
            private NamedVector2TextEntry elem;

            internal TextEntry( Vector2 inValue, String name, Action<Vector2> onChanged )
            {
                this.elem = new NamedVector2TextEntry( inValue, name, onChanged );
            }

            internal override void Draw()
            {
                this.elem.Draw();
            }
        }
    }
}
#endif