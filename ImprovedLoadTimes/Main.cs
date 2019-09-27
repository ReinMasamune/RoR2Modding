using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using R2API;
using R2API.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using EntityStates;
using ImprovedLoadTimes.Util;
using RoR2.Skills;

namespace ImprovedLoadTimes
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ImprovedLoadTime", "ImprovedLoadTime", "1.0.0")]
    public class ImprovedLoadTimesMain : BaseUnityPlugin
    {
        public void Start()
        {
            GameObject oneBodyMan = BodyCatalog.FindBodyPrefab("LoaderBody");
            SkillFamily oneLoaderFamilyMan = LoadoutUtilities.GetSkillFamily(oneBodyMan, SkillSlot.Utility);
            Sprite oneSpriteMan = Resources.Load<Sprite>("ONEPUNCH");
            ViewablesCatalog.Node oneNodeMan = LoadoutUtilities.CreateViewableNode("OneNodeMan");

            LoadoutUtilities.NewSkillInfo onePunchSkillMan = new LoadoutUtilities.NewSkillInfo
            {
                activationState = new SerializableEntityStateType(typeof(ImprovedLoadTimes.Skills.Utility.AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH)),
                activationStateMachineName = "Weapon",
                icon = oneSpriteMan,
                viewableNode = oneNodeMan,
                unlockableName = "",
                skillName = "OnePunch",
                skillNameToken = "ONE PUNCH",
                skillDescriptionToken = "ONE PUNCH!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!",
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
            LoadoutUtilities.AddSkillToVariants(oneLoaderFamilyMan, onePunchSkillMan);
        }
    }
}
