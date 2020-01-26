#if ANCIENTWISP
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace RogueWispPlugin
{

    internal partial class Main
    {
        partial void AW_SetupSkills()
        {
            this.Load += this.AW_SetupPrimary;
            this.Load += this.AW_SetupSecondary;
            this.Load += this.AW_SetupUtility;
        }

        private void AW_SetupUtility()
        {
            var skillLoc = this.AW_body.GetComponent<SkillLocator>();
            var utilityFam = ScriptableObject.CreateInstance<SkillFamily>();
            var utilityDef = ScriptableObject.CreateInstance<SkillDef>();
            LoadoutAPI.AddSkillDef( utilityDef );
            LoadoutAPI.AddSkillFamily( utilityFam );
            LoadoutAPI.AddSkill( typeof( AWChargeUtility ) );
            LoadoutAPI.AddSkill( typeof( AWFireUtility ) );

            utilityDef.activationState = new EntityStates.SerializableEntityStateType( typeof( AWChargeUtility ) );
            utilityDef.activationStateMachineName = "Weapon";
            utilityDef.baseMaxStock = 1;
            utilityDef.baseRechargeInterval = 10f;
            utilityDef.beginSkillCooldownOnSkillEnd = true;
            utilityDef.canceledFromSprinting = false;
            utilityDef.fullRestockOnAssign = true;
            utilityDef.icon = Resources.Load<Sprite>( "NotAPath" );
            utilityDef.interruptPriority = EntityStates.InterruptPriority.Skill;
            utilityDef.isBullets = false;
            utilityDef.isCombatSkill = true;
            utilityDef.mustKeyPress = false;
            utilityDef.noSprint = false;
            utilityDef.rechargeStock = 1;
            utilityDef.requiredStock = 1;
            utilityDef.shootDelay = 0.1f;
            utilityDef.skillDescriptionToken = "";
            utilityDef.skillName = "";
            utilityDef.skillNameToken = "";
            utilityDef.stockToConsume = 1;

            utilityFam.variants = new SkillFamily.Variant[]
            {
                new SkillFamily.Variant
                {
                    skillDef = utilityDef,
                    unlockableName = "",
                    viewableNode = new ViewablesCatalog.Node( "aaaaaa", false )
                }
            };
            skillLoc.utility.SetFieldValue<SkillFamily>( "_skillFamily", utilityFam );
        }
        private void AW_SetupSecondary()
        {
            var skillLoc = this.AW_body.GetComponent<SkillLocator>();
            var secondaryFam = ScriptableObject.CreateInstance<SkillFamily>();
            var secondaryDef = ScriptableObject.CreateInstance<SkillDef>();
            LoadoutAPI.AddSkillDef( secondaryDef );
            LoadoutAPI.AddSkillFamily( secondaryFam );
            LoadoutAPI.AddSkill( typeof( AWSecondary ) );

            secondaryDef.activationState = new EntityStates.SerializableEntityStateType( typeof( AWSecondary ) );
            secondaryDef.activationStateMachineName = "Weapon";
            secondaryDef.baseMaxStock = 1;
            secondaryDef.baseRechargeInterval = 10f;
            secondaryDef.beginSkillCooldownOnSkillEnd = true;
            secondaryDef.canceledFromSprinting = false;
            secondaryDef.fullRestockOnAssign = true;
            secondaryDef.icon = Resources.Load<Sprite>( "NotAPath" );
            secondaryDef.interruptPriority = EntityStates.InterruptPriority.Skill;
            secondaryDef.isBullets = false;
            secondaryDef.isCombatSkill = true;
            secondaryDef.mustKeyPress = false;
            secondaryDef.noSprint = false;
            secondaryDef.rechargeStock = 1;
            secondaryDef.requiredStock = 1;
            secondaryDef.shootDelay = 0.1f;
            secondaryDef.skillDescriptionToken = "";
            secondaryDef.skillName = "";
            secondaryDef.skillNameToken = "";
            secondaryDef.stockToConsume = 1;

            secondaryFam.variants = new SkillFamily.Variant[]
            {
                new SkillFamily.Variant
                {
                    skillDef = secondaryDef,
                    unlockableName = "",
                    viewableNode = new ViewablesCatalog.Node( "aaaa", false )
                }
            };
            skillLoc.secondary.SetFieldValue<SkillFamily>( "_skillFamily", secondaryFam );
        }
        private void AW_SetupPrimary()
        {
            var skillLoc = this.AW_body.GetComponent<SkillLocator>();

            var primaryFam = ScriptableObject.CreateInstance<SkillFamily>();
            var primaryDef = ScriptableObject.CreateInstance<SkillDef>();
            LoadoutAPI.AddSkillDef( primaryDef );
            LoadoutAPI.AddSkillFamily( primaryFam );
            LoadoutAPI.AddSkill( typeof( AWChargePrimary ) );
            LoadoutAPI.AddSkill( typeof( AWFirePrimary ) );

            primaryDef.activationState = new EntityStates.SerializableEntityStateType( typeof( AWChargePrimary ) );
            primaryDef.activationStateMachineName = "Weapon";
            primaryDef.baseMaxStock = 1;
            primaryDef.baseRechargeInterval = 3f;
            primaryDef.beginSkillCooldownOnSkillEnd = true;
            primaryDef.canceledFromSprinting = false;
            primaryDef.fullRestockOnAssign = true;
            primaryDef.icon = Resources.Load<Sprite>( "NotAPath" );
            primaryDef.interruptPriority = EntityStates.InterruptPriority.Any;
            primaryDef.isBullets = false;
            primaryDef.isCombatSkill = true;
            primaryDef.mustKeyPress = false;
            primaryDef.noSprint = false;
            primaryDef.rechargeStock = 1;
            primaryDef.requiredStock = 1;
            primaryDef.shootDelay = 0.1f;
            primaryDef.skillDescriptionToken = "";
            primaryDef.skillName = "";
            primaryDef.skillNameToken = "";
            primaryDef.stockToConsume = 1;

            primaryFam.variants = new SkillFamily.Variant[]
            {
                new SkillFamily.Variant
                {
                    skillDef = primaryDef,
                    unlockableName = "",
                    viewableNode = new ViewablesCatalog.Node("aaa", false )
                }
            };
            skillLoc.primary.SetFieldValue<SkillFamily>( "_skillFamily", primaryFam );
        }
    }
}
#endif