#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    /*
    internal static class Vector4Menu
    {
        internal static Vector4 Draw( Vector4 inValue, MenuRenderer renderer, MenuAttribute settings )
        {
            var outValue = inValue;
            if( renderer.context == null ) renderer.context = new Vector4Context( inValue, settings.name, (val) => outValue = val );
            var con = renderer.context as Vector4Context;

            con.Draw();

            return outValue;
        }

        internal class Vector4Context : MenuRenderer.MenuContext
        {
            internal Vector4Context( Vector4 inValue, String name, Action<Vector4> onChanged )
            {
                this.elem = new NamedVector4TextEntry( inValue, name, onChanged );
            }

            private NamedVector4TextEntry elem;

            internal void Draw()
            {
                this.elem.Draw();
            }
        }
    }
    */
}
#endif