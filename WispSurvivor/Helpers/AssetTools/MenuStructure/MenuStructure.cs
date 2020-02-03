#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class MenuStructure
    {
        internal static MenuStructure FindOrCreate( Type t )
        {
            if( typeLookup.ContainsKey( t ) && typeLookup[t] != null )
            {
                return typeLookup[t];
            } else
            {
                return new MenuStructure( t );
            }

        }

        internal void Draw( object obj )
        {

        }

        private static Dictionary<Type,MenuStructure> typeLookup = new Dictionary<Type, MenuStructure>();
        private MenuStructure( Type t )
        {
            typeLookup[t] = this;
            this.type = t;

            var tempList = new List<MenuRenderer>();
            foreach( var p in t.GetProperties( BindingFlags.Public | BindingFlags.Instance ) )
            {
                var atr = p.GetCustomAttribute<MenuAttribute>();
                if( atr == null ) continue;
                
                
                tempList.Add( new MenuRenderer( p, atr ) );
            }


        }
        private Type type;
        //private List<MenuRenderer> renderers;

    }
}
#endif