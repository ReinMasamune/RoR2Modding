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
        private static HashSet<EquipmentIndex> gestureBlacklist = new HashSet<EquipmentIndex>();
        private static Dictionary<EquipmentIndex,Single> equipDestroyDelays = new Dictionary<EquipmentIndex, Single>();
        private static Dictionary<GameObject,Coroutine> beingDestroyed = new Dictionary<GameObject, Coroutine>();

        partial void BalanceGesture()
        {
            gestureBlacklist.Add( EquipmentIndex.GoldGat );
            gestureBlacklist.Add( EquipmentIndex.CrippleWard );
            gestureBlacklist.Add( EquipmentIndex.QuestVolatileBattery );
            gestureBlacklist.Add( EquipmentIndex.Enigma );

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
        private void SetActiveEquipmentSlot_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.After, x => x.MatchLdarg( 1 ) );
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
        private void FixedUpdate_Il2( ILContext il )
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
        private void FixedUpdate_Il1( ILContext il )
        {
            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.After,
                x => x.MatchLdcI4( (Int32)ItemIndex.AutoCastEquipment ),
                x => x.MatchCallOrCallvirt<RoR2.Inventory>( "GetItemCount" )
            );
            c.Emit( OpCodes.Pop );
            c.Emit( OpCodes.Ldc_I4_0 );
        }
        private void UpdateEquipment_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );
            base.Logger.LogWarning( "Check1" );
            c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<UnityEngine.Mathf>( "Pow" ) );
            base.Logger.LogWarning( "Check2" );
            c.GotoNext( MoveType.After, x => x.MatchLdcR4( 0.5f ) );
            c.Remove();
            c.Emit( OpCodes.Ldc_R4, 0.5f );
        }

        private void CalculateEquipmentCooldownScale_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );
            c.GotoNext( MoveType.Before, x => x.MatchLdcR4( 0.5f ), x => x.MatchLdcR4( 0.85f ) );
            c.Index++;
            c.Remove();
            c.Emit( OpCodes.Ldc_R4, 0.5f );
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
                    if( gestureCount > 0 && !RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.enigmaArtifactDef) )
                    {
                        if( Util.CheckRoll( 100f * (1f - (Mathf.Pow( 1f - this.gestureBreakChance, gestureCount ))), body.master ) )
                        {
                            DestroyEquipment( inv, equipInd );
                        }
                    }
                }
            }
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
