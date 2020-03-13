#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    /*
    internal static class Vector2Menu
    {
        internal static Vector2 Draw( Vector2 inValue, MenuRenderer renderer, MenuAttribute settings )
        {
            var outValue = inValue;
            if( renderer.context == null ) renderer.context = new Vector2Context( inValue, settings.name, (val) => outValue = val );
            var con = renderer.context as Vector2Context;

            con.Draw();

            return outValue;
        }

        internal class Vector2Context : MenuRenderer.MenuContext
        {
            internal Vector2Context( Vector2 inValue, String name, Action<Vector2> onChanged )
            {
                this.elem = new NamedVector2TextEntry( inValue, name, onChanged );
            }

            private NamedVector2TextEntry elem;

            internal void Draw()
            {
                this.elem.Draw();
            }
        }
    }
    */
}
#endif