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
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Rein.GeneralBalance", "General Balance + Fixes", "1.0.0")]
    internal partial class Main : BaseUnityPlugin
    {
        internal Single gestureBreakChance = 0.1f;














        private event Action Load;
        private event Action Enable;
        private event Action Disable;
        private event Action FirstFrame;
        private event Action Frame;
        private event Action PostFrame;
        private event Action Tick;
        private event Action GUI;

        partial void DisableOPItems();
        partial void DisableUPItems();

        partial void EditCommandoCDs();
        partial void EditCommandoRoll();

        partial void EditVisionsCrosshair();


        partial void FixOSP();
        partial void FixGesture();
        partial void FixTesla();
        partial void FixRazorWire();
        partial void FixResDisk();


        private Main()
        {
            this.DisableOPItems();
            this.DisableUPItems();


            this.EditCommandoCDs();
            this.EditCommandoRoll();

            this.EditVisionsCrosshair();

            this.FixOSP();

            this.FixGesture();
            this.FixTesla();
            this.FixRazorWire();
            this.FixResDisk();
        }

#pragma warning disable IDE0051 // Remove unused private members
        private void Awake() => this.Load?.Invoke();
        private void Start() => this.FirstFrame?.Invoke();
        private void OnEnable() => this.Enable?.Invoke();
        private void OnDisable() => this.Disable?.Invoke();
        private void Update() => this.Frame?.Invoke();
        private void LateUpdate() => this.PostFrame?.Invoke();
        private void FixedUpdate() => this.Tick?.Invoke();
        private void OnGUI() => this.GUI?.Invoke();
#pragma warning restore IDE0051 // Remove unused private members
    }
}
/*
Changelist:
Commando Phase round 2s cooldown, phase blast 3s cooldown
Commando dodge roll now counts as sprinting
// TODO: Commando roll nosprint false
// TODO: Commando roll movespeed mult down

OSP now is applied after shields and barrier.
OSP can now no longer block more than 180% of your max health in damage from any single hit.

// TODO: Disable OP items
// TODO: Disable UP items


Gesture now has chance of breaking equipment on use.
Gesture now reduces cooldown by 50% per stack.

Visions now gives huntress and mercenary commandos crosshair to make aiming possible.













*/