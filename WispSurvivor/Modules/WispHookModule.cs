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
using MonoMod.RuntimeDetour;

namespace WispSurvivor
{
    public partial class WispSurvivorMain
    {
        public override void RemoveHooks()
        {
            IL.RoR2.GlobalEventManager.OnHitEnemy -= this.RemoveStunOnHit;
            On.RoR2.GlobalEventManager.OnHitEnemy -= this.AddStunOnHit;
            On.RoR2.CharacterBody.Start -= this.AddBurnManager;
            On.RoR2.CharacterBody.RecalculateStats -= this.ArmorBoost;
            IL.RoR2.CharacterBody.RecalculateStats -= this.ModifyMSBoost;
            On.RoR2.CharacterBody.FixedUpdate -= this.ShieldRegenStuff;
            On.RoR2.CharacterModel.InstanceUpdate -= this.CharacterModel_InstanceUpdate;
            On.RoR2.CharacterModel.UpdateOverlays -= this.CharacterModel_UpdateOverlays;
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
            On.RoR2.CharacterModel.UpdateOverlays += this.CharacterModel_UpdateOverlays;
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

        private void CharacterModel_UpdateOverlays( On.RoR2.CharacterModel.orig_UpdateOverlays orig, CharacterModel self )
        {
            orig( self );
            if( self.body && self.body.baseNameToken == "WISP_SURVIVOR_BODY_NAME" )
            {
                Material[] mats = self.GetFieldValue<Material[]>("currentOverlays" );
                uint skin = self.body.skinIndex;
                for( Int32 i = 0; i < mats.Length; i++ )
                {
                    if( mats[i] == CharacterModel.energyShieldMaterial )
                    {
                        mats[i] = WispMaterialModule.shieldOverlayMaterials[skin];
                    }// else if( mats[i] == CharacterModel.material)
                }
            }
        }

    }
}
