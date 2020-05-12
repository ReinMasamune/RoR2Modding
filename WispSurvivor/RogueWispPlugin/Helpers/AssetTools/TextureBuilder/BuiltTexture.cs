using System;

using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal abstract class BuiltTexture
    {
        internal BuiltTexture( Int32 resX, Int32 resY )
        {
            this.tex = new Texture2D( resX, resY );
        }
        internal Texture2D tex;


    }
}
