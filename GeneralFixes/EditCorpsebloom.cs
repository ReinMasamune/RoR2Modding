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
        partial void EditCorpsebloom()
        {
            this.Enable += this.AddCorpsebloomEdits;
            this.Disable += this.RemoveCorpsebloomEdits;
        }

        private void RemoveCorpsebloomEdits()
        {
            IL.RoR2.HealthComponent.Heal -= this.HealthComponent_Heal;
            IL.RoR2.HealthComponent.RepeatHealComponent.FixedUpdate -= this.RepeatHealComponent_FixedUpdate;
        }
        private void AddCorpsebloomEdits()
        {
            IL.RoR2.HealthComponent.Heal += this.HealthComponent_Heal;
            IL.RoR2.HealthComponent.RepeatHealComponent.FixedUpdate += this.RepeatHealComponent_FixedUpdate;
        }

        private void RepeatHealComponent_FixedUpdate( ILContext il )
        {
            ILCursor c = new ILCursor( il );
            c.GotoNext( MoveType.Before, x => x.MatchCallOrCallvirt<RoR2.HealthComponent>( "get_fullHealth" ));
            c.Remove();
            c.EmitDelegate<Func<HealthComponent, Single>>( CorpseMaxHPCalc );
        }

        private void HealthComponent_Heal( ILContext il )
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
