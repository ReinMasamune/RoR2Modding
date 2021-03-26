namespace Rein.Sniper.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;

    using Rein.Sniper.Components;
    using Rein.Sniper.ScriptableObjects;

    using UnityEngine;

    using BF = System.Reflection.BindingFlags;
    using Rein.Sniper.Ammo;
    using System.Linq;

    internal static class HooksModule
    {
        

        internal static void Remove()
        {
            //HooksCore.RoR2.UI.LoadoutPanelController.Row.FromSkillSlot.Il -= FromSkillSlot_Il;
            //HooksCore.RoR2.SkillLocator.FindSkillSlot.Il -= FindSkillSlot_Il;
            HooksCore.RoR2.CameraRigController.Start.On -= Start_On;
            HooksCore.RoR2.CameraTargetParams.Update.Il -= Update_Il;
            //HooksCore.RoR2.CharacterBody.FixedUpdate.On -= FixedUpdate_On;
        }
        internal static void Add()
        {
            //HooksCore.RoR2.UI.LoadoutPanelController.Row.FromSkillSlot.Il += FromSkillSlot_Il;
            //HooksCore.RoR2.SkillLocator.FindSkillSlot.Il += FindSkillSlot_Il;
            HooksCore.RoR2.CameraRigController.Start.On += Start_On;
            HooksCore.RoR2.CameraTargetParams.Update.Il += Update_Il;
            //HooksCore.RoR2.CharacterBody.FixedUpdate.On += FixedUpdate_On;
        }

        private static void FixedUpdate_On(HooksCore.RoR2.CharacterBody.FixedUpdate.Orig orig, CharacterBody self)
        {
            var dt = Time.fixedDeltaTime;
            try { orig(self); } catch { }
            if(self.hasEffectiveAuthority)
            {
                var maxHp = self.maxHealth;
                var bcount = self.GetBuffCount(CatalogModule.sporeHealBuff);
                if(bcount > 0)
                {
                    self.healthComponent.Heal(bcount * maxHp * dt * SporeContext.healPercentBase, default);
                }
                var cbcount = self.GetBuffCount(CatalogModule.critSporeHealBuff);
                if(cbcount > 0)
                {
                    var ogch = self.critHeal;
                    self.critHeal = 100f;
                    self.healthComponent.Heal(cbcount * maxHp * dt * SporeContext.healPercentBase, default);
                    self.critHeal = ogch;
                }
            }
        }

        private static void Update_Il(ILContext il)
        {
            var c = new ILCursor(il);
            Int32 vecLocation = 1;
            var f1 = typeof(SniperCameraParams).GetField(nameof(SniperCameraParams.standardDamp), BF.Public | BF.NonPublic | BF.Instance );
            var f2 = typeof(SniperCameraParams).GetField(nameof(SniperCameraParams.scopeDamp), BF.Public | BF.NonPublic | BF.Instance );
            var f3 = typeof(SniperCameraParams).GetField(nameof(SniperCameraParams.sprintDamp), BF.Public | BF.NonPublic | BF.Instance );
            var camParamsField = typeof(CameraTargetParams).GetField(nameof(CameraTargetParams.cameraParams), BF.Public | BF.NonPublic | BF.Instance );
            FieldInfo SelectField(Single value) => value switch
            {
                0.5f => f1,
                0.4f => f2,
                1f => f3,
                _ => f1,
            };
            var c2 = c.Clone();
            var c3 = c.Clone();
            Single baseValue = 0.0f;
            var smoothDampMethod = typeof(Vector3).GetMethod("SmoothDamp", BF.Public | BF.Static, null, new[] { typeof(Vector3), typeof(Vector3), typeof(Vector3).MakeByRefType(), typeof(Single) }, null );
            while(c2.TryGotoNext(MoveType.AfterLabel,
                x => x.MatchLdcR4(out baseValue),
                x => x.MatchCall(smoothDampMethod)))
            {
                _ = c2.Clone()
                    .Mark(out var defLab)
                    .Move(1)
                    .Mark(out var skipLab);
                _ = c2
                    .MoveBeforeLabels()
                    .LdArg_(0).LdFld_(camParamsField)
                    .Is_<SniperCameraParams>()
                    .BrFalse_(defLab)
                    .LdArg_(0)
                    .LdFld_(camParamsField)
                    .LdFld_(SelectField(baseValue))
                    .Br_(skipLab)
                    .Move(1);
            }
            var camVertDamp = typeof(SniperCameraParams).GetField( nameof(SniperCameraParams.vertDamp), BF.Public | BF.NonPublic | BF.Instance );
            var singleSmoothDamp = typeof(Mathf).GetMethod( "SmoothDamp", BF.Public | BF.Static, null, new[] { typeof(Single), typeof(Single), typeof(Single).MakeByRefType(), typeof(Single) }, null );
            while(c3.TryGotoNext(MoveType.AfterLabel,
                x => x.MatchLdcR4(0.5f),
                x => x.MatchCall(singleSmoothDamp)))
            {
                _ = c3.Clone()
                    .Mark(out var defLab)
                    .Move(1)
                    .Mark(out var skipLab);
                _ = c3
                    .MoveBeforeLabels()
                    .LdArg_(0).LdFld_(camParamsField)
                    .Is_<SniperCameraParams>()
                    .BrFalse_(defLab)
                    .LdArg_(0)
                    .LdFld_(camParamsField)
                    .LdFld_(camVertDamp)
                    .Br_(skipLab)
                    .Move(1);
            }
            _ = c.GotoNext(MoveType.After,
                    x => x.MatchLdcR4(-8f),
                    x => x.MatchStfld<Vector3>(nameof(Vector3.z)),
                    x => x.MatchLdloca(out Int32 vecLocation),
                    x => x.MatchLdcR4(1f),
                    x => x.MatchStfld<Vector3>(nameof(Vector3.y)),
                    x => x.MatchLdloca(vecLocation),
                    x => x.MatchLdcR4(1f),
                    x => x.MatchStfld<Vector3>(nameof(Vector3.x))
                ).Mark(out ILLabel end)
                .GotoPrev(MoveType.AfterLabel,
                    x => x.MatchLdfld<CharacterCameraParams>(nameof(CharacterCameraParams.standardLocalCameraPos))
                ).Mark(out ILLabel standard)
                .GotoPrev(MoveType.After,
                    x => x.MatchLdarg(0),
                    x => x.MatchLdfld<CameraTargetParams>(nameof(CameraTargetParams.cameraParams))
                ).Dup_()
                .Is_<SniperCameraParams>()
                .BrFalse_(standard)
                .LdFld_(typeof(SniperCameraParams).GetField(nameof(SniperCameraParams.throwLocalCameraPos), BF.Instance | BF.NonPublic))
                .StLoc_((UInt16)vecLocation)
                .Br_(end);
        }

        // FUTURE: Refactor into Core for Hud edits
        private static void Start_On(HooksCore.RoR2.CameraRigController.Start.Orig orig, CameraRigController self)
        {
            orig(self);

            _ = self.sceneCam.gameObject.AddComponent<ScopeOverlayShaderController>();

            if(self.hud == null)
            {
                return;
            }

            GameObject reloadBar = UIModule.GetRelodBar();


            //LogTransform(self.hud.transform, 0);

            Transform par = self.hud.transform.Find( "MainContainer/MainUIArea/SpringCanvas/BottomCenterCluster" );
            var barTrans = reloadBar.transform as RectTransform;
            barTrans.SetParent(par, false);
            barTrans.localPosition = new Vector3(0f, 256f, 0f);
            barTrans.localScale = new Vector3(0.5f, 0.5f, 1f);

            //LogTransform(self.hud.transform, 0);
        }

        private static void LogTransform(Transform par, Int32 tabs)
        {
            Log.Message($"{String.Join("", Enumerable.Repeat(" | ", tabs))}{par.name}");
            foreach(Transform t in par)
            {
                LogTransform(t, tabs + 1);
            }
        }

        internal static void AddReturnoverride(GenericSkill skill) => slotReturnOverrides[skill] = (SkillSlot)counter++;
        private static Int32 counter = (Int32)SkillSlot.Special + 1;
        private static readonly Dictionary<GenericSkill,SkillSlot> slotReturnOverrides = new Dictionary<GenericSkill, SkillSlot>();
        private static void FindSkillSlot_Il(ILContext il) => new ILCursor(il)
            .DefLabel(out var label)
            .GotoNext(x => x.MatchBrtrue(out label))
            .GotoLabel(label)
            .Nop_()
            .Mark(out var newLabel)
            .GotoLabel(label)
            .LdArg_(1)
            .CallDel_<Func<GenericSkill, Boolean>>((skill) => slotReturnOverrides.ContainsKey(skill))
            .BrFalse_(newLabel)
            .LdArg_(1)
            .CallDel_<Func<GenericSkill, SkillSlot>>((skill) => slotReturnOverrides[skill])
            .Ret_();

        //private static void FromSkillSlot_Il(ILContext il)
        //{
        //    ILLabel endSwitchLabel = null, newLabel1 = null, newLabel2 = null;
        //    ILLabel[] switchLabels = null;
        //    ILCursor c,c2;
        //    _ = (c = (c2 = new ILCursor(il)).Clone())
        //        .GotoNext(x => x.MatchStloc(3), x => x.MatchBr(out endSwitchLabel))
        //        .GotoPrev(MoveType.Before, x => x.MatchSwitch(out switchLabels));


        //    _ = c2.GotoNext(MoveType.Before, x => x.MatchLdstr("LOADOUT_SKILL_MISC"))
        //        .Mark(out newLabel1)
        //        .LdC_(Properties.Tokens.LOADOUT_SNIPER_AMMO)
        //        .StLoc_(2)
        //        .LdC_(0)
        //        .StLoc_(3)
        //        .Emit(OpCodes.Br_S, endSwitchLabel)
        //        .Mark(out newLabel2)
        //        .LdC_(Properties.Tokens.LOADOUT_SNIPER_PASSIVE)
        //        .StLoc_(2)
        //        .LdC_(0)
        //        .StLoc_(3)
        //        .Emit(OpCodes.Br_S, endSwitchLabel);

        //    Array.Resize(ref switchLabels, switchLabels.Length + 2);
        //    switchLabels[switchLabels.Length - 2] = newLabel1;
        //    switchLabels[switchLabels.Length - 1] = newLabel2;

        //    _ = c.Remove()
        //        .Emit(OpCodes.Switch, switchLabels);
        //}
    }
}
