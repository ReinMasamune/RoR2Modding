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

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        partial void BalanceWillOWisp()
        {
            this.Enable += this.Main_Enable4;
            this.Disable += this.Main_Disable4;
        }

        private void Main_Disable4()
        {
            HooksCore.RoR2.GlobalEventManager.OnCharacterDeath.Il -= this.OnCharacterDeath_Il;
        }
        private void Main_Enable4()
        {
            HooksCore.RoR2.GlobalEventManager.OnCharacterDeath.Il += this.OnCharacterDeath_Il;
        }

        private void OnCharacterDeath_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );
            c.GotoNext( MoveType.After, x => x.MatchLdloc( 15 ), x => x.MatchLdcI4( 4 ), x => x.MatchCallOrCallvirt<RoR2.Inventory>( "GetItemCount" ), x => x.MatchStloc( 28 ), x => x.MatchLdloc( 28 ) );

            c.GotoNext( MoveType.Before, x => x.MatchLdcR4( 3.5f ) );
            c.RemoveRange( 10 );
            c.Emit( OpCodes.Ldloc, 28 );
            c.EmitDelegate<Func<Int32, Single>>( WillOWispDamage );

            c.GotoNext( MoveType.Before, x => x.MatchLdcR4( 12f ), x => x.MatchLdcR4( 2.4f ) );
            c.RemoveRange( 8 );
            c.Emit( OpCodes.Ldc_R4, 15f );

        }


        private static Single WillOWispDamage( Int32 itemCount )
        {
            return 3f + itemCount;
        }
    }
}
