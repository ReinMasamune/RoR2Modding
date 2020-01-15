using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;

namespace ReinSniperRework
{
    internal partial class Main
    {
        partial void Skills()
        {
            this.Load += this.AddScopeStateMachine;
            this.Load += this.AddLoadStateMachine;
            this.Load += this.CreateNewFamilies;
            this.Load += this.CreateDefaultPrimary;
            this.Load += this.CreateDefaultSecondary;
            //this.Load += this.CreateAltSecondary1;
            this.Load += this.CreateDefaultUtility;
            this.Load += this.CreateDefaultSpecial;

            this.FirstFrame += () =>
            {
                base.Logger.LogWarning( ((EntityStates.Commando.DodgeState)EntityState.Instantiate( new SerializableEntityStateType( typeof( EntityStates.Commando.DodgeState ) ) )).duration );
            };
        }



        private void CreateDefaultSpecial()
        {
            var skillDef = ScriptableObject.CreateInstance<ReactivatableSkillDef>();
            var variant = new SkillFamily.Variant
            {
                skillDef = skillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node( "SniperDefaultSpecial", false )
            };
            var skillLoc = this.sniperBody.GetComponent<SkillLocator>();
            var ind = skillLoc.special.skillFamily.variants.Length;
            Array.Resize<SkillFamily.Variant>( ref skillLoc.special.skillFamily.variants, ind + 1 );
            skillLoc.special.skillFamily.variants[ind] = variant;
            SkillAPI.AddSkillDef( skillDef );
            SkillAPI.AddSkill( typeof( SniperKnife ) );
            SkillAPI.AddSkill( typeof( SniperKnifeBlink ) );
            SkillAPI.AddSkill( typeof( SniperKnifeSlash ) );

            skillDef.activationState = new SerializableEntityStateType( typeof( SniperKnife ) );
            skillDef.activationStateMachineName = "Weapon";
            skillDef.interruptPriority = InterruptPriority.PrioritySkill;
            skillDef.icon = Resources.Load<Sprite>( "NotAPath" );

            skillDef.reactivationState = new SerializableEntityStateType( typeof( SniperKnifeBlink ) );
            skillDef.reactivationInterruptPriority = InterruptPriority.PrioritySkill;
            skillDef.reactivationIcon = Resources.Load<Sprite>( "NotAPath" );

            skillDef.skillName = "SniperKnife";
            skillDef.skillNameToken = "SNIPER_SPECIAL_NAME";
            skillDef.skillDescriptionToken = "SNIPER_SPECIAL_DESC";

            skillDef.isBullets = false;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.isCombatSkill = true;
            skillDef.noSprint = true;
            skillDef.canceledFromSprinting = false;
            skillDef.mustKeyPress = true;
            skillDef.fullRestockOnAssign = true;

            skillDef.reactivationNoSprint = false;
            skillDef.reactivationRestartsCooldown = false;

            skillDef.baseMaxStock = 1;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 1;
            skillDef.stockToConsume = 1;

            skillDef.reactivationRequiredStock = 0;
            skillDef.reactivationStockToConsume = 0;

            skillDef.baseRechargeInterval = 10f;
            skillDef.shootDelay = 0.1f;

            skillDef.reactivationDelay = 0.2f;
            skillDef.reactivationWindow = 5f;
        }

        private void CreateDefaultUtility()
        {
            var skillDef = ScriptableObject.CreateInstance<SkillDef>();
            var variant = new SkillFamily.Variant
            {
                skillDef = skillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node( "SniperDefaultUtility", false )
            };
            var skillLoc = this.sniperBody.GetComponent<SkillLocator>();
            var ind = skillLoc.utility.skillFamily.variants.Length;
            Array.Resize<SkillFamily.Variant>( ref skillLoc.utility.skillFamily.variants, ind + 1 );
            skillLoc.utility.skillFamily.variants[ind] = variant;
            SkillAPI.AddSkillDef( skillDef );
            SkillAPI.AddSkill( typeof( SniperBackflip ) );

            skillDef.activationState = new SerializableEntityStateType( typeof( SniperBackflip ) );
            skillDef.activationStateMachineName = "Body";
            skillDef.interruptPriority = InterruptPriority.Skill;
            skillDef.icon = Resources.Load<Sprite>( "NotAPath" );

            skillDef.skillName = "SniperBackflip";
            skillDef.skillNameToken = "SNIPER_UTILITY_NAME";
            skillDef.skillDescriptionToken = "SNIPER_UTILITY_DESC";

            skillDef.isBullets = false;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.isCombatSkill = true;
            skillDef.noSprint = true;
            skillDef.canceledFromSprinting = false;
            skillDef.mustKeyPress = true;
            skillDef.fullRestockOnAssign = true;

            skillDef.baseMaxStock = 1;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 1;
            skillDef.stockToConsume = 1;

            skillDef.baseRechargeInterval = 8f;
            skillDef.shootDelay = 0.1f;
        }

        private void CreateAltSecondary1()
        {
            var skillDef = ScriptableObject.CreateInstance<SkillDef>();
            var variant = new SkillFamily.Variant
            {
                skillDef = skillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node( "SniperAltSecondary1", false )
            };
            var skillLoc = this.sniperBody.GetComponent<SkillLocator>();
            var ind = skillLoc.secondary.skillFamily.variants.Length;
            Array.Resize<SkillFamily.Variant>( ref skillLoc.secondary.skillFamily.variants, ind + 1 );
            skillLoc.secondary.skillFamily.variants[ind] = variant;
            SkillAPI.AddSkillDef( skillDef );
            SkillAPI.AddSkill( typeof( SniperPrepSidearm ) );

            skillDef.activationState = new SerializableEntityStateType( typeof( SniperPrepSidearm ) );
            skillDef.activationStateMachineName = "Weapon";
            skillDef.interruptPriority = InterruptPriority.Skill;
            skillDef.icon = Resources.Load<Sprite>( "NotAPath" );

            skillDef.skillName = "SniperShoot";
            skillDef.skillNameToken = "SNIPER_SECONDARY_NAME";
            skillDef.skillDescriptionToken = "SNIPER_SECONDARY_DESC";

            skillDef.isBullets = false;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.isCombatSkill = false;
            skillDef.noSprint = true;
            skillDef.canceledFromSprinting = false;
            skillDef.mustKeyPress = false;
            skillDef.fullRestockOnAssign = true;

            skillDef.baseMaxStock = 4;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 1;
            skillDef.stockToConsume = 0;

            skillDef.baseRechargeInterval = 10f;
            skillDef.shootDelay = 0.1f;
        }

        private void CreateDefaultSecondary()
        {
            var skillDef = ScriptableObject.CreateInstance<SkillDef>();
            var variant = new SkillFamily.Variant
            {
                skillDef = skillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node( "SniperDefaultSecondary", false )
            };
            var skillLoc = this.sniperBody.GetComponent<SkillLocator>();
            var ind = skillLoc.secondary.skillFamily.variants.Length;
            Array.Resize<SkillFamily.Variant>( ref skillLoc.secondary.skillFamily.variants, ind + 1 );
            skillLoc.secondary.skillFamily.variants[ind] = variant;
            SkillAPI.AddSkillDef( skillDef );
            SkillAPI.AddSkill( typeof( SniperCharging ) );

            skillDef.activationState = new SerializableEntityStateType( typeof( SniperCharging ) );
            skillDef.activationStateMachineName = "Scope";
            skillDef.interruptPriority = InterruptPriority.Skill;
            skillDef.icon = Resources.Load<Sprite>( "NotAPath" );

            skillDef.skillName = "SniperShoot";
            skillDef.skillNameToken = "SNIPER_SECONDARY_NAME";
            skillDef.skillDescriptionToken = "SNIPER_SECONDARY_DESC";

            skillDef.isBullets = false;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.isCombatSkill = false;
            skillDef.noSprint = true;
            skillDef.canceledFromSprinting = false;
            skillDef.mustKeyPress = false;
            skillDef.fullRestockOnAssign = true;

            skillDef.baseMaxStock = 4;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 1;
            skillDef.stockToConsume = 0;

            skillDef.baseRechargeInterval = 10f;
            skillDef.shootDelay = 0.1f;
        }

        private void CreateDefaultPrimary()
        {
            var skillDef = ScriptableObject.CreateInstance<SkillDef>();
            var variant = new SkillFamily.Variant
            {
                skillDef = skillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node( "SniperDefaultPrimary", false )
            };
            var skillLoc = this.sniperBody.GetComponent<SkillLocator>();
            var ind = skillLoc.primary.skillFamily.variants.Length;
            Array.Resize<SkillFamily.Variant>( ref skillLoc.primary.skillFamily.variants, ind + 1 );
            skillLoc.primary.skillFamily.variants[ind] = variant;
            SkillAPI.AddSkillDef( skillDef );
            SkillAPI.AddSkill( typeof( SniperShoot ) );
            SkillAPI.AddSkill( typeof( SniperReload ) );
            SkillAPI.AddSkill( typeof( SniperLoaded ) );

            skillDef.activationState = new SerializableEntityStateType( typeof( SniperShoot ) );
            skillDef.activationStateMachineName = "Load";
            skillDef.interruptPriority = InterruptPriority.Skill;
            skillDef.icon = Resources.Load<Sprite>( "NotAPath" );

            skillDef.skillName = "SniperShoot";
            skillDef.skillNameToken = "SNIPER_PRIMARY_NAME";
            skillDef.skillDescriptionToken = "SNIPER_PRIMARY_DESC";

            skillDef.isBullets = false;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.isCombatSkill = true;
            skillDef.noSprint = true;
            skillDef.canceledFromSprinting = false;
            skillDef.mustKeyPress = true;
            skillDef.fullRestockOnAssign = true;

            skillDef.baseMaxStock = 1;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 1;
            skillDef.stockToConsume = 1;

            skillDef.baseRechargeInterval = 0f;
            skillDef.shootDelay = 0.1f;
        }

        private void CreateNewFamilies()
        {
            var primaryFamily = ScriptableObject.CreateInstance<SkillFamily>();
            var secondaryFamily = ScriptableObject.CreateInstance<SkillFamily>();
            var utilityFamily = ScriptableObject.CreateInstance<SkillFamily>();
            var specialFamily = ScriptableObject.CreateInstance<SkillFamily>();

            SkillAPI.AddSkillFamily( primaryFamily );
            SkillAPI.AddSkillFamily( secondaryFamily );
            SkillAPI.AddSkillFamily( utilityFamily );
            SkillAPI.AddSkillFamily( specialFamily );

            var skillLoc = this.sniperBody.GetComponent<SkillLocator>();
            skillLoc.primary.SetFieldValue<SkillFamily>( "_skillFamily", primaryFamily );
            skillLoc.secondary.SetFieldValue<SkillFamily>( "_skillFamily", secondaryFamily );
            skillLoc.utility.SetFieldValue<SkillFamily>( "_skillFamily", utilityFamily );
            skillLoc.special.SetFieldValue<SkillFamily>( "_skillFamily", specialFamily );

            primaryFamily.variants = Array.Empty<SkillFamily.Variant>();
            secondaryFamily.variants = Array.Empty<SkillFamily.Variant>();
            utilityFamily.variants = Array.Empty<SkillFamily.Variant>();
            specialFamily.variants = Array.Empty<SkillFamily.Variant>();

        }

        private void AddLoadStateMachine()
        {
            var machine = this.sniperBody.AddComponent<EntityStateMachine>();
            machine.initialStateType = new SerializableEntityStateType( typeof( SniperLoaded ) );
            machine.mainStateType = new SerializableEntityStateType( typeof( SniperLoaded ) );
            machine.customName = "Load";

            var networkStateMachine = this.sniperBody.GetComponent<NetworkStateMachine>();
            var machines = networkStateMachine.GetFieldValue<EntityStateMachine[]>("stateMachines");
            Array.Resize<EntityStateMachine>( ref machines, machines.Length + 1 );
            machines[machines.Length - 1] = machine;
            networkStateMachine.SetFieldValue<EntityStateMachine[]>( "stateMachines", machines );
        }

        private void AddScopeStateMachine()
        {
            var machine = this.sniperBody.AddComponent<EntityStateMachine>();
            machine.initialStateType = new SerializableEntityStateType( typeof( Idle ) );
            machine.mainStateType = new SerializableEntityStateType( typeof( Idle ) );
            machine.customName = "Scope";
            
            var networkStateMachine = this.sniperBody.GetComponent<NetworkStateMachine>();
            var machines = networkStateMachine.GetFieldValue<EntityStateMachine[]>("stateMachines");
            Array.Resize<EntityStateMachine>( ref machines, machines.Length + 1 );
            machines[machines.Length - 1] = machine;
            networkStateMachine.SetFieldValue<EntityStateMachine[]>( "stateMachines", machines );

            var stateOnHurt = this.sniperBody.GetComponent<SetStateOnHurt>();
            Array.Resize<EntityStateMachine>( ref stateOnHurt.idleStateMachine, stateOnHurt.idleStateMachine.Length + 1 );
            stateOnHurt.idleStateMachine[stateOnHurt.idleStateMachine.Length - 1] = machine;
        }
    }
}


