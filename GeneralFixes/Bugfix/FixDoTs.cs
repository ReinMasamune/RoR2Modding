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
        private delegate void OnHitDelegate(DamageInfo damage, CharacterBody attacker, CharacterBody victim);
        private static event OnHitDelegate onHitEffects;

        partial void FixDoTs()
        {
            this.Enable += this.Main_Enable7;
            this.Disable += this.Main_Disable7;

            onHitEffects += BleedFromDamageType;
            onHitEffects += BleedFromTriTip;
            onHitEffects += BurnFromDamageType;
            onHitEffects += PercentBurnFromAffix;
            onHitEffects += PercentBurnFromDamageType;
        }

        private void Main_Disable7() => HooksCore.RoR2.GlobalEventManager.OnHitEnemy.Il -= this.OnHitEnemy_Il2;
        private void Main_Enable7() => HooksCore.RoR2.GlobalEventManager.OnHitEnemy.Il += this.OnHitEnemy_Il2;

        private void OnHitEnemy_Il2( ILContext il )
        {
            var c = new ILCursor( il );
            _ = c.GotoNext( MoveType.AfterLabel, x => x.MatchLdarg( 1 ), x => x.MatchLdflda( typeof( DamageInfo ).GetField( nameof( DamageInfo.procChainMask ) ) ), x => x.MatchLdcI4( 5 ),
               x => x.MatchCallOrCallvirt<ProcChainMask>( "HasProc" ) );
            _ = c.Emit( OpCodes.Ldarg_1 );
            _ = c.Emit( OpCodes.Ldloc_0 );
            _ = c.Emit( OpCodes.Ldloc_1 );
            _ = c.EmitDelegate<OnHitDelegate>( ( damageInfo, attacker, victim ) => onHitEffects?.Invoke( damageInfo, attacker, victim ) );
            _ = c.RemoveRange( 50 );

            _ = c.GotoNext( MoveType.AfterLabel, x => x.MatchLdarg( 1 ), x => x.MatchLdfld( typeof( DamageInfo ).GetField( nameof( DamageInfo.damageType ) ) ), x => x.MatchLdcI4( 128 ) );
            _ = c.RemoveRange( 34 );

            //c.EmitDelegate<OnHitDelegate>( onHitEffects );
        }

        private static void BleedFromDamageType( DamageInfo damage, CharacterBody attacker, CharacterBody victim )
        {
            if( (damage.damageType & DamageType.BleedOnHit) == 0 ) return;
            DotController.InflictDot( victim.gameObject, attacker.gameObject, DotController.DotIndex.Bleed, 3f * damage.procCoefficient, 1f );
        }

        private static void BleedFromTriTip( DamageInfo damage, CharacterBody attacker, CharacterBody victim )
        {
            CharacterMaster master = attacker.master;
            if( !master ) return;
            Inventory inv = attacker.inventory;
            if( !inv ) return;
            Int32 itemCount = inv.GetItemCount( ItemIndex.BleedOnHit );
            if( itemCount <= 0 ) return;
            Single coef = Mathf.Sqrt(damage.procCoefficient);
            Single chance = Mathf.Min(100f,  15f * itemCount  * coef );
            if( !Util.CheckRoll( chance, attacker.master ) ) return;
            DotController.InflictDot( victim.gameObject, attacker.gameObject, DotController.DotIndex.Bleed, 3f * coef, 1f );
        }

        private static void BurnFromDamageType( DamageInfo damage, CharacterBody attacker, CharacterBody victim )
        {
            if( (damage.damageType & DamageType.IgniteOnHit) == 0 ) return;
            DotController.InflictDot( victim.gameObject, attacker.gameObject, DotController.DotIndex.Burn, 4f * damage.procCoefficient, 1f );
        }

        private static void PercentBurnFromDamageType( DamageInfo damage, CharacterBody attacker, CharacterBody victim )
        {
            if( (damage.damageType & DamageType.PercentIgniteOnHit) == 0 ) return;
            DotController.InflictDot( victim.gameObject, attacker.gameObject, DotController.DotIndex.PercentBurn, 4f * damage.procCoefficient, 1f );
        }

        private static void PercentBurnFromAffix( DamageInfo damage, CharacterBody attacker, CharacterBody victim )
        {
            if( !attacker.HasBuff( BuffIndex.AffixRed ) ) return;
            DotController.InflictDot( victim.gameObject, attacker.gameObject, DotController.DotIndex.PercentBurn, 4f * damage.procCoefficient, 1f );
        }
    }
}
