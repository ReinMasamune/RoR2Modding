using BepInEx;
using RoR2;
using UnityEngine;
using R2API.Utils;


namespace ReinAcridMod
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinAcridMod", "ReinAcrid", "0.0.2")]

    public class ReinSurvivorMod : BaseUnityPlugin
    {
        public void Start()
        {
            GameObject body = BodyCatalog.FindBodyPrefab("BeetleGuardAllyBody");

            ReinDataLibrary data = body.AddComponent<ReinDataLibrary>();
            //data.g_ui = body.AddComponent<AcridUIController>();
            //data.g_ui.data = data;

            CharacterBody charBody = body.GetComponent<CharacterBody>();
            charBody.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            charBody.autoCalculateLevelStats = false;
            charBody.baseMaxHealth = data.g_baseMaxHealth;
            charBody.levelMaxHealth = data.g_lvlMaxHealth;
            charBody.baseRegen = data.g_baseRegen;
            charBody.levelRegen = data.g_lvlRegen;
            charBody.baseMoveSpeed = data.g_baseMoveSpeed;
            charBody.levelMoveSpeed = data.g_lvlMoveSpeed;
            charBody.baseJumpPower = data.g_baseJumpPower;
            charBody.levelJumpPower = data.g_lvlJumpPower;
            charBody.baseDamage = data.g_baseDamage;
            charBody.levelDamage = data.g_lvlDamage;
            charBody.baseAttackSpeed = data.g_baseAttackSpeed;
            charBody.levelAttackSpeed = data.g_lvlAttackSpeed;
            charBody.baseCrit = data.g_baseCrit;
            charBody.levelCrit = data.g_lvlCrit;
            charBody.baseArmor = data.g_baseArmor;
            charBody.levelArmor = data.g_lvlArmor;



            SkillLocator SL = body.GetComponent<SkillLocator>();

            GenericSkill acridPrimary = SL.primary;
            GenericSkill acridSecondary = SL.secondary;
            GenericSkill acridUtility = SL.utility;
            GenericSkill acridSpecial = body.AddComponent<GenericSkill>();
            SL.special = acridSpecial;

            var survivor = new SurvivorDef
            {
                bodyPrefab = body,
                descriptionToken = "",
                displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/PaladinDisplay"),
                primaryColor = new Color(0.25f, 0.25f, 0.25f),
                unlockableName = "",
                survivorIndex = SurvivorIndex.Count
            };
            R2API.SurvivorAPI.AddSurvivorOnReady(survivor);
        }
    }
}


/*
Major issues to fix:
    Flinching from damage
   
Tasks to complete:
    Skills



*/