namespace ReinCore
{
    using System;
    using System.Reflection;
    using Mono.Cecil.Cil;
    using MonoMod.Cil;
    using RoR2;
    using UnityEngine;

    public static class DamageColorsCore
    {
        public static Boolean loaded { get; internal set; } = false;
        public static DamageColorIndex AddDamageColor( Color color )
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( DamageColorsCore ) );
            }

            Int32 ind = DamageColor.colors.Length;
            if( ind >= 255 )
            {
                throw new ArgumentOutOfRangeException( "Too many rainbows" );
            }

            Array.Resize<Color>( ref DamageColor.colors, ind + 1 );
            DamageColor.colors[ind] = color;
            curMax++;
            return (DamageColorIndex)ind;
        }

        static DamageColorsCore()
        {
            fieldRef = typeof( DamageColorsCore ).GetField( nameof( curMax ), BindingFlags.NonPublic | BindingFlags.Static );
            HooksCore.RoR2.DamageColor.FindColor.Il += FindColor_Il;
            loaded = true;
        }

        private static Int32 curMax = 8;
        private static readonly FieldInfo fieldRef;

        private static void FindColor_Il(MonoMod.Cil.ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.AfterLabel, x => x.MatchLdcI4((Int32)DamageColorIndex.Count))
            .Remove()
            .LdSFld_(fieldRef);
    }
}
