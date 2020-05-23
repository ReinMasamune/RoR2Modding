namespace NavigationOverrides.Mod
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using BepInEx.Logging;

    public class Plugin : ReinCore.CorePlugin
    {
        protected override void Init()
        {
            logSource = base.logger;
        }

        internal static BepInEx.Logging.ManualLogSource logSource;
    }


    #region Logging
    internal static class Log
    {
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
                Plugin.logSource.Log( LogLevel.Info, data );
                break;

            }

            if( log )
            {
                Plugin.logSource.Log( level, data );
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
                Plugin.logSource.Log( LogLevel.Info, data );
                break;

            }

            if( log )
            {
                Plugin.logSource.Log( level, data );
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
                Plugin.logSource.Log( LogLevel.Info, String.Format( "{0}; Line:{1}: {2}", member, line, data ) );
                break;

            }

            if( log )
            {
                Plugin.logSource.Log( level, String.Format( "{0}; Line:{1}: {2}", member, line, data ) );
            }
        }
    }
    #endregion
}
