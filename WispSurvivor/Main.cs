using BepInEx;
using BepInEx.Configuration;
using RoR2;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using RoR2.Networking;
using WispSurvivor.Util;
using WispSurvivor.Modules;
using R2API.Utils;
using static WispSurvivor.Util.PrefabUtilities;

namespace WispSurvivor
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.RogueWisp", "Rein-RogueWisp", "1.1.1")]
    public class WispSurvivorMain : BaseUnityPlugin
    {
        Dictionary<Type, Component> componentLookup = new Dictionary<Type, Component>();

        GameObject body;

        //public static ConfigWrapper<bool> configWrappingPaper;

        public void Awake()
        {
            /*
            configWrappingPaper = Config.Wrap<bool>("Settings", "Enable Logging", "Should run stats be saved to a file seperate from PreviousRun.xml? This info is not tranmitted anywhere automatically. If you wish, you may send me the file via discord for me to improve overall mod quality.", false);
            bool logging = configWrappingPaper.Value;

            if( logging )
            {
                Debug.Log("Local stat tracking is enabled. This means that run reports will not overwrite eachother." + 
                    "\nYou can disable this setting in the config." + 
                    "\nIf you wish, you may also send these archived reports to me via discord where I will use the data to help better balance my mods (and nothing else)");
            } else
            {
                Debug.Log("Local stat tracking is disabled. Run reports will function as they do in vanilla game. If you wish to enable stat tracking you may do so in the config." +
                    "\nWhen enabled, a second run report is saved to a subfolder and does not overwrite previous ones. You then may submit those reports to me via discord to me if you want to" +
                    "\nIf you do choose to send them to me, I will use them to gather information about the state of my mod" +
                    "\nAn example of that would be: 80% of run reports for Rogue Wisp show that players struggle to get past stage 3. That may mean I need to address the starting power of the character.");
            }
            */


            var execAssembly = Assembly.GetExecutingAssembly();
            var stream = execAssembly.GetManifestResourceStream("WispSurvivor.Bundle.wispsurvivor");
            var bundle = AssetBundle.LoadFromStream(stream);

            //Get a copy of the body prefab we need
            body = Resources.Load<GameObject>("Prefabs/CharacterBodies/AncientWispBody").InstantiateClone("Rogue Wisp");
            //GameObject body = bundle.LoadAsset<GameObject>("Assets/Ancient Wisp/ancientwispbody.prefab");

            //Rename the body so it doesn't conflict with existing one
            //body.name = "Rogue Wisp";

            //Queue the body to be added to the BodyCatalog
            PrefabUtilities.RegisterNewBody(body);

            //Perform the components module edits, save to the component cache
            componentLookup = WispComponentModule.DoModule(body);

            //Perform the material module edits
            WispMaterialModule.DoModule(body, componentLookup);

            //Perform the effect module edits
            WispEffectModule.DoModule(body, componentLookup);

            //Perform the model module edits
            WispModelModule.DoModule(body, componentLookup);

            //Perform the motion module edits
            WispMotionModule.DoModule(body, componentLookup);

            //Perform the UI module edits
            WispUIModule.DoModule(body, componentLookup);

            //Perform the charBody module edits
            WispBodySetupModule.DoModule(body, componentLookup);

            //Perform the camera module edits
            WispCameraModule.DoModule(body, componentLookup);

            //Perform the animation module edits
            WispAnimationModule.DoModule(body, componentLookup, bundle);

            //Perform the orb module edits
            WispOrbModule.DoModule(body, componentLookup);

            //Perform the projectile module edits
            WispProjectileModule.DoModule(body, componentLookup);

            //Perform the skills module edits
            WispSkillsModule.DoModule(body, componentLookup);

            //Perform the buff module edits
            WispBuffModule.DoModule(body, componentLookup);

            //Perform the info module edits
            WispInfoModule.DoModule(body, componentLookup);

            //Create a characterDisplay

            GameObject display = body.GetComponent<ModelLocator>().modelTransform.gameObject;
            //display.transform.localScale = Vector3.one;
            //Destroy(display.transform.Find("CannonPivot").Find("AncientWispArmature").Find("Head").Find("GameObject").gameObject);
            //Destroy(display.GetComponent<CharacterModel>().baseParticleSystemInfos[0].particleSystem.gameObject);
            //Destroy(display.GetComponent<CharacterModel>().baseParticleSystemInfos[1].particleSystem.gameObject);

            SurvivorDef bodySurvivorDef = new SurvivorDef
            {
                bodyPrefab = body,
                descriptionToken = "WISP_SURVIVOR_BODY_DESC",
                displayPrefab = display,
                primaryColor = new Color(0.7f, 0.2f, 0.9f),
            };

            R2API.SurvivorAPI.AddSurvivor(bodySurvivorDef);
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #region Other random stuff
            //Fix this thing that does something? I think?
            InteractionDriver bodyIntDriver = body.GetComponent<InteractionDriver>();
            bodyIntDriver.highlightInteractor = true;

            //Adjust network transform settings
            CharacterNetworkTransform bodyNetTrans = body.GetComponent<CharacterNetworkTransform>();
            bodyNetTrans.positionTransmitInterval = 0.05f;
            bodyNetTrans.interpolationFactor = 3f;
            #endregion
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        public void Start()
        {
            //Register effects
            WispEffectModule.Register();
            //Register buffs
            WispBuffModule.RegisterBuffs();
        }
    }

   
}

//For next release:
// TODO: Utility: Further gameplay tweaks
    //Create a gameplay pattern around being inside the burn zone with enemies
    //Remove the flame buff stacks from killing enemies
    //


// TODO: Convert flat consumption charge funcs to return ChargeState
// TODO: Utility sounds (unsure what to do here)
// TODO: Visual flare thing on utility stuff...
// TODO: Update Descriptions

// Future plans and shit


// TOD: Need indicator for utility duration (spawn in the entitystate, have sync vars set up on object, pass ref to object to the orb)
// TOD: Body Animation smoothing params
// TOD: Rewrite secondary for MP (2 states possibly?)
// TOD: Offset the modelbase instead of the model
// TOD: Character lobby description
// TOD: Primary explosion effect tweak (yellow stuff)
// TOD: Customize crosshair
// TOD: Death Effects
// TOD: Set up screen scaling on UI
// TOD: R hovering and movement
// TOD: Readme lore sillyness
// TOD: Hide UI when paused
// TOD: Add a second charge readout 
// TOD: Face flare
// TOD: Hurtbox during special
// TOD: Portrait,Pod
// TOD: Ragdoll stuff
// TOD: Effects obscuring vision
// TOD: Effect brightness settings
// TOD: Armor material with skins
// TOD: Muzzle flashes
// TOD: Skill Icons
// TOD: Animation cleanup and improvements
// TOD: Base class for mod plugin with helpers
// TOD: Additional bonuses from charge level
// TOD: ItemDisplayRuleset
// TOD: Null ref on kill enemy with primary when client
