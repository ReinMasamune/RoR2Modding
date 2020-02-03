#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class MenuRenderer
    {
        internal MenuRenderer( PropertyInfo property, MenuAttribute settings )
        {
            var type = property.PropertyType;
            this.handler = TypeHandler.FindOrCreate( type );
            if( String.IsNullOrEmpty( settings.name ) )
            {

            }

        }
        internal void Draw( System.Object instance )
        {
            var initValue = this.get( instance );
            initValue = this.handler.Draw( initValue, this.settings );
            this.set( initValue, instance );
        }


        private TypeHandler handler;
        private Func<System.Object,System.Object> get;
        private Action<System.Object,System.Object> set;
        private MenuAttribute settings;
    }
}
#endif