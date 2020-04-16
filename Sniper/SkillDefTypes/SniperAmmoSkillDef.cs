using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using RoR2.Networking;
using UnityEngine;
using KinematicCharacterController;
using EntityStates;
using RoR2.Skills;
using System.Reflection;
using Sniper.Expansions;

namespace Sniper.Skills
{
    internal class SniperAmmoSkillDef : SniperSkillDef
    {
        internal static BulletAttack.HitCallback defaultBulletHit { get; private set; }

        internal static SniperAmmoSkillDef Create( ExpandableBulletAttack.OnHitDelegate onHit, Data.BulletModifier modifier, GameObject hitEffect = null, GameObject tracerEffect = null )
        {
            var def = ScriptableObject.CreateInstance<SniperAmmoSkillDef>();

            def.onHitEffect = onHit;
            def.bulletModifier = modifier;
            def.hitEffectPrefab = hitEffect;
            def.tracerEffectPrefab = tracerEffect;


            def.activationState = SkillsCore.StateType<Idle>();
            def.activationStateMachineName = "";
            def.baseMaxStock = 0;
            def.baseRechargeInterval = 0f;
            def.beginSkillCooldownOnSkillEnd = false;
            def.canceledFromSprinting = false;
            def.fullRestockOnAssign = false;
            def.interruptPriority = InterruptPriority.Any;
            def.isBullets = false;
            def.isCombatSkill = false;
            def.mustKeyPress = false;
            def.noSprint = false;
            def.rechargeStock = 0;
            def.requiredStock = 0;
            def.shootDelay = 0f;
            def.stockToConsume = 0;

            return def;
        }


        private static Int32 currentIndex = 0;
        private static List<ExpandableBulletAttack.OnHitDelegate> hitCallbacksLookup = new List<ExpandableBulletAttack.OnHitDelegate>();
        private static Int32 AddCallback( ExpandableBulletAttack.OnHitDelegate onHit )
        {
            if( onHit == null )
            {
                return -1;
            }
            hitCallbacksLookup.Add( onHit );
            return currentIndex++;
        }

        [SerializeField]
        private Data.BulletModifier bulletModifier;
        private ExpandableBulletAttack.OnHitDelegate onHitEffect
        {
            get
            {
                if( this.callbackIndex < 0 )
                {
                    return null;
                }
                if( this.callbackIndex > hitCallbacksLookup.Count )
                {
                    Log.Error( "Out of range callback index, returning null" );
                    return null;
                }
                return hitCallbacksLookup[this.callbackIndex];
            }
            set
            {
                this.callbackIndex = AddCallback( value );
            }
        }
        [SerializeField]
        private Int32 callbackIndex;

        [SerializeField]
        private GameObject hitEffectPrefab;

        [SerializeField]
        private GameObject tracerEffectPrefab;

        internal void ModifyBullet( ExpandableBulletAttack bulletAttack )
        {
            var onHit = this.onHitEffect;
            if( onHit != null ) bulletAttack.onHit += onHit;
            if( this.hitEffectPrefab != null ) bulletAttack.hitEffectPrefab = this.hitEffectPrefab;
            if( this.tracerEffectPrefab != null ) bulletAttack.tracerEffectPrefab = this.tracerEffectPrefab;
            this.bulletModifier.Apply( bulletAttack );
        }
    }
}
