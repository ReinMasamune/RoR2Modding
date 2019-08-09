using BepInEx;
using RoR2;
using UnityEngine;
using R2API.Utils;

namespace ReinSniperRework
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinSniperRework", "ReinSniperRework", "1.0.2")]

    public class ReinSniperReworkMain : BaseUnityPlugin
    {
        public void Start()
        {
            GameObject body = BodyCatalog.FindBodyPrefab("SniperBody");

            ReinDataLibrary data = body.AddComponent<ReinDataLibrary>();
            data.g_ui = body.AddComponent<SniperUIController>();
            data.g_ui.data = data;

            SkillLocator SL = body.GetComponent<SkillLocator>();
            CharacterBody charbody = body.GetComponent<CharacterBody>();

            GenericSkill Sniper1 = SL.primary;
            GenericSkill Sniper2 = SL.secondary;
            GenericSkill Sniper3 = SL.utility;
            GenericSkill Sniper4 = SL.special;

            charbody.baseDamage = data.g_baseDamage;
            charbody.baseMaxHealth = data.g_baseHealth;
            charbody.baseRegen = data.g_baseRegen;
            charbody.crosshairPrefab = Resources.Load<GameObject>(data.g_crosshairString);
            charbody.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;

            //Config skill1
            Sniper1.baseRechargeInterval = data.p_rechargeInterval;
            Sniper1.baseMaxStock = data.p_baseMaxStock;
            Sniper1.rechargeStock = data.p_rechargeStock;
            Sniper1.isBullets = data.p_isBullets;
            Sniper1.shootDelay = data.p_shootDelay;
            Sniper1.beginSkillCooldownOnSkillEnd = data.p_beginCDOnEnd;
            Sniper1.isCombatSkill = data.p_isCombatSkill;
            Sniper1.noSprint = data.p_noSprint;
            Sniper1.mustKeyPress = data.p_mustKeyPress;
            Sniper1.requiredStock = data.p_requiredStock;
            Sniper1.stockToConsume = data.p_stockToConsome;
            Sniper1.SetFieldValue("activationState", new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperPrimary)));


            Sniper2.baseRechargeInterval = data.s_rechargeInterval;
            Sniper2.baseMaxStock = data.s_baseMaxStock;
            Sniper2.rechargeStock = data.s_rechargeStock;
            Sniper2.isBullets = data.s_isBullets;
            Sniper2.shootDelay = data.s_shootDelay;
            Sniper2.beginSkillCooldownOnSkillEnd = data.s_beginCDOnEnd;
            Sniper2.isCombatSkill = data.s_isCombatSkill;
            Sniper2.noSprint = data.s_noSprint;
            Sniper2.mustKeyPress = data.s_mustKeyPress;
            Sniper2.requiredStock = data.s_requiredStock;
            Sniper2.stockToConsume = data.s_stockToConsume;
            Sniper2.SetFieldValue("activationState", new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperSecondary)));


            Sniper3.baseRechargeInterval = data.u_rechargeInterval;
            Sniper3.baseMaxStock = data.u_baseMaxStock;
            Sniper3.rechargeStock = data.u_rechargeStock;
            Sniper3.isBullets = data.u_isBullets;
            Sniper3.shootDelay = data.u_shootDelay;
            Sniper3.beginSkillCooldownOnSkillEnd = data.u_beginCDOnEnd;
            Sniper3.isCombatSkill = data.u_isCombatSkill;
            Sniper3.noSprint = data.u_noSprint;
            Sniper3.mustKeyPress = data.u_mustKeyPress;
            Sniper3.requiredStock = data.u_requiredStock;
            Sniper3.stockToConsume = data.u_stockToConsume;
            Sniper3.SetFieldValue("activationState", new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperUtility)));


            Sniper4.baseRechargeInterval = data.r_rechargeInterval;
            Sniper4.baseMaxStock = data.r_baseMaxStock;
            Sniper4.rechargeStock = data.r_rechargeStock;
            Sniper4.isBullets = data.r_isBullets;
            Sniper4.shootDelay = data.r_shootDelay;
            Sniper4.beginSkillCooldownOnSkillEnd = data.r_beginCDOnEnd;
            Sniper4.isCombatSkill = data.r_isCombatSkill;
            Sniper4.noSprint = data.r_noSprint;
            Sniper4.mustKeyPress = data.r_mustKeyPress;
            Sniper4.requiredStock = data.r_requiredStock;
            Sniper4.stockToConsume = data.r_stocktoConsume;
            Sniper4.SetFieldValue("activationState", new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperSpecial)));


            Sniper1.skillNameToken = "Snipe";
            Sniper2.skillNameToken = "Steady Aim";
            Sniper3.skillNameToken = "Military Training";
            Sniper4.skillNameToken = "Snare trap";

            Sniper1.skillDescriptionToken = "Shoot enemies for 250% damage. Must reload after shooting, good timing increases damage but bad timing decreases damage. Pushes you back slightly when fired.";
            Sniper2.skillDescriptionToken = "Aim with the scope of your rifle, reducing speed but powering up Snipe to pierce enemies and increase damage with charge." + "\n" + "When fully charged fire a powerful shot with a cooldown." + "\n" + "Scope and normal charged shots can be used while on cooldown.";
            Sniper3.skillDescriptionToken = "Launch yourself a forward along the direction you are currently moving. Gain very short duration invisibility that causes enemies to lose track of you momentarily";
            Sniper4.skillDescriptionToken = "Place a trap. When activated the trap pulls nearby enemies in, slows them, and reduces their armor.";

            SL.passiveSkill.enabled = true;
            SL.passiveSkill.skillNameToken = "Headshot";
            SL.passiveSkill.skillDescriptionToken = "Some enemies can be shot in the head for 50% bonus damage. [Currently only works on Lemurians, Beetles, and Beetle Guards, more coming hopefully soon]";


            var survivor = new SurvivorDef
            {
                bodyPrefab = body,
                descriptionToken = "Sniper is a high single-target damage survivor with moderate mobility." + "\n\n\n" +
                "Snipe is your only source of damage, hitting perfect reloads is essential to keeping a fast pace. " + "\n" + "The recoil can be used to extend time in the air." + "\n" + "Attack speed can make it more difficult to time reloads, but never makes it impossible." + "\n\n\n" +
                "Steady Aim is a high-risk high-reward option that takes a very long time to charge, but does incredible damage." + "\n\n\n" +
                "Military Training is essential to staying alive. When it is on cooldown you are at your most vulnerable." + "\n\n\n" +
                "Snare Trap deals no damage, but can be used for a variety of purposes: Weakening high priority targets, grouping up enemies for Steady Aim, or simply to slow enemies that are chasing you.",
                displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/SniperDisplay"),
                primaryColor = new Color(0.25f, 0.25f, 0.25f),
                unlockableName = "",
                survivorIndex = SurvivorIndex.Count
            };
            R2API.SurvivorAPI.AddSurvivorOnReady(survivor);

            EditHurtBoxes();
        }

        private void EditHurtBoxes()
        {
            EditElderLemurian();
            EditDunestrider();
            EditTemplar();
            EditBrassHole();
            EditGolem();
            //EditTitan();
            //EditGoldie();
            EditFatImp();
            //EditBeetleQueen();
        }

        private void EditElderLemurian()
        {
            GameObject target = BodyCatalog.FindBodyPrefab("LemurianBruiserBody");
            if( target )
            {
                Debug.Log("Searching for Elder Lemurian box");
                foreach (HurtBox hb in target.GetComponentsInChildren<HurtBox>())
                {
                    //Debug.Log(hb.name);
                    //Debug.Log(hb.gameObject.transform.parent.gameObject.name);
                    if (hb.gameObject.transform.parent.gameObject.name == "head")
                    {
                        Debug.Log("Head hitbox found");
                        hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
                    }
                }
            }
            else
            {
                Debug.Log("Elder Lemurians don't exist. I guess you win?");
            }
        }

        private void EditDunestrider()
        {
            GameObject target = BodyCatalog.FindBodyPrefab("ClayBossBody");
            if( target )
            {
                Debug.Log("Searching for Dunestrider lights");
                HurtBox sampleHB = target.GetComponentInChildren<HurtBox>();
                HurtBoxGroup boxes = target.GetComponentInChildren<HurtBoxGroup>();
                int startLength = boxes.hurtBoxes.Length;
                HurtBox[] hboxes = new HurtBox[startLength + 2];
                
                for( int i = 0; i < startLength; i++ )
                {
                    hboxes[i] = boxes.hurtBoxes[i];
                }

                foreach( Light l in target.GetComponentsInChildren<Light>() )
                {
                    if( l.gameObject.name == "EarLight.r" )
                    {
                        Debug.Log("Right ear light found");
                        l.gameObject.layer = sampleHB.gameObject.layer;
                        SphereCollider col = l.gameObject.GetComponent<SphereCollider>();
                        if( !col )
                        {
                            col = l.gameObject.AddComponent<SphereCollider>();
                        }
                        col.radius = 0.75f;
                        col.enabled = true;
                        col.isTrigger = false;
                        HurtBox hb = l.gameObject.GetComponent<HurtBox>();
                        if( !hb )
                        {
                            hb = l.gameObject.AddComponent<HurtBox>();
                        }
                        hb.healthComponent = sampleHB.healthComponent;
                        hb.isBullseye = false;
                        hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
                        hb.teamIndex = sampleHB.teamIndex;
                        hboxes[startLength] = hb;
                    }
                    if (l.gameObject.name == "EarLight.l")
                    {
                        Debug.Log("Left ear light found");
                        l.gameObject.layer = sampleHB.gameObject.layer;
                        SphereCollider col = l.gameObject.GetComponent<SphereCollider>();
                        if (!col)
                        {
                            col = l.gameObject.AddComponent<SphereCollider>();
                        }
                        col.radius = 0.75f;
                        col.enabled = true;
                        col.isTrigger = false;
                        HurtBox hb = l.gameObject.GetComponent<HurtBox>();
                        if (!hb)
                        {
                            hb = l.gameObject.AddComponent<HurtBox>();
                        }
                        hb.healthComponent = sampleHB.healthComponent;
                        hb.isBullseye = false;
                        hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
                        hb.teamIndex = sampleHB.teamIndex;
                        hboxes[startLength + 1] = hb;
                    }
                }
                boxes.hurtBoxes = hboxes;
            }
        }

        private void EditTemplar()
        {
            GameObject target = BodyCatalog.FindBodyPrefab("ClayBruiserBody");
            if (target)
            {
                Debug.Log("Searching for Templar head");
                HurtBox sampleHB = target.GetComponentInChildren<HurtBox>();
                HurtBoxGroup boxes = target.GetComponentInChildren<HurtBoxGroup>();
                int startLength = boxes.hurtBoxes.Length;
                HurtBox[] hboxes = new HurtBox[startLength + 1];
                CapsuleCollider col1 = sampleHB.gameObject.GetComponent<CapsuleCollider>();
                col1.radius = 2.8f;
                col1.center = new Vector3(0f, -0.25f, 0f);

                for (int i = 0; i < startLength; i++)
                {
                    hboxes[i] = boxes.hurtBoxes[i];
                }

                foreach (SkinnedMeshRenderer smr in target.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    if (smr.gameObject.name == "ClayBruiserHeadMesh")
                    {
                        Debug.Log("Head mesh found");
                        smr.gameObject.layer = sampleHB.gameObject.layer;
                        SphereCollider col = smr.gameObject.GetComponent<SphereCollider>();
                        if (!col)
                        {
                            col = smr.gameObject.AddComponent<SphereCollider>();
                        }
                        col.radius = 0.4f;
                        col.center = new Vector3(0f, 0.1f, 0.3f);
                        col.enabled = true;
                        col.isTrigger = false;
                        HurtBox hb = smr.gameObject.GetComponent<HurtBox>();
                        if (!hb)
                        {
                            hb = smr.gameObject.AddComponent<HurtBox>();
                        }
                        hb.healthComponent = sampleHB.healthComponent;
                        hb.isBullseye = false;
                        hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
                        hb.teamIndex = sampleHB.teamIndex;
                        hboxes[startLength] = hb;
                    }
                }
                boxes.hurtBoxes = hboxes;
            }
        }

        private void EditBrassHole()
        {
            GameObject target = BodyCatalog.FindBodyPrefab("BellBody");
            if (target)
            {
                Debug.Log("Searching for BrassHole box");
                foreach (HurtBox hb in target.GetComponentsInChildren<HurtBox>())
                {
                    //Debug.Log(hb.name);
                    //Debug.Log(hb.gameObject.transform.parent.gameObject.name);
                    if (hb.gameObject.transform.parent.gameObject.name == "base")
                    {
                        Debug.Log("Bell hitbox found");
                        hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
                        SphereCollider col = hb.gameObject.GetComponent<SphereCollider>();
                        col.radius = 0.5f;
                        col.center = new Vector3(0f, 0.5f, 0f);
                    }
                }
            }
            else
            {
                Debug.Log("Elder Lemurians don't exist. I guess you win?");
            }
        }

        private void EditGolem()
        {
            GameObject target = BodyCatalog.FindBodyPrefab("GolemBody");
            if (target)
            {
                Debug.Log("Searching for Golem lights");
                HurtBox sampleHB = target.GetComponentInChildren<HurtBox>();
                HurtBoxGroup boxes = target.GetComponentInChildren<HurtBoxGroup>();
                int startLength = boxes.hurtBoxes.Length;
                HurtBox[] hboxes = new HurtBox[startLength + 1];

                for (int i = 0; i < startLength; i++)
                {
                    hboxes[i] = boxes.hurtBoxes[i];
                }

                foreach (Light l in target.GetComponentsInChildren<Light>())
                {
                    if (l.gameObject.name == "Eye")
                    {
                        Debug.Log("Eye Light found");
                        l.gameObject.layer = sampleHB.gameObject.layer;
                        SphereCollider col = l.gameObject.GetComponent<SphereCollider>();
                        if (!col)
                        {
                            col = l.gameObject.AddComponent<SphereCollider>();
                        }
                        col.radius = 0.25f;
                        col.enabled = true;
                        col.isTrigger = false;
                        HurtBox hb = l.gameObject.GetComponent<HurtBox>();
                        if (!hb)
                        {
                            hb = l.gameObject.AddComponent<HurtBox>();
                        }
                        hb.healthComponent = sampleHB.healthComponent;
                        hb.isBullseye = false;
                        hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
                        hb.teamIndex = sampleHB.teamIndex;
                        hboxes[startLength] = hb;
                    }
                }
                boxes.hurtBoxes = hboxes;
            }
        }

        private void EditTitan()
        {
            GameObject target = BodyCatalog.FindBodyPrefab("TitanBody");
            if (target)
            {
                Debug.Log("Searching for Titan lights");
                HurtBox sampleHB = target.GetComponentInChildren<HurtBox>();
                HurtBoxGroup boxes = target.GetComponentInChildren<HurtBoxGroup>();
                int startLength = boxes.hurtBoxes.Length;
                HurtBox[] hboxes = new HurtBox[startLength + 1];

                for (int i = 0; i < startLength; i++)
                {
                    hboxes[i] = boxes.hurtBoxes[i];
                }

                foreach (Light l in target.GetComponentsInChildren<Light>())
                {
                    if (l.gameObject.name == "Point Light")
                    {
                        Debug.Log("Eye Light found");
                        l.gameObject.layer = sampleHB.gameObject.layer;
                        SphereCollider col = l.gameObject.GetComponent<SphereCollider>();
                        if (!col)
                        {
                            col = l.gameObject.AddComponent<SphereCollider>();
                        }
                        col.radius = 0.35f;
                        col.enabled = true;
                        col.isTrigger = false;
                        HurtBox hb = l.gameObject.GetComponent<HurtBox>();
                        if (!hb)
                        {
                            hb = l.gameObject.AddComponent<HurtBox>();
                        }
                        hb.healthComponent = sampleHB.healthComponent;
                        hb.isBullseye = false;
                        hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
                        hb.teamIndex = sampleHB.teamIndex;
                        hboxes[startLength] = hb;
                    }
                }
                boxes.hurtBoxes = hboxes;
            }
        }

        private void EditGoldie()
        {
            GameObject target = BodyCatalog.FindBodyPrefab("TitanGoldBody");
            if (target)
            {
                Debug.Log("Searching for Goldie lights");
                HurtBox sampleHB = target.GetComponentInChildren<HurtBox>();
                HurtBoxGroup boxes = target.GetComponentInChildren<HurtBoxGroup>();
                int startLength = boxes.hurtBoxes.Length;
                HurtBox[] hboxes = new HurtBox[startLength + 1];

                for (int i = 0; i < startLength; i++)
                {
                    hboxes[i] = boxes.hurtBoxes[i];
                }

                foreach (Light l in target.GetComponentsInChildren<Light>())
                {
                    if (l.gameObject.name == "Point Light")
                    {
                        Debug.Log("Eye Light found");
                        l.gameObject.layer = sampleHB.gameObject.layer;
                        SphereCollider col = l.gameObject.GetComponent<SphereCollider>();
                        if (!col)
                        {
                            col = l.gameObject.AddComponent<SphereCollider>();
                        }
                        col.radius = 0.35f;
                        col.enabled = true;
                        col.isTrigger = false;
                        HurtBox hb = l.gameObject.GetComponent<HurtBox>();
                        if (!hb)
                        {
                            hb = l.gameObject.AddComponent<HurtBox>();
                        }
                        hb.healthComponent = sampleHB.healthComponent;
                        hb.isBullseye = false;
                        hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
                        hb.teamIndex = sampleHB.teamIndex;
                        hboxes[startLength] = hb;
                    }
                }
                boxes.hurtBoxes = hboxes;
            }
        }

        private void EditFatImp()
        {
            GameObject target = BodyCatalog.FindBodyPrefab("ImpBossBody");
            if (target)
            {
                CharacterBody body = target.GetComponent<CharacterBody>();
                //if( !body )
                //{
                //    Debug.Log("Body Missing");
                //}

                foreach (HurtBox hb in target.GetComponentsInChildren<HurtBox>())
                {
                    Debug.Log(hb.gameObject.name);
                    Debug.Log(hb.gameObject.transform.parent.gameObject.name);
                    if (hb.gameObject.transform.parent.gameObject.name == "chest")
                    {
                        GameObject par = hb.gameObject.transform.parent.Find("Eyeball.1").gameObject;
                        if (par)
                        {
                            Debug.Log("Step 3/3");
                            par.layer = hb.gameObject.layer;
                            SphereCollider col = par.GetComponent<SphereCollider>();
                            if (!col)
                            { 
                                col = par.AddComponent<SphereCollider>();
                            }
                            col.radius = 0.8f;
                            col.center = new Vector3(0f, 0.1f, 0f);
                            col.enabled = true;
                            col.isTrigger = false;

                            HurtBox newBox = par.GetComponent<HurtBox>();
                            if (!newBox)
                            {
                                Debug.Log("Added hurtbox");
                                newBox = par.AddComponent<HurtBox>();
                            }
                            newBox.healthComponent = hb.healthComponent;
                            newBox.isBullseye = false;
                            newBox.damageModifier = HurtBox.DamageModifier.SniperTarget;
                        }
                    }

                }
            }
        }

        private void EditBeetleQueen()
        {
            GameObject target = BodyCatalog.FindBodyPrefab("BeetleQueen2Body");
            if (target)
            {
                Debug.Log("Searching for Beetle Queen Eye lights");
                HurtBox sampleHB = target.GetComponentInChildren<HurtBox>();
                HurtBoxGroup boxes = target.GetComponentInChildren<HurtBoxGroup>();
                int startLength = boxes.hurtBoxes.Length;
                HurtBox[] hboxes = new HurtBox[startLength + 2];

                for (int i = 0; i < startLength; i++)
                {
                    hboxes[i] = boxes.hurtBoxes[i];
                }

                foreach (Light l in target.GetComponentsInChildren<Light>())
                {
                    int i = 0;
                    if (l.gameObject.name == "Point Light")
                    {
                        Debug.Log("Eye Light found");
                        l.gameObject.layer = sampleHB.gameObject.layer;
                        SphereCollider col = l.gameObject.GetComponent<SphereCollider>();
                        if (!col)
                        {
                            col = l.gameObject.AddComponent<SphereCollider>();
                        }
                        col.radius = 0.5f;
                        col.enabled = true;
                        col.isTrigger = false;
                        HurtBox hb = l.gameObject.GetComponent<HurtBox>();
                        if (!hb)
                        {
                            hb = l.gameObject.AddComponent<HurtBox>();
                        }
                        hb.healthComponent = sampleHB.healthComponent;
                        hb.isBullseye = false;
                        hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
                        hb.teamIndex = sampleHB.teamIndex;
                        hboxes[startLength + i] = hb;
                        i++;
                    }
                }
                boxes.hurtBoxes = hboxes;
            }
        }
    }
}


