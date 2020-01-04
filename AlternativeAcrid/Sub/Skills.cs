namespace AlternativeAcrid
{
    using R2API;
    using RoR2;
    using RoR2.Skills;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    partial class Main
    {
        private Int32 numberOfPellets = 5;

        private void RegisterSkills()
        {
            SkillAPI.AddSkill( typeof( States.Secondary.FireSpray ) );
            var newSecondaryDef = ScriptableObject.CreateInstance<SkillDef>();
            newSecondaryDef.activationState = new EntityStates.SerializableEntityStateType( typeof( States.Secondary.FireSpray ) );
            newSecondaryDef.activationStateMachineName = "Mouth";
            newSecondaryDef.baseMaxStock = 1;
            newSecondaryDef.baseRechargeInterval = 2f;
            newSecondaryDef.beginSkillCooldownOnSkillEnd = false;
            newSecondaryDef.canceledFromSprinting = false;
            newSecondaryDef.fullRestockOnAssign = true;
            newSecondaryDef.interruptPriority = EntityStates.InterruptPriority.Any;
            newSecondaryDef.isBullets = false;
            newSecondaryDef.isCombatSkill = true;
            newSecondaryDef.mustKeyPress = false;
            newSecondaryDef.noSprint = false;
            newSecondaryDef.rechargeStock = 1;
            newSecondaryDef.requiredStock = 1;
            newSecondaryDef.shootDelay = 0.3f;
            newSecondaryDef.skillDescriptionToken = "";
            newSecondaryDef.skillName = "";
            newSecondaryDef.skillNameToken = "";
            newSecondaryDef.stockToConsume = 1;
            newSecondaryDef.icon = Resources.Load<Sprite>( "NotAPath" );
            SkillFamily.Variant variant = new SkillFamily.Variant
            {
                skillDef = newSecondaryDef,
                unlockableName = "",
                viewableNode = new RoR2.ViewablesCatalog.Node("M2",false)
            };
            var skillLocator = acridBody.GetComponent<SkillLocator>();
            var skillFamily = skillLocator.secondary.skillFamily;
            var variants = skillFamily.variants;
            Array.Resize<SkillFamily.Variant>( ref variants, variants.Length + 1 );
            variants[variants.Length - 1] = variant;
            skillFamily.variants = variants;
            SkillAPI.AddSkillDef( newSecondaryDef );

            var refState = (EntityStates.Croco.FireSpit)EntityStates.EntityState.Instantiate( new EntityStates.SerializableEntityStateType( typeof( EntityStates.Croco.FireSpit ) ) );
            States.Secondary.FireSpray.effectPrefab = refState.effectPrefab;
            States.Secondary.FireSpray.attackString = refState.attackString;
            States.Secondary.FireSpray.baseDuration = refState.baseDuration;
            States.Secondary.FireSpray.bloom = refState.bloom;
            States.Secondary.FireSpray.force = refState.force;
            States.Secondary.FireSpray.recoilAmplitude = refState.recoilAmplitude;
            States.Secondary.FireSpray.damageRatio = refState.damageCoefficient * 1.1f / numberOfPellets;
            States.Secondary.FireSpray.spreadCone = Mathf.PI / 36f;
            States.Secondary.FireSpray.pellets = numberOfPellets;

            SkillAPI.AddSkill( typeof( States.Utility.PrepPoisonJump ) );
            SkillAPI.AddSkill( typeof( States.Utility.PrepChainJump ) );
            SkillAPI.AddSkill( typeof( States.Utility.BasePrepJump ) );

            var utilityFamily = skillLocator.utility.skillFamily;

            var utilDef1 = utilityFamily.variants[0].skillDef;
            var utilDef2 = utilityFamily.variants[1].skillDef;

            utilDef1.activationState = new EntityStates.SerializableEntityStateType( typeof( States.Utility.PrepPoisonJump ) );
            utilDef2.activationState = new EntityStates.SerializableEntityStateType( typeof( States.Utility.PrepChainJump ) );

            utilDef1.mustKeyPress = true;
            utilDef2.mustKeyPress = true;
            utilDef1.beginSkillCooldownOnSkillEnd = true;
            utilDef2.beginSkillCooldownOnSkillEnd = true;
            utilDef1.noSprint = false;
            utilDef2.noSprint = false;
        }
    }
}
