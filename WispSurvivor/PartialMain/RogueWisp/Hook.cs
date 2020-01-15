using BepInEx;
using R2API.Utils;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers;
using RogueWispPlugin.Modules;
//using static RogueWispPlugin.Helpers.APIInterface;
using MonoMod.Cil;
using System.Collections;
using UnityEngine.Networking;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        const Single shieldRegenFrac = 0.002f;
        partial void RW_Hook()
        {
            this.Enable += this.RW_AddHooks;
            this.Disable += this.RW_RemoveHooks;
        }

        private void RW_RemoveHooks()
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
        private void RW_AddHooks()
        {
            IL.RoR2.GlobalEventManager.OnHitEnemy += this.RemoveStunOnHit;
            On.RoR2.GlobalEventManager.OnHitEnemy += this.AddStunOnHit;
            On.RoR2.CharacterBody.Start += this.AddBurnManager;
            On.RoR2.CharacterBody.RecalculateStats += this.ArmorBoost;
            IL.RoR2.CharacterBody.RecalculateStats += this.ModifyMSBoost;
            On.RoR2.CharacterBody.FixedUpdate += this.ShieldRegenStuff;
            On.RoR2.CharacterModel.InstanceUpdate += this.CharacterModel_InstanceUpdate;
            On.RoR2.CharacterModel.UpdateOverlays += this.CharacterModel_UpdateOverlays;
            On.RoR2.EffectManager.TransmitEffect += this.DoVeryVeryBadThings;
        }

        private void DoVeryVeryBadThings( On.RoR2.EffectManager.orig_TransmitEffect orig, EffectIndex effectPrefabIndex, EffectData effectData, NetworkConnection netOrigin )
        {
            orig( effectPrefabIndex, effectData, netOrigin );

            if( NetworkServer.active && CheckIndex( (UInt32)effectPrefabIndex, this.restoreIndex ) && (effectData.color.r != 0 || effectData.color.g != 0 || effectData.color.b != 0 || effectData.color.a != 0) )
            {
                GameObject temp = effectData.ResolveHurtBoxReference();
                if( temp )
                {
                    CharacterBody comp = temp.GetComponent<CharacterBody>();
                    if( comp && comp.baseNameToken == "WISP_SURVIVOR_BODY_NAME" )
                    {
                        Single value = HeatwaveClientOrb.DoMoreBadThings(effectData.color);
                        if( value > 0f )
                        {
                            this.StartCoroutine( this.DelayBuff( this.RW_flameChargeBuff, comp, value, effectData.genericUInt, effectData.genericFloat ) );
                        }
                    }
                }
            }
        }

        private IEnumerator DelayBuff( BuffIndex buff, CharacterBody body, Single duration, UInt32 stacks, Single delay )
        {
            yield return new WaitForSeconds( delay );

            for( UInt32 i = 0; i < stacks; i++ )
            {
                body.AddTimedBuff( buff, duration );
            }
        }

        private static Boolean CheckIndex( UInt32 index, UInt32[] indicies )
        {
            for( Int32 i = 0; i < indicies.Length; i++ )
            {
                if( index == indicies[i] ) return true;
            }
            return false;
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
            CharacterBody body = info.attacker.GetComponent<CharacterBody>();
            if( !body ) return;
            Inventory inventory = body.inventory;
            if( !inventory ) return;
            Int32 stunCount = inventory.GetItemCount(ItemIndex.StunChanceOnHit);
            if( stunCount <= 0 ) return;
            Single sqCoef = Mathf.Sqrt(info.procCoefficient);
            if( !RoR2.Util.CheckRoll( RoR2.Util.ConvertAmplificationPercentageIntoReductionPercentage( sqCoef * 5f * stunCount ), body.master ) ) return;
            SetStateOnHurt stateOnHurt = victim.GetComponent<SetStateOnHurt>();
            if( !stateOnHurt ) return;
            stateOnHurt.SetStun( sqCoef * 2f );
        }

        private void AddBurnManager( On.RoR2.CharacterBody.orig_Start orig, CharacterBody self )
        {
            orig( self );
            self.gameObject.AddComponent<WispBurnManager>();
        }

        private void ArmorBoost( On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self )
        {
            orig( self );
            if( self && self.inventory )
            {
                if( self.HasBuff( this.RW_armorBuff ) )
                {
                    self.SetPropertyValue<Single>( "armor", self.armor + 50f );
                }
            }
        }

        private void ModifyMSBoost( ILContext il )
        {
            ILCursor c = new ILCursor(il);
            c.GotoNext( MoveType.After,
                x => x.MatchLdarg( 0 ),
                x => x.MatchLdcI4( 6 ),
                x => x.MatchCallOrCallvirt<RoR2.CharacterBody>( "HasBuff" ),
                x => x.MatchBrfalse( out _ ),
                x => x.MatchLdloc( 47 )
                );

            c.Remove();
            c.Emit( Mono.Cecil.Cil.OpCodes.Ldc_R4, 0.25f );
        }

        private void ShieldRegenStuff( On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self )
        {
            orig( self );
            if( NetworkServer.active )
            {
                Int32 count = self.GetBuffCount( this.RW_flameChargeBuff );
                if( count > 0 )
                {
                    self.healthComponent.RechargeShield( Time.fixedDeltaTime * shieldRegenFrac * count * self.maxHealth );
                }
            }
        }

        private void CharacterModel_InstanceUpdate( On.RoR2.CharacterModel.orig_InstanceUpdate orig, CharacterModel self )
        {
            if( self.body && self.body.baseNameToken == "WISP_SURVIVOR_BODY_NAME" )
            {
                EliteIndex eliteInd = self.GetFieldValue<EliteIndex>( "myEliteIndex");
                if( eliteInd == EliteIndex.Poison )
                {
                    self.SetFieldValue<Material>( "particleMaterialOverride", Main.eliteFlameMaterials[self.body.skinIndex][0] );
                } else if( eliteInd == EliteIndex.Haunted )
                {
                    self.SetFieldValue<Material>( "particleMaterialOverride", Main.eliteFlameMaterials[self.body.skinIndex][1] );
                } else
                {
                    self.SetFieldValue<Material>( "particleMaterialOverride", null );
                }

                self.InvokeMethod( "UpdateGoldAffix" );
                self.InvokeMethod( "UpdatePoisonAffix" );
                self.InvokeMethod( "UpdateHauntedAffix" );
                self.InvokeMethod( "UpdateLights" );
            } else
            {
                orig( self );
            }
        }

        private void CharacterModel_UpdateOverlays( On.RoR2.CharacterModel.orig_UpdateOverlays orig, CharacterModel self )
        {
            orig( self );
            if( self.body && self.body.baseNameToken == "WISP_SURVIVOR_BODY_NAME" )
            {
                Material[] mats = self.GetFieldValue<Material[]>("currentOverlays" );
                UInt32 skin = self.body.skinIndex;
                for( Int32 i = 0; i < mats.Length; i++ )
                {
                    if( mats[i] == CharacterModel.energyShieldMaterial )
                    {
                        mats[i] = Main.shieldOverlayMaterials[skin];
                    }
                }
            }
        }
    }
#endif
}
