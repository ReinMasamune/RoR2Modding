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
        partial void BalanceCorpsebloom()
        {
            this.Enable += this.AddCorpsebloomEdits;
            this.Disable += this.RemoveCorpsebloomEdits;
        }

        private void RemoveCorpsebloomEdits()
        {
            HooksCore.RoR2.HealthComponent.Heal.Il -= this.Heal_Il;
            HooksCore.RoR2.HealthComponent.RepeatHealComponent.FixedUpdate.Il -= this.FixedUpdate_Il;
        }
        private void AddCorpsebloomEdits()
        {
            HooksCore.RoR2.HealthComponent.Heal.Il += this.Heal_Il;
            HooksCore.RoR2.HealthComponent.RepeatHealComponent.FixedUpdate.Il += this.FixedUpdate_Il;
        }

        private void FixedUpdate_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );
            c.GotoNext( MoveType.Before, x => x.MatchCallOrCallvirt<RoR2.HealthComponent>( "get_fullHealth" ) );
            c.Remove();
            c.EmitDelegate<Func<HealthComponent, Single>>( CorpseMaxHPCalc );
        }

        private void Heal_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );
            c.GotoNext( MoveType.After,
                x => x.MatchLdfld<RoR2.HealthComponent>( "repeatHealComponent" ),
                x => x.MatchLdcR4( 0.1f )
            );
            c.Index--;
            c.Remove();
            c.Emit( OpCodes.Ldc_R4, 0.15f );
            c.GotoNext( MoveType.Before, x => x.MatchCallOrCallvirt<RoR2.HealthComponent>( "get_fullHealth" ) );
            c.Remove();
            c.EmitDelegate<Func<HealthComponent, Single>>( CorpseMaxHPCalc );
            c.Emit( OpCodes.Ldarg_0 );
            c.Emit<RoR2.HealthComponent>( OpCodes.Ldfld, "repeatHealCount" );
            c.Emit( OpCodes.Conv_R4 );
            c.Emit( OpCodes.Ldc_R4, 2f );
            c.Emit( OpCodes.Mul );
            c.Emit( OpCodes.Mul );
        }

        private static Single CorpseMaxHPCalc( HealthComponent hc )
        {
            return hc.fullHealth + hc.fullShield * 0.25f;
        }
    }
}
