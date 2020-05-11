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
using Sniper.Components;
using Sniper.Enums;

namespace Sniper.SkillDefs
{
    internal delegate ExpandableBulletAttack BulletCreationDelegate( SniperCharacterBody body, ReloadTier reload, Ray aim, String muzzleName );
    internal class SniperAmmoSkillDef : SniperSkillDef
    {
        internal static SniperAmmoSkillDef Create( BulletCreationDelegate createBullet )
        {
#if ASSERT
            if( createBullet == null ) Log.ErrorL( "Null Create delegate" );
#endif

            SniperAmmoSkillDef def = ScriptableObject.CreateInstance<SniperAmmoSkillDef>();

            def.createBullet = createBullet;


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

        private BulletCreationDelegate createBullet;


        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        internal ExpandableBulletAttack CreateBullet( SniperCharacterBody body, ReloadTier tier, Ray aim, String muzzleName )
        {
            ExpandableBulletAttack bullet = this.createBullet(body, tier, aim, muzzleName );
            return bullet;
        }
    }
}
