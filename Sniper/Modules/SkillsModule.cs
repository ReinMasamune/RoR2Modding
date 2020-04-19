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
using Sniper.Skills;
using Sniper.Data;
using Sniper.Properties;

namespace Sniper.Modules
{
    internal static class SkillsModule
    {
        internal static void CreateAmmoSkills()
        {
            var skills = new List<SkillDef>();

            var standardAmmo = SniperAmmoSkillDef.Create( null, BulletModifier.identity );
            standardAmmo.icon = null;
            standardAmmo.skillName = "Standard Ammo";
            standardAmmo.skillNameToken = Tokens.SNIPER_AMMO_STANDARD_NAME;
            standardAmmo.skillDescriptionToken = Tokens.SNIPER_AMMO_STANDARD_DESC;
            skills.Add( standardAmmo );

            var pierceAmmo = SniperAmmoSkillDef.Create( null, BulletModifier.identity );
            pierceAmmo.icon = null;
            pierceAmmo.skillName = "Piercing Ammo";
            pierceAmmo.skillNameToken = Tokens.SNIPER_AMMO_PIERCING_NAME;
            pierceAmmo.skillDescriptionToken = Tokens.SNIPER_AMMO_PIERCING_DESC;
            skills.Add( pierceAmmo );

            SkillFamiliesModule.ammoSkills = skills;
        }

        internal static void CreatePassiveSkills()
        {
            var skills = new List<SkillDef>();

            var critPassive = SniperPassiveSkillDef.Create( BulletModifier.identity, false, 1.2f );
            critPassive.icon = null;
            critPassive.skillName = "Precise Aim";
            critPassive.skillNameToken = Tokens.SNIPER_PASSIVE_CRITICAL_NAME;
            critPassive.skillDescriptionToken = Tokens.SNIPER_PASSIVE_CRITICAL_DESC;
            skills.Add( critPassive );

            var headshot = SniperPassiveSkillDef.Create( BulletModifier.identity, true, 1.0f );
            headshot.icon = null;
            headshot.skillName = "Headshot";
            headshot.skillNameToken = Tokens.SNIPER_PASSIVE_HEADSHOT_NAME;
            headshot.skillDescriptionToken = Tokens.SNIPER_PASSIVE_HEADSHOT_DESC;
            skills.Add( headshot );


            SkillFamiliesModule.passiveSkills = skills;
        }

        internal static void CreatePrimarySkills()
        {
            var skills = new List<SkillDef>();


            var snipe = SniperReloadableFireSkillDef.Create<DefaultSnipe,DefaultReload>("Weapon", "Weapon");
            snipe.baseMaxStock = 1;
            snipe.icon = null; // TODO: Assign
            snipe.interruptPriority = InterruptPriority.Skill;
            snipe.isBullets = false;
            snipe.rechargeStock = 0;
            snipe.reloadIcon = null; // TODO: Assign
            snipe.reloadInterruptPriority = InterruptPriority.Skill;
            snipe.reloadParams = new ReloadParams
            {
                attackSpeedCap = 3f,
                attackSpeedDecayCoef = 10f,
                badMult = 0.8f,
                baseDuration = 1.5f,
                goodMult = 1.2f,
                perfectMult = 2f,
                reloadDelay = 0.3f,
                reloadEndDelay = 0.1f,
                perfectStart = 0.3f,
                perfectEnd = 0.4f,
                goodStart = 0.4f,
                goodEnd = 0.6f,
            };
            snipe.requiredStock = 1;
            snipe.shootDelay = 0.0f;
            snipe.skillDescriptionToken = Tokens.SNIPER_PRIMARY_SNIPE_DESC;
            snipe.skillNameToken = Tokens.SNIPER_PRIMARY_SNIPE_NAME;
            snipe.stockToConsume = 1;
            skills.Add( snipe );

            var slide = SniperReloadableFireSkillDef.Create<SlideSnipe,SlideReload>("Weapon", "Body");
            slide.baseMaxStock = 1;
            slide.icon = null; // TODO: Assign
            slide.interruptPriority = InterruptPriority.Skill;
            slide.isBullets = false;
            slide.rechargeStock = 0;
            slide.reloadIcon = null; // TODO: Assign
            slide.reloadInterruptPriority = InterruptPriority.Skill;
            slide.reloadParams = new ReloadParams
            {
                attackSpeedCap = 3f,
                attackSpeedDecayCoef = 10f,
                badMult = 0.8f,
                baseDuration = 1.5f,
                goodMult = 1.2f,
                perfectMult = 2f,
                reloadDelay = 0.3f,
                reloadEndDelay = 0.1f,
                perfectStart = 0.3f,
                perfectEnd = 0.4f,
                goodStart = 0.4f,
                goodEnd = 0.6f,
            };
            slide.requiredStock = 1;
            slide.shootDelay = 0.0f;
            slide.skillDescriptionToken = Tokens.SNIPER_PRIMARY_DASH_DESC;
            slide.skillNameToken = Tokens.SNIPER_PRIMARY_DASH_NAME;
            slide.stockToConsume = 1;
            skills.Add( slide );

            SkillFamiliesModule.primarySkills = skills;
        }

        internal static void CreateSecondarySkills()
        {
            var skills = new List<SkillDef>();

            var charge = SniperScopeSkillDef.Create<DefaultScope>( UIModule.GetChargeScope(), default ); // TODO: Zoom params
            charge.baseMaxStock = 1;
            charge.baseRechargeInterval = 0f;
            charge.icon = null; // TODO: Assign
            charge.isBullets = false;
            charge.rechargeStock = 1;
            charge.requiredStock = 0;
            charge.skillDescriptionToken = Tokens.SNIPER_SECONDARY_STEADY_DESC;
            charge.skillName = "Steady Aim";
            charge.skillNameToken = Tokens.SNIPER_SECONDARY_STEADY_NAME;
            skills.Add( charge );

            var quick = SniperScopeSkillDef.Create<DefaultScope>( UIModule.GetQuickScope(), default ); // TODO: Zoom params
            quick.baseMaxStock = 1;
            quick.baseRechargeInterval = 0f;
            quick.icon = null; // TODO: Assign
            quick.isBullets = false;
            quick.rechargeStock = 1;
            quick.requiredStock = 0;
            quick.skillDescriptionToken = Tokens.SNIPER_SECONDARY_QUICK_DESC;
            quick.skillName = "Steady Aim";
            quick.skillNameToken = Tokens.SNIPER_SECONDARY_QUICK_NAME;
            skills.Add( quick );



            SkillFamiliesModule.secondarySkills = skills;
        }

        internal static void CreateUtilitySkills()
        {
            var skills = new List<SkillDef>();

            var backflip = SniperSkillDef.Create<Backflip>("Body");
            backflip.baseMaxStock = 1;
            backflip.baseRechargeInterval = 8f;
            backflip.beginSkillCooldownOnSkillEnd = true;
            backflip.canceledFromSprinting = false;
            backflip.fullRestockOnAssign = true;
            backflip.icon = null; // TODO: Assign
            backflip.interruptPriority = InterruptPriority.Skill;
            backflip.isBullets = false;
            backflip.isCombatSkill = true;
            backflip.mustKeyPress = true;
            backflip.noSprint = true;
            backflip.rechargeStock = 1;
            backflip.requiredStock = 1;
            backflip.shootDelay = 0.1f;
            backflip.skillDescriptionToken = Tokens.SNIPER_UTILITY_BACKFLIP_DESC;
            backflip.skillName = "Military Training";
            backflip.skillNameToken = Tokens.SNIPER_UTILITY_BACKFLIP_NAME;
            backflip.stockToConsume = 1;
            skills.Add( backflip );

            SkillFamiliesModule.utilitySkills = skills;
        }

        internal static void CreateSpecialSkills()
        {
            var skills = new List<SkillDef>();

            var decoy = DecoySkillDef.Create<DecoyActivation,DecoyReactivation>( "Weapon", "Weapon" );
            decoy.baseMaxStock = 1;
            decoy.baseRechargeInterval = 18f;
            decoy.beginSkillCooldownOnSkillEnd = true;
            decoy.fullRestockOnAssign = true;
            decoy.icon = null; // TODO: Assign
            decoy.interruptPriority = InterruptPriority.Skill;
            decoy.isCombatSkill = false;
            decoy.maxReactivationTimer = 10f;
            decoy.minReactivationTimer = 2f;
            decoy.noSprint = false;
            decoy.reactivationIcon = null; // TODO: Assign
            decoy.reactivationInterruptPriority = InterruptPriority.Skill;
            decoy.reactivationRequiredStock = 0;
            decoy.reactivationStockToConsume = 0;
            decoy.rechargeStock = 1;
            decoy.requiredStock = 1;
            decoy.skillDescriptionToken = Tokens.SNIPER_SPECIAL_DECOY_DESC;
            decoy.skillName = "Decoy";
            decoy.skillNameToken = Tokens.SNIPER_SPECIAL_DECOY_NAME;
            decoy.startCooldownAfterReactivation = true;
            decoy.stockToConsume = 1;
            skills.Add( decoy );

            var knife = KnifeSkillDef.Create<KnifeActivation,KnifeReactivation>( "Weapon", "Body" );
            knife.baseMaxStock = 1;
            knife.baseRechargeInterval = 14f;
            knife.beginSkillCooldownOnSkillEnd = true;
            knife.fullRestockOnAssign = true;
            knife.icon = null; // TODO: Assign
            knife.interruptPriority = InterruptPriority.Skill;
            knife.isCombatSkill = true;
            knife.maxReactivationTimer = 6f;
            knife.minReactivationTimer = 0.5f;
            knife.noSprint = true;
            knife.reactivationIcon = null; // TODO: Assign
            knife.reactivationInterruptPriority = InterruptPriority.Skill;
            knife.reactivationRequiredStock = 0;
            knife.reactivationStockToConsume = 0;
            knife.rechargeStock = 1;
            knife.requiredStock = 1;
            knife.skillDescriptionToken = Tokens.SNIPER_SPECIAL_KNIFE_DESC;
            knife.skillName = "Blink Knife";
            knife.skillNameToken = Tokens.SNIPER_SPECIAL_KNIFE_NAME;
            knife.startCooldownAfterReactivation = true;
            knife.stockToConsume = 1;
            skills.Add( knife );
            
            SkillFamiliesModule.specialSkills = skills;
        }
    }
}
