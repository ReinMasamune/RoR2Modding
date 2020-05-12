namespace ReinGeneralFixes
{
    using BepInEx;
    using RoR2;
    using UnityEngine;
    using System.Collections.Generic;
    using RoR2.Navigation;
    using Mono.Cecil.Cil;
    using MonoMod.Cil;
    using System;
    using System.Reflection;
    using EntityStates;
    using RoR2.Skills;
    using System.Collections;
    using ReinCore;

    internal partial class Main
    {
        const Single radiusMult = 0.75f;
        const Single rateMult = 0.9f;

        partial void BalanceConvergence()
        {
            Type type = typeof(HoldoutZoneController).GetNestedType( "FocusConvergenceController", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic );
            cap = new StaticAccessor<Int32>( type, "cap" );
            this.Enable += this.Main_Enable6;
            this.Disable += this.Main_Disable6;
        }

        private void Main_Disable6()
        {
            cap.Set( 3 );

            HooksCore.RoR2.HoldoutZoneController.FocusConvergenceController.ApplyRadius.Il -= this.ApplyRadius_Il;
            HooksCore.RoR2.HoldoutZoneController.FocusConvergenceController.ApplyRate.Il -= this.ApplyRate_Il;
        }
        private void Main_Enable6()
        {
            cap.Set( 10 );

            HooksCore.RoR2.HoldoutZoneController.FocusConvergenceController.ApplyRadius.Il += this.ApplyRadius_Il;
            HooksCore.RoR2.HoldoutZoneController.FocusConvergenceController.ApplyRate.Il += this.ApplyRate_Il;
        }

        private static StaticAccessor<Int32> cap;

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
