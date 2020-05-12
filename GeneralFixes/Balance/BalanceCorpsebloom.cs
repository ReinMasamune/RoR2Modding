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
    using ReinCore;

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
            var c = new ILCursor( il );
            _ = c.GotoNext( MoveType.Before, x => x.MatchCallOrCallvirt<HealthComponent>( "get_fullHealth" ) );
            _ = c.Remove();
            _ = c.EmitDelegate<Func<HealthComponent, Single>>( CorpseMaxHPCalc );
        }

        private void Heal_Il( ILContext il )
        {
            var c = new ILCursor( il );
            _ = c.GotoNext( MoveType.After,
                x => x.MatchLdfld<HealthComponent>( "repeatHealComponent" ),
                x => x.MatchLdcR4( 0.1f )
            );
            c.Index--;
            _ = c.Remove();
            _ = c.Emit( OpCodes.Ldc_R4, 0.15f );
            _ = c.GotoNext( MoveType.Before, x => x.MatchCallOrCallvirt<HealthComponent>( "get_fullHealth" ) );
            _ = c.Remove();
            _ = c.EmitDelegate<Func<HealthComponent, Single>>( CorpseMaxHPCalc );
            _ = c.Emit( OpCodes.Ldarg_0 );
            _ = c.Emit<HealthComponent>( OpCodes.Ldfld, "repeatHealCount" );
            _ = c.Emit( OpCodes.Conv_R4 );
            _ = c.Emit( OpCodes.Ldc_R4, 2f );
            _ = c.Emit( OpCodes.Mul );
            _ = c.Emit( OpCodes.Mul );
        }

        private static Single CorpseMaxHPCalc( HealthComponent hc ) => hc.fullHealth + ( hc.fullShield * 0.25f );
    }
}
