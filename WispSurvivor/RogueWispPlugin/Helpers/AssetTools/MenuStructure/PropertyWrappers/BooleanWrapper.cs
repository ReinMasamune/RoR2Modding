#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class BooleanWrapper<TMenu> : PropertyWrapper<TMenu>
    {
        private Dictionary<TMenu, BooleanContext> contextLookup = new Dictionary<TMenu,BooleanContext>();
        private Func<TMenu,Boolean> getter;
        private Action<TMenu,Boolean> setter;
        private MenuAttribute settings;
        private String name;
        private TMenu lastInstance;
        private BooleanContext lastContext;

        internal BooleanWrapper( PropertyInfo property, MenuAttribute settings )
        {
            var getMethod = property.GetGetMethod(true);
            var setMethod = property.GetSetMethod(true);
            if( getMethod == null || setMethod == null )
            {
                throw new ArgumentException( property.Name + " is missing a get or set method" );
            }
            this.getter = base.CompileGetter<Boolean>( getMethod );
            this.setter = base.CompileSetter<Boolean>( setMethod );
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

        private BooleanContext GetContext( TMenu instance )
        {
            if( !this.contextLookup.ContainsKey(instance) || this.contextLookup[instance] == null )
            {
                this.contextLookup[instance] = this.ChooseContext(instance);
            }

            return this.contextLookup[instance];
        }

        private BooleanContext ChooseContext( TMenu instance )
        {
            var inValue = this.getter(instance);
            return new Checkbox( inValue, this.name, ( val ) => this.setter( instance, val ) );
        }



        private abstract class BooleanContext
        {
            internal abstract void Draw();
        }

        private class Checkbox : BooleanContext
        {
            private NamedCheckbox elem;

            internal Checkbox( Boolean inValue, String name, Action<Boolean> onChanged )
            {
                this.elem = new NamedCheckbox( inValue, name, onChanged );
            }

            internal override void Draw()
            {
                this.elem.Draw();
            }
        }
    }
}
#endif