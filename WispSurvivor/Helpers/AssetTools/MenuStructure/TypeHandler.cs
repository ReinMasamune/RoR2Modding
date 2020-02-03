#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal abstract class TypeHandler
    {
        internal static TypeHandler FindOrCreate( Type t )
        {
            if( typeLookup.ContainsKey( t ) && typeLookup[t] != null )
            {
                return typeLookup[t];
            } else
            {
                Main.LogE( "Unhandled type: " + t.Name );
                return null;
            }
        }
        private static Dictionary<Type, TypeHandler> typeLookup = new Dictionary<Type, TypeHandler>()
        {
            { typeof(Single), new GenericTypeHandler<Single>( (val, settings) =>
            {
                return val;
            })},



            { typeof(Boolean), new GenericTypeHandler<Boolean>( (val, settings) =>
            {
                return val;
            })},



            { typeof(Int32), new GenericTypeHandler<Int32>( (val, settings) =>
            {
                return val;
            })}
        };

        internal abstract object Draw( object instance, MenuAttribute settings );
    }
}
#endif