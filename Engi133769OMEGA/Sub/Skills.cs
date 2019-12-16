using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Engi133769OMEGA
{
    public partial class OmegaTurretMain
    {
        private void CreateEngiSkill()
        {
            SkillDef def = ScriptableObject.CreateInstance<SkillDef>();
            var family = engi.GetComponent<SkillLocator>().special.skillFamily;

            def.activationState = new EntityStates.SerializableEntityStateType( typeof( Skills.Engi.Special.PlaceOmegaTurret ) );
            def.activationStateMachineName = "Weapon";
            def.interruptPriority = EntityStates.InterruptPriority.Skill;
            def.icon = family.variants[1].skillDef.icon;

            def.baseRechargeInterval = (_DEBUGGING ? 1f : 60f);
            def.shootDelay = 0.0f;

            def.baseMaxStock = 1;
            def.rechargeStock = 1;
            def.requiredStock = 1;
            def.stockToConsume = 0;

            def.isBullets = false;
            def.beginSkillCooldownOnSkillEnd = true;
            def.isCombatSkill = false;
            def.noSprint = true;
            def.canceledFromSprinting = false;
            def.mustKeyPress = false;
            def.fullRestockOnAssign = true;

            var variant = new SkillFamily.Variant
            {
                viewableNode = new RoR2.ViewablesCatalog.Node("OmegaNodeThing", false ),
                skillDef = def,
                unlockableName = ""
            };

            RoR2Plugin.SkillsAPI.AddSkillDef( def );

            Array.Resize<SkillFamily.Variant>( ref family.variants, family.variants.Length + 1 );
            family.variants[family.variants.Length - 1] = variant;
        }
    }
}
