namespace ReinCore.Wooting
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    internal struct KeyColour
    {
        internal Byte r;
        internal Byte g;
        internal Byte b;

        internal KeyColour( Byte r, Byte g, Byte b )
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
