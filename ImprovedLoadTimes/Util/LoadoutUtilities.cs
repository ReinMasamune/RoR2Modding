using EntityStates;
using RoR2;
using RoR2.Skills;
using R2API;
using R2API.Utils;
using UnityEngine;

namespace ImprovedLoadTimes.Util
{
    public static class LoadoutUtilities
    {
        public struct NewSkillInfo
        {
            public SerializableEntityStateType activationState;
            public string activationStateMachineName;
            public Sprite icon;
            public ViewablesCatalog.Node viewableNode;
            public string unlockableName;
            public string skillName;
            public string skillNameToken;
            public string skillDescriptionToken;
            public InterruptPriority interruptPriority;
            public float baseRechargeInterval;
            public int baseMaxStock;
            public int rechargeStock;
            public bool isBullets;
            public float shootDelay;
            public bool beginSkillCooldownOnSkillEnd;
            public int requiredStock;
            public int stockToConsume;
            public bool isCombatSkill;
            public bool noSprint;
            public bool canceledFromSprinting;
            public bool mustKeyPress;
            public bool fullRestockOnAssign;
        }

        public static NewSkillInfo GetGenericPrimaryNewSkillInfo()
        {
            NewSkillInfo nsi = new NewSkillInfo
            {
                unlockableName = "",
                skillName = "",
                skillNameToken = "",
                skillDescriptionToken = "",
                interruptPriority = InterruptPriority.Skill,
                baseRechargeInterval = 0f,
                baseMaxStock = 1,
                rechargeStock = 1,
                isBullets = false,
                shootDelay = 0.3f,
                beginSkillCooldownOnSkillEnd = false,
                requiredStock = 1,
                stockToConsume = 0,
                isCombatSkill = true,
                noSprint = true,
                canceledFromSprinting = true,
                mustKeyPress = true,
                fullRestockOnAssign = true
            };
            return nsi;
        }

        public static void AddSkillToVariants( SkillFamily fam , NewSkillInfo nsi )
        {
            SkillDef skill = ScriptableObject.CreateInstance<SkillDef>();
            skill.activationState = nsi.activationState;
            skill.activationStateMachineName = nsi.activationStateMachineName;
            skill.icon = nsi.icon;
            skill.skillName = nsi.skillName;
            skill.skillNameToken = nsi.skillNameToken;
            skill.skillDescriptionToken = nsi.skillDescriptionToken;
            skill.interruptPriority = nsi.interruptPriority;
            skill.baseRechargeInterval = nsi.baseRechargeInterval;
            skill.baseMaxStock = nsi.baseMaxStock;
            skill.rechargeStock = nsi.rechargeStock;
            skill.isBullets = nsi.isBullets;
            skill.shootDelay = nsi.shootDelay;
            skill.beginSkillCooldownOnSkillEnd = nsi.beginSkillCooldownOnSkillEnd;
            skill.requiredStock = nsi.requiredStock;
            skill.stockToConsume = nsi.stockToConsume;
            skill.isCombatSkill = nsi.isCombatSkill;
            skill.noSprint = nsi.noSprint;
            skill.canceledFromSprinting = nsi.canceledFromSprinting;
            skill.mustKeyPress = nsi.mustKeyPress;
            skill.fullRestockOnAssign = nsi.fullRestockOnAssign;

            AddSkillToVariants(fam, skill, nsi.viewableNode , nsi.unlockableName );
        }

        public static void AddSkillToVariants( SkillFamily fam , SkillDef skill, ViewablesCatalog.Node viewableNode , string unlockableName = "")
        {
            SkillFamily.Variant variant = new SkillFamily.Variant();
            variant.skillDef = skill;
            variant.viewableNode = viewableNode;
            variant.unlockableName = unlockableName;

            int prevLength = fam.variants.Length;
            System.Array.Resize<SkillFamily.Variant>(ref fam.variants, prevLength + 1);
            fam.variants[prevLength] = variant;
        }

        public static void ReplaceSkillVariant( SkillFamily fam , NewSkillInfo nsi, uint index = 0 )
        {
            SkillDef skill = ScriptableObject.CreateInstance<SkillDef>();
            skill.activationState = nsi.activationState;
            skill.activationStateMachineName = nsi.activationStateMachineName;
            skill.icon = nsi.icon;
            skill.skillName = nsi.skillName;
            skill.skillNameToken = nsi.skillNameToken;
            skill.skillDescriptionToken = nsi.skillDescriptionToken;
            skill.interruptPriority = nsi.interruptPriority;
            skill.baseRechargeInterval = nsi.baseRechargeInterval;
            skill.baseMaxStock = nsi.baseMaxStock;
            skill.rechargeStock = nsi.rechargeStock;
            skill.isBullets = nsi.isBullets;
            skill.shootDelay = nsi.shootDelay;
            skill.beginSkillCooldownOnSkillEnd = nsi.beginSkillCooldownOnSkillEnd;
            skill.requiredStock = nsi.requiredStock;
            skill.stockToConsume = nsi.stockToConsume;
            skill.isCombatSkill = nsi.isCombatSkill;
            skill.noSprint = nsi.noSprint;
            skill.canceledFromSprinting = nsi.canceledFromSprinting;
            skill.mustKeyPress = nsi.mustKeyPress;
            skill.fullRestockOnAssign = nsi.fullRestockOnAssign;

            SkillFamily.Variant variant = new SkillFamily.Variant();
            variant.skillDef = skill;
            variant.viewableNode = nsi.viewableNode;
            variant.unlockableName = nsi.unlockableName;
            fam.variants[index] = variant;
        }

        public static SkillFamily GetSkillFamily(GameObject body , SkillSlot slot )
        {
            SkillLocator sl = body.GetComponent<SkillLocator>();
            GenericSkill skill;
            switch ( slot )
            {
                case SkillSlot.Primary:
                    skill = sl.primary;
                    break;

                case SkillSlot.Secondary:
                    skill = sl.secondary;
                    break;

                case SkillSlot.Utility:
                    skill = sl.utility;
                    break;

                case SkillSlot.Special:
                    skill = sl.special;
                    break;

                default:
                    skill = sl.primary;
                    break;
            }
            if ( !skill )
            {
                skill = body.AddComponent<GenericSkill>();
                switch( slot )
                {
                    case SkillSlot.Primary:
                        sl.primary = skill;
                        break;

                    case SkillSlot.Secondary:
                        sl.secondary = skill;
                        break;

                    case SkillSlot.Utility:
                        sl.utility = skill;
                        break;

                    case SkillSlot.Special:
                        sl.special = skill;
                        break;

                    default:
                        break;
                }
            }
            if ( !skill.skillFamily )
            {
                skill.SetFieldValue("_skillFamily", ScriptableObject.CreateInstance<SkillFamily>());
                skill.skillFamily.defaultVariantIndex = 0;
                skill.skillFamily.variants = new SkillFamily.Variant[0];
            }
            return skill.skillFamily;
        }

        public static SkillFamily GetSkillFamily(CharacterBody body, SkillSlot slot )
        {
            return GetSkillFamily(body.gameObject, slot);
        }
        
        public static ViewablesCatalog.Node CreateViewableNode(string name)
        {
            ViewablesCatalog.Node node = new ViewablesCatalog.Node(name , false );
            return node;
        }

        public static void LogAllEntityStateIndex()
        {
            EntityState state;
            for( short i = 0; i < 500; i++ )
            {
                state = EntityState.Instantiate(i);
                Debug.Log(i + ":" + state.GetType().ToString());
            }
        }
    }
}
