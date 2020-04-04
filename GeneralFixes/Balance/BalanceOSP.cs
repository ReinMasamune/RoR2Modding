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
using ReinCore;

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        partial void BalanceOSP()
        {
            this.Enable += this.AddOSPFix;
            this.Disable += this.RemoveOSPFix;
        }

        private void RemoveOSPFix()
        {
            HooksCore.RoR2.HealthComponent.TakeDamage.Il -= this.TakeDamage_Il;
        }
        private void AddOSPFix()
        {
            HooksCore.RoR2.HealthComponent.TakeDamage.Il += this.TakeDamage_Il;
        }

        private void TakeDamage_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<RoR2.HealthComponent>( "get_hasOneshotProtection" ) );
            c.GotoNext( MoveType.Before, x => x.MatchCallOrCallvirt<RoR2.HealthComponent>( "get_fullCombinedHealth" ) );
            c.RemoveRange( 2 );
            c.GotoNext( MoveType.Before, x => x.MatchLdfld<RoR2.HealthComponent>( "barrier" ) );
            c.RemoveRange( 2 );
            c.GotoNext( MoveType.Before, x => x.MatchMul() );
            c.RemoveRange( 2 );
            c.EmitDelegate<Func<Single, HealthComponent, Single, Single>>( ( num, healthComp, osp ) =>
            {
                var temp = num;
                temp -= healthComp.shield;
                temp -= healthComp.barrier;

                var protection = healthComp.fullHealth * osp;
                if( temp <= protection )
                {
                    return num;
                }
                temp -= healthComp.fullHealth * (1f + (1f / osp));
                temp = Mathf.Max( 0f, temp );
                temp += protection;
                temp += healthComp.shield;
                temp += healthComp.barrier;

                return temp;
            } );
        }
    }
}
/*
Changelist:
Commando Phase round 2s cooldown, phase blast 3s cooldown



















*/