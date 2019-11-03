using RoR2;
using UnityEngine;
using R2API.Utils;
using System;
using System.Collections.Generic;
using static WispSurvivor.Util.PrefabUtilities;
using static WispSurvivor.Util.SkillsHelper;
using RoR2.Skills;
using EntityStates;

namespace WispSurvivor.Modules
{
    public static class WispSkillsModule
    {
        public static void DoModule(GameObject body, Dictionary<Type, Component> dic)
        {
            RegisterStates();
            SkillLocator SL = SetupGenericSkills(body, dic);
            SkillFamily[] SF = SetupSkillFamilies(body, dic, SL);
            DoStatemachines(body, dic);
            DoPassiveStuff(body, SL);
            DoPrimaries(body, dic, SL, SF);
            DoSecondaries(body, dic, SL, SF);
            DoUtilities(body, dic, SL, SF);
            DoSpecials(body, dic, SL, SF);
        }

        private static void RegisterStates()
        {
            AddSkill(typeof(WispSurvivor.Skills.Primary.PrepHeatwave));
            AddSkill(typeof(WispSurvivor.Skills.Primary.FireHeatwave));
            AddSkill(typeof(WispSurvivor.Skills.Secondary.TestSecondary));
            AddSkill(typeof(WispSurvivor.Skills.Utility.TestUtility));
            AddSkill(typeof(Skills.Utility.PrepGaze));
            AddSkill(typeof(Skills.Utility.FireGaze));
            AddSkill(typeof(WispSurvivor.Skills.Special.TestSpecial));
            AddSkill(typeof(WispSurvivor.Skills.Special.TestSpecialFire));
        }

        private static SkillLocator SetupGenericSkills(GameObject body, Dictionary<Type, Component> dic)
        {
            foreach( GenericSkill g in body.GetComponents<GenericSkill>() )
            {
                MonoBehaviour.DestroyImmediate(g);
            }

            SkillLocator SL = body.AddOrGetComponent<SkillLocator>();
            if (!SL.primary)
            {
                SL.primary = body.AddComponent<GenericSkill>();
            }
            if (!SL.secondary)
            {
                SL.secondary = body.AddComponent<GenericSkill>();
            }
            if (!SL.utility)
            {
                SL.utility = body.AddComponent<GenericSkill>();
            }
            if (!SL.special)
            {
                SL.special = body.AddComponent<GenericSkill>();
            }

            return SL;
        }

        private static SkillFamily[] SetupSkillFamilies(GameObject body, Dictionary<Type, Component> dic, SkillLocator SL)
        {
            SkillFamily[] skillFams = new SkillFamily[4];
            skillFams[0] = GetNewSkillFamily(SL.primary);
            skillFams[1] = GetNewSkillFamily(SL.secondary);
            skillFams[2] = GetNewSkillFamily(SL.utility);
            skillFams[3] = GetNewSkillFamily(SL.special);

            return skillFams;
        }

        private static void DoStatemachines(GameObject body, Dictionary<Type, Component> dic)
        {
            NetworkStateMachine net = dic.C<NetworkStateMachine>();

            EntityStateMachine[] netStates = net.GetFieldValue<EntityStateMachine[]>("stateMachines");
            Array.Resize<EntityStateMachine>(ref netStates, 3);

            SetStateOnHurt hurtState = dic.C<SetStateOnHurt>();
            hurtState.canBeFrozen = true;
            hurtState.canBeHitStunned = false;
            hurtState.canBeStunned = false;
            hurtState.hitThreshold = 5f;
            hurtState.hurtState = new SerializableEntityStateType(typeof( EntityStates.FrozenState));

            foreach( EntityStateMachine esm in body.GetComponents<EntityStateMachine>() )
            {
                switch( esm.customName )
                {
                    case "Body":
                        esm.initialStateType = new SerializableEntityStateType(typeof(SpawnTeleporterState));
                        esm.mainStateType = new SerializableEntityStateType(typeof(GenericCharacterMain));
                        netStates[0] = esm;
                        hurtState.targetStateMachine = esm;
                        break;

                    case "Weapon":
                        esm.initialStateType = new SerializableEntityStateType(typeof(Idle));
                        esm.mainStateType = new SerializableEntityStateType(typeof(Idle));
                        netStates[1] = esm;
                        break;

                    case "Gaze":
                        esm.initialStateType = new SerializableEntityStateType(typeof(Idle));
                        esm.mainStateType = new SerializableEntityStateType(typeof(Idle));
                        netStates[2] = esm;
                        break;

                    default:
                        Debug.Log("Wisp has an extra statemachine");
                        break;
                }
            }
        }

        private static void DoPassiveStuff( GameObject body, SkillLocator sl )
        {
            sl.passiveSkill.enabled = true;
            sl.passiveSkill.icon = Resources.Load<Sprite>("NotAPath");
            sl.passiveSkill.skillNameToken = "WISP_SURVIVOR_PASSIVE_NAME";
            sl.passiveSkill.skillDescriptionToken = "WISP_SURVIVOR_PASSIVE_DESC";
        }

        private static void DoPrimaries(GameObject body, Dictionary<Type, Component> dic, SkillLocator SL, SkillFamily[] fam)
        {
            SkillDef[] primaries = new SkillDef[1];
            primaries[0] = DoPrimary1(body, dic);

            AssignVariants(fam[0], primaries);
        }

        private static SkillDef DoPrimary1(GameObject body, Dictionary<Type, Component> dic)
        {
            SkillDef skill = ScriptableObject.CreateInstance<SkillDef>();
            skill.activationState = new SerializableEntityStateType(typeof(Skills.Primary.PrepHeatwave));
            skill.activationStateMachineName = "Weapon";

            skill.baseMaxStock = 3;
            skill.baseRechargeInterval = 2f;
            skill.beginSkillCooldownOnSkillEnd = true;
            skill.canceledFromSprinting = false;
            skill.fullRestockOnAssign = true;
            skill.interruptPriority = InterruptPriority.Any;
            skill.isBullets = true;
            skill.isCombatSkill = true;
            skill.mustKeyPress = false;
            skill.noSprint = true;
            skill.rechargeStock = 3;
            skill.requiredStock = 0;
            skill.shootDelay = 0.5f;
            skill.stockToConsume = 0;

            skill.icon = Resources.Load<Sprite>("NotAPath");
            skill.skillDescriptionToken = "WISP_SURVIVOR_PRIMARY_1_DESC";
            skill.skillName = "Primry1";
            skill.skillNameToken = "WISP_SURVIVOR_PRIMARY_1_NAME";

            return skill;
        }

        private static void DoSecondaries(GameObject body, Dictionary<Type, Component> dic, SkillLocator SL, SkillFamily[] fam)
        {
            SkillDef[] secondaries = new SkillDef[1];
            secondaries[0] = DoSecondary1(body, dic);

            AssignVariants(fam[1], secondaries);
        }

        private static SkillDef DoSecondary1(GameObject body, Dictionary<Type, Component> dic)
        {
            SkillDef skill = ScriptableObject.CreateInstance<SkillDef>();
            skill.activationState = new SerializableEntityStateType(typeof(Skills.Secondary.TestSecondary));
            skill.activationStateMachineName = "Weapon";

            skill.baseMaxStock = 1;
            skill.baseRechargeInterval = 6f;
            skill.beginSkillCooldownOnSkillEnd = false;
            skill.canceledFromSprinting = false;
            skill.fullRestockOnAssign = true;
            skill.interruptPriority = InterruptPriority.Skill;
            skill.isBullets = false;
            skill.isCombatSkill = true;
            skill.mustKeyPress = true;
            skill.noSprint = true;
            skill.rechargeStock = 1;
            skill.requiredStock = 1;
            skill.shootDelay = 0.5f;
            skill.stockToConsume = 1;

            skill.icon = Resources.Load<Sprite>("NotAPath");
            skill.skillDescriptionToken = "WISP_SURVIVOR_SECONDARY_1_DESC";
            skill.skillName = "Secondary1";
            skill.skillNameToken = "WISP_SURVIVOR_SECONDARY_1_NAME";

            return skill;
        }



        private static void DoUtilities(GameObject body, Dictionary<Type, Component> dic, SkillLocator SL, SkillFamily[] fam)
        {
            SkillDef[] utilities = new SkillDef[1];
            utilities[0] = DoUtility1(body, dic);

            AssignVariants(fam[2], utilities);
        }

        private static SkillDef DoUtility1(GameObject body, Dictionary<Type, Component> dic)
        {
            SkillDef skill = ScriptableObject.CreateInstance<SkillDef>();
            skill.activationState = new SerializableEntityStateType(typeof(Skills.Utility.PrepGaze));
            skill.activationStateMachineName = "Gaze";

            skill.baseMaxStock = 1;
            skill.baseRechargeInterval = 10f;
            skill.beginSkillCooldownOnSkillEnd = false;
            skill.canceledFromSprinting = false;
            skill.fullRestockOnAssign = true;
            skill.interruptPriority = InterruptPriority.Skill;
            skill.isBullets = false;
            skill.isCombatSkill = true;
            skill.mustKeyPress = false;
            skill.noSprint = false;
            skill.rechargeStock = 1;
            skill.requiredStock = 1;
            skill.shootDelay = 0.5f;
            skill.stockToConsume = 1;

            skill.icon = Resources.Load<Sprite>("NotAPath");
            skill.skillDescriptionToken = "WISP_SURVIVOR_UTILITY_1_DESC";
            skill.skillName = "Utility1";
            skill.skillNameToken = "WISP_SURVIVOR_UTILITY_1_NAME";

            return skill;
        }



        private static void DoSpecials(GameObject body, Dictionary<Type, Component> dic, SkillLocator SL, SkillFamily[] fam)
        {
            SkillDef[] specials = new SkillDef[1];
            specials[0] = DoSpecial1(body, dic);

            AssignVariants(fam[3], specials);
        }

        private static SkillDef DoSpecial1(GameObject body, Dictionary<Type, Component> dic)
        {
            SkillDef skill = ScriptableObject.CreateInstance<SkillDef>();
            skill.activationState = new SerializableEntityStateType(typeof(Skills.Special.TestSpecial));
            skill.activationStateMachineName = "Weapon";

            skill.baseMaxStock = 1;
            skill.baseRechargeInterval = 6f;
            skill.beginSkillCooldownOnSkillEnd = true;
            skill.canceledFromSprinting = false;
            skill.fullRestockOnAssign = true;
            skill.interruptPriority = InterruptPriority.PrioritySkill;
            skill.isBullets = false;
            skill.isCombatSkill = true;
            skill.mustKeyPress = true;
            skill.noSprint = true;
            skill.rechargeStock = 1;
            skill.requiredStock = 1;
            skill.shootDelay = 0.5f;
            skill.stockToConsume = 1;

            skill.icon = Resources.Load<Sprite>("NotAPath");
            skill.skillDescriptionToken = "WISP_SURVIVOR_SPECIAL_1_DESC";
            skill.skillName = "Special1";
            skill.skillNameToken = "WISP_SURVIVOR_SPECIAL_1_NAME";

            return skill;
        }



        private static void ExFunction(GameObject body, Dictionary<Type, Component> dic)
        {

        }

        private static T C<T>(this Dictionary<Type, Component> dic) where T : Component
        {
            return dic[typeof(T)] as T;
        }

        private static SkillFamily GetNewSkillFamily(GenericSkill s)
        {
            //if( !s.skillFamily )
            //{
            s.SetFieldValue<SkillFamily>("_skillFamily", ScriptableObject.CreateInstance<SkillFamily>());
            //}
            s.skillFamily.variants = new SkillFamily.Variant[0];
            return s.skillFamily;
        }

        private static void AssignVariants(SkillFamily fam, SkillDef[] skills)
        {
            SkillFamily.Variant[] variants = new SkillFamily.Variant[skills.Length];

            for( int i = 0; i < skills.Length; i++ )
            {
                variants[i] = new SkillFamily.Variant
                {
                    skillDef = skills[i],
                    unlockableName = "",
                    viewableNode = new ViewablesCatalog.Node(skills[i].skillNameToken, false)
                };
            }

            fam.variants = variants;
        }

    }
}
