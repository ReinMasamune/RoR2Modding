using BepInEx;
using RoR2;
using UnityEngine;
using R2API.Utils;
using System.Runtime.CompilerServices;
using System.Reflection;
using EntityStates;
using RoR2.Skills;

namespace ReinSniperRework
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinSniperRework", "ReinSniperRework", "1.0.3")]

    public class ReinSniperReworkMain : BaseUnityPlugin
    {
        public void Start()
        {
            var execAssembly = Assembly.GetExecutingAssembly();
            var stream = execAssembly.GetManifestResourceStream("ReinSniperRework.sniperassetbundle");
            var sniperBundle = AssetBundle.LoadFromStream(stream);
            
            GameObject body = BodyCatalog.FindBodyPrefab("SniperBody");

            ReinDataLibrary data = body.AddComponent<ReinDataLibrary>();
            data.g_ui = body.AddComponent<SniperUIController>();
            data.g_ui.data = data;
            data.bundle = sniperBundle;

            SkillLocator SL = body.GetComponent<SkillLocator>();
            CharacterBody charbody = body.GetComponent<CharacterBody>();
            SetStateOnHurt hurtState = body.AddComponent<SetStateOnHurt>();

            hurtState.canBeFrozen = true;
            hurtState.canBeHitStunned = false;
            hurtState.canBeStunned = false;
            hurtState.hitThreshold = 5f;

            hurtState.hurtState = new SerializableEntityStateType(EntityState.Instantiate(219).GetType());

            int i = 0;
            EntityStateMachine[] esmr = new EntityStateMachine[2]; 
            foreach( EntityStateMachine esm in body.GetComponentsInChildren<EntityStateMachine>())
            {
                switch (esm.customName)
                {
                    case "Body":
                        hurtState.targetStateMachine = esm;
                        break;
                    default:
                        if (i < 2)
                        {
                            esmr[i] = esm;
                            Debug.Log(esm.customName);
                        }
                        i++;
                        Debug.Log(i);
                        break;
                }
            }

            hurtState.idleStateMachine = esmr;

            GenericSkill Sniper1G = SL.primary;
            GenericSkill Sniper2G = SL.secondary;
            GenericSkill Sniper3G = SL.utility;
            GenericSkill Sniper4G = SL.special;

            SkillFamily SniperFam1 = Sniper1G.skillFamily;
            SkillFamily SniperFam2 = Sniper2G.skillFamily;
            SkillFamily SniperFam3 = Sniper3G.skillFamily;
            SkillFamily SniperFam4 = Sniper4G.skillFamily;

            SkillDef Sniper1 = SniperFam1.variants[SniperFam1.defaultVariantIndex].skillDef;
            SkillDef Sniper2 = SniperFam2.variants[SniperFam2.defaultVariantIndex].skillDef;
            SkillDef Sniper3 = SniperFam3.variants[SniperFam3.defaultVariantIndex].skillDef;
            SkillDef Sniper4 = SniperFam4.variants[SniperFam4.defaultVariantIndex].skillDef;

            charbody.baseDamage = data.g_baseDamage;
            charbody.baseMaxHealth = data.g_baseHealth;
            charbody.baseRegen = data.g_baseRegen;
            charbody.crosshairPrefab = Resources.Load<GameObject>(data.g_crosshairString);
            charbody.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;

            charbody.baseNameToken = "Sniper";

            //Config skills
            Sniper1.SetFieldValue("baseRechargeInterval",  data.p_rechargeInterval);
            Sniper1.SetFieldValue("baseMaxStock" , data.p_baseMaxStock);
            Sniper1.SetFieldValue("rechargeStock" , data.p_rechargeStock);
            Sniper1.SetFieldValue("isBullets" , data.p_isBullets);
            Sniper1.SetFieldValue("shootDelay" , data.p_shootDelay);
            Sniper1.SetFieldValue("beginSkillCooldownOnSkillEnd" , data.p_beginCDOnEnd);
            Sniper1.SetFieldValue("isCombatSkill" , data.p_isCombatSkill);
            Sniper1.SetFieldValue("noSprint" , data.p_noSprint);
            Sniper1.SetFieldValue("mustKeyPress" , data.p_mustKeyPress);
            Sniper1.SetFieldValue("requiredStock" , data.p_requiredStock);
            Sniper1.SetFieldValue("stockToConsume" , data.p_stockToConsome);
            Sniper1.SetFieldValue("activationState", new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperPrimary)));

            Sniper2.SetFieldValue("baseRechargeInterval" , data.s_rechargeInterval);
            Sniper2.SetFieldValue("baseMaxStock" , data.s_baseMaxStock);
            Sniper2.SetFieldValue("rechargeStock" , data.s_rechargeStock);
            Sniper2.SetFieldValue("isBullets" , data.s_isBullets);
            Sniper2.SetFieldValue("shootDelay" , data.s_shootDelay);
            Sniper2.SetFieldValue("beginSkillCooldownOnSkillEnd" , data.s_beginCDOnEnd);
            Sniper2.SetFieldValue("isCombatSkill" , data.s_isCombatSkill);
            Sniper2.SetFieldValue("noSprint" , data.s_noSprint);
            Sniper2.SetFieldValue("mustKeyPress" , data.s_mustKeyPress);
            Sniper2.SetFieldValue("requiredStock" , data.s_requiredStock);
            Sniper2.SetFieldValue("stockToConsume" , data.s_stockToConsume);
            Sniper2.SetFieldValue("activationState", new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperSecondary)));


            Sniper3.SetFieldValue("baseRechargeInterval" , data.u_rechargeInterval);
            Sniper3.SetFieldValue("baseMaxStock" , data.u_baseMaxStock);
            Sniper3.SetFieldValue("rechargeStock" , data.u_rechargeStock);
            Sniper3.SetFieldValue("isBullets" , data.u_isBullets);
            Sniper3.SetFieldValue("shootDelay" , data.u_shootDelay);
            Sniper3.SetFieldValue("beginSkillCooldownOnSkillEnd" , data.u_beginCDOnEnd);
            Sniper3.SetFieldValue("isCombatSkill" , data.u_isCombatSkill);
            Sniper3.SetFieldValue("noSprint" , data.u_noSprint);
            Sniper3.SetFieldValue("mustKeyPress" , data.u_mustKeyPress);
            Sniper3.SetFieldValue("requiredStock" , data.u_requiredStock);
            Sniper3.SetFieldValue("stockToConsume" , data.u_stockToConsume);
            Sniper3.SetFieldValue("activationState", new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperUtility)));


            Sniper4.SetFieldValue("baseRechargeInterval" , data.r_rechargeInterval);
            Sniper4.SetFieldValue("baseMaxStock" , data.r_baseMaxStock);
            Sniper4.SetFieldValue("rechargeStock" , data.r_rechargeStock);
            Sniper4.SetFieldValue("isBullets" , data.r_isBullets);
            Sniper4.SetFieldValue("shootDelay" , data.r_shootDelay);
            Sniper4.SetFieldValue("beginSkillCooldownOnSkillEnd" , data.r_beginCDOnEnd);
            Sniper4.SetFieldValue("isCombatSkill" , data.r_isCombatSkill);
            Sniper4.SetFieldValue("noSprint" , data.r_noSprint);
            Sniper4.SetFieldValue("mustKeyPress" , data.r_mustKeyPress);
            Sniper4.SetFieldValue("requiredStock" , data.r_requiredStock);
            Sniper4.SetFieldValue("stockToConsume" , data.r_stocktoConsume);
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
            SL.passiveSkill.skillDescriptionToken = "Some enemies can be shot in the head for 50% bonus damage.";


            var survivor = new SurvivorDef
            {
                bodyPrefab = body,
                descriptionToken = "Sniper is a high single-target damage survivor with moderate mobility.",
                displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/SniperDisplay"),
                primaryColor = new Color(0.15f, 0.15f, 0.15f),
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
                    Debug.Log("Beetle queen head adjusted");
                    //colb.size = new Vector3(1f, 1f, 0.8f);
                    colb.center = new Vector3(0f, 0f, -0.1f);
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

            target = target.Find("Mouth");

            GameObject g = target.gameObject;
            g.layer = sample.gameObject.layer;
            CapsuleCollider col = g.AddComponent<CapsuleCollider>();
            col.radius = 0.75f;
            col.isTrigger = false;
            col.enabled = true;
            col.center = new Vector3(0f, -0.2f, -0.25f);
            col.height = 2.5f;
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

            foreach( CapsuleCollider coll in targetObj.GetComponentsInChildren<CapsuleCollider>() )
            {
                if( coll.gameObject.name == "TempHurtbox" )
                {
                    Debug.Log("Templar main hurtbox adjusted");
                    coll.center = new Vector3(0f, -0.3f, 0f);
                }
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


