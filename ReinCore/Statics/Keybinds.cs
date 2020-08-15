
namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    using MonoMod.Cil;
    using MonoMod.RuntimeDetour;
    using MonoMod.Utils;

    using Rewired;

    using RoR2;

    using UnityEngine;

    using BF = System.Reflection.BindingFlags;
    using ILPM = MonoMod.Cil.ILPatternMatchingExt;

    public static class KeybindsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        //public static Int32 AddInput()

        //public static Int32 AddInput( String name )
        //{

        //}






        static KeybindsCore()
        {
            //Log.Warning("KeybindsCore loaded");
            HooksCore.RoR2.PlayerCharacterMasterController.FixedUpdate.Il += FixedUpdate_Il;
            HooksCore.RoR2.RoR2Application.OnLoad.Il += OnLoad_Il;

            var t = typeof(InputManager_Base);
            var method = t.GetMethod("Awake", BF.NonPublic|BF.Public|BF.Instance);
            var cfg = new ILHookConfig
            {
                ManualApply = true
            };
            awakeHook = new ILHook(method, Awake_IL, ref cfg);


            //Log.Warning("KeybindsCore loaded");
            loaded = true;
        }

        private static void Awake_IL(ILContext il) => _ = new ILCursor(il)
            .Ret_();

        private static void OnLoad_Il(ILContext il) => _ = new ILCursor(il)
            .GotoNext(MoveType.After, x => x.MatchLdstr("Prefabs/Rewired Input Manager"))
            .GotoNext(MoveType.Before, x => x.MatchPop())
            .Move(-1)
            .Call_(typeof(KeybindsCore).GetMethod(nameof(AddAwakeHook), BF.NonPublic | BF.Static))
            .Move(1)
            .Call_(typeof(KeybindsCore).GetMethod(nameof(RemoveAwakeHook), BF.NonPublic | BF.Static))
            .Remove()
            .StSFld_(typeof(KeybindsCore).GetField(nameof(rwInputManagerObj), BF.NonPublic | BF.Static))
            .LdC_(1)
            .StSFld_(typeof(KeybindsCore).GetField(nameof(objFound), BF.NonPublic | BF.Static))
            .Call_(typeof(KeybindsCore).GetMethod(nameof(OnObjFound), BF.NonPublic | BF.Static))
            .StSFld_(typeof(KeybindsCore).GetField(nameof(rwInputManager), BF.NonPublic | BF.Static));


        private static readonly GameObject rwInputManagerObj;
        private static readonly InputManager rwInputManager;
        private static readonly Boolean objFound = false;
        private static InputManager OnObjFound()
        {
            Log.Warning("Input object created");
            var manager = rwInputManagerObj.GetComponent<InputManager>();
            // TODO: Modify the array here

            manager.Awake();
            return manager;
        }
        private static readonly ILHook awakeHook;
        private static void AddAwakeHook() => awakeHook.Apply();
        private static void RemoveAwakeHook() => awakeHook.Undo();


        private static readonly GameObject gamepadSettingPrefab;
        private static readonly GameObject mkbSettingPrefab;


        public readonly struct InputInfo
        {
            internal readonly String name;
            internal readonly Int32 id;
            internal readonly Int32 buttonId;

            internal void Deconstruct(out String name, out Int32 id, out Int32 buttonId)
            {
                name = this.name;
                id = this.id;
                buttonId = this.buttonId;
            }
        }



        private static readonly List<InputInfo> inputInfos = new List<InputInfo>();
        private static event Action<Int32> onCountChanged;

        internal static Int32 GetInputsCount(Action<Int32> onCountChanged)
        {
            KeybindsCore.onCountChanged += onCountChanged;
            return inputInfos.Count;
        }

        private static void FixedUpdate_Il(MonoMod.Cil.ILContext il)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static void EmittedFunc(Boolean canSendInputs, Rewired.Player player, InputBankTest inputs_Uncast)
            {
                if(player is null || !(inputs_Uncast is XInputBank inputs && inputs)) return;
                foreach(var (name, id, buttonId) in inputInfos)
                {
                    inputs[id].PushState(canSendInputs && player.GetButton(buttonId));
                }
            }

            ILLabel loc = null;
            Int32 localInd = 0;
            _ = new ILCursor(il)
                .GotoNext
                (
                    x => x.MatchCall(typeof(PlayerCharacterMasterController).GetMethod("CanSendBodyInput", BF.Public | BF.NonPublic | BF.Static)),
                    x => x.MatchBrfalse(out loc),
                    x => x.MatchLdcI4(out _),
                    x => x.MatchStloc(out localInd)
                )
                .GotoLabel(loc)
                .Move(-1)
                .LdC_(1)
                .StLoc_(localInd)
                .GotoLabel(loc, MoveType.AfterLabel)
                .LdLoc_(localInd)
                .LdLoc_(10)
                .LdArg_(0)
                .LdFld_(typeof(PlayerCharacterMasterController).GetField("bodyInputs", BF.Public | BF.NonPublic | BF.Instance))
                .Calli_<Action<Boolean, Rewired.Player, InputBankTest>>(EmittedFunc);
        }
    }
}