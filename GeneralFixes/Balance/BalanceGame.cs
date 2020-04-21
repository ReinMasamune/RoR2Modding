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
        const Single diffMod = 0.0000000000000000000025f;
        partial void BalanceGame()
        {
            this.Enable += this.Main_Enable9;
            this.Disable += this.Main_Disable9;
        }

        private void Main_Disable9()
        {
            HooksCore.RoR2.Run.OnFixedUpdate.Il -= this.OnFixedUpdate_Il;
        }
        private void Main_Enable9()
        {
            HooksCore.RoR2.Run.OnFixedUpdate.Il += this.OnFixedUpdate_Il;
        }

        private void OnFixedUpdate_Il( ILContext il )
        {
            var c = new ILCursor( il );


            c.GotoNext( MoveType.After, x => x.MatchStfld<Run>( nameof( Run.difficultyCoefficient ) ) );
            c.GotoNext( MoveType.After, x => x.MatchLdcR4( 1.15f ) );
            c.EmitDelegate<Func<Single>>( () =>
            {
                UInt64 total = 0u;
                for( Int32 i = 0; i < NetworkUser.readOnlyInstancesList.Count; ++i )
                {
                    var player = NetworkUser.readOnlyInstancesList[i];
                    total += player.NetworknetLunarCoins;
                }

                return 2f - (1f / (1f + Mathf.Pow( diffMod, 1f / (Single)total )));
            } );
            c.Emit( OpCodes.Mul );
        }
    }
}
