using BepInEx;
using RoR2.UI;
using MonoMod.Cil;
using R2API.Utils;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using WispSurvivor.Modules;
using WispSurvivor.Helpers;
using static WispSurvivor.Helpers.PrefabHelpers;
using System.IO;
using UnityEngine.Networking;

namespace WispSurvivor
{
    [R2APISubmoduleDependency( nameof( R2API.SurvivorAPI ) )]
    [BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.ReinThings.RogueWisp", "Rein-RogueWisp", "1.2.4" )]
    public class WispSurvivorMain : BaseUnityPlugin
    {
        

        private Dictionary<Type, Component> componentLookup = new Dictionary<Type, Component>();
        private GameObject body;

        Texture2D bgTex;
        Rect posRect = new Rect(32f, 32f, 1524f, 96f);

        public void Awake()
        {
            String thing1 = "To anyone looking to troubleshoot compatibility/bugs, feel free to message me on Discord (@Rein#7551) so I can fix/change how things work.";
            String thing2 = "To anyone looking to learn from my code, feel free to message me on Discord (@Rein#7551) and ask any questions you want.";
            String thing3 = "To anyone looking to simply copy paste my code... Thanks...";


            Assembly execAssembly = Assembly.GetExecutingAssembly();
            System.IO.Stream stream = execAssembly.GetManifestResourceStream( "WispSurvivor.Bundle.wispsurvivor" );
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);

            //Get a copy of the body prefab we need
            this.body = Resources.Load<GameObject>( "Prefabs/CharacterBodies/AncientWispBody" ).InstantiateClone( "Rogue Wisp" );

            //Queue the body to be added to the BodyCatalog
            PrefabHelpers.RegisterNewBody( this.body );

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
            WispProjectileModule.DoModule( this.body, this.componentLookup );

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

            //Hooks to make stun grenades scale with proc coef correctly
            IL.RoR2.GlobalEventManager.OnHitEnemy += ( il ) =>
            {
                ILCursor c = new ILCursor(il);
                c.GotoNext( MoveType.After,
                    x => x.MatchCallvirt( "RoR2.SetStateOnHurt", "SetStun" )
                    );
                c.Index += -3;
                c.RemoveRange( 3 );
            };

            On.RoR2.GlobalEventManager.OnHitEnemy += ( orig, self, info, victim ) =>
            {
                orig( self, info, victim );
                if( info.procCoefficient <= 0f || info.rejected || !NetworkServer.active || !info.attacker ) return;
                var body = info.attacker.GetComponent<CharacterBody>();
                if( !body ) return;
                var inventory = body.inventory;
                if( !inventory ) return;
                int stunCount = inventory.GetItemCount(ItemIndex.StunChanceOnHit);
                if( stunCount <= 0 ) return;
                var sqCoef = Mathf.Sqrt(info.procCoefficient);
                if( !RoR2.Util.CheckRoll( RoR2.Util.ConvertAmplificationPercentageIntoReductionPercentage( sqCoef * 5f * (float)stunCount ), body.master ) ) return;
                var stateOnHurt = victim.GetComponent<SetStateOnHurt>();
                if( !stateOnHurt ) return;
                stateOnHurt.SetStun( sqCoef * 2f );
            };

            //Hook to add a burn manager to all characterbodies.
            On.RoR2.CharacterBody.Start += ( orig, self ) =>
            {
                orig( self );
                self.gameObject.AddComponent<Components.WispBurnManager>();
            };
            #endregion
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            bgTex = CreateBGTexture();
        }

        public void Start()
        {
            //Register effects
            WispEffectModule.Register();
            //Register buffs
            WispBuffModule.RegisterBuffs();

            DebugMaterialInfo( CharacterModel.eliteHauntedParticleReplacementMaterial );
            DebugMaterialInfo( CharacterModel.elitePoisonParticleReplacementMaterial );
        }

        public void FixedUpdate()
        {
            //Ensure that the isModded flag is always set
            RoR2Application.isModded = true;
        }

        
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
        
    }


}

//For next release:

// TODO: Incineration needs some impact effects
// TODO: Primary particles still aren't visible sometimes.
// TODO: Iridescent needs to be more rainbowey?

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
// TOD: Readme lore sillyness
// TOD: Effects obscuring vision
// TOD: Effect brightness settings
// TOD: Muzzle flashes
// TOD: Skill Icons
// TOD: Animation cleanup and improvements
// TOD: Base class for mod plugin with helpers
// TOD: Null ref on kill enemy with primary when client
// TOD: Improved Utility effects (orb of fire in center, orbs from enemies burning to center)
// TOD: Improve itemdisplayruleset
// TOD: Pod UV mapping (need to duplicate mesh?)
// TOD: Capacitor limb issue
// TOD: ParticleBuilder class, and convert everything to use it.

