namespace ReinGeneralFixes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using BepInEx.Configuration;

    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;

    using UnityEngine;

    internal partial class Main
    {
        private const Single gestureBaseCD = 0.95f;
        private const Single gestureStackCD = 0.95f;
        //private static ConfigEntry<Boolean> gestureBreakEnabled;

        private static readonly HashSet<EquipmentIndex> gestureBlacklist = new HashSet<EquipmentIndex>();
        //private static readonly Dictionary<EquipmentIndex,Single> equipDestroyDelays = new Dictionary<EquipmentIndex, Single>();
        //private static readonly Dictionary<GameObject,Coroutine> beingDestroyed = new Dictionary<GameObject, Coroutine>();

        partial void BalanceGesture()
        {
            //gestureBreakEnabled = base.Config.Bind<Boolean>( "Temporary:", "Gesture break enabled", true, "This is a temporary option until I have time to set up a more consistent change for gesture. This option will not be staying once that is done" );
            _ = gestureBlacklist.Add(EquipmentIndex.GoldGat);
            _ = gestureBlacklist.Add(EquipmentIndex.QuestVolatileBattery);
            //_ = gestureBlacklist.Add( EquipmentIndex.Enigma );

            _ = gestureBlacklist.Add(EquipmentIndex.AffixBlue);
            _ = gestureBlacklist.Add(EquipmentIndex.AffixGold);
            _ = gestureBlacklist.Add(EquipmentIndex.AffixHaunted);
            _ = gestureBlacklist.Add(EquipmentIndex.AffixPoison);
            _ = gestureBlacklist.Add(EquipmentIndex.AffixRed);
            _ = gestureBlacklist.Add(EquipmentIndex.AffixWhite);
            _ = gestureBlacklist.Add(EquipmentIndex.AffixYellow);

            //equipDestroyDelays[EquipmentIndex.None] = 0f;
            //equipDestroyDelays[EquipmentIndex.BFG] = 2f;
            //equipDestroyDelays[EquipmentIndex.BurnNearby] = 8f;
            //equipDestroyDelays[EquipmentIndex.CommandMissile] = 1.5f;
            //equipDestroyDelays[EquipmentIndex.CritOnUse] = 8f;
            //equipDestroyDelays[EquipmentIndex.DroneBackup] = 28f;
            //equipDestroyDelays[EquipmentIndex.FireBallDash] = 10f;
            //equipDestroyDelays[EquipmentIndex.GainArmor] = 5f;
            //equipDestroyDelays[EquipmentIndex.Jetpack] = 15f;
            //equipDestroyDelays[EquipmentIndex.Meteor] = 20f;
            //equipDestroyDelays[EquipmentIndex.Tonic] = 20f;

            this.Enable += this.AddGestureFix;
            this.Disable += this.RemoveGestureFix;
        }
        private void RemoveGestureFix()
        {
            HooksCore.RoR2.Inventory.CalculateEquipmentCooldownScale.Il -= this.CalculateEquipmentCooldownScale_Il;
            HooksCore.EntityStates.GoldGat.BaseGoldGatState.FixedUpdate.Il -= this.FixedUpdate_Il1;
            HooksCore.RoR2.EquipmentSlot.FixedUpdate.Il -= this.FixedUpdate_Il2;
        }
        private void AddGestureFix()
        {
            HooksCore.RoR2.Inventory.CalculateEquipmentCooldownScale.Il += this.CalculateEquipmentCooldownScale_Il;
            HooksCore.EntityStates.GoldGat.BaseGoldGatState.FixedUpdate.Il += this.FixedUpdate_Il1;
            HooksCore.RoR2.EquipmentSlot.FixedUpdate.Il += this.FixedUpdate_Il2;
        }

        private void FixedUpdate_Il2(ILContext il)
        {
            var c = new ILCursor( il );

            _ = c.GotoNext(MoveType.After,
                x => x.MatchLdcI4((Int32)ItemIndex.AutoCastEquipment),
                x => x.MatchCallOrCallvirt<Inventory>("GetItemCount")
            );
            _ = c.Emit(OpCodes.Ldarg_0);
            _ = c.Emit<EquipmentSlot>(OpCodes.Call, "get_equipmentIndex");
            _ = c.EmitDelegate<Func<Int32, EquipmentIndex, Int32>>(ModGestureCount);
        }
        private void FixedUpdate_Il1(ILContext il)
        {
            var c = new ILCursor( il );

            _ = c.GotoNext(MoveType.After,
                x => x.MatchLdcI4((Int32)ItemIndex.AutoCastEquipment),
                x => x.MatchCallOrCallvirt<Inventory>("GetItemCount")
            );
            _ = c.Emit(OpCodes.Pop);
            _ = c.Emit(OpCodes.Ldc_I4_0);
        }
        private void UpdateEquipment_Il(ILContext il)
        {
            var c = new ILCursor( il );
            _ = c.GotoNext(MoveType.After, x => x.MatchCallOrCallvirt<Mathf>("Pow"));
            _ = c.GotoNext(MoveType.After, x => x.MatchLdcR4(0.5f));
            _ = c.Remove();
            _ = c.Emit(OpCodes.Ldc_R4, 0.5f);
        }

        private void CalculateEquipmentCooldownScale_Il(ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.AfterLabel,
                x => x.MatchLdcR4(out _),
                x => x.MatchLdcR4(out _)
            ).RemoveRange(2)
            .LdC_(gestureBaseCD)
            .LdC_(gestureStackCD);

        private static Int32 ModGestureCount(Int32 count, EquipmentIndex currentEquipment) => gestureBlacklist.Contains(currentEquipment) ? 0 : count;
    }
}
