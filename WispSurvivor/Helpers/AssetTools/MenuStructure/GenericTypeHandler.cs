#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class GenericTypeHandler<THandled> : TypeHandler
    {
        internal Func<THandled,MenuAttribute,THandled> draw;
        internal GenericTypeHandler( Func<THandled,MenuAttribute,THandled> drawFunc )
        {
            this.draw = drawFunc;
        }

        internal override object Draw( object instance, MenuAttribute settings )
        {
            return this.draw( (THandled)instance, settings );
        }
    }

}
#endif