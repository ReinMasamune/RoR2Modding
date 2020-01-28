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
    [R2APISubmoduleDependency(
        nameof( PrefabAPI )
    )]
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Rein.GeneralBalance", "General Balance + Fixes", "2.1.0.32")]
    internal partial class Main : BaseUnityPlugin
    {
        internal Single gestureBreakChance = 0.025f;




        private event Action Load;
        private event Action Enable;
        private event Action Disable;
        private event Action FirstFrame;
        private event Action Frame;
        private event Action PostFrame;
        private event Action Tick;
        private event Action GUI;


        partial void BalanceCommandoCDs();
        partial void QoLCommandoRoll();

        partial void QoLVisionsCrosshair();
        partial void BalanceCorpsebloom();
        partial void FixBandolier();


        partial void BalanceOSP();
        partial void BalanceGesture();

        partial void QoLOvergrownPrinters();

#if DPSMETER
        partial void SetupDPSMeter();
#endif

        private Main()
        {
            this.BalanceCommandoCDs();
            this.QoLCommandoRoll();

            this.QoLVisionsCrosshair();
            this.BalanceCorpsebloom();
            this.FixBandolier();

            this.BalanceOSP();

            this.BalanceGesture();

            this.QoLOvergrownPrinters();





#if DPSMETER
            this.SetupDPSMeter();
#endif
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

Overgrown printers can now have any boss tier item, including fancy pearls and halcyon seeds.



// TODO: Focus crystal + crowbar self damage
// TODO: Add model to spectral circlet





ITEMS:


Fireworks
Gasoline
Medkit
Monster Tooth
Stun Grenade
Warbanner

Berzerkers
Chronobauble
Razor Wire
Will o Wisp

Ceremonial Dagger
Happiest Mask
Resonance Disk
Tesla Coil

Queens Gland

Blast Shower
Royal Capacitor
Crowdfunder

Effigy
Tincture

*/