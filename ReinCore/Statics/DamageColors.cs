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
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System.Reflection;

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
            curMax++;
            return (DamageColorIndex)ind;
        }

        static DamageColorsCore()
        {
            fieldRef = typeof( DamageColorsCore ).GetField( nameof( curMax ), BindingFlags.NonPublic | BindingFlags.Static );
            HooksCore.RoR2.DamageColor.FindColor.Il += FindColor_Il;
            loaded = true;
        }

        private static StaticAccessor<Color[]> damageColors = new StaticAccessor<Color[]>( typeof(DamageColor), "colors" );
        private static Int32 curMax = 8;
        private static FieldInfo fieldRef;

        private static void FindColor_Il( MonoMod.Cil.ILContext il )
        {
            var cursor = new ILCursor( il );
            _ = cursor.GotoNext( MoveType.AfterLabel, x => x.MatchLdcI4( 8 ) );
            _ = cursor.Remove();
            _ = cursor.Emit( OpCodes.Ldsfld, fieldRef );
        }
    }
}
