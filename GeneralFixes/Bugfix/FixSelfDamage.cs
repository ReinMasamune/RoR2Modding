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
        partial void FixSelfDamage()
        {
            this.Enable += this.AddFixSelfDamage;
            this.Disable += this.RemoveFixSelfDamage;
        }

        private void RemoveFixSelfDamage()
        {
            IL.RoR2.HealthComponent.TakeDamage -= this.HealthComponent_TakeDamage1;
        }
        private void AddFixSelfDamage()
        {
            IL.RoR2.HealthComponent.TakeDamage += this.HealthComponent_TakeDamage1;
        }

        private void HealthComponent_TakeDamage1( ILContext il )
        {
            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.After,
                x => x.MatchLdcI4( (Int32)ItemIndex.Crowbar ),
                x => x.MatchCallOrCallvirt<RoR2.Inventory>( "GetItemCount" )
            );
            c.Emit( OpCodes.Ldarg_0 );
            c.Emit<RoR2.HealthComponent>( OpCodes.Ldfld, "body" );
            c.Emit( OpCodes.Ldloc_1 );
            c.EmitDelegate<Func<Int32, CharacterBody, CharacterBody, Int32>>( ModItemCount );

            c.GotoNext( MoveType.After,
                x => x.MatchLdcI4( (Int32)ItemIndex.NearbyDamageBonus ),
                x => x.MatchCallOrCallvirt<RoR2.Inventory>( "GetItemCount" )
            );
            c.Emit( OpCodes.Ldarg_0 );
            c.Emit<RoR2.HealthComponent>( OpCodes.Ldfld, "body" );
            c.Emit( OpCodes.Ldloc_1 );
            c.EmitDelegate<Func<Int32, CharacterBody, CharacterBody, Int32>>( ModItemCount );

            c.GotoNext( MoveType.After,
                x => x.MatchLdcI4( (Int32)ItemIndex.BossDamageBonus ),
                x => x.MatchCallOrCallvirt<RoR2.Inventory>( "GetItemCount" )
            );
            c.Emit( OpCodes.Ldarg_0 );
            c.Emit<RoR2.HealthComponent>( OpCodes.Ldfld, "body" );
            c.Emit( OpCodes.Ldloc_1 );
            c.EmitDelegate<Func<Int32, CharacterBody, CharacterBody, Int32>>( ModItemCount );
        }

        private static Int32 ModItemCount( Int32 count, CharacterBody body, CharacterBody body2 )
        {
            if( body == body2 ) return 0; else return count;
        }
    }

}
