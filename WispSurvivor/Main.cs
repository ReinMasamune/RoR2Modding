using BepInEx;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Reflection;
using UnityEngine;
#if NETWORKING
using NetLib;
#endif

namespace RogueWispPlugin
{
    [R2APISubmoduleDependency(
        nameof( R2API.SurvivorAPI ),
        nameof( R2API.EffectAPI ),
        nameof( R2API.PrefabAPI ),
        nameof( R2API.LoadoutAPI ),
        nameof( R2API.OrbAPI ),
        nameof( R2API.ItemAPI ),
#if ANCIENTWISP
        nameof( R2API.DirectorAPI ),
#endif
        nameof( R2API.AssetPlus.AssetPlus )
    )]
#pragma warning disable CA2243 // Attribute string literals should parse correctly
#if NETWORKING
    [BepInDependency( NetLib.NetLib.guid, BepInDependency.DependencyFlags.SoftDependency )]
#endif
    [BepInDependency( R2API.R2API.PluginGUID, BepInDependency.DependencyFlags.HardDependency )]
    [BepInPlugin( pluginGUID, pluginName, pluginVersion )]
#pragma warning restore CA2243 // Attribute string literals should parse correctly
    internal partial class Main : BaseUnityPlugin
    {
        private const String pluginGUID = "com.Rein.RogueWisp";
        private const String pluginName = "Rogue Wisp";
        private const String pluginVersion = Consts.ver;

        public String thing1;
        public String thing2;

        private readonly Boolean working;




        private static ExecutionState _state = ExecutionState.PreLoad;
        internal static ExecutionState state
        {
            get
            {
                return _state;
            }
            set
            {
                if( _state == ExecutionState.Broken ) return;
                _state = value;
                Main.LogI( "State inc: " + _state.ToString() );

            }
        }
        private static void IncState()
        {
            state = state + 1;
            UpdateLibraries();
        }
        internal enum ExecutionState
        {
            Broken = -1,
            PreLoad = 0,
            Constructor = 1,
            Awake = 2,
            Enable = 3,
            Catalogs = 4,
            Start = 5,
            Execution = 6
        }

#if TIMER
        private readonly Stopwatch watch;
#endif

        private List<PluginInfo> _plugins;
        private List<PluginInfo> plugins
        {
            get
            {
                if( this._plugins == null )
                {
                    this._plugins = new List<PluginInfo>();
                    foreach( PluginInfo p in BepInEx.Bootstrap.Chainloader.PluginInfos.Values )
                    {
                        this._plugins.Add( p );
                    }
                }
                return this._plugins;
            }
        }

        internal static Main instance;

        internal event Action Load;
        internal event Action FirstFrame;
        internal event Action Enable;
        internal event Action Disable;
        internal event Action Frame;
        internal event Action PostFrame;
        internal event Action Tick;
        internal event Action GUI;
#if LOADCHECKS
        partial Boolean LoadChecks();
#endif
#if COMPATCHECKS
        partial void CompatChecks();
#endif
#if CROSSMODFUNCTIONALITY
        partial void CrossModFunctionality();
#endif
        partial void CreateAccessors();
        static partial void UpdateLibraries();
#if ROGUEWISP
        partial void CreateRogueWisp();
#endif
#if BOSSHPBAR
        partial void EditBossHPBar();
#endif
#if NETWORKING
        partial void RegisterNetworkMessages();
#else
        partial void SetupNetworkingFramework();
        partial void SetupNetworkingFunctions();
#endif
#if ANCIENTWISP
        partial void CreateAncientWisp();
#endif
#if ARCHAICWISP
        partial void CreateArchaicWisp();
#endif
#if FIRSTWISP
        partial void CreateFirstWisp();
#endif
#if WISPITEM
        partial void CreateWispFriend();
#endif
#if STAGE
        partial void CreateStage();
#endif
        public Main()
        {
            instance = this;
            this.thing1 = "Note that everything in this codebase that can be used safely is already part of R2API.";
            this.thing2 = "If you're here to copy paste my code, please don't complain to me when it doesn't work like magic or you cause a major issue for a user.";

            this.CreateAccessors();

            IncState();

            this.working = true;

            this.Load += IncState;
            this.Enable += IncState;
            this.FirstFrame += IncState;

            this.CompatChecks();
            if( this.working )
            {
#if TIMER
                this.watch = new Stopwatch();
                this.Load += this.AwakeTimeStart;
                this.Enable += this.EnableTimeStart;
                this.FirstFrame += this.StartTimeStart;
#endif
                this.Tick += () => RoR2Application.isModded = true;
#if CROSSMODFUNCTIONALITY
                this.CrossModFunctionality();
#endif
#if ROGUEWISP
                this.CreateRogueWisp();
#endif
#if BOSSHPBAR
                this.EditBossHPBar();
#endif
#if ANCIENTWISP
                this.CreateAncientWisp();
#endif
#if ARCHAICWISP
                this.CreateArchaicWisp;
#endif
#if FIRSTWISP
                this.CreateFirstWisp();
#endif
#if WISPITEM
                this.CreateWispFriend();
#endif
#if STAGE
                this.CreateStage();
#endif
#if NETWORKING
                this.RegisterNetworkMessages();
#else
                this.SetupNetworkingFramework();
                this.SetupNetworkingFunctions();
#endif
                this.FirstFrame += this.Main_FirstFrame;
#if TIMER
                this.Load += this.AwakeTimeStop;
                this.Enable += this.EnableTimeStop;
                this.FirstFrame += this.StartTimeStop;
#endif
            } else
            {
                Main.LogF( "Rogue Wisp has failed to load properly and will not be enabled. See preceding errors." );
            }

            this.Enable += IncState;
            this.FirstFrame += IncState;

            this.FirstFrame += this.TempTestingShit;
        }

        private void TempTestingShit()
        {
            var mats = new HashSet<Material>();

            var fx1 = Resources.Load<GameObject>("Prefabs/Effects/NullifierDeathExplosion" );
            var fx2 = Resources.Load<GameObject>("Prefabs/ProjectileGhosts/NullifierPreBombGhost" );


            foreach( var r in fx1.GetComponentsInChildren<ParticleSystemRenderer>() )
            {
                foreach( var m in r.sharedMaterials )
                {
                    mats.Add( m );
                }
            }
            foreach( var r in fx2.GetComponentsInChildren<ParticleSystemRenderer>() )
            {
                foreach( var m in r.sharedMaterials )
                {
                    mats.Add( m );
                }
            }

            foreach( var m in mats )
            {
                if( m == null ) continue;
                RoR2Plugin.Main.MiscHelpers.DebugMaterialInfo( m );
            }
        }
        private void AddObjToSet( GameObject obj, HashSet<Texture> texSet, HashSet<Mesh> meshSet, HashSet<Shader> shaderSet  )
        {
            if( obj == null ) return;
            foreach( var psr in obj.GetComponents<ParticleSystemRenderer>() )
            {
                this.AddMatToSet( psr.material, texSet, meshSet, shaderSet );
                var mesh = psr.mesh;
                if( mesh != null ) meshSet.Add( mesh );
            }

            foreach( var psr in obj.GetComponentsInChildren<ParticleSystemRenderer>() )
            {
                this.AddMatToSet( psr.material, texSet, meshSet, shaderSet );
                var mesh = psr.mesh;
                if( mesh != null ) meshSet.Add( mesh );
            }
        }

        private void AddMatToSet( Material m, HashSet<Texture> texSet, HashSet<Mesh> meshSet, HashSet<Shader> shaderSet )
        {
            if( m == null ) return;
            var shader = m.shader;
            if( shader != null ) shaderSet.Add( shader );
            foreach( var ind in m.GetTexturePropertyNames() )
            {
                var tex = m.GetTexture( ind );
                if( tex != null ) texSet.Add( tex );
            }
        }

        private void Main_FirstFrame() => typeof( EffectCatalog ).InvokeMethod( "CCEffectsReload", new ConCommandArgs() );

#pragma warning disable IDE0051 // Remove unused private members
        public void Awake() => this.Load?.Invoke();
        public void Start() => this.FirstFrame?.Invoke();
        public void OnEnable() => this.Enable?.Invoke();
        public void OnDisable() => this.Disable?.Invoke();
        public void Update() => this.Frame?.Invoke();
        public void LateUpdate() => this.PostFrame?.Invoke();
        public void FixedUpdate() => this.Tick?.Invoke();
        public void OnGUI() => this.GUI?.Invoke();
#pragma warning restore IDE0051 // Remove unused private members

#if TIMER
        private void AwakeTimeStart() => this.watch.Restart();
        private void AwakeTimeStop()
        {
            this.watch.Stop();
            Main.LogI( "Awake Time: " + this.watch.ElapsedMilliseconds );
        }
        private void EnableTimeStart() => this.watch.Restart();
        private void EnableTimeStop()
        {
            this.watch.Stop();
            Main.LogI( "Enable Time: " + this.watch.ElapsedMilliseconds );
        }
        private void StartTimeStart() => this.watch.Restart();
        private void StartTimeStop()
        {
            this.watch.Stop();
            Main.LogI( "Start Time: " + this.watch.ElapsedMilliseconds );
        }
#endif
        internal static void Log( BepInEx.Logging.LogLevel level, System.Object data, String member, Int32 line )
        {
            if( data == null )
            {
                Main.instance.Logger.LogError( "Null data sent by: " + member + " line: " + line );
                return;
            }

            if( level == BepInEx.Logging.LogLevel.Fatal || level == BepInEx.Logging.LogLevel.Error || level == BepInEx.Logging.LogLevel.Warning | level == BepInEx.Logging.LogLevel.Message )
            {
                Main.instance.Logger.Log( level, data );
            } else
            {
#if LOGGING
                Main.instance.Logger.Log( level, data );
#endif
            }
#if FINDLOGS
            Main.instance.Logger.LogWarning( "Log: " + level.ToString() + " called by: " + file + " : " + member + " : " + line );
#endif
        }

        internal static void LogI( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Main.Log( BepInEx.Logging.LogLevel.Info, data, member, line );
        internal static void LogM( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Main.Log( BepInEx.Logging.LogLevel.Message, data, member, line );
        internal static void LogD( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Main.Log( BepInEx.Logging.LogLevel.Debug, data, member, line );
        internal static void LogW( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Main.Log( BepInEx.Logging.LogLevel.Warning, data, member, line );
        internal static void LogE( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Main.Log( BepInEx.Logging.LogLevel.Error, data, member, line );
        internal static void LogF( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Main.Log( BepInEx.Logging.LogLevel.Fatal, data, member, line );
        internal static Int32 logCounter = 0;
        internal static void LogC( [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Main.Log( BepInEx.Logging.LogLevel.Info, member + ": " + line + ":: " + logCounter++, member, line );
    }
}

//For next release:

// TODO: Transparency on barrier portion of boss hp bar
// TODO: Ancient wisp needs better vfx for all skills
// TODO: Tune damage values and stats on ancient wisp
// TODO: Flames are offset strangley during Ancient Wisp secondary
// TODO: Proper spawning conditions for ancient wisp
// TODO: Tune AI for ancient wisp
// TODO: Ancient wisp UV mapping (need to duplicate mesh)
// TODO: Add lunar scavs to the charge gain stuff


// Future plans and shit

// TOD: Undo hard coding of skin numbers
// TOD: IDRS not showing on server
// TOD: Utility sounds (unsure what to do here)
// TOD: Little Disciple and Will O Wisp color should change with skin
// TOD: Body Animation smoothing params
// TOD: Rewrite secondary
// TOD: Character lobby description
// TOD: Customize crosshair
// TOD: Readme lore sillyness
// TOD: Effects obscuring vision
// TOD: Effect brightness settings
// TOD: Muzzle flashes
// TOD: Animation cleanup and improvements
// TOD: Null ref on kill enemy with primary when client
// TOD: Improve itemdisplayruleset
// TOD: Pod UV mapping (need to duplicate mesh)
// TOD: Capacitor limb issue
// TOD: ParticleBuilder class, and convert everything to use it.
// TOD: Custom CharacterMain
// TOD: Broach shield location is weird
// TOD: Sounds for Special
// TOD: Meteor ignores wisp
// TOD: Shield material vertex offset
// TOD: Redo other elite materials
// TOD: Expanded skin selection (Set of plasma skins, hard white outline on flames like blighted currently has.
// TOD: When hopoo adds projectile and effect skins, redo all vfx
// TOD: GPU particlesystem
// TOD: Clipping through ground still...

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

