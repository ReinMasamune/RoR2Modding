namespace ReinGeneralFixes
{
    using System;

    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;

    internal partial class Main
    {
        partial void BalanceWillOWisp()
        {
            this.Enable += this.Main_Enable4;
            this.Disable += this.Main_Disable4;
        }

        private void Main_Disable4() => HooksCore.RoR2.GlobalEventManager.OnCharacterDeath.Il -= this.OnCharacterDeath_Il;
        private void Main_Enable4() => HooksCore.RoR2.GlobalEventManager.OnCharacterDeath.Il += this.OnCharacterDeath_Il;

        private void OnCharacterDeath_Il( ILContext il )
        {
            var c = new ILCursor( il );
            _ = c.GotoNext( MoveType.After, x => x.MatchLdloc( 15 ), x => x.MatchLdcI4( 4 ), x => x.MatchCallOrCallvirt<Inventory>( "GetItemCount" ), x => x.MatchStloc( 28 ), x => x.MatchLdloc( 28 ) );

            _ = c.GotoNext( MoveType.Before, x => x.MatchLdcR4( 3.5f ) );
            _ = c.RemoveRange( 10 );
            _ = c.Emit( OpCodes.Ldloc, 28 );
            _ = c.EmitDelegate<Func<Int32, Single>>( WillOWispDamage );

            _ = c.GotoNext( MoveType.Before, x => x.MatchLdcR4( 12f ), x => x.MatchLdcR4( 2.4f ) );
            _ = c.RemoveRange( 8 );
            _ = c.Emit( OpCodes.Ldc_R4, 15f );

        }


        private static Single WillOWispDamage( Int32 itemCount ) => 3f + itemCount;
    }
}
