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
using Sniper.SkillDefTypes.Bases;

namespace Sniper.SkillDefs
{
    internal class SniperPassiveSkillDef : SniperSkillDef
    {
        
        internal static SniperPassiveSkillDef Create( Data.BulletModifier modifier, Boolean headshots, Single critMultiplier )
        {
            var def = ScriptableObject.CreateInstance<SniperPassiveSkillDef>();

            def.bulletModifier = modifier;
            def.canHeadshot = headshots;
            def.onCritDamageMultiplier = critMultiplier;


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
        [SerializeField]
        private Boolean canHeadshot;
        [SerializeField]
        private Single onCritDamageMultiplier;


        internal void ModifyBullet( BulletAttack bulletAttack )
        {
            if( bulletAttack.isCrit ) bulletAttack.damage *= this.onCritDamageMultiplier;
            bulletAttack.sniper = this.canHeadshot;
            this.bulletModifier.Apply( bulletAttack );
        }
    }
}
