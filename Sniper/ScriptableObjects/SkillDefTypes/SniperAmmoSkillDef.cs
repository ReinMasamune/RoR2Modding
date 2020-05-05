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
using Sniper.SkillDefTypes.Bases;

namespace Sniper.SkillDefs
{
    internal class SniperAmmoSkillDef : SniperSkillDef
    {
        internal static SniperAmmoSkillDef Create( OnBulletDelegate onHit, OnBulletDelegate onStop, Data.BulletModifier modifier, GameObject hitEffect = null, GameObject tracerEffect = null )
        {
            SniperAmmoSkillDef def = ScriptableObject.CreateInstance<SniperAmmoSkillDef>();

            def.onHitEffect = onHit;
            def.onEndEffect = onStop;
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

        [SerializeField]
        private Data.BulletModifier bulletModifier;

        private OnBulletDelegate onHitEffect;

        private OnBulletDelegate onEndEffect;

        [SerializeField]
        private GameObject hitEffectPrefab;

        [SerializeField]
        private GameObject tracerEffectPrefab;


        // TODO: Switch to having ammo initialize the bullet

        internal void ModifyBullet( ExpandableBulletAttack bulletAttack )
        {
            bulletAttack.onHit = this.onHitEffect;
            bulletAttack.onStop = this.onEndEffect;
            if( this.hitEffectPrefab != null ) bulletAttack.hitEffectPrefab = this.hitEffectPrefab;
            if( this.tracerEffectPrefab != null ) bulletAttack.tracerEffectPrefab = this.tracerEffectPrefab;
            this.bulletModifier.Apply( bulletAttack );
        }
    }
}
