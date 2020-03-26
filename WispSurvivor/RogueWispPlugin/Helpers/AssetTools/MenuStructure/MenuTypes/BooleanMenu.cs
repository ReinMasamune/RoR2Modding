#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers.IMGUI;

namespace Rein.RogueWispPlugin.Helpers
{
    /*
    internal static class BooleanMenu
    {
        internal static Boolean Draw( Boolean inValue, MenuRenderer renderer, MenuAttribute settings )
        {
            var outValue = inValue;
            if( renderer.context == null ) renderer.context = new BooleanContext( inValue, settings.name, (res) => outValue = res );
            var con = renderer.context as BooleanContext;

            con.Draw();

            return outValue;
        }

        internal class BooleanContext : MenuRenderer.MenuContext
        {
            internal BooleanContext( Boolean inValue, String name, Action<Boolean> onChanged )
            {
                this.elem = new NamedCheckbox( inValue, name, onChanged );
            }
            private NamedCheckbox elem;

            internal void Draw()
            {
                this.elem.Draw();
            }

        }
    }
    */
}
#endif