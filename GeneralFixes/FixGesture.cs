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

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        partial void FixGesture()
        {
            this.Enable += this.AddGestureFix;
            this.Disable += this.RemoveGestureFix;
        }

        private void RemoveGestureFix()
        {
            RoR2.EquipmentSlot.onServerEquipmentActivated -= this.EquipmentSlot_onServerEquipmentActivated;
            IL.RoR2.Inventory.UpdateEquipment -= this.Inventory_UpdateEquipment;
        }
        private void AddGestureFix()
        {
            IL.RoR2.Inventory.UpdateEquipment += this.Inventory_UpdateEquipment;
            RoR2.EquipmentSlot.onServerEquipmentActivated += this.EquipmentSlot_onServerEquipmentActivated;
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
                            inv.SetEquipmentIndex( EquipmentIndex.None );
                            Util.PlaySound( "Play_item_proc_armorReduction_shatter", slot.gameObject );
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
    }
}
