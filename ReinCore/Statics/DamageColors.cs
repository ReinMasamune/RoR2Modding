using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Mono.Cecil;
using RoR2;
using RoR2.Networking;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine;

namespace ReinCore
{
    public static class DamageColorsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static DamageColorIndex AddDamageColor( Color color )
        {
            if( !loaded ) throw new CoreNotLoadedException( nameof( DamageColorsCore ) );

            Color[] colors = damageColors.Get();
            Int32 ind = colors.Length;
            if( colors.Length >= 255 ) throw new ArgumentOutOfRangeException( "Many rainbows" );

            Array.Resize<Color>( ref colors, ind + 1 );
            colors[ind] = color;
            damageColors.Set( colors );
            return (DamageColorIndex)ind;
        }

        static DamageColorsCore()
        {

            loaded = true;
        }

        private static StaticAccessor<Color[]> damageColors = new StaticAccessor<Color[]>( typeof(DamageColor), "colors" );
    }
}
