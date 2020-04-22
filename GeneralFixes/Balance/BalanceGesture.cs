using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System.Collections;
using UnityEngine.Networking;
using ReinCore;

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        private static readonly HashSet<EquipmentIndex> gestureBlacklist = new HashSet<EquipmentIndex>();
        private static readonly Dictionary<EquipmentIndex,Single> equipDestroyDelays = new Dictionary<EquipmentIndex, Single>();
        private static readonly Dictionary<GameObject,Coroutine> beingDestroyed = new Dictionary<GameObject, Coroutine>();

        partial void BalanceGesture()
        {
            _ = gestureBlacklist.Add( EquipmentIndex.GoldGat );
            _ = gestureBlacklist.Add( EquipmentIndex.CrippleWard );
            _ = gestureBlacklist.Add( EquipmentIndex.QuestVolatileBattery );
            _ = gestureBlacklist.Add( EquipmentIndex.Enigma );

            _ = gestureBlacklist.Add( EquipmentIndex.AffixBlue );
            _ = gestureBlacklist.Add( EquipmentIndex.AffixGold );
            _ = gestureBlacklist.Add( EquipmentIndex.AffixHaunted );
            _ = gestureBlacklist.Add( EquipmentIndex.AffixPoison );
            _ = gestureBlacklist.Add( EquipmentIndex.AffixRed );
            _ = gestureBlacklist.Add( EquipmentIndex.AffixWhite );
            _ = gestureBlacklist.Add( EquipmentIndex.AffixYellow );

            equipDestroyDelays[EquipmentIndex.None] = 0f;
            equipDestroyDelays[EquipmentIndex.BFG] = 2f;
            equipDestroyDelays[EquipmentIndex.BurnNearby] = 8f;
            equipDestroyDelays[EquipmentIndex.CommandMissile] = 1.5f;
            equipDestroyDelays[EquipmentIndex.CritOnUse] = 8f;
            equipDestroyDelays[EquipmentIndex.DroneBackup] = 28f;
            equipDestroyDelays[EquipmentIndex.FireBallDash] = 10f;
            equipDestroyDelays[EquipmentIndex.GainArmor] = 5f;
            equipDestroyDelays[EquipmentIndex.Jetpack] = 15f;
            equipDestroyDelays[EquipmentIndex.Meteor] = 20f;
            equipDestroyDelays[EquipmentIndex.Tonic] = 20f;

            this.Enable += this.AddGestureFix;
            this.Disable += this.RemoveGestureFix;
        }

        private void RemoveGestureFix()
        {
            RoR2.EquipmentSlot.onServerEquipmentActivated -= this.EquipmentSlot_onServerEquipmentActivated;

            //HooksCore.RoR2.Inventory.UpdateEquipment.Il -= this.UpdateEquipment_Il;
            HooksCore.EntityStates.GoldGat.BaseGoldGatState.FixedUpdate.Il -= this.FixedUpdate_Il1;
            HooksCore.RoR2.EquipmentSlot.FixedUpdate.Il -= this.FixedUpdate_Il2;
            HooksCore.RoR2.Inventory.SetActiveEquipmentSlot.Il -= this.SetActiveEquipmentSlot_Il;
            HooksCore.RoR2.GenericPickupController.GrantEquipment.Il -= this.GrantEquipment_Il;
        }
        private void AddGestureFix()
        {
            RoR2.EquipmentSlot.onServerEquipmentActivated += this.EquipmentSlot_onServerEquipmentActivated;

            //HooksCore.RoR2.Inventory.UpdateEquipment.Il += this.UpdateEquipment_Il;
            HooksCore.RoR2.Inventory.CalculateEquipmentCooldownScale.Il += this.CalculateEquipmentCooldownScale_Il;
            HooksCore.EntityStates.GoldGat.BaseGoldGatState.FixedUpdate.Il += this.FixedUpdate_Il1;
            HooksCore.RoR2.EquipmentSlot.FixedUpdate.Il += this.FixedUpdate_Il2;
            HooksCore.RoR2.Inventory.SetActiveEquipmentSlot.Il += this.SetActiveEquipmentSlot_Il;
            HooksCore.RoR2.GenericPickupController.GrantEquipment.Il += this.GrantEquipment_Il;
        }



        private void GrantEquipment_Il( ILContext il )
        {
            var c = new ILCursor( il );

            _ = c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<Inventory>( "get_currentEquipmentIndex" ) );
            _ = c.Emit( OpCodes.Ldarg_2 );
            _ = c.EmitDelegate<Func<EquipmentIndex, Inventory, EquipmentIndex>>( ( index, inv ) =>
              {
                  if( index == EquipmentIndex.None )
                      return index;

                  GameObject obj = inv.gameObject;
                  if( beingDestroyed.ContainsKey( obj ) )
                  {
                      TrueDestroyEquipment( inv );

                      return EquipmentIndex.None;
                  }
                  return index;
              } );
        }
        private void SetActiveEquipmentSlot_Il( ILContext il )
        {
            var c = new ILCursor( il );

            _ = c.GotoNext( MoveType.After, x => x.MatchLdarg( 1 ) );
            _ = c.EmitDelegate<Action<Inventory, Byte>>( ( inv, slot ) =>
              {
                  if( beingDestroyed.ContainsKey( inv.gameObject ) )
                  {
                      TrueDestroyEquipment( inv );
                  }
              } );
            _ = c.Emit( OpCodes.Ldarg_0 );
            _ = c.Emit( OpCodes.Ldarg_1 );
        }
        private void FixedUpdate_Il2( ILContext il )
        {
            var c = new ILCursor( il );

            _ = c.GotoNext( MoveType.After,
                x => x.MatchLdcI4( (Int32)ItemIndex.AutoCastEquipment ),
                x => x.MatchCallOrCallvirt<Inventory>( "GetItemCount" )
            );
            _ = c.Emit( OpCodes.Ldarg_0 );
            _ = c.Emit<EquipmentSlot>( OpCodes.Call, "get_equipmentIndex" );
            _ = c.EmitDelegate<Func<Int32, EquipmentIndex, Int32>>( ModGestureCount );
        }
        private void FixedUpdate_Il1( ILContext il )
        {
            var c = new ILCursor( il );

            _ = c.GotoNext( MoveType.After,
                x => x.MatchLdcI4( (Int32)ItemIndex.AutoCastEquipment ),
                x => x.MatchCallOrCallvirt<Inventory>( "GetItemCount" )
            );
            _ = c.Emit( OpCodes.Pop );
            _ = c.Emit( OpCodes.Ldc_I4_0 );
        }
        private void UpdateEquipment_Il( ILContext il )
        {
            var c = new ILCursor( il );
            _ = c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<Mathf>( "Pow" ) );
            _ = c.GotoNext( MoveType.After, x => x.MatchLdcR4( 0.5f ) );
            _ = c.Remove();
            _ = c.Emit( OpCodes.Ldc_R4, 0.5f );
        }

        private void CalculateEquipmentCooldownScale_Il( ILContext il )
        {
            var c = new ILCursor( il );
            _ = c.GotoNext( MoveType.Before, x => x.MatchLdcR4( 0.5f ), x => x.MatchLdcR4( 0.85f ) );
            c.Index++;
            _ = c.Remove();
            _ = c.Emit( OpCodes.Ldc_R4, 0.5f );
        }


        private void EquipmentSlot_onServerEquipmentActivated( EquipmentSlot slot, EquipmentIndex equipInd )
        {
            CharacterBody body = slot.characterBody;
            if( body )
            {
                Inventory inv = body.inventory;
                if( inv )
                {
                    Int32 gestureCount = inv.GetItemCount( ItemIndex.AutoCastEquipment );
                    if( gestureCount > 0 && !RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.enigmaArtifactDef) )
                    {
                        if( Util.CheckRoll( 100f * ( 1f -  Mathf.Pow( 1f - this.gestureBreakChance, gestureCount )  ), body.master ) )
                        {
                            DestroyEquipment( inv, equipInd );
                        }
                    }
                }
            }
        }

        private static Int32 ModGestureCount( Int32 count, EquipmentIndex currentEquipment ) => gestureBlacklist.Contains( currentEquipment ) ? 0 : count;

        private static void DestroyEquipment( Inventory inv, EquipmentIndex index )
        {
            if( beingDestroyed.ContainsKey( inv.gameObject ) ) return;

            if( !equipDestroyDelays.ContainsKey( index ) ) index = EquipmentIndex.None;

            _ = Util.PlaySound( "Play_item_proc_armorReduction_shatter", inv.gameObject );
            beingDestroyed[inv.gameObject] = inv.StartCoroutine( EquipDestroyer( inv, equipDestroyDelays[index] + 0.25f ) );
        }

        private static IEnumerator EquipDestroyer( Inventory inv, Single delay )
        {
            yield return new WaitForSeconds( delay );

            if( beingDestroyed.ContainsKey( inv.gameObject ) )
            {
                TrueDestroyEquipment( inv );
            }
        }

        private static void TrueDestroyEquipment( Inventory inv )
        {
            inv.SetEquipmentIndex( EquipmentIndex.None );
            _ = Util.PlaySound( "Play_item_proc_armorReduction_shatter", inv.gameObject );
            if( beingDestroyed.ContainsKey( inv.gameObject ) )
                _ = beingDestroyed.Remove( inv.gameObject );
        }
    }
}
