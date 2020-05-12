namespace ReinCore
{
    using System;
    using System.Reflection;

    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using RoR2;

    using UnityEngine;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class DamageColorsCore
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean loaded { get; internal set; } = false;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static DamageColorIndex AddDamageColor( Color color )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( DamageColorsCore ) );
            }

            Color[] colors = damageColors.Get();
            Int32 ind = colors.Length;
            if( colors.Length >= 255 )
            {
                throw new ArgumentOutOfRangeException( "Many rainbows" );
            }

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

        private static readonly StaticAccessor<Color[]> damageColors = new StaticAccessor<Color[]>( typeof(DamageColor), "colors" );
        private static Int32 curMax = 8;
        private static readonly FieldInfo fieldRef;

        private static void FindColor_Il( MonoMod.Cil.ILContext il )
        {
            var cursor = new ILCursor( il );
            _ = cursor.GotoNext( MoveType.AfterLabel, x => x.MatchLdcI4( 8 ) );
            _ = cursor.Remove();
            _ = cursor.Emit( OpCodes.Ldsfld, fieldRef );
        }
    }
}
