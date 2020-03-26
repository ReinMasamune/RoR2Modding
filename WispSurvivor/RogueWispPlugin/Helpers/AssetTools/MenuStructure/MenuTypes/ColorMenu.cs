#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    /*
    internal static class ColorMenu
    {
        internal static Color Draw( Color inValue, MenuRenderer renderer, MenuAttribute settings )
        {
            var outValue = inValue;
            if( renderer.context == null ) renderer.context = new ColorContext( inValue, settings.name, (val) => outValue = val );
            var con = renderer.context as ColorContext;

            con.Draw();

            return outValue;
        }

        internal class ColorContext : MenuRenderer.MenuContext
        {
            internal ColorContext( Color inValue, String name, Action<Color> onChanged )
            {
                this.elem = new NamedColorTextEntry( inValue, name, onChanged );
            }

            private NamedColorTextEntry elem;

            internal void Draw()
            {
                this.elem.Draw();
            }
        }
    }
    */
}
#endif