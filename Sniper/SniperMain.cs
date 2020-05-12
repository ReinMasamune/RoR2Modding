#region Metadata
#endregion
namespace Sniper
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using BepInEx.Logging;
    using ReinCore;
    using UnityEngine;
    using Sniper.Modules;
    using BepInEx;
    using RoR2;

    [BepInDependency( Properties.AssemblyLoad.guid)]
    [BepInPlugin( SniperMain.guid, "Sniper", Properties.Info.ver)]
    internal sealed class SniperMain : CorePlugin
    {
        private const String guid = "com.Rein.Sniper";

        internal static ManualLogSource logSource;
        private static SniperMain instance;

        internal List<StandardMaterial> sniperMaterials = new List<StandardMaterial>();
        internal static void AddMaterial( StandardMaterial mat, String name )
        {
            mat.name = name;
            SniperMain.instance.sniperMaterials.Add( mat );
        }

        protected sealed override void Init()
        {
            logSource = base.logger;
            Log.Message( "Sniper initializing" );
            instance = this;

            base.awake += () =>
            {
                SetModdedModule.SetModded();

                Properties.Tools.RegisterLanguageTokens();
                SoundModule.LoadBank();

                UIModule.EditHudPrefab();

                PrefabModule.CreatePrefab();
                DisplayModule.CreateDisplayPrefab();

                CatalogModule.RegisterBody();
                CatalogModule.RegisterSurvivor();
                CatalogModule.RegisterDoTType();
            };

            base.start += () =>
            {
                _ = TextureModule.GetExplosiveAmmoRamp();
                _ = TextureModule.GetPlasmaAmmoRamp();
                _ = TextureModule.GetStandardAmmoRamp();
                _ = TextureModule.GetShockAmmoRamp();
                _ = TextureModule.GetScatterAmmoRamp();
            };

            base.enable += HooksModule.Add;
            base.disable += HooksModule.Remove;
            Log.Message( "Sniper loaded successfully" );
        }


        #region static vars
        // TODO: Refactor these into their modules
        internal static GameObject sniperBodyPrefab;
        internal static GameObject sniperDisplayPrefab;
        #endregion


        #region static event mappings
#pragma warning disable IDE1006 // Naming Styles
        internal new static event Action Awake
        {
            add => instance.awake += value;
            remove => instance.awake -= value;
        }
        internal new static event Action Start
        {
            add => instance.start += value;
            remove => instance.start -= value;
        }
        internal new static event Action OnEnable
        {
            add => instance.enable += value;
            remove => instance.enable -= value;
        }
        internal new static event Action OnDisable
        {
            add => instance.disable += value;
            remove => instance.disable -= value;
        }
        internal new static event Action Update
        {
            add => instance.update += value;
            remove => instance.disable -= value;
        }
        internal new static event Action FixedUpdate
        {
            add => instance.fixedUpdate += value;
            remove => instance.fixedUpdate -= value;
        }
        internal new static event Action LateUpdate
        {
            add => instance.lateUpdate += value;
            remove => instance.lateUpdate -= value;
        }
        internal new static event Action OnDestroy
        {
            add => instance.destroy += value;
            remove => instance.destroy -= value;
        }
        internal new static event Action OnGUI
        {
            add => instance.gui += value;
            remove => instance.gui -= value;
        }
#pragma warning restore IDE1006 // Naming Styles
        #endregion
    }

    #region Logging
    internal static class Log
    {
        public static void Debug( System.Object data ) => InternalLog( LogLevel.Debug, data );
        public static void Info( System.Object data ) => InternalLog( LogLevel.Info, data);
        public static void Message( System.Object data ) => InternalLog( LogLevel.Message, data );
        public static void Warning( System.Object data ) => InternalLog( LogLevel.Warning, data );
        public static void Error( System.Object data ) => InternalLog( LogLevel.Error, data );
        public static void Fatal( System.Object data ) => InternalLog( LogLevel.Fatal, data );

        public static void DebugF(String text, params System.Object[] data ) => InternalLog( LogLevel.Debug, String.Format( text, data ) );
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
                    SniperMain.logSource.Log( LogLevel.Info, data );
                    break;

            }

            if( log )
            {
                SniperMain.logSource.Log( level, data );
            }
        }
#pragma warning disable IDE0060 // Remove unused parameter
        private static void InternalLog( LogLevel level, System.Object data, String member, Int32 line )
#pragma warning restore IDE0060 // Remove unused parameter
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
                SniperMain.logSource.Log( LogLevel.Info, data );
                break;

            }

            if( log )
            {
                SniperMain.logSource.Log( level, data );
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
                SniperMain.logSource.Log( LogLevel.Info, String.Format( "{0}; Line:{1}: {2}", member, line, data ) );
                break;

            }

            if( log )
            {
                SniperMain.logSource.Log( level, String.Format( "{0}; Line:{1}: {2}", member, line, data ) );
            }
        }
    }
    #endregion
}
