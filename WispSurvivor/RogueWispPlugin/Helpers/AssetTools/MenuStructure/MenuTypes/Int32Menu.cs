#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    /*
    internal static class Int32Menu
    {
        internal static Int32 Draw( Int32 inValue, MenuRenderer renderer, MenuAttribute settings )
        {
            var outValue = inValue;
            if( renderer.context == null ) renderer.context = new Int32Context( inValue, settings.name, (val) => outValue = val );
            var con = renderer.context as Int32Context;

            con.Draw();

            return outValue;
        }

        internal class Int32Context : MenuRenderer.MenuContext
        {
            internal Int32Context( Int32 inValue, String name, Action<Int32> onChanged )
            {
                this.elem = new NamedInt32TextEntry( inValue, name, onChanged );
            }

            private NamedInt32TextEntry elem;

            internal void Draw()
            {
                this.elem.Draw();
            }
        }
    }
    */
}
#endif