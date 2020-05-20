using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BepInEx;
using BepInEx.Logging;
using RoR2;
using UnityEngine;

namespace Rein.RogueWispPlugin
{
#pragma warning disable CA2243 // Attribute string literals should parse correctly
#if NETWORKING

    //[BepInDependency( NetLib.NetLib.guid, BepInDependency.DependencyFlags.SoftDependency )]
#endif
    [BepInDependency( Rein.AssemblyLoad.guid, BepInDependency.DependencyFlags.HardDependency )]
    [BepInPlugin( pluginGUID, pluginName, pluginVersion )]
#pragma warning restore CA2243 // Attribute string literals should parse correctly
    internal partial class Main : BaseUnityPlugin
    {
        #region random vars
        private const String pluginGUID = "com.Rein.RogueWisp";
        private const String pluginName = "Rogue Wisp";
        private const String pluginVersion = Properties.Info.ver;
        public String thing1;
        public String thing2;
        public String thing3;
        private readonly Boolean working;
        #endregion
        #region execution tracking stuff
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
                //Main.LogI( "State inc: " + _state.ToString() );

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
        #endregion
#if TIMER
        private readonly Stopwatch watch;
#endif
        internal static BepInEx.Logging.ManualLogSource logSource;

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
                        if( p == null ) continue;
                        this._plugins.Add( p );
                    }
                }
                return this._plugins;
            }
        }
        internal static Main instance;
        #region Events
        internal event Action Load;
        internal event Action FirstFrame;
        internal event Action Enable;
        internal event Action Disable;
        internal event Action Frame;
        internal event Action PostFrame;
        internal event Action Tick;
        internal event Action GUI;
        #endregion
#if LOADCHECKS
        //partial Boolean LoadChecks();
#endif
#if COMPATCHECKS
        partial void CompatChecks();
#endif
#if CROSSMODFUNCTIONALITY
        partial void CrossModFunctionality();
#endif
        partial void CreateAccessors();
        static partial void UpdateLibraries();
        partial void CreateCustomAccessors();
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
#if SOUNDPLAYER
        partial void SetupSoundPlayer();
#endif
#if MATEDITOR
        partial void SetupMatEditor();
#endif
        partial void SetupSkinSelector();
        partial void SetupEffectSkinning();
        static Main()
        {
        }
        public Main()
        {
            instance = this;
            logSource = base.Logger;
            HashSet<String> submodules = new HashSet<String>();
            //ReflectionOnlyAssemblyResolve
            for( Int32 i = 0; i < this.plugins.Count; ++i )
            {
                var p = this.plugins[i];
                if( p.Metadata.GUID == "com.bepis.r2api" )
                {
                }

            }

            this.CreateAccessors();
            this.CreateCustomAccessors();

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
                this.CreateArchaicWisp();
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
#if SOUNDPLAYER
                this.SetupSoundPlayer();
#endif
#if MATEDITOR
                this.SetupMatEditor();
#endif
                this.SetupSkinSelector();
                this.SetupEffectSkinning();


#if LV1DPSTEST
                SpawnsCore.monsterEdits += this.SpawnsCore_monsterEdits1;
                RoR2.GlobalEventManager.onCharacterDeathGlobal += this.GlobalEventManager_onCharacterDeathGlobal;
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

            this.FirstFrame += this.Main_FirstFrame2;

            this.Enable += IncState;
            this.FirstFrame += IncState;
        }

        private void Main_FirstFrame2()
        {
            rogueWispBodyIndex = BodyCatalog.FindBodyIndex( this.RW_body );
        }

        #region Boss dps timer stuff
#if LV1DPSTEST
        private void GlobalEventManager_onCharacterDeathGlobal( DamageReport obj )
        {
            var v = obj?.victimBody?.GetComponent<TimeThing>();
            if( v != null ) StopTimer();
        }

        private void SpawnsCore_monsterEdits1( ClassicStageInfo stageInfo, Run runInstance, DirectorCardCategorySelection monsterSelection )
        {
            foreach( var m in monsterSelection.categories )
            {
                if( m.name.Contains( "ions" ) )
                {
                    foreach( var n in m.cards )
                    {
                        //Main.LogW( n.spawnCard.name );
                        if( n.spawnCard.name != "cscBeetleQueen" )
                        {
                            n.selectionWeight = 0;
                        } else
                        {
                            var master = (n.spawnCard as CharacterSpawnCard).prefab;
                            foreach( var v in master.GetComponents<RoR2.CharacterAI.AISkillDriver>() )
                            {
                                if( v.skillSlot == SkillSlot.Secondary )
                                {
                                    v.maxUserHealthFraction = 0f;
                                    v.minUserHealthFraction = 0f;
                                }
                            }
                            master.GetComponent<CharacterMaster>().bodyPrefab.AddComponent<TimeThing>();
                        }
                    }
                } else
                {
                    foreach( var n in m.cards )
                    {
                        var body = n.spawnCard.prefab.GetComponent<CharacterMaster>().bodyPrefab.GetComponent<CharacterBody>();
                        body.baseDamage = 0f;
                        body.levelDamage = 0f;
                    }
                }
            }
        }


        private static Stopwatch bossTimer = new Stopwatch();
        internal static void StartTimer()
        {
            bossTimer.Restart();
        }
        internal static void StopTimer()
        {
            if( bossTimer.IsRunning )
            {
                bossTimer.Stop();
                Chat.AddMessage( String.Format( "Time: {0}ms, Ticks: {1}", bossTimer.ElapsedMilliseconds, bossTimer.ElapsedTicks ) );
            }
        }
        internal class TimeThing : MonoBehaviour
        {
            private void Start()
            {
                Main.StartTimer();
            }

            private void OnDisable()
            {
                Main.StopTimer();
            }
        }
#endif
        #endregion


        internal static Texture2D debugTexture;
        private void DebugTexture()
        {
            if( debugTexture != null )
            {
                UnityEngine.GUI.Label( new Rect( 0f, 0f, 1000f, 1000f ), debugTexture );
            }
        }

        private delegate void InvokeEffectsReloadDelegate( ConCommandArgs args );
        private InvokeEffectsReloadDelegate CCEffectsReload = (InvokeEffectsReloadDelegate)Delegate.CreateDelegate(typeof(InvokeEffectsReloadDelegate),typeof(EffectCatalog),"CCEffectsReload" );
        private void Main_FirstFrame() => this.CCEffectsReload( default );

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


        [Obsolete]
        internal static void LogM( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Log.MessageL( data, member, line );
        [Obsolete]
        internal static void LogW( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Log.WarningL( data, member, line );
        [Obsolete]
        internal static void LogE( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Log.ErrorL( data, member, line );
        [Obsolete]
        internal static void LogF( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Log.FatalL( data, member, line );
    }

    #region Logging
    internal static class Log
    {
        private static Stack<Stopwatch> watches = new Stack<Stopwatch>();

        private static Dictionary<String,TimerData> timerData = new Dictionary<String, TimerData>();
        private struct TimerData
        {
            public TimerData( UInt64 ticks )
            {
                this.counter = 1ul;
                this.ticks = ticks;
                this.lastTicks = ticks;
            }
            public void Update( UInt64 ticks )
            {
                this.counter++;
                this.ticks += ticks;
                this.lastTicks = ticks;
            }

            public void DoLog( String name )
            {
                InternalLog( LogLevel.None, String.Format( "{0}:\n{1} ticks\n{2} average", name, this.lastTicks, (Double)this.ticks / (Double)this.counter ) );
            }

            private UInt64 lastTicks;
            private UInt64 counter;
            private UInt64 ticks;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void CallProf( String name, Action target )
        {
            Stopwatch timer = watches.Count > 0 ? watches.Pop() : new Stopwatch();
            timer.Restart();
            target();
            timer.Stop();
            if( !timerData.TryGetValue( name, out var data ) )
            {
                data = new TimerData();
            }
            data.Update( (UInt64)timer.ElapsedTicks );
            timerData[name] = data;
            data.DoLog( name );
            watches.Push( timer );
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static TReturn CallProf<TReturn>( String name, Func<TReturn> target )
        {
            Stopwatch timer = watches.Count > 0 ? watches.Pop() : new Stopwatch();
            timer.Restart();
            TReturn ret = target();
            timer.Stop();
            if( !timerData.TryGetValue( name, out var data ) )
            {
                data = new TimerData();
            }
            data.Update( (UInt64)timer.ElapsedTicks );
            timerData[name] = data;
            data.DoLog( name );
            watches.Push( timer );
            return ret;
        }

        public static void Debug( System.Object data ) => InternalLog( LogLevel.Debug, data );
        public static void Info( System.Object data ) => InternalLog( LogLevel.Info, data );
        public static void Message( System.Object data ) => InternalLog( LogLevel.Message, data );
        public static void Warning( System.Object data ) => InternalLog( LogLevel.Warning, data );
        public static void Error( System.Object data ) => InternalLog( LogLevel.Error, data );
        public static void Fatal( System.Object data ) => InternalLog( LogLevel.Fatal, data );

        public static void DebugF( String text, params System.Object[] data ) => InternalLog( LogLevel.Debug, String.Format( text, data ) );
        public static void InfoF( String text, params System.Object[] data ) => InternalLog( LogLevel.Info, String.Format( text, data ) );
        public static void MessageF( String text, params System.Object[] data ) => InternalLog( LogLevel.Message, String.Format( text, data ) );
        public static void WarningF( String text, params System.Object[] data ) => InternalLog( LogLevel.Warning, String.Format( text, data ) );
        public static void ErrorF( String text, params System.Object[] data ) => InternalLog( LogLevel.Error, String.Format( text, data ) );
        public static void FatalF( String text, params System.Object[] data ) => InternalLog( LogLevel.Fatal, String.Format( text, data ) );

        public static void DebugL( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLogL( LogLevel.Debug, data, member, line );
        public static void InfoL( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLogL( LogLevel.Info, data, member, line );
        public static void MessageL( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLogL( LogLevel.Message, data, member, line );
        public static void WarningL( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLogL( LogLevel.Warning, data, member, line );
        public static void ErrorL( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLogL( LogLevel.Error, data, member, line );
        public static void FatalL( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLogL( LogLevel.Fatal, data, member, line );


        [Obsolete]
        public static void DebugT( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLogL( LogLevel.Debug, data, member, line );
        [Obsolete]
        public static void InfoT( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLogL( LogLevel.Info, data, member, line );
        [Obsolete]
        public static void MessageT( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLogL( LogLevel.Message, data, member, line );
        [Obsolete]
        public static void WarningT( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLogL( LogLevel.Warning, data, member, line );
        [Obsolete]
        public static void ErrorT( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLogL( LogLevel.Error, data, member, line );
        [Obsolete]
        public static void FatalT( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLogL( LogLevel.Fatal, data, member, line );

        public static void Counter( [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 )
        {
            if( !counters.ContainsKey( member ) )
            {
                counters[member] = 0UL;
            }

            InternalLog( LogLevel.None, String.Format( "{0}, member: {1}, line: {2}", counters[member]++, member, line ), member, line );
        }
        public static void ClearCounter( Boolean toConsole = false, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 )
        {
            if( !counters.ContainsKey( member ) )
            {
                return;
            }

            counters[member] = 0UL;
            if( toConsole )
            {
                InternalLog( LogLevel.None, String.Format( "Counter cleared for member: {0}, line: {1}", member, line ), member, line );
            }
        }
        private static readonly Dictionary<String,UInt64> counters = new Dictionary<String, UInt64>();

        private static void InternalLog( LogLevel level, System.Object data )
        {

            Boolean log = false;
            switch( level )
            {
                case LogLevel.Debug:
                log = true;
                break;
                case LogLevel.Info:
                log = true;
                break;
                case LogLevel.Message:
                log = true;
                break;
                case LogLevel.Warning:
                log = true;
                break;
                case LogLevel.Error:
                log = true;
                break;
                case LogLevel.Fatal:
                log = true;
                break;
                default:
                Main.logSource.Log( LogLevel.Info, data );
                break;

            }

            if( log )
            {
                Main.logSource.Log( level, data );
            }
        }
        private static void InternalLog( LogLevel level, System.Object data, String member, Int32 line )
        {

            Boolean log = false;
            switch( level )
            {
                case LogLevel.Debug:
                log = true;
                break;
                case LogLevel.Info:
                log = true;
                break;
                case LogLevel.Message:
                log = true;
                break;
                case LogLevel.Warning:
                log = true;
                break;
                case LogLevel.Error:
                log = true;
                break;
                case LogLevel.Fatal:
                log = true;
                break;
                default:
                Main.logSource.Log( LogLevel.Info, data );
                break;

            }

            if( log )
            {
                Main.logSource.Log( level, data );
            }
        }
        private static void InternalLogL( LogLevel level, System.Object data, String member, Int32 line )
        {

            Boolean log = false;
            switch( level )
            {
                case LogLevel.Debug:
                log = true;
                break;
                case LogLevel.Info:
                log = true;
                break;
                case LogLevel.Message:
                log = true;
                break;
                case LogLevel.Warning:
                log = true;
                break;
                case LogLevel.Error:
                log = true;
                break;
                case LogLevel.Fatal:
                log = true;
                break;
                default:
                Main.logSource.Log( LogLevel.Info, String.Format( "{0}; Line:{1}: {2}", member, line, data ) );
                break;

            }

            if( log )
            {
                Main.logSource.Log( level, String.Format( "{0}; Line:{1}: {2}", member, line, data ) );
            }
        }
    }
    #endregion
}
// Thought organization:::

// SKIN STUFF
//  Need to hook in to CharacterModel.UpdateRendererMaterials and check for 
//
//
//
//
//
//
//
//
//
//379.63%
//
//
//
//
//
//
//
//
//


//Particle builder
//Builders for other material types
//Remake all wisp vfx with improved materials
//Create custom materials




//For next release:



// TOD: Transparency on barrier portion of boss hp bar
// TOD: Archaic wisp add to wisp family event


/*
[Warning:Rogue Wisp]
MinMaxCurve
[Warning:Rogue Wisp] MinMaxGradient
[Warning:Rogue Wisp] ParticleSystemSimulationSpace
[Warning:Rogue Wisp] Transform
[Warning:Rogue Wisp] ParticleSystemScalingMode
[Warning:Rogue Wisp] ParticleSystemEmitterVelocityMode
[Warning:Rogue Wisp] ParticleSystemStopAction
[Warning:Rogue Wisp] ParticleSystemCullingMode
[Warning:Rogue Wisp] ParticleSystemRingBufferMode
[Warning:Rogue Wisp] Vector2
[Warning:Rogue Wisp] ParticleSystemEmissionType
[Warning:Rogue Wisp] Vector3
[Warning:Rogue Wisp] ParticleSystemShapeType
[Warning:Rogue Wisp] ParticleSystemShapeMultiModeValue
[Warning:Rogue Wisp] ParticleSystemMeshShapeType
[Warning:Rogue Wisp] Mesh
[Warning:Rogue Wisp] MeshRenderer
[Warning:Rogue Wisp] SkinnedMeshRenderer
[Warning:Rogue Wisp] Sprite
[Warning:Rogue Wisp] SpriteRenderer
[Warning:Rogue Wisp] Texture2D
[Warning:Rogue Wisp] ParticleSystemShapeTextureChannel
[Warning:Rogue Wisp] ParticleSystemInheritVelocityMode
[Warning:Rogue Wisp] ParticleSystemGameObjectFilter
[Warning:Rogue Wisp] ParticleSystemNoiseQuality
[Warning:Rogue Wisp] ParticleSystemCollisionType
[Warning:Rogue Wisp] ParticleSystemCollisionMode
[Warning:Rogue Wisp] LayerMask
[Warning:Rogue Wisp] ParticleSystemCollisionQuality
[Warning:Rogue Wisp] ParticleSystemOverlapAction
[Warning:Rogue Wisp] ParticleSystem
[Warning:Rogue Wisp] ParticleSystemAnimationMode
[Warning:Rogue Wisp] ParticleSystemAnimationTimeMode
[Warning:Rogue Wisp] ParticleSystemAnimationType
[Warning:Rogue Wisp] UVChannelFlags
[Warning:Rogue Wisp] Light
[Warning:Rogue Wisp] ParticleSystemTrailMode
[Warning:Rogue Wisp] ParticleSystemTrailTextureMode
*/


// Future plans and shit

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
// TOD: Animation cleanup and improvements
// TOD: Null ref on kill enemy with primary when client
// TOD: Pod UV mapping (need to duplicate mesh)
// TOD: Capacitor limb issue
// TOD: Custom CharacterMain
// TOD: Broach shield location is weird
// TOD: Sounds for Special
// TOD: Meteor ignores wisp
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

