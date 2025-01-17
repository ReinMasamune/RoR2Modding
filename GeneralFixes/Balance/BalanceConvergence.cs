﻿namespace ReinGeneralFixes
{
    using System;
    using System.Reflection;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;

    using UnityEngine;

    internal partial class Main
    {
        private const Single radiusMult = 0.75f;
        private const Single rateMult = 0.8f;

        partial void BalanceConvergence()
        {
            Type type = typeof(HoldoutZoneController).GetNestedType( "FocusConvergenceController", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic );
            //cap = new StaticAccessor<Int32>( type, "cap" );
            this.Enable += this.Main_Enable6;
            this.Disable += this.Main_Disable6;
        }

        private void Main_Disable6()
        {
            HoldoutZoneController.FocusConvergenceController.cap = 3;

            HooksCore.RoR2.HoldoutZoneController.FocusConvergenceController.ApplyRadius.Il -= this.ApplyRadius_Il;
            HooksCore.RoR2.HoldoutZoneController.FocusConvergenceController.ApplyRate.Il -= this.ApplyRate_Il;
        }
        private void Main_Enable6()
        {
            HoldoutZoneController.FocusConvergenceController.cap = 10;

            HooksCore.RoR2.HoldoutZoneController.FocusConvergenceController.ApplyRadius.Il += this.ApplyRadius_Il;
            HooksCore.RoR2.HoldoutZoneController.FocusConvergenceController.ApplyRate.Il += this.ApplyRate_Il;
        }

        //private static StaticAccessor<Int32> cap;

        private void ApplyRate_Il( ILContext il )
        {
            var c = new ILCursor( il );
            _ = c.GotoNext( MoveType.After, x => x.MatchLdcI4( 0 ) );
            c.Index += 4;
            _ = c.RemoveRange( 2 );
            c.Index += 2;
            _ = c.RemoveRange( 4 );
            _ = c.EmitDelegate<Func<Single, Int32, Single>>( ( rate, count ) =>
              {
                  Single mult = 1f / Mathf.Pow( rateMult, count );
                  rate *= mult;
                  return rate;
              } );
        }
        private void ApplyRadius_Il( ILContext il )
        {

            var c = new ILCursor( il );
            _ = c.GotoNext( MoveType.After, x => x.MatchLdcI4( 0 ) );
            c.Index += 4;
            _ = c.Remove();
            c.Index += 2;
            _ = c.RemoveRange( 3 );
            _ = c.EmitDelegate<Func<Single, Int32, Single>>( ( rad, count ) =>
              {
                  Single mult = Mathf.Pow( radiusMult, count );
                  rad *= mult;
                  return rad;
              } );
        }
    }
}
