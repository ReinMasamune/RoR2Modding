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

            //Get the skilllocator component
            SkillLocator huntressSL = huntress.GetComponent<SkillLocator>();
            if( !huntressSL )
            {
                Debug.Log("Huntress prefab does not have a skillLocator, breaking");
                return;
            }

            //Get the 4 skills from the skilllocator
            GenericSkill huntressPrimary = huntressSL.primary;
            if( !huntressPrimary )
            {
                Debug.Log("Huntress skilllocator does not have a primary, breaking");
                return;
            }
            GenericSkill huntressSecondary = huntressSL.secondary;
            if (!huntressSecondary)
            {
                Debug.Log("Huntress skilllocator does not have a secondary, breaking");
                return;
            }
            GenericSkill huntressUtility = huntressSL.utility;
            if (!huntressUtility)
            {
                Debug.Log("Huntress skilllocator does not have a utility, breaking");
                return;
            }
            GenericSkill huntressSpecial = huntressSL.special;
            if (!huntressSpecial)
            {
                Debug.Log("Huntress skilllocator does not have a special, breaking");
                return;
            }

            //Get the skillFamily from each skill
            SkillFamily huntressPrimaryFamily = huntressPrimary.skillFamily;
            if( !huntressPrimaryFamily )
            {
                Debug.Log("No skillfamily for huntress primar, breaking");
                return;
            }
            SkillFamily huntressSecondaryFamily = huntressSecondary.skillFamily;
            if (!huntressSecondaryFamily)
            {
                Debug.Log("No skillfamily for huntress secondary, breaking");
                return;
            }
            SkillFamily huntressUtilityFamily = huntressUtility.skillFamily;
            if (!huntressUtilityFamily)
            {
                Debug.Log("No skillfamily for huntress utility, breaking");
                return;
            }
            SkillFamily huntressSpecialFamily = huntressSpecial.skillFamily;
            if (!huntressSpecialFamily)
            {
                Debug.Log("No skillfamily for huntress special, breaking");
                return;
            }

            //Checking the lengths of the variants arrays, this will tell us if we need to replace them with new arrays
            Debug.Log("Primary Length: " + huntressPrimaryFamily.variants.Length);
            //Debug.Log("Secondary Length: " + huntressSecondaryFamily.variants.Length);
            //Debug.Log("Utility Length: " + huntressUtilityFamily.variants.Length);
            //Debug.Log("Special Length: " + huntressSpecialFamily.variants.Length);

            //Change the size of the array to fit however many new skills we are adding
            Array.Resize<SkillFamily.Variant>(ref huntressPrimaryFamily.variants, huntressPrimaryFamily.variants.Length + newPrimariesCount);
            //Array.Resize<SkillFamily.Variant>(ref huntressSecondaryFamily.variants, huntressSecondaryFamily.variants.Length + newPrimariesCount);
            //Array.Resize<SkillFamily.Variant>(ref huntressUtilityFamily.variants, huntressUtilityFamily.variants.Length + newPrimariesCount);
            //Array.Resize<SkillFamily.Variant>(ref huntressSpecialFamily.variants, huntressSpecialFamily.variants.Length + newPrimariesCount);

            //Log the new lengths so we can make sure things worked
            Debug.Log("Primary Length: " + huntressPrimaryFamily.variants.Length);
            //Debug.Log("Secondary Length: " + huntressSecondaryFamily.variants.Length);
            //Debug.Log("Utility Length: " + huntressUtilityFamily.variants.Length);
            //Debug.Log("Special Length: " + huntressSpecialFamily.variants.Length);

            //Trick the universe into thinking we actually have an icon
            Sprite huntressPrimarySprite1 = Resources.Load<Sprite>("NotActuallyAPath");
            //Sprite huntressSecondarySprite1 = Resources.Load<Sprite>("NotActuallyAPath");
            //Sprite huntressUtilitySprite1 = Resources.Load<Sprite>("NotActuallyAPath");
            //Sprite huntressSpecialSprite1 = Resources.Load<Sprite>("NotActuallyAPath");

            //Use helper func to create a skilldef with the state defined in our plugin (in a different file)
            SkillDef huntressPrimary1 = LoadoutUtilities.CreateSkillDef(new SerializableEntityStateType(typeof(ReinHuntressSkills.Skills.Primary.HuntressPrimary1)), "Weapon", huntressPrimarySprite1);
            //SkillDef huntressSecondary1 = LoadoutUtilities.CreateSkillDef(new SerializableEntityStateType(typeof(ReinHuntressSkills.Skills.Secondary.HuntressSecondary1)), "Weapon", huntressSecondarySprite1);
            //SkillDef HuntressUtility1 = LoadoutUtilities.CreateSkillDef(new SerializableEntityStateType(typeof(ReinHuntressSkills.Skills.Utility.HuntressUtility1)), "Weapon", huntressUtilitySprite1);
            //SkillDef HuntressSpecial1 = LoadoutUtilities.CreateSkillDef(new SerializableEntityStateType(typeof(ReinHuntressSkills.Skills.Special.HuntressSpecial1)), "Weapon", huntressSpecialSprite1);

            //Modify any of the basic skill values as needed
            huntressPrimary1.baseRechargeInterval = 0f;
            huntressPrimary1.baseMaxStock = 1;
            huntressPrimary1.rechargeStock = 1;
            huntressPrimary1.isBullets = false;
            huntressPrimary1.shootDelay = 0.3f;
            huntressPrimary1.beginSkillCooldownOnSkillEnd = false;
            huntressPrimary1.requiredStock = 1;
            huntressPrimary1.stockToConsume = 1;
            huntressPrimary1.canceledFromSprinting = false;
            huntressPrimary1.noSprint = false;
            huntressPrimary1.isCombatSkill = true;
            huntressPrimary1.mustKeyPress = false;
            huntressPrimary1.fullRestockOnAssign = true;


            //Use helper func to create a viewable node. No clue what these really do yet but w/e
            ViewablesCatalog.Node huntressPrimary1Node = LoadoutUtilities.CreateViewableNode("ReinHuntressPrimary1");
            //ViewablesCatalog.Node huntressSecondaryNode = LoadoutUtilities.CreateViewableNode("ReinHuntressSecondary1");
            //ViewablesCatalog.Node huntressUtilityNode = LoadoutUtilities.CreateViewableNode("ReinHuntressUtility1");
            //ViewablesCatalog.Node huntressSpecialNode = LoadoutUtilities.CreateViewableNode("ReinHuntressSpecial1");

            //Use another helper func to create a variant we can add to the array of variants
            SkillFamily.Variant huntressPrimaryVariant1 = LoadoutUtilities.CreateSkillVariant(huntressPrimary1, huntressPrimary1Node);
            //SkillFamily.Variant huntressSecondaryVariant1 = LoadoutUtilities.CreateSkillVariant(huntressSecondary1, huntressSecondaryNode);
            //SkillFamily.Variant huntressUtilityVariant1 = LoadoutUtilities.CreateSkillVariant(HuntressUtility1, huntressUtilityNode);
            //SkillFamily.Variant huntressSpecialVariant1 = LoadoutUtilities.CreateSkillVariant(HuntressSpecial1, huntressSpecialNode);



            huntressPrimaryFamily.variants[huntressPrimaryFamily.variants.Length - 1] = huntressPrimaryVariant1;
            //huntressSecondaryFamily.variants[huntressSecondaryFamily.variants.Length - 1] = huntressSecondaryVariant1;
            //huntressUtilityFamily.variants[huntressUtilityFamily.variants.Length - 2] = huntressUtilityVariant1;
            //huntressSpecialFamily.variants[huntressSpecialFamily.variants.Length - 2] = huntressSpecialVariant1;

        }
    }
}
