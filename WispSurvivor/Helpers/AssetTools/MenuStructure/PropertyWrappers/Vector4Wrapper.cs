#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class Vector4Wrapper<TMenu> : PropertyWrapper<TMenu>
    {
        private Dictionary<TMenu, Vector4Context> contextLookup = new Dictionary<TMenu,Vector4Context>();
        private Func<TMenu,Vector4> getter;
        private Action<TMenu,Vector4> setter;
        private MenuAttribute settings;
        private String name;
        private TMenu lastInstance;
        private Vector4Context lastContext;

        internal Vector4Wrapper( PropertyInfo property, MenuAttribute settings )
        {
            var getMethod = property.GetGetMethod(true);
            var setMethod = property.GetSetMethod(true);
            if( getMethod == null || setMethod == null )
            {
                Main.LogE( property.Name + " is missing a get or set method" );
                throw new ArgumentException();
            }
            this.getter = base.CompileGetter<Vector4>( getMethod );
            this.setter = base.CompileSetter<Vector4>( setMethod );
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

        private Vector4Context GetContext( TMenu instance )
        {
            if( !this.contextLookup.ContainsKey(instance) || this.contextLookup[instance] == null )
            {
                this.contextLookup[instance] = this.ChooseContext(instance);
            }

            return this.contextLookup[instance];
        }

        private Vector4Context ChooseContext( TMenu instance )
        {
            var inValue = this.getter(instance);
            return new TextEntry( inValue, this.name, ( val ) => this.setter( instance, val ) );
        }



        private abstract class Vector4Context
        {
            internal abstract void Draw();
        }

        private class TextEntry : Vector4Context
        {
            private NamedVector4TextEntry elem;

            internal TextEntry( Vector4 inValue, String name, Action<Vector4> onChanged )
            {
                this.elem = new NamedVector4TextEntry( inValue, name, onChanged );
            }

            internal override void Draw()
            {
                this.elem.Draw();
            }
        }
    }
}
#endif