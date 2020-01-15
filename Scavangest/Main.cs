namespace Scavangest
{
    using BepInEx;
    using R2API;
    using R2API.Utils;
    using System;
    using UnityEngine;

    [R2APISubmoduleDependency(
        nameof( LoadoutAPI ),
        nameof( SurvivorAPI ),
        nameof( EffectAPI ),
        nameof( PrefabAPI )
    )]
    [BepInDependency( R2API.PluginGUID, BepInDependency.DependencyFlags.HardDependency )]
    [BepInPlugin(guid, name, ver)]
    internal partial class Main : BaseUnityPlugin
    {
        const String guid = "com.Rein.Scavangest";
        const String name = "Scavangest";
        const String ver = "1.0.0.0";


        private event Action Load;
        private event Action Enable;
        private event Action Disable;
        private event Action FirstFrame;
        private event Action Frame;
        private event Action PostFrame;
        private event Action Tick;
        private event Action GUI;

        partial void Test();

        partial void BodySetup();
        partial void ModelEdits();
        partial void SkillEdits();


        internal Main()
        {
            this.Test();
            this.BodySetup();
            this.ModelEdits();
            this.SkillEdits();
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






        private GameObject body;


    }
}
