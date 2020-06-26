namespace Sniper.SkillDefs
{
    using System;
    using System.Runtime.CompilerServices;

    using EntityStates;

    using ReinCore;

    using Sniper.Components;
    using Sniper.Enums;
    using Sniper.Expansions;
    using Sniper.SkillDefTypes.Bases;

    using UnityEngine;

    internal delegate ExpandableBulletAttack BulletCreationDelegate( SniperCharacterBody body, ReloadTier reload, Ray aim, String muzzleName );
    internal delegate void ChargeBulletModifierDelegate( ExpandableBulletAttack bullet );
    internal class SniperAmmoSkillDef : SniperSkillDef
    {
        internal static SniperAmmoSkillDef Create( BulletCreationDelegate createBullet, ChargeBulletModifierDelegate chargeMod = null )
        {
#if ASSERT
            if( createBullet == null )
            {
                Log.ErrorL( "Null Create delegate" );
            }
#endif

            SniperAmmoSkillDef def = ScriptableObject.CreateInstance<SniperAmmoSkillDef>();

            def.createBullet = createBullet;
            def.chargeModifier = chargeMod;


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
        internal Modules.SoundModule.FireType fireSoundType;


        private BulletCreationDelegate createBullet;
        private ChargeBulletModifierDelegate chargeModifier;


        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        internal ExpandableBulletAttack CreateBullet( SniperCharacterBody body, ReloadTier tier, Ray aim, String muzzleName )
        {
            ExpandableBulletAttack bullet = this.createBullet(body, tier, aim, muzzleName );
            return bullet;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        internal void ApplyChargeModifiers( ExpandableBulletAttack bullet )
        {
            this.chargeModifier?.Invoke( bullet );
        }


    }
}
