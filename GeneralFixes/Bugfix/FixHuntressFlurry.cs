namespace ReinGeneralFixes
{
    using System;
    using System.Reflection;

    using EntityStates.Huntress.HuntressWeapon;

    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using UnityEngine.Networking;

    internal partial class Main
    {
        partial void FixHuntressFlurry()
        {
            this.Enable += this.Main_Enable1;
            this.Disable += this.Main_Disable1;
        }

        private void Main_Disable1()
        {

        }
        private void Main_Enable1()
        {
            //HooksCore.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.FireOrbArrow.Il += this.FireOrbArrow_Il;
        }

        private void FireOrbArrow_Il( ILContext il )
        {
            Type type = typeof(FireSeekingArrow);
            BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
            FieldInfo arrowReloadDuration = type.GetField( "arrowReloadDuration", allFlags );
            FieldInfo arrowReloadTimer = type.GetField( "arrowReloadTimer", allFlags );
            FieldInfo firedArrowCount = type.GetField( "firedArrowCount", allFlags );
            FieldInfo maxArrowCount = type.GetField( "maxArrowCount", allFlags );


            var c = new ILCursor( il );
            ILLabel label = null;

            _ = c.GotoNext( MoveType.AfterLabel, x => x.MatchLdarg( 0 ) );
            _ = c.RemoveRange( 10 );

            _ = c.GotoNext( MoveType.AfterLabel, x => x.MatchBrtrue( out label ) );

            ILLabel retLabel = null;
            _ = c.GotoNext( MoveType.AfterLabel, x => x.MatchBrfalse( out retLabel ) );
            _ = c.GotoLabel( retLabel, MoveType.Before );
            _ = c.Emit( OpCodes.Br, label );

            _ = c.GotoLabel( label, MoveType.AfterLabel );

            _ = c.Emit( OpCodes.Ldarg_0 );
            _ = c.Emit( OpCodes.Ldfld, firedArrowCount );
            _ = c.Emit( OpCodes.Ldarg_0 );
            _ = c.Emit( OpCodes.Ldfld, maxArrowCount );
            _ = c.Emit( OpCodes.Bge, retLabel );

            _ = c.Emit( OpCodes.Ldarg_0 );
            _ = c.Emit( OpCodes.Ldfld, arrowReloadTimer );
            _ = c.Emit( OpCodes.Ldc_R4, 0f );
            _ = c.Emit( OpCodes.Bge, retLabel );

            _ = c.GotoNext( MoveType.After, x => x.MatchLdfld( arrowReloadDuration ) );
            _ = c.Emit( OpCodes.Ldarg_0 );
            _ = c.Emit( OpCodes.Ldfld, arrowReloadTimer );
            _ = c.Emit( OpCodes.Add );
        }
    }
}
