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
        partial void FixSelfDamage()
        {
            this.Enable += this.AddFixSelfDamage;
            this.Disable += this.RemoveFixSelfDamage;
        }

        private void RemoveFixSelfDamage() => HooksCore.RoR2.HealthComponent.TakeDamage.Il -= this.TakeDamage_Il1;
        private void AddFixSelfDamage() => HooksCore.RoR2.HealthComponent.TakeDamage.Il += this.TakeDamage_Il1;

        private void TakeDamage_Il1( ILContext il )
        {
            var c = new ILCursor( il );

            _ = c.GotoNext( MoveType.After,
                x => x.MatchLdcI4( (Int32)ItemIndex.Crowbar ),
                x => x.MatchCallOrCallvirt<Inventory>( "GetItemCount" )
            );
            _ = c.Emit( OpCodes.Ldarg_0 );
            _ = c.Emit<HealthComponent>( OpCodes.Ldfld, "body" );
            _ = c.Emit( OpCodes.Ldloc_1 );
            _ = c.EmitDelegate<Func<Int32, CharacterBody, CharacterBody, Int32>>( ModItemCount );

            _ = c.GotoNext( MoveType.After,
                x => x.MatchLdcI4( (Int32)ItemIndex.NearbyDamageBonus ),
                x => x.MatchCallOrCallvirt<Inventory>( "GetItemCount" )
            );
            _ = c.Emit( OpCodes.Ldarg_0 );
            _ = c.Emit<HealthComponent>( OpCodes.Ldfld, "body" );
            _ = c.Emit( OpCodes.Ldloc_1 );
            _ = c.EmitDelegate<Func<Int32, CharacterBody, CharacterBody, Int32>>( ModItemCount );

            _ = c.GotoNext( MoveType.After,
                x => x.MatchLdcI4( (Int32)ItemIndex.BossDamageBonus ),
                x => x.MatchCallOrCallvirt<Inventory>( "GetItemCount" )
            );
            _ = c.Emit( OpCodes.Ldarg_0 );
            _ = c.Emit<HealthComponent>( OpCodes.Ldfld, "body" );
            _ = c.Emit( OpCodes.Ldloc_1 );
            _ = c.EmitDelegate<Func<Int32, CharacterBody, CharacterBody, Int32>>( ModItemCount );
        }

        private static Int32 ModItemCount( Int32 count, CharacterBody body, CharacterBody body2 ) => body == body2 ? 0 : count;
    }

}
