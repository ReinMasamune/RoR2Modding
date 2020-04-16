#if ROGUEWISP
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using Mono.Cecil;
using ReinCore;
using BepInEx.Configuration;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        private Accessor<CharacterBody,Single> armor = new Accessor<CharacterBody, Single>( "armor" );
        private Accessor<CharacterBody,Single> barrierDecayRate = new Accessor<CharacterBody,Single>( "<barrierDecayRate>k__BackingField" );
        private const Single barrierDecayMult = 0.5f;
        private const Single shieldRegenFrac = 0.02f;
        private const Single rootNumber = 6f;
        internal HashSet<CharacterBody> RW_BlockSprintCrosshair = new HashSet<CharacterBody>();
        private ConfigEntry<Boolean> chargeBarEnabled;
        partial void RW_Hook()
        {
            this.Enable += this.RW_AddHooks;
            this.Disable += this.RW_RemoveHooks;
            this.Load += this.Main_Load1;
            this.chargeBarEnabled = base.Config.Bind<Boolean>( "Visual (Client)", "SkillBarChargeIndicator", true, "Should a charge bar be displayed above the skill bar in addition to around the crosshair?" );
            //this.deathMarkStuff = base.Config.Bind<Boolean>( "Gameplay (Server)", "DeathMarkDebuffChange", true, "Should Death Mark use a new system that favors non-stacking debuffs over stacking Dots?" );
        }



        private void Main_Load1()
        {
            if( this.r2apiPlugin != null )
            {
                var r2apiType = r2apiPlugin.Instance.GetType();
                var loadoutAPIType = r2apiType.Assembly.GetType("LoadoutAPI");
                bodyloadouttoxmlbase = MethodBase.GetMethodFromHandle( loadoutAPIType.GetMethod( "BodyLoadout_ToXml", BindingFlags.Static | BindingFlags.NonPublic ).MethodHandle );
            }
        }

        private static MethodBase bodyloadouttoxmlbase;
        internal static event ILContext.Manipulator ilhook_R2API_LoadoutAPI_BodyLoadout_ToXml
        {
            add
            {
                HookEndpointManager.Modify(bodyloadouttoxmlbase,value);
            }
            remove
            {
                HookEndpointManager.Unmodify(bodyloadouttoxmlbase, value );
            }
        }

        private void RW_RemoveHooks()
        {



            if( this.r2apiPlugin != null && this.r2apiPlugin.Metadata.Version < System.Version.Parse( "2.4.1" ) )
            {
                ilhook_R2API_LoadoutAPI_BodyLoadout_ToXml -= this.Main_ilhook_R2API_LoadoutAPI_BodyLoadout_ToXml;
            }

            HooksCore.RoR2.CameraRigController.Start.On -= this.Start_On1;
            HooksCore.RoR2.CharacterBody.Start.On -= this.Start_On2;
            HooksCore.RoR2.CharacterBody.FixedUpdate.On -= this.FixedUpdate_On;
            HooksCore.RoR2.CharacterBody.RecalculateStats.Il -= this.RecalculateStats_Il;
            HooksCore.RoR2.CharacterBody.RecalculateStats.On -= this.RecalculateStats_On;
            HooksCore.RoR2.UI.CrosshairManager.UpdateCrosshair.Il -= this.UpdateCrosshair_Il;
            HooksCore.RoR2.CameraRigController.Update.Il -= this.Update_Il;
            HooksCore.RoR2.SetStateOnHurt.OnTakeDamageServer.Il -= this.OnTakeDamageServer_Il;
            HooksCore.RoR2.GlobalEventManager.OnHitEnemy.Il -= this.OnHitEnemy_Il;
            HooksCore.RoR2.CharacterModel.UpdateRendererMaterials.Il -= this.UpdateRendererMaterials_Il;
        }

        private static StaticAccessor<Dictionary<String,Dictionary<String,String>>> languageDictionaries = new StaticAccessor<Dictionary<string, Dictionary<string, string>>>( typeof(Language), "languageDictionaries" );


        private void RW_AddHooks()
        {
            HooksCore.RoR2.UI.QuickPlayButtonController.Start.On += this.Start_On4;

            if( this.r2apiPlugin != null && this.r2apiPlugin.Metadata.Version < System.Version.Parse( "2.4.1" ) )
            {
                ilhook_R2API_LoadoutAPI_BodyLoadout_ToXml += this.Main_ilhook_R2API_LoadoutAPI_BodyLoadout_ToXml;
            }

            HooksCore.RoR2.CameraRigController.Start.On += this.Start_On1;
            HooksCore.RoR2.CharacterBody.Start.On += this.Start_On2;
            HooksCore.RoR2.CharacterBody.FixedUpdate.On += this.FixedUpdate_On;
            HooksCore.RoR2.CharacterBody.RecalculateStats.Il += this.RecalculateStats_Il;
            HooksCore.RoR2.CharacterBody.RecalculateStats.On += this.RecalculateStats_On;
            HooksCore.RoR2.UI.CrosshairManager.UpdateCrosshair.Il += this.UpdateCrosshair_Il;
            HooksCore.RoR2.CameraRigController.Update.Il += this.Update_Il;
            HooksCore.RoR2.SetStateOnHurt.OnTakeDamageServer.Il += this.OnTakeDamageServer_Il;
            HooksCore.RoR2.GlobalEventManager.OnHitEnemy.Il += this.OnHitEnemy_Il;
            HooksCore.RoR2.CharacterModel.UpdateRendererMaterials.Il += this.UpdateRendererMaterials_Il;
        }

        private void FromMaster_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );
            c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<RoR2.Loadout>( nameof( Loadout.Copy ) ) );

            c.Emit( OpCodes.Ldloc_1 );
            c.Emit( OpCodes.Ldfld, typeof( RoR2.CharacterSpawnCard ).GetField( "runtimeLoadout", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance ) );
            c.Emit( OpCodes.Ldarg_0 );
            c.EmitDelegate<Action<RoR2.Loadout, RoR2.CharacterMaster>>( ( dest, source ) =>
            {
                if( dest == null || source == null || !source ) return;
                var sourceLoadout = source.loadout;
                if( sourceLoadout == null ) return;

                var sourceSkin = sourceLoadout.bodyLoadoutManager.GetSkinIndex(Main.rogueWispBodyIndex);
                var invertedSkin = (~Helpers.WispBitSkin.GetWispSkin(sourceSkin)).EncodeToSkinIndex();
                dest.bodyLoadoutManager.SetSkinIndex( Main.rogueWispBodyIndex, invertedSkin );
            });

        }

        private void CreateDoppelganger_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );
            c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<RoR2.DirectorCore>( nameof( DirectorCore.TrySpawnObject ) ), x => x.MatchPop() );
            c.Index--;
            c.Remove();
            c.EmitDelegate<Action<GameObject>>( ( obj ) =>
            {
                if( obj != null && obj )
                {
                    var master = obj.GetComponent<CharacterMaster>();
                    if( master != null && master )
                    {
                        var loadout = master.loadout;
                        if( loadout != null )
                        {
                            var bodyInd = BodyCatalog.FindBodyIndex(master.bodyPrefab);
                            if( bodyInd >= 0 && bodyInd == Main.rogueWispBodyIndex )
                            {
                                var bodyLoadoutManager = loadout.bodyLoadoutManager;
                                var skinInd = bodyLoadoutManager.GetSkinIndex( bodyInd );
                                UInt32 newSkinInd;
                                try
                                {
                                    var skin = ~Helpers.WispBitSkin.GetWispSkin( skinInd );
                                    newSkinInd = skin.EncodeToSkinIndex();
                                } catch
                                {
                                    Main.LogE( "Error inverting skin for wisp clone, please copy the line of 1s and 0s below and send them to me." );
                                    Main.LogE( Convert.ToString( skinInd, 2 ).PadLeft( 32, '0' ));
                                    newSkinInd = 0b0000_0000_0000_0000_0000_0000_0000_1000u;
                                }

                                bodyLoadoutManager.SetSkinIndex( bodyInd, newSkinInd );

                                if( master.hasBody )
                                {
                                    var body = master.GetBody();
                                    body.skinIndex = newSkinInd;
                                    if( body != null && body )
                                    {
                                        var ml = body.modelLocator;
                                        if( ml != null && ml )
                                        {
                                            var model = ml.modelTransform;
                                            if( model != null && model )
                                            {
                                                var skinController = model.GetComponent<Helpers.WispModelBitSkinController>();
                                                skinController?.Apply( Helpers.WispBitSkin.GetWispSkin( newSkinInd ) );
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } );


        }

        private void UpdateRendererMaterials_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );
            c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<RoR2.CharacterModel>( "get_isDoppelganger" ) );
            c.Emit( OpCodes.Ldarg_0 );
            c.EmitDelegate<Func<CharacterModel, Boolean>>( ( model ) => model.body != null && model.body.baseNameToken != Rein.Properties.Tokens.WISP_SURVIVOR_BODY_NAME );
            c.Emit( OpCodes.And );
        }

        private void Start_On4( HooksCore.RoR2.UI.QuickPlayButtonController.Start.Orig orig, RoR2.UI.QuickPlayButtonController self )
        {
            self.gameObject.SetActive( false );
            orig( self );
            self.gameObject.SetActive( false );
        }

        private void OnHitEnemy_On( HooksCore.RoR2.GlobalEventManager.OnHitEnemy.Orig orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim )
        {
            orig( self, damageInfo, victim );
            if( damageInfo.procCoefficient <= 0f || damageInfo.rejected || !NetworkServer.active || !damageInfo.attacker ) return;
            CharacterBody body = damageInfo.attacker.GetComponent<CharacterBody>();
            if( !body ) return;
            Inventory inventory = body.inventory;
            if( !inventory ) return;
            Int32 stunCount = inventory.GetItemCount(ItemIndex.StunChanceOnHit);
            if( stunCount <= 0 ) return;
            Single sqCoef = Mathf.Sqrt(damageInfo.procCoefficient);
            if( !RoR2.Util.CheckRoll( RoR2.Util.ConvertAmplificationPercentageIntoReductionPercentage( sqCoef * 5f * stunCount ), body.master ) ) return;
            SetStateOnHurt stateOnHurt = victim.GetComponent<SetStateOnHurt>();
            if( !stateOnHurt ) return;
            stateOnHurt.SetStun( sqCoef * 2f );
        }
        private void OnHitEnemy_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );


            //c.GotoNext( MoveType.Before, x => x.MatchLdloc( 17 ), x => x.MatchLdcI4( 1 ), x => x.MatchAdd(), x => x.MatchStloc( 17 ) );
            //c.Index++;

            //if( this.deathMarkStuff.Value )
            //{
            //    c.Remove();
            //    c.Emit( OpCodes.Ldloc_1 );
            //    c.Emit( OpCodes.Ldloc, 64 );
            //    c.EmitDelegate<Func<CharacterBody, BuffIndex, Int32>>( ( body, index ) =>
            //    {
            //        if( body.HasBuff( index ) )
            //        {
            //            var def = BuffCatalog.GetBuffDef(index);
            //            if( def.canStack )
            //            {
            //                return body.GetBuffCount( index );
            //            } else
            //            {
            //                return 10;
            //            }
            //        }
            //        return 0;
            //    } );
            //}

            c.GotoNext( MoveType.After, x => x.MatchAdd(), x => x.MatchStloc( 64 ), x => x.MatchLdloc( 64 ), x => x.MatchLdcI4( 53 ) );
            c.Index--;
            c.Remove();
            c.EmitDelegate<Func<Int32>>( () => BuffCatalog.buffCount );

            //c.GotoNext( MoveType.Before, x => x.MatchLdloc( 17 ), x => x.MatchLdcI4( 1 ), x => x.MatchAdd(), x => x.MatchStloc( 17 ) );
            //c.Index++;
            //if( this.deathMarkStuff.Value )
            //{
            //    c.Remove();
            //    c.Emit( OpCodes.Ldc_I4_0 );
            //}

            //c.GotoNext( MoveType.After, x => x.MatchLdloc( 17 ), x => x.MatchLdcI4( 4 ) );
            //c.Index--;
            //if( this.deathMarkStuff.Value )
            //{
            //    c.Remove();
            //    c.Emit( OpCodes.Ldc_I4, 30 );
            //}

        }
        private void OnTakeDamageServer_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );
            c.GotoNext( MoveType.Before, x => x.MatchCallOrCallvirt<SetStateOnHurt>( "SetStun" ) );
            c.Emit( OpCodes.Ldloc_0 );
            c.EmitDelegate<Func<DamageInfo, Single>>( ( info ) => info.procCoefficient );
            c.Emit( OpCodes.Mul );
        }
        private void Update_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<RoR2.CharacterBody>( "get_isSprinting" ) );
            c.Emit( OpCodes.Ldarg_0 );
            c.Emit<RoR2.CameraRigController>( OpCodes.Ldfld, "targetBody" );
            c.EmitDelegate<Func<CharacterBody, Boolean>>( ( body ) => !this.RW_BlockSprintCrosshair.Contains( body ) );
            c.Emit( OpCodes.And );
        }
        private void UpdateCrosshair_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );
            c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<RoR2.CharacterBody>( "get_isSprinting" ) );
            c.Emit( OpCodes.Ldarg_1 );
            c.EmitDelegate<Func<CharacterBody, Boolean>>( ( body ) => !this.RW_BlockSprintCrosshair.Contains( body ) );
            c.Emit( OpCodes.And );
        }
        private void RecalculateStats_On( HooksCore.RoR2.CharacterBody.RecalculateStats.Orig orig, CharacterBody self )
        {
            orig( self );
            if( self && self.inventory )
            {
                if( self.HasBuff( RW_armorBuff ) )
                {
                    this.armor.Set( self, this.armor.Get( self ) + 75f );
                }
                if( self.HasBuff( RW_flameChargeBuff ) )
                {
                    this.barrierDecayRate.Set( self, self.barrierDecayRate * barrierDecayMult );
                }
            }
        }
        private void RecalculateStats_Il( ILContext il )
        {
            ILCursor c = new ILCursor(il);
            c.GotoNext( MoveType.After,
                x => x.MatchLdarg( 0 ),
                x => x.MatchLdcI4( 6 ),
                x => x.MatchCallOrCallvirt<RoR2.CharacterBody>( "HasBuff" ),
                x => x.MatchBrfalse( out _ ),
                x => x.MatchLdloc( 51 )
                );

            c.Remove();
            c.Emit( Mono.Cecil.Cil.OpCodes.Ldc_R4, 0.25f );
        }
        private void FixedUpdate_On( HooksCore.RoR2.CharacterBody.FixedUpdate.Orig orig, CharacterBody self )
        {
            orig( self );
            if( NetworkServer.active )
            {
                Int32 count = self.GetBuffCount( RW_flameChargeBuff );
                if( count > 0 )
                {
                    self.healthComponent.AddBarrier( Time.fixedDeltaTime * shieldRegenFrac * Mathf.Pow( count, 1f / rootNumber ) * self.healthComponent.fullCombinedHealth );
                }
            }
        }
        private void Start_On2( HooksCore.RoR2.CharacterBody.Start.Orig orig, CharacterBody self )
        {
            orig( self );
            self.gameObject.AddComponent<WispBurnManager>();

            //if( self.baseNameToken == Properties.Tokens.WISP_SURVIVOR_BODY_NAME )
            //{
            //    var inv = self.inventory;
            //    if( inv && inv.GetItemCount(ItemIndex.InvadingDoppelganger) > 0 )
            //    {
            //        var newSkin = ~Helpers.WispBitSkin.GetWispSkin( self.skinIndex );
            //        self.skinIndex = newSkin.EncodeToSkinIndex();
            //        self.modelLocator.modelTransform.GetComponent<Helpers.WispModelBitSkinController>().Apply( newSkin );
            //    }
            //}
        }
        private void Start_On1( HooksCore.RoR2.CameraRigController.Start.Orig orig, CameraRigController self )
        {
            orig( self );



            if( self.hud != null )
            {

                var par = self.hud.transform.Find( "MainContainer/MainUIArea/BottomRightCluster/Scaler" );

                if( this.chargeBarEnabled.Value )
                {

                    var inst = Instantiate<GameObject>( wispHudPrefab, par );
                    var bar3Rect = inst.GetComponent<RectTransform>();
                    bar3Rect.anchoredPosition = new Vector2( 14f, 140f );
                    bar3Rect.sizeDelta = new Vector2( 310f, 32f );
                    bar3Rect.anchorMin = new Vector2( 0.5f, 0.5f );
                    bar3Rect.anchorMax = new Vector2( 0.5f, 0.5f );
                    bar3Rect.pivot = new Vector2( 0.5f, 0.5f );
                    bar3Rect.localEulerAngles = Vector3.zero;
                    bar3Rect.localScale = Vector3.one;
                }



                var par2 = self.hud.transform.Find( "MainContainer/MainUIArea/CrosshairCanvas");

                var cross1 = Instantiate<GameObject>( wispCrossBar1, par2 );

                var bar1Rect = cross1.GetComponent<RectTransform>();

                var cross2 = Instantiate<GameObject>( wispCrossBar2, par2 );

                var bar2Rect = cross2.GetComponent<RectTransform>();


                bar1Rect.anchoredPosition = new Vector2( 96f, 0f );
                bar2Rect.anchoredPosition = new Vector2( -96f, 0f );

                bar1Rect.sizeDelta = new Vector2( 256f, 128f );
                bar2Rect.sizeDelta = new Vector2( 256f, 128f );

                bar1Rect.anchorMin = new Vector2( 0.5f, 0.5f );
                bar2Rect.anchorMin = new Vector2( 0.5f, 0.5f );

                bar1Rect.anchorMax = new Vector2( 0.5f, 0.5f );
                bar2Rect.anchorMax = new Vector2( 0.5f, 0.5f );

                bar1Rect.pivot = new Vector2( 0.5f, 0.5f );
                bar2Rect.pivot = new Vector2( 0.5f, 0.5f );

                bar1Rect.localEulerAngles = new Vector3( 0f, 0f, -90f );
                bar2Rect.localEulerAngles = new Vector3( 0f, 0f, 90f );

                bar1Rect.localScale = Vector3.one * 0.125f;
                bar2Rect.localScale = Vector3.one * 0.125f;
            }
        }
        private void Main_ilhook_R2API_LoadoutAPI_BodyLoadout_ToXml( ILContext il )
        {
            ILCursor c = new ILCursor( il );
            ILLabel label = default;
            c.GotoNext( MoveType.After, x => x.MatchLdloc( 1 ) );
            c.Remove();
            ++c.Index;
            c.Remove();
            c.EmitDelegate<Func<ModelSkinController, UInt32, SkinDef>>( ( modelSkin, skinInd ) =>
            {
                if( modelSkin == null ) return null;
                if( skinInd >= modelSkin.skins.Length ) return null;
                return modelSkin.skins[skinInd];
            } );
        }
    }
}
#endif
