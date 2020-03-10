#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    /*
    internal static class Vector3Menu
    {
        internal static Vector3 Draw( Vector3 inValue, MenuRenderer renderer, MenuAttribute settings )
        {
            var outValue = inValue;
            if( renderer.context == null ) renderer.context = new Vector3Context( inValue, settings.name, (val) => outValue = val );
            var con = renderer.context as Vector3Context;

            con.Draw();

            return outValue;
        }

        internal class Vector3Context : MenuRenderer.MenuContext
        {
            internal Vector3Context( Vector3 inValue, String name, Action<Vector3> onChanged )
            {
                this.elem = new NamedVector3TextEntry( inValue, name, onChanged );
            }

            private NamedVector3TextEntry elem;

            internal void Draw()
            {
                this.elem.Draw();
            }
        }
    }
    */
}
#endif