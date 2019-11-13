using BepInEx;
using BepInEx.Configuration;
using RoR2;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Reflection;
using RoR2.Networking;
using WispSurvivor.Util;
using WispSurvivor.Modules;
using R2API.Utils;
using static WispSurvivor.Util.PrefabUtilities;
using System.Collections;
using MonoMod.Cil;

namespace WispSurvivor
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.RogueWisp", "Rein-RogueWisp", "1.1.3")]
    public class WispSurvivorMain : BaseUnityPlugin
    {
        Dictionary<Type, Component> componentLookup = new Dictionary<Type, Component>();

        GameObject body;

        //Texture2D bgTex;

        //Rect posRect = new Rect(32f, 32f, 1524f, 96f);

        public void Awake()
        {
            var execAssembly = Assembly.GetExecutingAssembly();
            var stream = execAssembly.GetManifestResourceStream("WispSurvivor.Bundle.wispsurvivor");
            var bundle = AssetBundle.LoadFromStream(stream);

            //Get a copy of the body prefab we need
            body = Resources.Load<GameObject>("Prefabs/CharacterBodies/AncientWispBody").InstantiateClone("Rogue Wisp");

            //Queue the body to be added to the BodyCatalog
            PrefabUtilities.RegisterNewBody(body);

            //Perform the components module edits, save to the component cache
            componentLookup = WispComponentModule.DoModule(body);

            //Perform the material module edits
            WispMaterialModule.DoModule(body, componentLookup, this);

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

            GameObject display = body.GetComponent<ModelLocator>().modelBaseTransform.gameObject;
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
            ///

            IL.RoR2.CharacterBody.RecalculateStats += (il) =>
            {
                ILCursor c = new ILCursor(il);
                c.GotoNext(MoveType.After,
                    x => x.MatchLdloc(39),
                    x => x.MatchLdcR4(0.1f),
                    x => x.MatchLdcR4(0.2f),
                    x => x.MatchLdloc(17),
                    x => x.MatchConvR4(),
                    x => x.MatchMul(),
                    x => x.MatchAdd(),
                    x => x.MatchLdarg(0),
                    x => x.MatchLdfld<RoR2.CharacterBody>("sprintingSpeedMultiplier"),
                    x => x.MatchDiv(),
                    x => x.MatchAdd(),
                    x => x.MatchStloc(39),
                    x => x.MatchLdarg(0),
                    x => x.MatchLdcI4(19)
                    );

                c.GotoNext(MoveType.After,
                    x => x.MatchBrfalse( out _ ),
                    x => x.MatchLdloc(39),
                    x => x.MatchLdcR4(0.2f),
                    x => x.MatchAdd(),
                    x => x.MatchStloc(39),
                    x => x.MatchLdarg(0),
                    x => x.MatchLdcI4(5)
                    );

                c.GotoNext(MoveType.After,
                    x => x.MatchBrfalse( out _ ),
                    x => x.MatchLdloc(39),
                    x => x.MatchLdcR4(0.3f),
                    x => x.MatchAdd(),
                    x => x.MatchStloc(39),
                    x => x.MatchLdarg(0),
                    x => x.MatchLdcI4(6)
                    );

                c.Index += 3;
                c.Remove();
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_R4, 0.25f);
            };

            //bgTex = CreateBGTexture();
        }

        public void Start()
        {
            //Register effects
            WispEffectModule.Register();
            //Register buffs
            WispBuffModule.RegisterBuffs();
        }

        /*
        public void OnGUI()
        {
            GUI.DrawTexture(posRect, bgTex);
            GUI.DrawTexture(posRect, WispMaterialModule.armorTextures[6][0]);
        }

        private Texture2D CreateBGTexture()
        {
            Texture2D tex = new Texture2D(256, 16, TextureFormat.ARGB32, false);

            bool swap = false;

            Color b = new Color(0f, 0f, 0f, 1f);
            Color w = new Color(1f, 1f, 1f, 1f);

            Color[] white = new Color[64];
            Color[] black = new Color[64];

            for (int i = 0; i < 64; i++)
            {
                white[i] = w;
                black[i] = b;
            }

            for (int i = 0; i < 256; i += 8)
            {
                tex.SetPixels(i, 0, 8, 8, (swap ? white : black));
                tex.SetPixels(i, 8, 8, 8, (swap ? black : white));
                swap = !swap;
            }

            tex.Apply();
            return tex;
        }



        private static void DebugMaterialInfo(Material m)
        {
            Debug.Log("Material name: " + m.name);
            string[] s = m.shaderKeywords;
            Debug.Log("Shader keywords");
            for (int i = 0; i < s.Length; i++)
            {
                Debug.Log(s[i]);
            }

            Debug.Log("Shader name: " + m.shader.name);

            Debug.Log("Texture Properties");
            string[] s2 = m.GetTexturePropertyNames();
            for (int i = 0; i < s2.Length; i++)
            {
                Debug.Log(s2[i] + " : " + m.GetTexture(s2[i]));
            }
        }
        */
    }

   
}

//For next release:
// TODO: Lower bound on consumption (Redirect all consumption to the passive for easier managment)

// TODO: Convert flat consumption charge funcs to return ChargeState
// TODO: Rename repurposed vars
// TODO: Update Descriptions
// TODO: Record video for the version

// Future plans and shit

// TOD: Utility sounds (unsure what to do here)
// TOD: Little Disciple and Will O Wisp color should change with skin
// TOD: Body Animation smoothing params
// TOD: Rewrite secondary for MP (2 states possibly?)
// TOD: Character lobby description
// TOD: Primary explosion effect tweak (yellow stuff)
// TOD: Customize crosshair
// TOD: Set up screen scaling on UI
// TOD: R hovering and movement
// TOD: Readme lore sillyness
// TOD: Particles in logbook are weird
// TOD: Portrait
// TOD: Effects obscuring vision
// TOD: Effect brightness settings
// TOD: Muzzle flashes
// TOD: Skill Icons
// TOD: Animation cleanup and improvements
// TOD: Base class for mod plugin with helpers
// TOD: Null ref on kill enemy with primary when client
// TOD: Survivor Pod flames
// TOD: Improved Utility effects (orb of fire in center, orbs from enemies burning to center)
// TOD: Primary clarity on charges
// TOD: Improve itemdisplayruleset

