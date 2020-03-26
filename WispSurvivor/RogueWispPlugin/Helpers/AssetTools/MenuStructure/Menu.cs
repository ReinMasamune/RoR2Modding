#if MATEDITOR
using System;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal class Menu<TMenu>
    {
        internal Menu( TMenu instance )
        {
            this.instance = instance;
        }
        private TMenu instance;

        internal void Draw()
        {
            MenuStructure<TMenu>.Draw( this.instance );
        }
    }


}
#endif