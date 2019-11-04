using BepInEx;
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
    [BepInPlugin("com.ReinThings.RogueWisp", "Rein-RogueWisp", "1.0.3")]
    public class WispSurvivorMain : BaseUnityPlugin
    {
        Dictionary<Type, Component> componentLookup = new Dictionary<Type, Component>();

        GameObject body;

        public void Awake()
        {
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

//For release:

// TODO: Charge indicator not working in MP for clients
// TODO: Charge not syncing in MP
// TODO: Primary firing correctly, but the blastattack is not happening in correct area when used by client (

// TODO: Primary animation issue needs fix (twitch at end) most likely need state chaining
// TODO: Material and AOE marker on utility aim
// TODO: Verify primary damage values are correct
// TODO: Need indicator for utility duration


// TOD: Rewrite secondary for MP (2 states possibly?)
// TOD: SkillsCatalog get additional skilldefs
// TOD: Offset the modelbase instead of the model
// TOD: Character lobby description
// TOD: Primary explosion effect tweak (yellow stuff)
// TOD: Customize crosshair
// TOD: Explosion falloffs
// TOD: Death Effects
// TOD: Utility sounds (unsure what to do here)
// TOD: Set up screen scaling on UI
// TOD: R hovering and movement
// TOD: Readme lore sillyness
// TOD: Hide UI when paused
// TOD: Add a second charge readout 
// TOD: Shift needs adjustments
// TOD: Face flare
// TOD: Networking
// TOD: Hurtbox during special
// TOD: Buffs setup
// TOD: Portrait,Pod
// TOD: Set up the item display ruleset
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
// TOD: Visual flare thing on utility stuff...
