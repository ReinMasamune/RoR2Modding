using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReinCore.Wooting
{
    internal struct KeyColour
    {
        internal byte r;
        internal byte g;
        internal byte b;

        internal KeyColour( byte r, byte g, byte b )
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public static implicit operator KeyColour( Color32 col ) => new KeyColour( col.r, col.g, col.b );
        public static implicit operator KeyColour( Color col ) => new KeyColour( 
            (Byte)(Mathf.Clamp01(  col.r * col.r ) * 255),
            (Byte)(Mathf.Clamp01(  col.g * col.g ) * 255),
            (Byte)(Mathf.Clamp01(  col.b * col.b ) * 255));
    }
}
