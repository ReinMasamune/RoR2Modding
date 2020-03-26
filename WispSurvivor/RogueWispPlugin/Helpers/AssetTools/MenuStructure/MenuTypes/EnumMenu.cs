#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    /*
    internal static class EnumMenu<TEnum> where TEnum : struct, System.Enum
    {
        internal static TEnum Draw( TEnum inValue, MenuRenderer renderer, MenuAttribute settings )
        {
            var outValue = inValue;
            if( renderer.context == null ) renderer.context = new EnumContext<TEnum>( inValue, settings.name, 5, (val) =>
            {
                outValue = val;
                Main.LogI( "Set" );
                Main.LogI( val.ToString() );
                Main.LogI( outValue.ToString() );
            } );
            var con = renderer.context as EnumContext<TEnum>;

            con.Draw();

            return outValue;
        }

        internal class EnumContext<TEnum> : MenuRenderer.MenuContext where TEnum : struct, System.Enum
        {
            internal EnumContext( TEnum inValue, String name, Int32 numCols, Action<TEnum> onChanged )
            {
                this.elem = new NamedEnumHiddenSelection<TEnum>( inValue, name, numCols, onChanged );
            }

            private NamedEnumHiddenSelection<TEnum> elem;

            internal void Draw()
            {
                this.elem.Draw();
            }

        }
    }
    */
}
#endif