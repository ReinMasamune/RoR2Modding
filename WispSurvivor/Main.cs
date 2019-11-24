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
using static WispSurvivor.Helpers.CatalogHelpers;
using static RoR2Plugin.ComponentHelpers;
using static RoR2Plugin.MiscHelpers;
using System.IO;
using UnityEngine.Networking;

namespace WispSurvivor
{
    [R2APISubmoduleDependency( nameof( R2API.SurvivorAPI ) )]
    [BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.ReinThings.RogueWisp", "Rein-RogueWisp", "1.2.5" )]
    public partial class WispSurvivorMain : RoR2Plugin.RoR2Plugin
    {
        private Dictionary<Type, Component> componentLookup = new Dictionary<Type, Component>();
        private GameObject body;

        private BuffIndex armorBuff;
        private BuffIndex chargeBuff;

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
            #endregion
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //bgTex = CreateBGTexture();
        }

        public void Start()
        {
            //Register effects
            WispEffectModule.Register();
            //Register buffs
            WispBuffModule.RegisterBuffs();

            //CharacterModel.eliteHauntedParticleReplacementMaterial.DebugMaterialInfo();
            //CharacterModel.elitePoisonParticleReplacementMaterial.DebugMaterialInfo();
            armorBuff = BuffCatalog.FindBuffIndex( "WispArmorBuff" );
            chargeBuff = BuffCatalog.FindBuffIndex( "WispFlameChargeBuff" );
        }

        public override void CreateHooks()
        {
            IL.RoR2.GlobalEventManager.OnHitEnemy += this.RemoveStunOnHit;
            On.RoR2.GlobalEventManager.OnHitEnemy += this.AddStunOnHit;
            On.RoR2.CharacterBody.Start += this.AddBurnManager;
            On.RoR2.CharacterBody.RecalculateStats += this.ArmorBoost;
            IL.RoR2.CharacterBody.RecalculateStats += this.ModifyMSBoost;
            On.RoR2.CharacterBody.FixedUpdate += this.ShieldRegenStuff;
            On.RoR2.CharacterModel.InstanceUpdate += this.CharacterModel_InstanceUpdate;
            //On.RoR2.CharacterModel.Awake += this.CharacterModel_Awake;
        }

        

        public override void RemoveHooks()
        {
            IL.RoR2.GlobalEventManager.OnHitEnemy -= this.RemoveStunOnHit;
            On.RoR2.GlobalEventManager.OnHitEnemy -= this.AddStunOnHit;
            On.RoR2.CharacterBody.Start -= this.AddBurnManager;
            On.RoR2.CharacterBody.RecalculateStats -= this.ArmorBoost;
            IL.RoR2.CharacterBody.RecalculateStats -= this.ModifyMSBoost;
            On.RoR2.CharacterBody.FixedUpdate -= this.ShieldRegenStuff;
            On.RoR2.CharacterModel.InstanceUpdate -= this.CharacterModel_InstanceUpdate;
        }

        private void RemoveStunOnHit( ILContext il )
        {
            ILCursor c = new ILCursor(il);
            c.GotoNext( MoveType.After,
                x => x.MatchCallvirt( "RoR2.SetStateOnHurt", "SetStun" )
                );
            c.Index += -3;
            c.RemoveRange( 3 );
        }

        private void AddStunOnHit( On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo info, GameObject victim )
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
        }

        private void AddBurnManager( On.RoR2.CharacterBody.orig_Start orig, CharacterBody self )
        {
            orig( self );
            self.gameObject.AddComponent<Components.WispBurnManager>();
        }

        private void ArmorBoost( On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self )
        {
            orig( self );
            if( self && self.inventory )
            {
                if( self.HasBuff( armorBuff ) )
                {
                    self.SetPropertyValue<float>( "armor", self.armor + 100f );
                }
            }
        }

        private void ModifyMSBoost( ILContext il )
        {
            ILCursor c = new ILCursor(il);
            c.GotoNext( MoveType.After,
                x => x.MatchLdloc( 39 ),
                x => x.MatchLdcR4( 0.1f ),
                x => x.MatchLdcR4( 0.2f ),
                x => x.MatchLdloc( 17 ),
                x => x.MatchConvR4(),
                x => x.MatchMul(),
                x => x.MatchAdd(),
                x => x.MatchLdarg( 0 ),
                x => x.MatchLdfld<RoR2.CharacterBody>( "sprintingSpeedMultiplier" ),
                x => x.MatchDiv(),
                x => x.MatchAdd(),
                x => x.MatchStloc( 39 ),
                x => x.MatchLdarg( 0 ),
                x => x.MatchLdcI4( 19 )
                );

            c.GotoNext( MoveType.After,
                x => x.MatchBrfalse( out _ ),
                x => x.MatchLdloc( 39 ),
                x => x.MatchLdcR4( 0.2f ),
                x => x.MatchAdd(),
                x => x.MatchStloc( 39 ),
                x => x.MatchLdarg( 0 ),
                x => x.MatchLdcI4( 5 )
                );

            c.GotoNext( MoveType.After,
                x => x.MatchBrfalse( out _ ),
                x => x.MatchLdloc( 39 ),
                x => x.MatchLdcR4( 0.3f ),
                x => x.MatchAdd(),
                x => x.MatchStloc( 39 ),
                x => x.MatchLdarg( 0 ),
                x => x.MatchLdcI4( 6 )
                );

            c.Index += 3;
            c.Remove();
            c.Emit( Mono.Cecil.Cil.OpCodes.Ldc_R4, 0.25f );
        }

        private void ShieldRegenStuff( On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self )
        {
            orig( self );
            if( NetworkServer.active )
            {
                var count = self.GetBuffCount( chargeBuff );
                if( count > 0 )
                {
                    self.healthComponent.RechargeShield( Time.fixedDeltaTime * 0.005f * count * self.maxShield );
                }
            }
        }



        private void CharacterModel_InstanceUpdate( On.RoR2.CharacterModel.orig_InstanceUpdate orig, CharacterModel self )
        {
            if( self.body && self.body.baseNameToken == "WISP_SURVIVOR_BODY_NAME" )
            {
                var eliteInd = self.GetFieldValue<EliteIndex>( "myEliteIndex");
                if( eliteInd == EliteIndex.Poison )
                {
                    self.SetFieldValue<Material>( "particleMaterialOverride", WispMaterialModule.eliteFlameMaterials[self.body.skinIndex][0] );
                } else if( eliteInd == EliteIndex.Haunted )
                {
                    self.SetFieldValue<Material>( "particleMaterialOverride", WispMaterialModule.eliteFlameMaterials[self.body.skinIndex][1] );
                } else
                {
                    self.SetFieldValue<Material>( "particleMaterialOverride", null );
                }

                self.InvokeMethod( "UpdateGoldAffix" );
                self.InvokeMethod( "UpdatePoisonAffix" );
                self.InvokeMethod( "UpdateHauntedAffix" );
                self.InvokeMethod( "UpdateLights" );
            } else orig( self );
        }

        private void CharacterModel_Awake( On.RoR2.CharacterModel.orig_Awake orig, CharacterModel self )
        {

            orig( self );
            if( self && self.transform && self.transform.parent && self.transform.parent.parent && self.transform.parent.parent.HasComponent<CharacterBody>() )
            {
                Debug.Log( "Setting body" );
                self.body = self.transform.root.GetComponent<CharacterBody>();
            }
        }

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

// TODO: Incineration needs some impact effects
// TODO: Primary particles still aren't visible sometimes.

// TODO: Update Descriptions

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
// TOD: Custom CharacterMain
// TOD: Updater stuff

