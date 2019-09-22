using RoR2;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ReinSniperRework
{
    public static class SniperHeadshotHitboxStuff
    {
        private static Transform FindChildPath(Transform parent, string s, char seperator = '/', [CallerFilePath] string file = null, [CallerMemberName] string name = null, [CallerLineNumber] int lineNumber = 0)
        {
            string pathString = s;
            if (!pathString.StartsWith(seperator.ToString()))
            {
                pathString = seperator + pathString;
            }
            string[] path = pathString.Split(seperator);

            Transform temp = parent;

            for (int i = 1; i < path.Length; i++)
            {
                temp = temp.Find(path[i]);
                if (!temp)
                {
                    Debug.Log("Null Transform:" + path[i]);
                    Debug.Log(file + " -- " + name + " Line: " + lineNumber);
                    return null;
                }
            }
            return temp;
        }

        public static void AddHurtboxes()
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

        private static void AddDunestriderHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("ClayBossBody");
            if (!targetObj)
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "ModelBase/mdlClayBoss/ClayBossArmature/ROOT/PotBase", '/');
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
            }


            Transform earL = target.Find("EarLight.l");
            Transform earR = target.Find("EarLight.r");

            if (earL)
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
            if (earR)
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

        private static void AddFatImpHurtbox()
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

        private static void AddBeetleQueenHurtbox()
        {
            GameObject targetObj = BodyCatalog.FindBodyPrefab("BeetleQueen2Body");
            if (!targetObj)
            {
                Debug.Log("No targetObj");
                return;
            }

            Transform target = FindChildPath(targetObj.transform, "ModelBase/mdlBeetleQueen/BeetleQueenArmature/ROOT/Base/Chest/Neck/Head", '/');
            if (!target)
            {
                Debug.Log("No Target");
                return;
            }

            foreach (HurtBox hbb in target.gameObject.GetComponentsInChildren<HurtBox>())
            {
                BoxCollider colb = hbb.gameObject.GetComponent<BoxCollider>();
                if (colb.transform.localScale.x > 5f)
                {
                    //Debug.Log("Beetle queen head adjusted");
                    //colb.size = new Vector3(1f, 1f, 0.8f);
                    colb.center = new Vector3(0f, 0f, -0.1f);
                }
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

        private static void AddTitanHurtbox()
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

        private static void AddGoldTitanHurtbox()
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

        private static void AddVagrantHurtbox()
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

        private static void AddGroveBoiHurtbox()
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

        private static void AddZappySnokHurtbox()
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

        private static void AddSpicySnokHurtbox()
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
        private static void AddBrassholeHurtbox()
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

        private static void AddBisonHurtbox()
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

        private static void AddTemplarHurtbox()
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

            foreach (CapsuleCollider coll in targetObj.GetComponentsInChildren<CapsuleCollider>())
            {
                if (coll.gameObject.name == "TempHurtbox")
                {
                    //Debug.Log("Templar main hurtbox adjusted");
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

        private static void AddGolemHurtbox()
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

        private static void AddElderLemurianHurtbox()
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
        private static void AddImpHurtbox()
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