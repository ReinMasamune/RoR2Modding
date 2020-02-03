#if MATEDITOR
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class Menu<TMenu>
    {
        internal Menu( TMenu instance )
        {
            this.structure = MenuStructure.FindOrCreate( typeof( TMenu ) );
            this.instance = instance;
        }
        private MenuStructure structure;
        private TMenu instance;

        internal void Draw()
        {
            this.structure.Draw( this.instance );
        }
    }


}
#endif