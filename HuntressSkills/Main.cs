using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using ReinHuntressSkills.Misc;
using System;


namespace ReinHuntressSkills
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinHuntressSkills", "ReinHuntressSkills", "1.0.0")]

    public class ReinHuntressSkillsMain : BaseUnityPlugin
    {
        //Throwing some vars up here for config and whatnot.
        private const int newPrimariesCount = 1;
        private const int newSecondariesCount = 0;
        private const int newUtilitiesCount = 0;
        private const int newSpecialsCount = 0;

        //Using start because you should never use Awake() unless you specifically need to.
        public void Start()
        {
            //Get the gameobject for huntress
            GameObject huntress = BodyCatalog.FindBodyPrefab("HuntressBody");
            if( !huntress )
            {
                Debug.Log("Huntress prefab not found, breaking");
                return;
            }

            SkillFamily huntressPrimaryFamily = LoadoutUtilities.GetSkillFamily(huntress, SkillSlot.Primary);

            //Trick the universe into thinking we actually have an icon
            Sprite huntressPrimarySprite1 = Resources.Load<Sprite>("NotActuallyAPath");
            Sprite huntressPrimarySprite2 = Resources.Load<Sprite>("NotActuallyAPath");
            Sprite huntressPrimarySprite3 = Resources.Load<Sprite>("NotActuallyAPath");
            Sprite huntressPrimarySprite4 = Resources.Load<Sprite>("NotActuallyAPath");

            // Get viewable nodes for skills. No clue what these even do!
            ViewablesCatalog.Node huntressPrimary1Node = LoadoutUtilities.CreateViewableNode("ReinHuntressPrimary1");
            ViewablesCatalog.Node huntressPrimary2Node = LoadoutUtilities.CreateViewableNode("ReinHuntressPrimary2");
            ViewablesCatalog.Node huntressPrimary3Node = LoadoutUtilities.CreateViewableNode("ReinHuntressPrimary3");
            ViewablesCatalog.Node huntressPrimary4Node = LoadoutUtilities.CreateViewableNode("ReinHuntressPrimary4");

            //Create a newskillinfo struct and assign values as needed
            LoadoutUtilities.NewSkillInfo huntressPrimary1 = new LoadoutUtilities.NewSkillInfo
            {
                activationState = new SerializableEntityStateType(typeof(ReinHuntressSkills.Skills.Primary.HuntressPrimary1)),
                activationStateMachineName = "Weapon",
                icon = huntressPrimarySprite1,
                viewableNode = huntressPrimary1Node,
                unlockableName = "",
                skillName = "RandomName1",
                skillNameToken = "RandomName1",
                skillDescriptionToken = "This skill does stuff",
                interruptPriority = InterruptPriority.Any,
                baseRechargeInterval = 0f,
                baseMaxStock = 1,
                rechargeStock = 1,
                isBullets = false,
                shootDelay = 0.3f,
                beginSkillCooldownOnSkillEnd = false,
                requiredStock = 1,
                stockToConsume = 1,
                canceledFromSprinting = false,
                noSprint = false,
                isCombatSkill = true,
                mustKeyPress = false,
                fullRestockOnAssign = true
            };

            LoadoutUtilities.NewSkillInfo huntressPrimary2 = new LoadoutUtilities.NewSkillInfo
            {
                activationState = new SerializableEntityStateType(typeof(ReinHuntressSkills.Skills.Primary.HuntressPrimary2)),
                activationStateMachineName = "Weapon",
                icon = huntressPrimarySprite1,
                viewableNode = huntressPrimary2Node,
                unlockableName = "",
                skillName = "RandomName1",
                skillNameToken = "RandomName1",
                skillDescriptionToken = "This skill does stuff",
                interruptPriority = InterruptPriority.Any,
                baseRechargeInterval = 0f,
                baseMaxStock = 1,
                rechargeStock = 1,
                isBullets = false,
                shootDelay = 0.3f,
                beginSkillCooldownOnSkillEnd = false,
                requiredStock = 1,
                stockToConsume = 1,
                canceledFromSprinting = false,
                noSprint = false,
                isCombatSkill = true,
                mustKeyPress = false,
                fullRestockOnAssign = true
            };

            LoadoutUtilities.NewSkillInfo huntressPrimary3 = new LoadoutUtilities.NewSkillInfo
            {
                activationState = new SerializableEntityStateType(typeof(ReinHuntressSkills.Skills.Primary.HuntressPrimary3)),
                activationStateMachineName = "Weapon",
                icon = huntressPrimarySprite1,
                viewableNode = huntressPrimary3Node,
                unlockableName = "",
                skillName = "RandomName1",
                skillNameToken = "RandomName1",
                skillDescriptionToken = "This skill does stuff",
                interruptPriority = InterruptPriority.Any,
                baseRechargeInterval = 0f,
                baseMaxStock = 1,
                rechargeStock = 1,
                isBullets = false,
                shootDelay = 0.3f,
                beginSkillCooldownOnSkillEnd = false,
                requiredStock = 1,
                stockToConsume = 1,
                canceledFromSprinting = false,
                noSprint = false,
                isCombatSkill = true,
                mustKeyPress = false,
                fullRestockOnAssign = true
            };

            LoadoutUtilities.NewSkillInfo huntressPrimary4 = new LoadoutUtilities.NewSkillInfo
            {
                activationState = new SerializableEntityStateType(typeof(ReinHuntressSkills.Skills.Primary.HuntressPrimary4)),
                activationStateMachineName = "Weapon",
                icon = huntressPrimarySprite1,
                viewableNode = huntressPrimary4Node,
                unlockableName = "",
                skillName = "RandomName1",
                skillNameToken = "RandomName1",
                skillDescriptionToken = "This skill does stuff",
                interruptPriority = InterruptPriority.Any,
                baseRechargeInterval = 0f,
                baseMaxStock = 1,
                rechargeStock = 1,
                isBullets = false,
                shootDelay = 0.3f,
                beginSkillCooldownOnSkillEnd = false,
                requiredStock = 1,
                stockToConsume = 1,
                canceledFromSprinting = false,
                noSprint = false,
                isCombatSkill = true,
                mustKeyPress = false,
                fullRestockOnAssign = true
            };

            LoadoutUtilities.AddSkillToVariants(huntressPrimaryFamily, huntressPrimary1);
            LoadoutUtilities.AddSkillToVariants(huntressPrimaryFamily, huntressPrimary2);
            LoadoutUtilities.AddSkillToVariants(huntressPrimaryFamily, huntressPrimary3);
            LoadoutUtilities.AddSkillToVariants(huntressPrimaryFamily, huntressPrimary4);

            //Modify any of the basic skill values as needed
        }
    }
}
