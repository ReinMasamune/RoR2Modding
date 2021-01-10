namespace Rein.Sniper.Modules
{
    using System;
    using System.Collections.Generic;

    using EntityStates;

    using ReinCore;

    using Rewired;
    using RoR2.Skills;
    using Rein.Sniper.Data;
    using Rein.Sniper.Properties;
    using Rein.Sniper.SkillDefs;
    using Rein.Sniper.SkillDefTypes.Bases;
    using Rein.Sniper.States.Bases;
    using Rein.Sniper.States.Primary.Reload;
    using Rein.Sniper.States.Secondary;
    using Rein.Sniper.States.Special;
    using Rein.Sniper.States.Utility;

    using UnityEngine;
    using Rein.Sniper.Ammo;

    internal static partial class SkillsModule
    {
        private static (SkillDef, String) wip
        {
            get
            {
                var def = ScriptableObject.CreateInstance<SkillDef>();
                return (def, Unlockables.WIPUnlockable.unlockable_Identifier);
            }
        }







        internal static void CreateAmmoSkills()
        {
            var skills = new List<(SkillDef,String)>();


            var standardAmmo = SniperAmmoSkillDef.Create<FMJContext>();
            standardAmmo.icon = Properties.Icons.StandardAmmoIcon;
            standardAmmo.skillName = "StandardAmmo";
            standardAmmo.skillNameToken = Tokens.SNIPER_AMMO_STANDARD_NAME;
            standardAmmo.skillDescriptionToken = Tokens.SNIPER_AMMO_STANDARD_DESC;
            standardAmmo.fireSoundType = SoundModule.FireType.Normal;
            standardAmmo.keywordTokens = new[]
            {    
                Tokens.SNIPER_KEYWORD_PIERCING,
                Tokens.SNIPER_KEYWORD_RICOCHET,
                Tokens.SNIPER_KEYWORD_PRIMARYDMG,
                Tokens.SNIPER_KEYWORD_BOOST,
            };
            skills.Add((standardAmmo, ""));



            var explosive = SniperAmmoSkillDef.Create<ExplosiveContext>();
            
            explosive.icon = Properties.Icons.ExplosiveAmmoIcon;
            explosive.skillName = "ExplosiveAmmo";
            explosive.skillNameToken = Tokens.SNIPER_AMMO_EXPLOSIVE_NAME;
            explosive.skillDescriptionToken = Tokens.SNIPER_AMMO_EXPLOSIVE_DESC;
            explosive.fireSoundType = SoundModule.FireType.Normal;
            explosive.keywordTokens = new[]
            {
                Tokens.SNIPER_KEYWORD_EXPLOSIVE,
                Tokens.SNIPER_KEYWORD_PRIMARYDMG,
                Tokens.SNIPER_KEYWORD_BOOST,
            };
            skills.Add((explosive, ""));
 


            var burstAmmo = SniperAmmoSkillDef.Create<BurstContext>();
            burstAmmo.icon = Properties.Icons.BurstAmmoIcon;
            burstAmmo.skillName = "Burst Ammo";
            burstAmmo.skillNameToken = Tokens.SNIPER_AMMO_BURST_NAME;
            burstAmmo.skillDescriptionToken = Tokens.SNIPER_AMMO_BURST_DESC;
            burstAmmo.fireSoundType = SoundModule.FireType.Burst;
            burstAmmo.keywordTokens = new[]
            {
                Tokens.SNIPER_KEYWORD_PRIMARYDMG,
                Tokens.SNIPER_KEYWORD_BOOST,
            };
            skills.Add((burstAmmo, ""));
            //skills.Add(wip);


            var plasmaAmmo = SniperAmmoSkillDef.Create<PlasmaContext>();
            plasmaAmmo.icon = Properties.Icons.PlasmaAmmoIcon;
            plasmaAmmo.skillName = "PlasmaAmmo";
            plasmaAmmo.skillNameToken = Tokens.SNIPER_AMMO_PLASMA_NAME;
            plasmaAmmo.skillDescriptionToken = Tokens.SNIPER_AMMO_PLASMA_DESC;
            plasmaAmmo.fireSoundType = SoundModule.FireType.Plasma;
            plasmaAmmo.keywordTokens = new[]
            {
                Tokens.SNIPER_KEYWORD_PRIMARYDMG,
                Tokens.SNIPER_KEYWORD_BOOST,
            };
            skills.Add((plasmaAmmo, ""));

            var shockAmmo = SniperAmmoSkillDef.Create<ShockContext>();
            shockAmmo.icon = Properties.Icons.ShockAmmoIcon;
            shockAmmo.skillName = "ShockAmmo";
            shockAmmo.skillNameToken = Tokens.SNIPER_AMMO_SHOCK_NAME;
            shockAmmo.skillDescriptionToken = Tokens.SNIPER_AMMO_SHOCK_DESC;
            shockAmmo.fireSoundType = SoundModule.FireType.Plasma;
            shockAmmo.keywordTokens = new[]
            {
                Tokens.SNIPER_KEYWORD_PRIMARYDMG,
                Tokens.SNIPER_KEYWORD_CHARGED,
                Tokens.SNIPER_KEYWORD_BOOST,
            };
            skills.Add((shockAmmo, ""));

            var sporeAmmo = SniperAmmoSkillDef.Create<SporeContext>();
            sporeAmmo.icon = Properties.Icons.SporeAmmoIcon;
            sporeAmmo.skillName = "SporeAmmo";
            sporeAmmo.skillNameToken = Tokens.SNIPER_AMMO_SPORE_NAME;
            sporeAmmo.skillDescriptionToken = Tokens.SNIPER_AMMO_SPORE_DESC;
            sporeAmmo.fireSoundType = SoundModule.FireType.Normal;
            sporeAmmo.keywordTokens = new[]
            {
                Tokens.SNIPER_KEYWORD_PRIMARYDMG,
                Tokens.SNIPER_KEYWORD_BOOST,
                Tokens.SNIPER_KEYWORD_SPORECLOUD,
            };
            skills.Add((sporeAmmo, ""));
            SkillFamiliesModule.ammoSkills = skills;
        }

        private struct DefaultSnipe : ISniperPrimaryDataProvider
        {
            public Single baseDuration => 0.15f;

            public Single recoilStrength => 6f;

            public Single damageMultiplier => 6f;

            public Single forceMultiplier => 200f;

            public String muzzleName => "MuzzleRailgun";

            public Single upBoostForce => 500f;
        }
        private struct MagSnipe : ISniperPrimaryDataProvider
        {
            public Single baseDuration => 0.15f;

            public Single recoilStrength => 4f;

            public Single damageMultiplier => 2.5f;

            public Single forceMultiplier => 100f;

            public String muzzleName => "MuzzleRailgun";

            public Single upBoostForce => 200f;
        }
        internal static void CreatePrimarySkills()
        {
            var skills = new List<(SkillDef,String)>();


            var snipe = SniperReloadableFireSkillDef.Create<DefaultSnipe,DefaultReload>("Weapon", "Weapon");
            snipe.actualMaxStock = 1;
            snipe.icon = Properties.Icons.SnipeIcon;
            snipe.interruptPriority = InterruptPriority.Skill;
            snipe.isBullets = false;
            snipe.rechargeStock = 0;
            snipe.reloadIcon = Properties.Icons.ReloadIcon;
            snipe.reloadInterruptPriority = InterruptPriority.Skill;
            snipe.reloadParams = new ReloadParams
            {
                attackSpeedCap = 1.25f,
                attackSpeedDecayCoef = 10f,
                baseDuration = 1.5f,
                badMult = 1.0f,
                goodMult = 1.2f,
                perfectMult = 1.5f,
                reloadDelay = 0.4f,
                reloadEndDelay = 0.05f,
                perfectStart = 0.25f,
                perfectEnd = 0.4f,
                goodStart = 0.4f,
                goodEnd = 0.6f,
            };
            snipe.requiredStock = 1;
            snipe.shootDelay = 0.15f;
            snipe.skillDescriptionToken = Tokens.SNIPER_PRIMARY_SNIPE_DESC;
            snipe.skillNameToken = Tokens.SNIPER_PRIMARY_SNIPE_NAME;
            snipe.stockToConsume = 1;
            snipe.stockToReload = 1;
            snipe.skillName = "Snipe";
            snipe.noSprint = true;
            snipe.noSprintReload = false;
            skills.Add((snipe, ""));

            var mag = SniperReloadableFireSkillDef.Create<MagSnipe,MagReload>("Weapon", "Weapon");
            mag.actualMaxStock = 4;
            mag.icon = Properties.Icons.SnipeMag;
            mag.interruptPriority = InterruptPriority.Skill;
            mag.isBullets = false;
            mag.rechargeStock = 0;
            mag.reloadIcon = Properties.Icons.SnipeMagReload;
            mag.reloadInterruptPriority = InterruptPriority.Skill;
            mag.reloadParams = new ReloadParams
            {
                attackSpeedCap = 1.5f,
                attackSpeedDecayCoef = 10f,
                badMult = 1.0f,
                baseDuration = 1.5f,
                goodMult = 1.2f,
                perfectMult = 1.5f,
                reloadDelay = 0.4f,
                reloadEndDelay = 0.05f,
                perfectStart = 0.25f,
                perfectEnd = 0.4f,
                goodStart = 0.4f,
                goodEnd = 0.6f,
            };
            mag.requiredStock = 1;
            mag.shootDelay = 0.4f;
            mag.skillDescriptionToken = Tokens.SNIPER_PRIMARY_MAG_DESC;
            mag.skillNameToken = Tokens.SNIPER_PRIMARY_MAG_NAME;
            mag.stockToConsume = 1;
            mag.stockToReload = 4;
            mag.skillName = "MagSnipe";
            mag.noSprint = true;
            mag.noSprintReload = false;
            skills.Add((mag, ""));
            //skills.Add(wip);

            //var slide = SniperReloadableFireSkillDef.Create<SlideSnipe,SlideReload>("Weapon", "Body");
            //slide.actualMaxStock = 1;
            //slide.icon = null; // TODO: Slide snipe icon
            //slide.interruptPriority = InterruptPriority.Skill;
            //slide.isBullets = false;
            //slide.rechargeStock = 0;
            //slide.reloadIcon = null; // TODO: Slide Snipe Reload icon
            //slide.reloadInterruptPriority = InterruptPriority.Skill;
            //slide.reloadParams = new ReloadParams
            //{
            //    attackSpeedCap = 1.25f,
            //    attackSpeedDecayCoef = 10f,
            //    badMult = 0.4f,
            //    baseDuration = 1.5f,
            //    goodMult = 1f,
            //    perfectMult = 2f,
            //    reloadDelay = 0.5f,
            //    reloadEndDelay = 0.75f,
            //    perfectStart = 0.25f,
            //    perfectEnd = 0.4f,
            //    goodStart = 0.4f,
            //    goodEnd = 0.6f,
            //};
            //slide.requiredStock = 1;
            //slide.shootDelay = 0.15f;
            //slide.skillDescriptionToken = Tokens.SNIPER_PRIMARY_DASH_DESC;
            //slide.skillNameToken = Tokens.SNIPER_PRIMARY_DASH_NAME;
            //slide.stockToConsume = 1;
            //slide.stockToReload = 1;
            //slide.skillName = "Slide";
            //skills.Add( slide );

            skills.Add(wip);

            SkillFamiliesModule.primarySkills = skills;
        }

        internal static void CreateSecondarySkills()
        {
            var skills = new List<(SkillDef,String)>();

            var charge = SniperScopeSkillDef.Create<DefaultScope>(
            new(
                shoulderStart: 1f, 
                shoulderEnd: 5f,
                scopeStart: 3f, 
                scopeEnd: 8f,
                shoulderFrac: 0.25f,
                defaultZoom: 0f,
                inputScale: 0.03f,
                baseFoV: 60f
            ));
            charge.baseMaxStock = 1;
            charge.baseRechargeInterval = 10f;
            charge.icon = Properties.Icons.SteadyAimIcon;
            charge.isBullets = false;
            charge.rechargeStock = 1;
            charge.requiredStock = 0;
            charge.skillDescriptionToken = Tokens.SNIPER_SECONDARY_STEADY_DESC;
            charge.skillName = "SteadyAim";
            charge.skillNameToken = Tokens.SNIPER_SECONDARY_STEADY_NAME;
            charge.stockToConsumeOnFire = 1;
            charge.stockRequiredToKeepZoom = 0;
            charge.stockRequiredToModifyFire = 1;
            charge.beginSkillCooldownOnSkillEnd = false;
            charge.initialCarryoverLoss = 0.0f;
            charge.decayType = SniperScopeSkillDef.DecayType.Exponential;
            charge.decayValue = 0.1f;
            charge.chargeCanCarryOver = true;
            charge.keywordTokens = new[]
            {
                Tokens.SNIPER_KEYWORD_SCOPED,
                Tokens.SNIPER_KEYWORD_BOOST,
            };
            charge.consumeChargeOnFire = true;
            skills.Add((charge, ""));

            var quick = SniperScopeSkillDef.Create<QuickScope>( new ZoomParams(shoulderStart: 1f, shoulderEnd: 5f,
                                                                                                             scopeStart: 3f, scopeEnd: 8f,
                                                                                                             shoulderFrac: 0.25f, defaultZoom: 0f,
                                                                                                             inputScale: 0.03f, baseFoV: 60f) );
            quick.baseMaxStock = 4;
            quick.baseRechargeInterval = 6f;
            quick.icon = Properties.Icons.QuickscopeIcon;
            quick.isBullets = false;
            quick.rechargeStock = 1;
            quick.requiredStock = 0;
            quick.skillDescriptionToken = Tokens.SNIPER_SECONDARY_QUICK_DESC;
            quick.skillName = "Quickscope";
            quick.skillNameToken = Tokens.SNIPER_SECONDARY_QUICK_NAME;
            quick.stockToConsumeOnFire = 1;
            quick.stockRequiredToKeepZoom = 0;
            quick.stockRequiredToModifyFire = 1;
            quick.beginSkillCooldownOnSkillEnd = false;
            quick.keywordTokens = new[]
            {
                Tokens.SNIPER_KEYWORD_SCOPED,
                Tokens.SNIPER_KEYWORD_BOOST,
            };
            quick.consumeChargeOnFire = true;
            skills.Add((quick, ""));


            skills.Add(wip);



            SkillFamiliesModule.secondarySkills = skills;
        }

        internal static void CreateUtilitySkills()
        {
            var skills = new List<(SkillDef,String)>();

            var backflip = SniperSkillDef.Create<Backflip>("Body");
            backflip.baseMaxStock = 1;
            backflip.baseRechargeInterval = 5f;
            backflip.beginSkillCooldownOnSkillEnd = true;
            backflip.canceledFromSprinting = false;
            backflip.fullRestockOnAssign = true;
            backflip.icon = Properties.Icons.BackflipIcon;
            backflip.interruptPriority = InterruptPriority.PrioritySkill;
            backflip.isBullets = false;
            backflip.isCombatSkill = false;
            backflip.mustKeyPress = true;
            backflip.noSprint = true;
            backflip.rechargeStock = 1;
            backflip.requiredStock = 1;
            backflip.shootDelay = 0.1f;
            backflip.skillDescriptionToken = Tokens.SNIPER_UTILITY_BACKFLIP_DESC;
            backflip.skillName = "Military Training";
            backflip.skillNameToken = Tokens.SNIPER_UTILITY_BACKFLIP_NAME;
            backflip.stockToConsume = 1;
            backflip.keywordTokens = new[]
            {
                Tokens.SNIPER_KEYWORD_RELOADS,
                "KEYWORD_STUNNING",
            };
            skills.Add((backflip, ""));

            skills.Add(wip);

            SkillFamiliesModule.utilitySkills = skills;
        }

        internal static void CreateSpecialSkills()
        {
            var skills = new List<(SkillDef,String)>();

            var decoy = DecoySkillDef.Create<DecoyActivation,DecoyReactivation>( "Weapon", "Weapon" );
            decoy.baseMaxStock = 1;
            decoy.baseRechargeInterval = 12f;
            decoy.beginSkillCooldownOnSkillEnd = true;
            decoy.fullRestockOnAssign = true;
            decoy.icon = Properties.Icons.DecoyIcon;
            decoy.interruptPriority = InterruptPriority.PrioritySkill;
            decoy.isCombatSkill = false;
            decoy.maxReactivationTimer = -1f;
            decoy.minReactivationTimer = 0.75f;
            decoy.noSprint = false;
            decoy.reactivationIcon = Properties.Icons.DecoyReactivateIcon;
            decoy.reactivationInterruptPriority = InterruptPriority.PrioritySkill;
            decoy.reactivationRequiredStock = 1;
            decoy.reactivationStockToConsume = 1;
            decoy.consumeOnInvalidate = true;
            decoy.cdRefundOnInvalidate = 0.0f;
            decoy.rechargeStock = 1;
            decoy.requiredStock = 1;
            decoy.skillDescriptionToken = Tokens.SNIPER_SPECIAL_DECOY_DESC;
            decoy.skillName = "Decoy";
            decoy.skillNameToken = Tokens.SNIPER_SPECIAL_DECOY_NAME;
            decoy.startCooldownAfterReactivation = true;
            decoy.stockToConsume = 0;
            decoy.keywordTokens = new[]
            {
                Tokens.SNIPER_KEYWORD_REACTIVATION,
                Tokens.SNIPER_KEYWORD_PHASED,
                "KEYWORD_WEAK",
                "KEYWORD_STUNNING",
            };
            skills.Add((decoy, ""));

            var knife = KnifeSkillDef.Create<KnifeActivation,KnifeReactivation>( "Weapon", "Body" );
            knife.baseMaxStock = 1;
            knife.baseRechargeInterval = 18f;
            knife.beginSkillCooldownOnSkillEnd = true;
            knife.fullRestockOnAssign = true;
            knife.icon = Properties.Icons.KnifeIcon;
            knife.interruptPriority = InterruptPriority.PrioritySkill;
            knife.isCombatSkill = true;
            knife.maxReactivationTimer = 8f;
            knife.minReactivationTimer = 0.3f;
            knife.noSprint = true;
            knife.reactivationIcon = Properties.Icons.KnifeReactivateIcon;
            knife.reactivationInterruptPriority = InterruptPriority.PrioritySkill;
            knife.reactivationRequiredStock = 1;
            knife.reactivationStockToConsume = 1;
            knife.rechargeStock = 1;
            knife.requiredStock = 1;
            knife.skillDescriptionToken = Tokens.SNIPER_SPECIAL_KNIFE_DESC;
            knife.skillName = "BlinkKnife";
            knife.skillNameToken = Tokens.SNIPER_SPECIAL_KNIFE_NAME;
            knife.startCooldownAfterReactivation = true;
            knife.stockToConsume = 0;
            knife.keywordTokens = new[]
            {
                Tokens.SNIPER_KEYWORD_REACTIVATION,
                Tokens.SNIPER_KEYWORD_RELOADS,
            };
            KnifeSkillData.interruptPriority = InterruptPriority.PrioritySkill;
            KnifeSkillData.targetMachineName = "Weapon";
            KnifeSkillData.slashState = SkillsCore.StateType<KnifePickupSlash>();
           
            skills.Add((knife, ""));

            SkillFamiliesModule.specialSkills = skills;
        }
    }
}
