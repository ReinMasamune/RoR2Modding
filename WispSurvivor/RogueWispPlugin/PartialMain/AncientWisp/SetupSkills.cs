#if ANCIENTWISP
using RoR2;
using RoR2.Skills;
using UnityEngine;
using ReinCore;
using EntityStates;

namespace Rein.RogueWispPlugin
{
    // TODO: Skills
    internal partial class Main
    {
        private static Accessor<GenericSkill,SkillFamily> skillFamily
        {
            get
            {
                if( _skillFamily == null )
                {
                    _skillFamily = new Accessor<GenericSkill, SkillFamily>( "_skillFamily" );
                }
                return _skillFamily;
            }
        }
        private static Accessor<GenericSkill,SkillFamily> _skillFamily;
        partial void AW_SetupSkills()
        {
            this.Load += this.AW_SetupPrimary;
            this.Load += this.AW_SetupSecondary;
            this.Load += this.AW_SetupUtility;
            this.Load += this.AW_SetupEnrage;
        }

        private void AW_SetupEnrage()
        {
            SkillsCore.AddSkill( typeof( AWDefaultMain ) );
            SkillsCore.AddSkill( typeof( AWEnrageTransition ) );
            SkillsCore.AddSkill( typeof( AWEnrageMainState ) );
        } 

        private void AW_SetupUtility()
        {
            var skillLoc = this.AW_body.GetComponent<SkillLocator>();
            var utilityFam = ScriptableObject.CreateInstance<SkillFamily>();
            var utilityDef = ScriptableObject.CreateInstance<SkillDef>();
            SkillsCore.AddSkillDef( utilityDef );
            SkillsCore.AddSkillFamily( utilityFam );
            SkillsCore.AddSkill( typeof( AWChargeUtility ) );
            SkillsCore.AddSkill( typeof( AWFireUtility ) );
            

            utilityDef.activationState = new SerializableEntityStateType( typeof( AWChargeUtility ) );
            utilityDef.activationStateMachineName = "Weapon";
            utilityDef.baseMaxStock = 1;
            utilityDef.baseRechargeInterval = 23f;
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
            skillFamily.Set( skillLoc.utility, utilityFam );
        }
        private void AW_SetupSecondary()
        {
            var skillLoc = this.AW_body.GetComponent<SkillLocator>();
            var secondaryFam = ScriptableObject.CreateInstance<SkillFamily>();
            var secondaryDef = ScriptableObject.CreateInstance<SkillDef>();
            SkillsCore.AddSkillDef( secondaryDef );
            SkillsCore.AddSkillFamily( secondaryFam );
            SkillsCore.AddSkill( typeof( AWSecondary ) );

            secondaryDef.activationState = new SerializableEntityStateType( typeof( AWSecondary ) );
            secondaryDef.activationStateMachineName = "Weapon";
            secondaryDef.baseMaxStock = 1;
            secondaryDef.baseRechargeInterval = 31f;
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
            skillFamily.Set( skillLoc.secondary, secondaryFam );
        }
        private void AW_SetupPrimary()
        {
            var skillLoc = this.AW_body.GetComponent<SkillLocator>();

            var primaryFam = ScriptableObject.CreateInstance<SkillFamily>();
            var primaryDef = ScriptableObject.CreateInstance<SkillDef>();
            SkillsCore.AddSkillDef( primaryDef );
            SkillsCore.AddSkillFamily( primaryFam );
            SkillsCore.AddSkill( typeof( AWChargePrimary ) );
            SkillsCore.AddSkill( typeof( AWFirePrimary ) );

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

            skillFamily.Set( skillLoc.primary, primaryFam );
        }
    }
}
#endif