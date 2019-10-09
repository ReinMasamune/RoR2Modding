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
using RoR2.Projectile;
using BepInEx.Configuration;

namespace ReinHuntressSkills
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinHuntressSkills", "ReinHuntressSkills", "1.0.0")]

    public class ReinHuntressSkillsMain : BaseUnityPlugin
    {
        public static ConfigWrapper<bool> configWrappingPaper;
        //Using start because you should never use Awake() unless you specifically need to.
        public void Start()
        {
            configWrappingPaper = Config.Wrap<bool>("Settings", "Use default crosshair", "Use the default dot crosshair?", false);

            //Get the gameobject for huntress
            GameObject huntress = BodyCatalog.FindBodyPrefab("HuntressBody");
            if( !huntress )
            {
                Debug.Log("Huntress prefab not found, breaking");
                return;
            }

            if (!configWrappingPaper.Value)
            {
                huntress.GetComponent<CharacterBody>().crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/tiltedbracketcrosshair");
            }

            SkillFamily huntressPrimaryFamily = LoadoutUtilities.GetSkillFamily(huntress, SkillSlot.Primary);

            Sprite huntressPrimarySprite = Resources.Load<Sprite>("NotActuallyAPath");

            ViewablesCatalog.Node huntressPrimaryNode = LoadoutUtilities.CreateViewableNode("ReinHuntressPrimary");

            LoadoutUtilities.NewSkillInfo huntressPrimary = new LoadoutUtilities.NewSkillInfo
            {
                activationState = new SerializableEntityStateType(typeof(ReinHuntressSkills.Skills.Primary.HuntressPrimary)),
                activationStateMachineName = "Weapon",
                icon = huntressPrimarySprite,
                viewableNode = huntressPrimaryNode,
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

            LoadoutUtilities.AddSkillToVariants(huntressPrimaryFamily, huntressPrimary);
        }
    }
}
