using BepInEx;
using R2API.Utils;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using WispSurvivor.Helpers;
using WispSurvivor.Modules;
using static WispSurvivor.Helpers.APIInterface;

namespace WispSurvivor
{
    [R2APISubmoduleDependency( nameof( R2API.SurvivorAPI ) )]
    [BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.ReinThings.RogueWisp", "Rein-RogueWisp", "1.3.1" )]
    public partial class WispSurvivorMain : RoR2Plugin.RoR2Plugin
    {
        private Dictionary<Type, Component> componentLookup = new Dictionary<Type, Component>();
        private GameObject body;

        private BuffIndex armorBuff;
        private BuffIndex chargeBuff;
        private Texture2D bgTex;
        private Rect posRect = new Rect(32f, 32f, 1524f, 96f);

        public void Awake()
        {
            String thing1 = "To anyone looking to troubleshoot compatibility/bugs, feel free to message me on Discord (@Rein#7551) so I can fix/change how things work.";
            String thing2 = "To anyone looking to learn from my code, feel free to message me on Discord (@Rein#7551) and ask any questions you want.";
            String thing3 = "To anyone looking to simply copy paste my code... Thanks...";

            Assembly execAssembly = Assembly.GetExecutingAssembly();
            System.IO.Stream stream = execAssembly.GetManifestResourceStream( "WispSurvivor.Bundle.wispsurvivor" );
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);

            //Get a copy of the body prefab we need
            //Resources.UnloadAsset( Resources.Load<GameObject>( "Prefabs/CharacterBodies/AncientWispBody" ) );
            this.body = Resources.Load<GameObject>( "Prefabs/CharacterBodies/AncientWispBody" ).InstantiateClone( "Rogue Wisp" );
            //Resources.UnloadAsset( Resources.Load<GameObject>( "Prefabs/CharacterBodies/AncientWispBody" ) );

            //Queue the body to be added to the BodyCatalog
            RegisterNewBody( this.body );

            //Perform the components module edits, save to the component cache
            this.componentLookup = WispComponentModule.DoModule( this.body );

            //Perform the material module edits
            WispMaterialModule.DoModule( this.body, this.componentLookup, this );

            //Perform the effect module edits
            WispEffectModule.DoModule( this.body, this.componentLookup );

            //Perform the model module edits
            WispModelModule.DoModule( this.body, this.componentLookup );

            //Perform the motion module edits
            WispMotionModule.DoModule( this.body, this.componentLookup );

            //Perform the UI module edits
            WispUIModule.DoModule( this.body, this.componentLookup );

            //Perform the charBody module edits
            WispBodySetupModule.DoModule( this.body, this.componentLookup, bundle );

            //Perform the camera module edits
            WispCameraModule.DoModule( this.body, this.componentLookup );

            //Perform the animation module edits
            WispAnimationModule.DoModule( this.body, this.componentLookup, bundle );

            //Perform the orb module edits
            WispOrbModule.DoModule( this.body, this.componentLookup );

            //Perform the projectile module edits
            //WispProjectileModule.DoModule( this.body, this.componentLookup );

            //Perform the skills module edits
            WispSkillsModule.DoModule( this.body, this.componentLookup, bundle );

            //Perform the buff module edits
            WispBuffModule.DoModule( this.body, this.componentLookup );

            //Perform the info module edits
            WispInfoModule.DoModule( this.body, this.componentLookup );

            //Create a characterDisplay
            GameObject display = this.body.GetComponent<ModelLocator>().modelBaseTransform.gameObject;

            SurvivorDef bodySurvivorDef = new SurvivorDef
            {
                bodyPrefab = body,
                descriptionToken = "WISP_SURVIVOR_BODY_DESC",
                displayPrefab = display,
                primaryColor = new Color(0.7f, 0.2f, 0.9f),
            };

            R2API.SurvivorAPI.AddSurvivor( bodySurvivorDef );
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #region Other random stuff
            //Fix this thing that does something? I think?
            InteractionDriver bodyIntDriver = this.body.GetComponent<InteractionDriver>();
            bodyIntDriver.highlightInteractor = true;

            //Adjust network transform settings
            CharacterNetworkTransform bodyNetTrans = this.body.GetComponent<CharacterNetworkTransform>();
            bodyNetTrans.positionTransmitInterval = 0.05f;
            bodyNetTrans.interpolationFactor = 3f;
            #endregion
#if DEBUG
            BuildMainLayer();
#endif
        }

        public void Start()
        {
            //Register effects
            WispEffectModule.Register();
            Dictionary<GameObject, UInt32> dict = (EffectManager.instance).GetFieldValue<Dictionary<GameObject, UInt32>>( "effectPrefabToIndexMap" );
            for( Int32 i = 0; i < 8; i++ )
            {
                this.restoreIndex[i] = dict[Modules.WispEffectModule.utilityLeech[i]];
            }

            //Register buffs
            WispBuffModule.RegisterBuffs();

            this.armorBuff = BuffCatalog.FindBuffIndex( "WispArmorBuff" );
            this.chargeBuff = BuffCatalog.FindBuffIndex( "WispFlameChargeBuff" );
        }

        public void FixedUpdate() => RoR2Application.isModded = true;

#if DEBUG
        public void Update()
        {
            KeyListen();
        }

        public void OnGUI()
        {
            DrawUI();
        }
#endif
        /*
        public void OnGUI()
        {
            GUI.DrawTexture(posRect, bgTex);
            GUI.DrawTexture(posRect, WispMaterialModule.fireTextures[7]);
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

// TODO: Primary particles still aren't visible sometimes.
// TODO: Primary needs impact effects and world hit effects.
// TODO: Merge primary into single state again

// TODO: Update Descriptions

// Future plans and shit

// TOD: Utility sounds (unsure what to do here)
// TOD: Little Disciple and Will O Wisp color should change with skin
// TOD: Body Animation smoothing params
// TOD: Rewrite secondary for MP (2 states possibly?)
// TOD: Character lobby description
// TOD: Customize crosshair
// TOD: Readme lore sillyness
// TOD: Effects obscuring vision
// TOD: Effect brightness settings
// TOD: Muzzle flashes
// TOD: Skill Icons
// TOD: Animation cleanup and improvements
// TOD: Null ref on kill enemy with primary when client
// TOD: Improve itemdisplayruleset
// TOD: Pod UV mapping (need to duplicate mesh?)
// TOD: Capacitor limb issue
// TOD: ParticleBuilder class, and convert everything to use it.
// TOD: Custom CharacterMain
// TOD: Broach shield location is weird
// TOD: Sounds for Special
// TOD: Special beam should do OnHitGlobal at hit spot
// TOD: Secondary doing onhitglobal for all enemies hit and not at center
// TOD: Meteor ignores wisp
// TOD: Shield material vertex offset
// TOD: Incinteration impact effects
// TOD: Redo other elite materials
// TOD: Expanded skin selection (Set of plasma skins, hard white outline on flames like blighted currently has.
// TOD: Explore networking potential from handleOverlapAttack



// ERRORS:
/*
[Info   : Unity Log] <style=cDeath><sprite name="Skull" tint=1> Close! <sprite name="Skull" tint=1></color>
[Info   : Unity Log] Could not save RunReport P:/Program Files (x86)/Steam/steamapps/common/Risk of Rain 2/Risk of Rain 2_Data/RunReports/PreviousRun.xml: The ' ' character, hexadecimal value 0x20, cannot be included in a name.
[Warning: Unity Log] Parent of RectTransform is being set with parent property. Consider using the SetParent method instead, with the worldPositionStays argument set to false. This will retain local orientation and scale rather than world orientation and scale, which can prevent common UI scaling issues.
[Error  : Unity Log] NullReferenceException
Stack trace:
UnityEngine.Transform.get_position () (at <f2abf40b37c34cf19b7fd98865114d88>:0)
RoR2.UI.CombatHealthBarViewer.UpdateAllHealthbarPositions (UnityEngine.Camera sceneCam, UnityEngine.Camera uiCam) (at <eae781bd93824da1b7902a2b6526887c>:0)
RoR2.UI.CombatHealthBarViewer.LayoutForCamera (RoR2.UICamera uiCamera) (at <eae781bd93824da1b7902a2b6526887c>:0)
RoR2.UI.CombatHealthBarViewer.SetLayoutHorizontal () (at <eae781bd93824da1b7902a2b6526887c>:0)
UnityEngine.UI.LayoutRebuilder.<Rebuild>m__3 (UnityEngine.Component e) (at <adbbd84a6a874fb3bb8dd55fe88db73d>:0)
UnityEngine.UI.LayoutRebuilder.PerformLayoutControl (UnityEngine.RectTransform rect, UnityEngine.Events.UnityAction`1[T0] action) (at <adbbd84a6a874fb3bb8dd55fe88db73d>:0)
UnityEngine.UI.LayoutRebuilder.Rebuild (UnityEngine.UI.CanvasUpdate executing) (at <adbbd84a6a874fb3bb8dd55fe88db73d>:0)
UnityEngine.UI.CanvasUpdateRegistry.PerformUpdate () (at <adbbd84a6a874fb3bb8dd55fe88db73d>:0)
UnityEngine.Canvas:SendWillRenderCanvases()

    */

