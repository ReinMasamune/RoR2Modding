using System;
using System.Collections.Generic;
using BepInEx;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System.Linq;

namespace ReinCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class DeployablesCore
    {
        /// <summary>
        /// 
        /// </summary>
        public static Boolean loaded { get; internal set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        public static DeployableSlot AddDeployableSlot(DeployableSlotDef def)
        {
            if( !loaded ) throw new CoreNotLoadedException( nameof( DeployablesCore ) );
            if( def is null ) throw new ArgumentNullException( nameof( def ) );

            DeployableSlot slot = nextSlot;
            addedDeployables[slot] = def;
            def.slot = slot;
            return slot;
        }

        static DeployablesCore()
        {
            startingIndex = EnumExtensions.GetMax<DeployableSlot>();

            HooksCore.RoR2.CharacterMaster.GetDeployableSameSlotLimit.Il += GetDeployableSameSlotLimit_Il;

            loaded = true;
        }

        private static DeployableSlot startingIndex;

        private static DeployableSlot nextSlot
        {
            get => ++startingIndex;
        }


        private static readonly Dictionary<DeployableSlot,DeployableSlotDef> addedDeployables = new Dictionary<DeployableSlot, DeployableSlotDef>();
        private static void GetDeployableSameSlotLimit_Il( ILContext il )
        {
            ILLabel endLabel = null;
            var cursor = new ILCursor( il );
            _ = cursor.GotoNext( MoveType.Before,
                instr => instr.MatchBr( out endLabel ),
                insrt => insrt.MatchLdcI4( 4 )
            );
            _ = cursor.Emit( OpCodes.Nop );
            cursor.Index--;
            ILLabel breakLabel = cursor.MarkLabel();
            _ = cursor.GotoPrev( MoveType.After,
                instr => instr.MatchSwitch( out _ )
            );

            Int32 loc1 = cursor.EmitReference<Dictionary<DeployableSlot, DeployableSlotDef>>( addedDeployables );
            _ = cursor.Emit( OpCodes.Ldarg_1 );
            _ = cursor.Emit<Dictionary<DeployableSlot, DeployableSlotDef>>( OpCodes.Callvirt, "ContainsKey" );
            _ = cursor.Emit( OpCodes.Brfalse, breakLabel );
            cursor.EmitGetReference<Dictionary<DeployableSlot, DeployableSlotDef>>( loc1 );
            _ = cursor.Emit( OpCodes.Ldarg_1 );
            _ = cursor.Emit<Dictionary<DeployableSlot, DeployableSlotDef>>( OpCodes.Callvirt, "get_Item" );
            _ = cursor.Emit<DeployableSlotDef>( OpCodes.Callvirt, "get_GetLimit" );
            _ = cursor.Emit( OpCodes.Ldarg_0 );
            _ = cursor.Emit<DeployableSlotLimitDelegate>( OpCodes.Callvirt, "Invoke" );
            _ = cursor.Emit( OpCodes.Stloc_0 );
            _ = cursor.Emit( OpCodes.Br, endLabel );
        }
    }
}
