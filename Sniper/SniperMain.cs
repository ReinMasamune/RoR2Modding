using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using UnityEngine;
using Sniper.Modules;
using BepInEx;

namespace Sniper
{
    [BepInDependency( Properties.AssemblyLoad.guid)]
    [BepInPlugin( SniperMain.guid, "Sniper", Properties.Info.ver)]
    internal sealed class SniperMain : CorePlugin
    {
        const String guid = "com.Rein.Sniper";

        internal static BepInEx.Logging.ManualLogSource logSource;
        private static SniperMain instance;

        protected sealed override void Init()
        {
            logSource = base.logger;
            Log.Message( "Sniper initializing" );
            instance = this;

            base.awake += () =>
            {
                SetModdedModule.SetModded();

                Properties.Tools.RegisterLanguageTokens();

                PrefabModule.CreatePrefab();
                DisplayModule.CreateDisplayPrefab();



                CatalogModule.RegisterBody();
                CatalogModule.RegisterSurvivor();
            };

            base.enable += HooksModule.Add;
            base.disable += HooksModule.Remove;
            Log.Message( "Sniper loaded successfully" );
        }
        #region static vars
        internal static GameObject sniperBodyPrefab;
        internal static GameObject sniperDisplayPrefab;





        #endregion


        #region static event mappings
        internal static event Action Awake
        {
            add => instance.awake += value;
            remove => instance.awake -= value;
        }

        internal static event Action Start
        {
            add => instance.start += value;
            remove => instance.start -= value;
        }

        internal static event Action OnEnable
        {
            add => instance.enable += value;
            remove => instance.enable -= value;
        }

        internal static event Action OnDisable
        {
            add => instance.disable += value;
            remove => instance.disable -= value;
        }

        internal static event Action Update
        {
            add => instance.update += value;
            remove => instance.disable -= value;
        }

        internal static event Action FixedUpdate
        {
            add => instance.fixedUpdate += value;
            remove => instance.fixedUpdate -= value;
        }

        internal static event Action LateUpdate
        {
            add => instance.lateUpdate += value;
            remove => instance.lateUpdate -= value;
        }

        internal static event Action OnDestroy
        {
            add => instance.destroy += value;
            remove => instance.destroy -= value;
        }

        internal static event Action OnGUI
        {
            add => instance.gui += value;
            remove => instance.gui -= value;
        }
        #endregion
    }

    #region Logging
    internal static class Log
    {
        public static void Debug( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 )
        {
            InternalLog( LogLevel.Debug, data, member, line );
        }
        public static void Info( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 )
        {
            InternalLog( LogLevel.Info, data, member, line );
        }
        public static void Message( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 )
        {
            InternalLog( LogLevel.Message, data, member, line );
        }
        public static void Warning( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 )
        {
            InternalLog( LogLevel.Warning, data, member, line );
        }
        public static void Error( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 )
        {
            InternalLog( LogLevel.Error, data, member, line );
        }
        public static void Fatal( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 )
        {
            InternalLog( LogLevel.Fatal, data, member, line );
        }
        public static void Counter( [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 )
        {
            if( !counters.ContainsKey( member ) ) counters[member] = 0UL;
            InternalLog( LogLevel.None, String.Format( "{0}, member: {1}, line: {2}", counters[member]++, member, line ), member, line );
        }
        public static void ClearCounter( Boolean toConsole = false, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 )
        {
            if( !counters.ContainsKey( member ) ) return;
            counters[member] = 0UL;
            if( toConsole )
            {
                InternalLog( LogLevel.None, String.Format( "Counter cleared for member: {0}, line: {1}", member, line ), member, line );
            }
        }
        private static Dictionary<String,UInt64> counters = new Dictionary<String, UInt64>();
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
                    SniperMain.logSource.Log( LogLevel.Info, data );
                    break;

            }

            if( log )
            {
                SniperMain.logSource.Log( level, data );
            }
        }
    }
    #endregion
}
