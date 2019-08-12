using BepInEx;
using RoR2;
using UnityEngine;
using R2API.Utils;
using System.Runtime.CompilerServices;

namespace ReinSniperRework
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinSniperRework", "ReinSniperRework", "1.0.3")]

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

            AddHurtboxes();
        }

        
        private Transform FindChildPath(Transform parent, string s, char seperator = '/', [CallerFilePath] string file = null, [CallerMemberName] string name = null, [CallerLineNumber] int lineNumber = 0)
        {
            string pathString = s;
            if( !pathString.StartsWith(seperator.ToString() ) )
            {
                pathString = seperator + pathString;
            }
            string[] path = pathString.Split(seperator);

            Transform temp = parent;

            for (int i = 1; i < path.Length; i++)
            {
                temp = temp.Find(path[i]);
                if( !temp )
                {
                    Debug.Log("Null Transform:" + path[i] );
                    Debug.Log(file + " -- " + name + " Line: " + lineNumber);
                    return null;
                }
            }
            return temp;
        }

        private void AddHurtboxes()
        {
            //Bosses
            //Debug.Log("Dunestrider");
            AddDunestriderHurtbox();
            //Debug.Log("Overlord");
            AddFatImpHurtbox();
            //Debug.Log("BeetleQueen");
            AddBeetleQueenHurtbox();
            //Debug.Log("StoneTitan");
            AddTitanHurtbox();
            //Debug.Log("GoldTitan");
            AddGoldTitanHurtbox();
            //Debug.Log("Vagrant");
            AddVagrantHurtbox();
            //Debug.Log("GroveBoi");
            AddGroveBoiHurtbox();
            //Debug.Log("ZappySnok");
            AddZappySnokHurtbox();
            //Debug.Log("SpicySnok");
            AddSpicySnokHurtbox();
            //MiniBosses
            //Debug.Log("Brasshole");
            AddBrassholeHurtbox();
            //Debug.Log("Bison");
            AddBisonHurtbox();
            //Debug.Log("Templar");
            AddTemplarHurtbox();
            //Debug.Log("Golem");
            AddGolemHurtbox();
            //Debug.Log("Greater Wisp");
            //Debug.Log("Elder Lemurian");
            AddElderLemurianHurtbox();
            //Debug.Log("Archaic Wisp");
            //Small bois
            //Debug.Log("HermitCrab");
            //Debug.Log("Imp");
            AddImpHurtbox();
            //Debug.Log("Jellyfish");
            //Debug.Log("Urchin");
            //Debug.Log("Wisp");
            //Other
            //Debug.Log("Shopkeeper");
        }

        private void AddDunestriderHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("ClayBossBody");
            if( !targetObj )
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "ModelBase/mdlClayBoss/ClayBossArmature/ROOT/PotBase", '/');
            if ( !target )
            {
                Debug.Log("No Target");
                return;
            }

            HurtBoxGroup hbg = targetObj.GetComponent<HurtBoxGroup>();
            if( !hbg )
            {
                
                hbg = targetObj.GetComponentInChildren<HurtBoxGroup>();
            }
            if( !hbg )
            {
                Debug.Log("No hbg");
            }


            HurtBox sample = targetObj.GetComponent<HurtBox>();
            if( !sample )
            {
                
                sample = targetObj.GetComponentInChildren<HurtBox>();
            }
            if( !sample )
            {
                Debug.Log("No Sample hurtbox found");
            }


            Transform earL = target.Find("EarLight.l");
            Transform earR = target.Find("EarLight.r");

            if( earL )
            {
                GameObject g = earL.gameObject;
                g.layer = sample.gameObject.layer;
                SphereCollider col = g.AddComponent<SphereCollider>();
                col.radius = 0.3f;
                col.isTrigger = false;
                col.enabled = true;
                col.center = new Vector3(0f, 0f, 0f);
                //col.height = 1f;
                //col.direction = 2;
                HurtBox hb = g.AddComponent<HurtBox>();
                hb.healthComponent = sample.healthComponent;
                hb.isBullseye = false;
                hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
                ReinHurtBoxManager manager = g.AddComponent<ReinHurtBoxManager>();
                manager.managedHB = hb;
                manager.refHB = sample;
            }
            else
            {
                Debug.Log("No EarL");
            }
            if( earR )
            {
                GameObject g = earR.gameObject;
                g.layer = sample.gameObject.layer;
                SphereCollider col = g.AddComponent<SphereCollider>();
                col.radius = 0.25f;
                col.isTrigger = false;
                col.enabled = true;
                col.center = new Vector3(0f, 0f, 0f);
                //col.height = 1f;
                //col.direction = 2;
                HurtBox hb = g.AddComponent<HurtBox>();
                hb.healthComponent = sample.healthComponent;
                hb.isBullseye = false;
                hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
                ReinHurtBoxManager manager = g.AddComponent<ReinHurtBoxManager>();
                manager.managedHB = hb;
                manager.refHB = sample;
            }
            else
            {
                Debug.Log("No EarR");
            }

        }

        private void AddFatImpHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("ImpBossBody");
            if (!targetObj)
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "ModelBase/mdlImpBoss/ImpBossArmature/ROOT/base/stomach/chest/Eyeball.1/Eyeball1.Pupil/Eyeball1.Pupil_end", '/');
            if (!target)
            {
                Debug.Log("No Target");
                return;
            }

            HurtBoxGroup hbg = targetObj.GetComponent<HurtBoxGroup>();
            if (!hbg)
            {
                
                hbg = targetObj.GetComponentInChildren<HurtBoxGroup>();
            }
            if (!hbg)
            {
                Debug.Log("No hbg");
                return;
            }

            HurtBox sample = targetObj.GetComponent<HurtBox>();
            if (!sample)
            {
                
                sample = targetObj.GetComponentInChildren<HurtBox>();
            }
            if (!sample)
            {
                Debug.Log("No Sample hurtbox found");
                return;
            }

            GameObject g = target.gameObject;
            g.layer = sample.gameObject.layer;
            SphereCollider col = g.AddComponent<SphereCollider>();
            col.radius = 0.65f;
            col.isTrigger = false;
            col.enabled = true;
            col.center = new Vector3(0f, -0.50f, 0f);
            HurtBox hb = g.AddComponent<HurtBox>();
            hb.healthComponent = sample.healthComponent;
            hb.isBullseye = false;
            hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
            ReinHurtBoxManager manager = g.AddComponent<ReinHurtBoxManager>();
            manager.managedHB = hb;
            manager.refHB = sample;
        }

        private void AddBeetleQueenHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("BeetleQueen2Body");
            if( !targetObj )
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "ModelBase/mdlBeetleQueen/BeetleQueenArmature/ROOT/Base/Chest/Neck/Head", '/');
            if ( !target )
            {
                Debug.Log("No Target");
                return;
            }

            foreach( HurtBox hbb in target.gameObject.GetComponentsInChildren<HurtBox>() )
            {
                BoxCollider colb = hbb.gameObject.GetComponent<BoxCollider>();
                if( colb.transform.localScale.x > 5f )
                {
                    colb.size = new Vector3(1f, 1f, 0.8f);
                }
            }

            HurtBoxGroup hbg = targetObj.GetComponent<HurtBoxGroup>();
            if( !hbg )
            {
                
                hbg = targetObj.GetComponentInChildren<HurtBoxGroup>();
            }
            if( !hbg )
            {
                Debug.Log("No hbg");
            }

            HurtBox sample = targetObj.GetComponent<HurtBox>();
            if (!sample)
            {
                
                sample = targetObj.GetComponentInChildren<HurtBox>();
            }
            if (!sample)
            {
                Debug.Log("No Sample hurtbox found");
                return;
            }

            GameObject g = target.gameObject;
            g.layer = sample.gameObject.layer;
            CapsuleCollider col = g.AddComponent<CapsuleCollider>();
            col.radius = 0.5f;
            col.isTrigger = false;
            col.enabled = true;
            col.center = new Vector3(0f, -0.1f, 0f);
            col.height = 2f;
            col.direction = 0;
            HurtBox hb = g.AddComponent<HurtBox>();
            hb.healthComponent = sample.healthComponent;
            hb.isBullseye = false;
            hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
            ReinHurtBoxManager manager = g.AddComponent<ReinHurtBoxManager>();
            manager.managedHB = hb;
            manager.refHB = sample;

        }

        private void AddTitanHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("TitanBody");
            if (!targetObj)
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "ModelBase/mdlTitan/TitanArmature/ROOT/base/stomach/chest/Head/MuzzleLaser", '/');
            if (!target)
            {
                Debug.Log("No Target");
                return;
            }

            HurtBoxGroup hbg = targetObj.GetComponent<HurtBoxGroup>();
            if (!hbg)
            {
                
                hbg = targetObj.GetComponentInChildren<HurtBoxGroup>();
            }
            if (!hbg)
            {
                Debug.Log("No hbg");
            }

            HurtBox sample = targetObj.GetComponent<HurtBox>();
            if (!sample)
            {
                
                sample = targetObj.GetComponentInChildren<HurtBox>();
            }
            if (!sample)
            {
                Debug.Log("No Sample hurtbox found");
                return;
            }

            GameObject g = target.gameObject;
            g.layer = sample.gameObject.layer;
            SphereCollider col = g.AddComponent<SphereCollider>();
            col.radius = 0.75f;
            col.isTrigger = false;
            col.enabled = true;
            col.center = new Vector3(0f, -0.2f, -0.3f);
            //col.height = 2f;
            //col.direction = 0;
            HurtBox hb = g.AddComponent<HurtBox>();
            hb.healthComponent = sample.healthComponent;
            hb.isBullseye = false;
            hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
            ReinHurtBoxManager manager = g.AddComponent<ReinHurtBoxManager>();
            manager.managedHB = hb;
            manager.refHB = sample;
        }

        private void AddGoldTitanHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("TitanGoldBody");
            if (!targetObj)
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "ModelBase/mdlTitan/TitanArmature/ROOT/base/stomach/chest/Head/MuzzleLaser", '/');
            if (!target)
            {
                Debug.Log("No Target");
                return;
            }

            HurtBoxGroup hbg = targetObj.GetComponent<HurtBoxGroup>();
            if (!hbg)
            {
                
                hbg = targetObj.GetComponentInChildren<HurtBoxGroup>();
            }
            if (!hbg)
            {
                Debug.Log("No hbg");
            }

            HurtBox sample = targetObj.GetComponent<HurtBox>();
            if (!sample)
            {
                
                sample = targetObj.GetComponentInChildren<HurtBox>();
            }
            if (!sample)
            {
                Debug.Log("No Sample hurtbox found");
                return;
            }

            GameObject g = target.gameObject;
            g.layer = sample.gameObject.layer;
            SphereCollider col = g.AddComponent<SphereCollider>();
            col.radius = 0.75f;
            col.isTrigger = false;
            col.enabled = true;
            col.center = new Vector3(0f, -0.2f, -0.3f);
            //col.height = 2f;
            //col.direction = 0;
            HurtBox hb = g.AddComponent<HurtBox>();
            hb.healthComponent = sample.healthComponent;
            hb.isBullseye = false;
            hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
            ReinHurtBoxManager manager = g.AddComponent<ReinHurtBoxManager>();
            manager.managedHB = hb;
            manager.refHB = sample;
        }

        private void AddVagrantHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("VagrantBody");
            if (!targetObj)
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "Model Base/mdlVagrant/VagrantArmature/DetatchedHull/Hull.003/Hull.002", '/');
            if (!target)
            {
                Debug.Log("No Target");
                return;
            }

            HurtBoxGroup hbg = targetObj.GetComponent<HurtBoxGroup>();
            if (!hbg)
            {
                
                hbg = targetObj.GetComponentInChildren<HurtBoxGroup>();
            }
            if (!hbg)
            {
                Debug.Log("No hbg");
            }

            HurtBox sample = targetObj.GetComponent<HurtBox>();
            if (!sample)
            {
                
                sample = targetObj.GetComponentInChildren<HurtBox>();
            }
            if (!sample)
            {
                Debug.Log("No Sample hurtbox found");
                return;
            }

            GameObject g = target.gameObject;
            g.layer = sample.gameObject.layer;
            SphereCollider col = g.AddComponent<SphereCollider>();
            col.radius = 0.5f;
            col.isTrigger = false;
            col.enabled = true;
            col.center = new Vector3(0f, -0.9f, 0f);
            //col.height = 2f;
            //col.direction = 0;
            HurtBox hb = g.AddComponent<HurtBox>();
            hb.healthComponent = sample.healthComponent;
            hb.isBullseye = false;
            hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
            ReinHurtBoxManager manager = g.AddComponent<ReinHurtBoxManager>();
            manager.managedHB = hb;
            manager.refHB = sample;
        }

        private void AddGroveBoiHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("GravekeeperBody");
            if (!targetObj)
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "ModelBase/mdlGravekeeper/GravekeeperArmature/ROOT/base/stomach/chest/neck.1/neck.2/head/mask/mask_end", '/');
            if (!target)
            {
                Debug.Log("No Target");
                return;
            }

            HurtBoxGroup hbg = targetObj.GetComponent<HurtBoxGroup>();
            if (!hbg)
            {
                
                hbg = targetObj.GetComponentInChildren<HurtBoxGroup>();
            }
            if (!hbg)
            {
                Debug.Log("No hbg");
            }

            HurtBox sample = targetObj.GetComponent<HurtBox>();
            if (!sample)
            {
                
                sample = targetObj.GetComponentInChildren<HurtBox>();
            }
            if (!sample)
            {
                Debug.Log("No Sample hurtbox found");
                return;
            }

            GameObject g = target.gameObject;
            g.layer = sample.gameObject.layer;
            SphereCollider col = g.AddComponent<SphereCollider>();
            col.radius = 0.85f;
            col.isTrigger = false;
            col.enabled = true;
            col.center = new Vector3(0f, -0.5f, -0.21f);
            //col.height = 2f;
            //col.direction = 0;
            HurtBox hb = g.AddComponent<HurtBox>();
            hb.healthComponent = sample.healthComponent;
            hb.isBullseye = false;
            hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
            ReinHurtBoxManager manager = g.AddComponent<ReinHurtBoxManager>();
            manager.managedHB = hb;
            manager.refHB = sample;
        }

        private void AddZappySnokHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("ElectricWormBody");
            if (!targetObj)
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "ModelBase/mdlMagmaWorm/WormArmature/Head/MouthMuzzle", '/');
            if (!target)
            {
                Debug.Log("No Target");
                return;
            }

            HurtBoxGroup hbg = targetObj.GetComponent<HurtBoxGroup>();
            if (!hbg)
            {
                
                hbg = targetObj.GetComponentInChildren<HurtBoxGroup>();
            }
            if (!hbg)
            {
                Debug.Log("No hbg");
            }

            HurtBox sample = targetObj.GetComponent<HurtBox>();
            if (!sample)
            {
                
                sample = targetObj.GetComponentInChildren<HurtBox>();
            }
            if (!sample)
            {
                Debug.Log("No Sample hurtbox found");
                return;
            }

            GameObject g = target.gameObject;
            g.layer = sample.gameObject.layer;
            SphereCollider col = g.AddComponent<SphereCollider>();
            col.radius = 0.85f;
            col.isTrigger = false;
            col.enabled = true;
            col.center = new Vector3(0f, 0f, -1f);
            //col.height = 2f;
            //col.direction = 0;
            HurtBox hb = g.AddComponent<HurtBox>();
            hb.healthComponent = sample.healthComponent;
            hb.isBullseye = false;
            hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
            ReinHurtBoxManager manager = g.AddComponent<ReinHurtBoxManager>();
            manager.managedHB = hb;
            manager.refHB = sample;
        }

        private void AddSpicySnokHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("MagmaWormBody");
            if (!targetObj)
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "ModelBase/mdlMagmaWorm/WormArmature/Head/MouthMuzzle", '/');
            if (!target)
            {
                Debug.Log("No Target");
                return;
            }

            HurtBoxGroup hbg = targetObj.GetComponent<HurtBoxGroup>();
            if (!hbg)
            {
                
                hbg = targetObj.GetComponentInChildren<HurtBoxGroup>();
            }
            if (!hbg)
            {
                Debug.Log("No hbg");
            }

            HurtBox sample = targetObj.GetComponent<HurtBox>();
            if (!sample)
            {
                
                sample = targetObj.GetComponentInChildren<HurtBox>();
            }
            if (!sample)
            {
                Debug.Log("No Sample hurtbox found");
                return;
            }

            GameObject g = target.gameObject;
            g.layer = sample.gameObject.layer;
            SphereCollider col = g.AddComponent<SphereCollider>();
            col.radius = 0.85f;
            col.isTrigger = false;
            col.enabled = true;
            col.center = new Vector3(0f, 0f, -1f);
            //col.height = 2f;
            //col.direction = 0;
            HurtBox hb = g.AddComponent<HurtBox>();
            hb.healthComponent = sample.healthComponent;
            hb.isBullseye = false;
            hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
            ReinHurtBoxManager manager = g.AddComponent<ReinHurtBoxManager>();
            manager.managedHB = hb;
            manager.refHB = sample;
        }

        //Minibosses
        private void AddBrassholeHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("BellBody");
            if (!targetObj)
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "Model Base/mdlBell/BellArmature/ROOT/Base/Chain.1/Chain.2/Chain.3/Chain.4/Bell/Muzzle", '/');
            if (!target)
            {
                Debug.Log("No Target");
                return;
            }

            HurtBoxGroup hbg = targetObj.GetComponent<HurtBoxGroup>();
            if (!hbg)
            {
                
                hbg = targetObj.GetComponentInChildren<HurtBoxGroup>();
            }
            if (!hbg)
            {
                Debug.Log("No hbg");
                return;
            }

            HurtBox sample = targetObj.GetComponent<HurtBox>();
            if (!sample)
            {
                
                sample = targetObj.GetComponentInChildren<HurtBox>();
            }
            if (!sample)
            {
                Debug.Log("No Sample hurtbox found");
                return;
            }

            GameObject g = target.gameObject;
            g.layer = sample.gameObject.layer;
            CapsuleCollider col = g.AddComponent<CapsuleCollider>();
            col.radius = 0.5f;
            col.isTrigger = false;
            col.enabled = true;
            col.center = new Vector3(0f, 0.30f, 0f);
            col.height = 3f;
            col.direction = 2;
            HurtBox hb = g.AddComponent<HurtBox>();
            hb.healthComponent = sample.healthComponent;
            hb.isBullseye = false;
            hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
            ReinHurtBoxManager manager = g.AddComponent<ReinHurtBoxManager>();
            manager.managedHB = hb;
            manager.refHB = sample;
        }

        private void AddBisonHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("BisonBody");
            if (!targetObj)
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "ModelBase/mdlBison/BisonArmature/ROOT/Base/stomach/chest/neck/head/head_end", '/');
            if (!target)
            {
                Debug.Log("No Target");
                return;
            }

            HurtBoxGroup hbg = targetObj.GetComponent<HurtBoxGroup>();
            if (!hbg)
            {
                
                hbg = targetObj.GetComponentInChildren<HurtBoxGroup>();
            }
            if (!hbg)
            {
                Debug.Log("No hbg");
                return;
            }

            HurtBox sample = targetObj.GetComponent<HurtBox>();
            if (!sample)
            {
                
                sample = targetObj.GetComponentInChildren<HurtBox>();
            }
            if (!sample)
            {
                Debug.Log("No Sample hurtbox found");
                return;
            }

            GameObject g = target.gameObject;
            g.layer = sample.gameObject.layer;
            CapsuleCollider col = g.AddComponent<CapsuleCollider>();
            col.radius = 0.25f;
            col.isTrigger = false;
            col.enabled = true;
            col.center = new Vector3(0f, 0f, 0.02f);
            col.height = 1.2f;
            col.direction = 2;
            HurtBox hb = g.AddComponent<HurtBox>();
            hb.healthComponent = sample.healthComponent;
            hb.isBullseye = false;
            hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
            ReinHurtBoxManager manager = g.AddComponent<ReinHurtBoxManager>();
            manager.managedHB = hb;
            manager.refHB = sample;
        }

        private void AddTemplarHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("ClayBruiserBody");
            if (!targetObj)
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "ModelBase/mdlClayBruiser/ClayBruiserArmature/ROOT/base", '/');
            if (!target)
            {
                Debug.Log("No Target");
                return;
            }

            HurtBoxGroup hbg = targetObj.GetComponent<HurtBoxGroup>();
            if (!hbg)
            {
                
                hbg = targetObj.GetComponentInChildren<HurtBoxGroup>();
            }
            if (!hbg)
            {
                Debug.Log("No hbg");
                return;
            }

            HurtBox sample = targetObj.GetComponent<HurtBox>();
            if (!sample)
            {
                
                sample = targetObj.GetComponentInChildren<HurtBox>();
            }
            if (!sample)
            {
                Debug.Log("No Sample hurtbox found");
                return;
            }

            GameObject g = target.gameObject;
            g.layer = sample.gameObject.layer;
            SphereCollider col = g.AddComponent<SphereCollider>();
            col.radius = 0.75f;
            col.isTrigger = false;
            col.enabled = true;
            col.center = new Vector3(0.15f, 0.1f, -0.8f);
            //col.height = 1f;
            //col.direction = 2;
            HurtBox hb = g.AddComponent<HurtBox>();
            hb.healthComponent = sample.healthComponent;
            hb.isBullseye = false;
            hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
            ReinHurtBoxManager manager = g.AddComponent<ReinHurtBoxManager>();
            manager.managedHB = hb;
            manager.refHB = sample;
        }

        private void AddGolemHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("GolemBody");
            if (!targetObj)
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "ModelBase/mdlGolem/GolemArmature/ROOT/base/stomach/chest/head/MuzzleLaser", '/');
            if (!target)
            {
                Debug.Log("No Target");
                return;
            }

            HurtBoxGroup hbg = targetObj.GetComponent<HurtBoxGroup>();
            if (!hbg)
            {
                
                hbg = targetObj.GetComponentInChildren<HurtBoxGroup>();
            }
            if (!hbg)
            {
                Debug.Log("No hbg");
                return;
            }

            HurtBox sample = targetObj.GetComponent<HurtBox>();
            if (!sample)
            {
                
                sample = targetObj.GetComponentInChildren<HurtBox>();
            }
            if (!sample)
            {
                Debug.Log("No Sample hurtbox found");
                return;
            }

            GameObject g = target.gameObject;
            g.layer = sample.gameObject.layer;
            SphereCollider col = g.AddComponent<SphereCollider>();
            col.radius = 0.35f;
            col.isTrigger = false;
            col.enabled = true;
            col.center = new Vector3(0f, 0f, 0f);
            //col.height = 1f;
            //col.direction = 2;
            HurtBox hb = g.AddComponent<HurtBox>();
            hb.healthComponent = sample.healthComponent;
            hb.isBullseye = false;
            hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
            ReinHurtBoxManager manager = g.AddComponent<ReinHurtBoxManager>();
            manager.managedHB = hb;
            manager.refHB = sample;
        }

        private void AddElderLemurianHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("LemurianBruiserBody");
            if (!targetObj)
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "ModelBase/mdlLemurianBruiser/LemurianBruiserArmature/ROOT/base/stomach/chest/neck.1/neck.2/HURTBOX, Lemmy Bruiser", '/');
            if (!target)
            {
                Debug.Log("No Target");
                return;
            }

            target.GetComponent<HurtBox>().damageModifier = HurtBox.DamageModifier.SniperTarget;
        }

        //Basic monsters
        private void AddImpHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("ImpBody");
            if (!targetObj)
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "ModelBase/mdlImp/ImpArmature/ROOT/base/stomach/chest/Eyeball/Eyeball_end", '/');
            if (!target)
            {
                Debug.Log("No Target");
                return;
            }

            HurtBoxGroup hbg = targetObj.GetComponent<HurtBoxGroup>();
            if (!hbg)
            {
                
                hbg = targetObj.GetComponentInChildren<HurtBoxGroup>();
            }
            if (!hbg)
            {
                Debug.Log("No hbg");
                return;
            }

            HurtBox sample = targetObj.GetComponent<HurtBox>();
            if (!sample)
            {
                
                sample = targetObj.GetComponentInChildren<HurtBox>();
            }
            if (!sample)
            {
                Debug.Log("No Sample hurtbox found");
                return;
            }

            GameObject g = target.gameObject;
            g.layer = sample.gameObject.layer;
            SphereCollider col = g.AddComponent<SphereCollider>();
            col.radius = 0.2f;
            col.isTrigger = false;
            col.enabled = true;
            col.center = new Vector3(0f, 0f, 0f);
            //col.height = 1f;
            //col.direction = 2;
            HurtBox hb = g.AddComponent<HurtBox>();
            hb.healthComponent = sample.healthComponent;
            hb.isBullseye = false;
            hb.damageModifier = HurtBox.DamageModifier.SniperTarget;
            ReinHurtBoxManager manager = g.AddComponent<ReinHurtBoxManager>();
            manager.managedHB = hb;
            manager.refHB = sample;
        }
        
    }
}


