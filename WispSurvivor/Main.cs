using BepInEx;
using R2API.Utils;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers;
using RogueWispPlugin.Modules;
using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
    [R2APISubmoduleDependency( 
        nameof( R2API.SurvivorAPI ), 
        nameof( R2API.EffectAPI ), 
        nameof( R2API.SkillAPI ), 
        nameof( R2API.SkinAPI ), 
        nameof( R2API.EntityAPI ), 
        nameof( R2API.PrefabAPI ),
        nameof( R2API.LoadoutAPI ),
        nameof( R2API.OrbAPI ) 
    )]

    [BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.ReinThings.RogueWisp", "Rein-RogueWisp", "1.3.3" )]
    internal partial class Main : BaseUnityPlugin
    {
        public String thing1;
        public String thing2;
        public String thing3;

        private event Action Load;
        private event Action FirstFrame;
        private event Action Enable;
        private event Action Disable;
        private event Action Frame;
        private event Action PostFrame;
        private event Action Tick;
        private event Action GUI;

        partial void CreateRogueWisp();
        partial void CreateAncientWisp();
        partial void CreateFirstWisp();
        partial void CreateWispFriend();
        partial void CreateStage();

        public Main()
        {
            this.thing1 = "Note that everything in this codebase is already part of R2API. I spent a month of my time integrating all of that so that people would stop copy pasting from here.";
            this.thing2 = "If you are truly insistant on taking the lazy way out (Looking at you ravens) then screw you too I guess?";
            this.thing3 = "I also no longer at all interested in answering any questions about my code or helping anyone in any way. If people are just going to copy paste anyway then they aren't worth my time.";
 

            this.Tick += () => RoR2Application.isModded = true;

            this.CreateRogueWisp();
            this.CreateAncientWisp();
            this.CreateFirstWisp();
            this.CreateWispFriend();
            this.CreateStage();
        }

        private void Awake() => this.Load?.Invoke();
        private void Start() => this.FirstFrame?.Invoke();
        private void OnEnable() => this.Enable?.Invoke();
        private void OnDisable() => this.Disable?.Invoke();
        private void Update() => this.Frame?.Invoke();
        private void LateUpdate() => this.PostFrame?.Invoke();
        private void FixedUpdate() => this.Tick?.Invoke();
        private void OnGUI() => this.GUI?.Invoke();
    }


}

//For next release:

// TODO: Primary particles still aren't visible sometimes.
// TODO: Primary needs impact effects and world hit effects.
// TODO: Merge primary into single state again

// TODO: Update Descriptions

// Future plans and shit

// TOD: Utility sounds (unsure what to do here)
// TOD: Little Disciple and Will O Wisp color should change with skin
// TOD: Body Animation smoothing params
// TOD: Rewrite secondary for MP (2 states possibly?)
// TOD: Character lobby description
// TOD: Customize crosshair
// TOD: Readme lore sillyness
// TOD: Effects obscuring vision
// TOD: Effect brightness settings
// TOD: Muzzle flashes
// TOD: Skill Icons
// TOD: Animation cleanup and improvements
// TOD: Null ref on kill enemy with primary when client
// TOD: Improve itemdisplayruleset
// TOD: Pod UV mapping (need to duplicate mesh?)
// TOD: Capacitor limb issue
// TOD: ParticleBuilder class, and convert everything to use it.
// TOD: Custom CharacterMain
// TOD: Broach shield location is weird
// TOD: Sounds for Special
// TOD: Special beam should do OnHitGlobal at hit spot
// TOD: Secondary doing onhitglobal for all enemies hit and not at center
// TOD: Meteor ignores wisp
// TOD: Shield material vertex offset
// TOD: Incinteration impact effects
// TOD: Redo other elite materials
// TOD: Expanded skin selection (Set of plasma skins, hard white outline on flames like blighted currently has.
// TOD: Explore networking potential from handleOverlapAttack



// ERRORS:
/*
[Info   : Unity Log] <style=cDeath><sprite name="Skull" tint=1> Close! <sprite name="Skull" tint=1></color>
[Info   : Unity Log] Could not save RunReport P:/Program Files (x86)/Steam/steamapps/common/Risk of Rain 2/Risk of Rain 2_Data/RunReports/PreviousRun.xml: The ' ' character, hexadecimal value 0x20, cannot be included in a name.
[Warning: Unity Log] Parent of RectTransform is being set with parent property. Consider using the SetParent method instead, with the worldPositionStays argument set to false. This will retain local orientation and scale rather than world orientation and scale, which can prevent common UI scaling issues.
[Error  : Unity Log] NullReferenceException
Stack trace:
UnityEngine.Transform.get_position () (at <f2abf40b37c34cf19b7fd98865114d88>:0)
RoR2.UI.CombatHealthBarViewer.UpdateAllHealthbarPositions (UnityEngine.Camera sceneCam, UnityEngine.Camera uiCam) (at <eae781bd93824da1b7902a2b6526887c>:0)
RoR2.UI.CombatHealthBarViewer.LayoutForCamera (RoR2.UICamera uiCamera) (at <eae781bd93824da1b7902a2b6526887c>:0)
RoR2.UI.CombatHealthBarViewer.SetLayoutHorizontal () (at <eae781bd93824da1b7902a2b6526887c>:0)
UnityEngine.UI.LayoutRebuilder.<Rebuild>m__3 (UnityEngine.Component e) (at <adbbd84a6a874fb3bb8dd55fe88db73d>:0)
UnityEngine.UI.LayoutRebuilder.PerformLayoutControl (UnityEngine.RectTransform rect, UnityEngine.Events.UnityAction`1[T0] action) (at <adbbd84a6a874fb3bb8dd55fe88db73d>:0)
UnityEngine.UI.LayoutRebuilder.Rebuild (UnityEngine.UI.CanvasUpdate executing) (at <adbbd84a6a874fb3bb8dd55fe88db73d>:0)
UnityEngine.UI.CanvasUpdateRegistry.PerformUpdate () (at <adbbd84a6a874fb3bb8dd55fe88db73d>:0)
UnityEngine.Canvas:SendWillRenderCanvases()

    */

