using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using R2API;
using R2API.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using EntityStates;
using RoR2.Skills;

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        partial void FixOSP()
        {
            this.Enable += this.AddOSPFix;
            this.Disable += this.RemoveOSPFix;
        }

        private void RemoveOSPFix()
        {
            IL.RoR2.HealthComponent.TakeDamage -= this.HealthComponent_TakeDamage;
        }
        private void AddOSPFix()
        {
            IL.RoR2.HealthComponent.TakeDamage += this.HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage( ILContext il )
        {
            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<RoR2.HealthComponent>( "get_hasOneshotProtection" ) );
            c.Index += 3;
            c.RemoveRange( 7 );
            c.EmitDelegate<Func<Single, HealthComponent, Single>>( ( num, healthComp ) =>
            {
                var temp = num;
                temp -= healthComp.shield;
                temp -= healthComp.barrier;

                var protection = healthComp.fullHealth * 0.9f;
                if( temp <= protection )
                {
                    return num;
                }

                temp -= protection * 2f;
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