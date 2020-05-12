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
    using System.Collections;
    using ReinCore;

    internal partial class Main
    {
        private static readonly Dictionary<CharacterBody,CrocoDamageTypeController> componentCache = new Dictionary<CharacterBody, CrocoDamageTypeController>();
        private static Int32 crocoBodyIndex;

        partial void BalanceBlight()
        {
            this.Enable += this.Main_Enable3;
            this.Disable += this.Main_Disable3;
            this.FirstFrame += () => crocoBodyIndex = BodyCatalog.FindBodyIndex( Resources.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody" ) );
        }

        private void Main_Disable3()
        {
            HooksCore.RoR2.GlobalEventManager.OnHitEnemy.Il -= this.OnHitEnemy_Il1;
            onHitEffects -= BlightFromAcridHit;
            onHitEffects -= BlightFromDamageType;
        }
        private void Main_Enable3()
        {
            HooksCore.RoR2.GlobalEventManager.OnHitEnemy.Il += this.OnHitEnemy_Il1;
            onHitEffects += BlightFromAcridHit;
            onHitEffects += BlightFromDamageType;
        }

        private void OnHitEnemy_Il1( ILContext il )
        {
            var c = new ILCursor( il );
            _ = c.GotoNext( MoveType.AfterLabel, x => x.MatchLdarg( 1 ), x => x.MatchLdfld( typeof( DamageInfo ).GetField( nameof( DamageInfo.damageType ) ) ), x => x.MatchLdcI4( 1048576 ) );
            _ = c.RemoveRange( 24 );
        }

        private static void BlightFromAcridHit( DamageInfo damage, CharacterBody attacker, CharacterBody victim )
        {
            if( damage.dotIndex != DotController.DotIndex.None )
            {
                return;
            }

            if( !ShouldApplyBlight( attacker ) )
            {
                return;
            }

            Single dist = Math.Max( 0f, Vector3.Distance(damage.position, attacker.corePosition) - 10f );
            Single mult = 0.7f * ( 10 / (10+dist) );
            DotController.InflictDot( victim.gameObject, attacker.gameObject, DotController.DotIndex.Blight, 5f * damage.procCoefficient, mult );
        }

        private static void BlightFromDamageType( DamageInfo damage, CharacterBody attacker, CharacterBody victim )
        {
            if( (damage.damageType & DamageType.BlightOnHit) == 0 )
            {
                return;
            }

            DotController.InflictDot( victim.gameObject, attacker.gameObject, DotController.DotIndex.Blight, 5f * damage.procCoefficient, 0.7f );
        }

        private static Boolean ShouldApplyBlight( CharacterBody body )
        {
            if( body.bodyIndex != crocoBodyIndex )
            {
                return false;
            }

            if( !componentCache.ContainsKey( body ) )
            {
                componentCache[body] = body.GetComponent<CrocoDamageTypeController>();
            }
            CrocoDamageTypeController controller = componentCache[body];
            return controller.passiveSkillSlot.skillDef == controller.blightSkillDef;
        }
    }
}
