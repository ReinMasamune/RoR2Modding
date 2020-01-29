using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using R2API;
using R2API.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System.Collections;
using UnityEngine.Networking;

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        private static HashSet<EquipmentIndex> gestureBlacklist = new HashSet<EquipmentIndex>();
        private static Dictionary<EquipmentIndex,Single> equipDestroyDelays = new Dictionary<EquipmentIndex, Single>();
        private static Dictionary<GameObject,Coroutine> beingDestroyed = new Dictionary<GameObject, Coroutine>();

        partial void BalanceGesture()
        {
            gestureBlacklist.Add( EquipmentIndex.GoldGat );
            gestureBlacklist.Add( EquipmentIndex.CrippleWard );
            gestureBlacklist.Add( EquipmentIndex.QuestVolatileBattery );

            gestureBlacklist.Add( EquipmentIndex.AffixBlue );
            gestureBlacklist.Add( EquipmentIndex.AffixGold );
            gestureBlacklist.Add( EquipmentIndex.AffixHaunted );
            gestureBlacklist.Add( EquipmentIndex.AffixPoison );
            gestureBlacklist.Add( EquipmentIndex.AffixRed );
            gestureBlacklist.Add( EquipmentIndex.AffixWhite );
            gestureBlacklist.Add( EquipmentIndex.AffixYellow );

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
            IL.RoR2.Inventory.UpdateEquipment -= this.Inventory_UpdateEquipment;
            IL.EntityStates.GoldGat.BaseGoldGatState.FixedUpdate -= this.BaseGoldGatState_FixedUpdate1;
        }
        private void AddGestureFix()
        {
            IL.RoR2.Inventory.UpdateEquipment += this.Inventory_UpdateEquipment;
            RoR2.EquipmentSlot.onServerEquipmentActivated += this.EquipmentSlot_onServerEquipmentActivated;
            IL.EntityStates.GoldGat.BaseGoldGatState.FixedUpdate += this.BaseGoldGatState_FixedUpdate1;
            IL.RoR2.EquipmentSlot.FixedUpdate += this.EquipmentSlot_FixedUpdate;
            IL.RoR2.Inventory.SetActiveEquipmentSlot += this.Inventory_SetActiveEquipmentSlot1;
            IL.RoR2.GenericPickupController.GrantEquipment += this.GenericPickupController_GrantEquipment;
        }

        private void Inventory_SetActiveEquipmentSlot1( ILContext il )
        {
            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.After, x => x.MatchLdarg(1) );
            c.EmitDelegate<Action<Inventory, Byte>>( ( inv, slot ) =>
            {
                if( beingDestroyed.ContainsKey( inv.gameObject ) )
                {
                    TrueDestroyEquipment( inv );
                }
            } );
            c.Emit( OpCodes.Ldarg_0 );
            c.Emit( OpCodes.Ldarg_1 );
        }

        private void GenericPickupController_GrantEquipment( ILContext il )
        {
            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<RoR2.Inventory>( "get_currentEquipmentIndex" ) );
            c.Emit( OpCodes.Ldarg_2 );
            c.EmitDelegate<Func<EquipmentIndex, Inventory, EquipmentIndex>>( ( index, inv ) =>
            {
                if( index == EquipmentIndex.None ) return index;

                var obj = inv.gameObject;
                if( beingDestroyed.ContainsKey( obj ) )
                {
                    TrueDestroyEquipment( inv );

                    return EquipmentIndex.None;
                } else return index;
            } );
        }

        private void EquipmentSlot_FixedUpdate( ILContext il )
        {
            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.After,
                x => x.MatchLdcI4( (Int32)ItemIndex.AutoCastEquipment ),
                x => x.MatchCallOrCallvirt<RoR2.Inventory>( "GetItemCount" )
            );
            c.Emit( OpCodes.Ldarg_0 );
            c.Emit<RoR2.EquipmentSlot>( OpCodes.Call, "get_equipmentIndex" );
            c.EmitDelegate<Func<Int32, EquipmentIndex, Int32>>( ModGestureCount );
        }

        private void BaseGoldGatState_FixedUpdate1( ILContext il )
        {
            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.After,
                x => x.MatchLdcI4( (Int32)ItemIndex.AutoCastEquipment ),
                x => x.MatchCallOrCallvirt<RoR2.Inventory>( "GetItemCount" )
            );
            c.Emit( OpCodes.Pop );
            c.Emit( OpCodes.Ldc_I4_0 );
        }

        private void EquipmentSlot_onServerEquipmentActivated( EquipmentSlot slot, EquipmentIndex equipInd )
        {
            var body = slot.characterBody;
            if( body )
            {
                var inv = body.inventory;
                if( inv )
                {
                    var gestureCount = inv.GetItemCount( ItemIndex.AutoCastEquipment );
                    if( gestureCount > 0 )
                    {
                        if( Util.CheckRoll( 100f * (1f - (Mathf.Pow( 1f - this.gestureBreakChance, gestureCount ))), body.master ) )
                        {
                            DestroyEquipment( inv, equipInd );
                        }
                    }
                }
            }
        }

        private void Inventory_UpdateEquipment( ILContext il )
        {
            ILCursor c = new ILCursor( il );
            c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<UnityEngine.Mathf>( "Pow" ) );
            c.GotoNext( MoveType.After, x => x.MatchLdcR4( 0.5f ) );
            c.Remove();
            c.Emit( OpCodes.Ldc_R4, 0.5f );
        }

        private static Int32 ModGestureCount( Int32 count, EquipmentIndex currentEquipment )
        {
            if( gestureBlacklist.Contains( currentEquipment ) ) return 0; else return count;
        }

        private static void DestroyEquipment( Inventory inv, EquipmentIndex index )
        {
            if( beingDestroyed.ContainsKey( inv.gameObject ) ) return;

            if( !equipDestroyDelays.ContainsKey( index ) ) index = EquipmentIndex.None;

            Util.PlaySound( "Play_item_proc_armorReduction_shatter", inv.gameObject );
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
            Util.PlaySound( "Play_item_proc_armorReduction_shatter", inv.gameObject );
            if( beingDestroyed.ContainsKey( inv.gameObject ) ) beingDestroyed.Remove( inv.gameObject );
        }
    }
}
