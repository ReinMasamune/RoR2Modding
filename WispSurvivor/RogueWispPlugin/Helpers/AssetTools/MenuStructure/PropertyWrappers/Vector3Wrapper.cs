#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using ReinCore;

namespace Rein.RogueWispPlugin.Helpers
{
    internal class Vector3Wrapper<TMenu> : PropertyWrapper<TMenu>
    {
        private Dictionary<TMenu, Vector3Context> contextLookup = new Dictionary<TMenu,Vector3Context>();
        private Func<TMenu,Vector3> getter;
        private Action<TMenu,Vector3> setter;
        private MenuAttribute settings;
        private String name;
        private TMenu lastInstance;
        private Vector3Context lastContext;

        internal Vector3Wrapper( PropertyInfo property, MenuAttribute settings )
        {
            var getMethod = property.GetGetMethod(true);
            var setMethod = property.GetSetMethod(true);
            if( getMethod == null || setMethod == null )
            {
                Main.LogE( property.Name + " is missing a get or set method" );
                throw new ArgumentException();
            }
            this.getter = base.CompileGetter<Vector3>( getMethod );
            this.setter = base.CompileSetter<Vector3>( setMethod );
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

        private Vector3Context GetContext( TMenu instance )
        {
            if( !this.contextLookup.ContainsKey(instance) || this.contextLookup[instance] == null )
            {
                this.contextLookup[instance] = this.ChooseContext(instance);
            }

            return this.contextLookup[instance];
        }

        private Vector3Context ChooseContext( TMenu instance )
        {
            var inValue = this.getter(instance);
            return new TextEntry( inValue, this.name, ( val ) => this.setter( instance, val ) );
        }



        private abstract class Vector3Context
        {
            internal abstract void Draw();
        }

        private class TextEntry : Vector3Context
        {
            private NamedVector3TextEntry elem;

            internal TextEntry( Vector3 inValue, String name, Action<Vector3> onChanged )
            {
                this.elem = new NamedVector3TextEntry( inValue, name, onChanged );
            }

            internal override void Draw()
            {
                this.elem.Draw();
            }
        }
    }
}
#endif