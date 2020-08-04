
namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    using MonoMod.Cil;
    using MonoMod.Utils;

    using Rewired;

    using RoR2;

    using UnityEngine;

    using BF = System.Reflection.BindingFlags;
    using ILPM = MonoMod.Cil.ILPatternMatchingExt;

    public static class KeybindsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        //public static Int32 AddInput( String name )
        //{

        //}






        static KeybindsCore()
        {
            Log.Warning( "KeybindsCore loaded" );
            HooksCore.RoR2.PlayerCharacterMasterController.FixedUpdate.Il += FixedUpdate_Il;



            Log.Warning( "KeybindsCore loaded" );
            loaded = true;
        }


        public readonly struct InputInfo
        {
            internal readonly String name;
            internal readonly Int32 id;
            internal readonly Int32 buttonId;

            internal void Deconstruct( out String name, out Int32 id, out Int32 buttonId )
            {
                name = this.name;
                id = this.id;
                buttonId = this.buttonId;
            }
        }

        private delegate InputAction CreateActionDelegate( Int32 id, String name, InputActionType type, String descriptiveName, String positiveDescriptiveName );
        private static CreateActionDelegate ___createActionDelegate;
        private static CreateActionDelegate CreateInputAction { get => ___createActionDelegate ??= GenerateCreateInputAction(); }
        private static CreateActionDelegate GenerateCreateInputAction()
        {
            var dmd = new DynamicMethodDefinition( "CreateInputAction>>>>>", typeof(InputAction), new[]
            {
                typeof(Int32), typeof(String), typeof(InputActionType), typeof(String), typeof(String), typeof(String), typeof(Int32), typeof(Boolean),
                typeof(Int32),
            });
                return null;
        }

        private static readonly List<InputInfo> inputInfos = new List<InputInfo>();
        private static event Action<Int32> onCountChanged;

        internal static Int32 GetInputsCount( Action<Int32> onCountChanged )
        {
            KeybindsCore.onCountChanged += onCountChanged;
            return inputInfos.Count;
        }

        private static void FixedUpdate_Il( MonoMod.Cil.ILContext il )
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static void EmittedFunc( Boolean canSendInputs, Rewired.Player player, InputBankTest inputs_Uncast )
            {
                if( player is null || !(inputs_Uncast is XInputBank inputs && inputs) ) return;
                foreach( var (name, id, buttonId) in inputInfos )
                {
                    inputs[id].PushState( canSendInputs && player.GetButton( buttonId ) );
                }
            }

            ILLabel loc = null;
            Int32 localInd = 0;
            _ = new ILCursor( il )
                .GotoNext
                (
                    x => x.MatchCall( typeof( PlayerCharacterMasterController ).GetMethod( "CanSendBodyInput", BF.Public | BF.NonPublic | BF.Static ) ),
                    x => x.MatchBrfalse( out loc ),
                    x => x.MatchLdcI4( out _ ),
                    x => x.MatchStloc( out localInd )
                )
                .GotoLabel( loc )
                .Move( -1 )
                .LdC_( 1 )
                .StLoc_( localInd )
                .GotoLabel( loc, MoveType.AfterLabel )
                .LdLoc_( localInd )
                .LdLoc_( 10 )
                .LdArg_( 0 )
                .LdFld_( typeof(PlayerCharacterMasterController).GetField( "bodyInputs", BF.Public | BF.NonPublic | BF.Instance ) )
                .Calli_<Action<Boolean,Rewired.Player,InputBankTest>>( EmittedFunc );
        }
    }
}