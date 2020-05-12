namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    using BepInEx.Logging;

    internal static class Log
    {
        public static Boolean loaded { get; internal set; } = false;

        public static void WarningSTR( Int32 data ) => InternalLog( LogLevel.Warning, data, default, default );

        public static void Debug( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLog( LogLevel.Debug, data, member, line );
        public static void Info( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLog( LogLevel.Info, data, member, line );
        public static void Message( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLog( LogLevel.Message, data, member, line );
        public static void Warning( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLog( LogLevel.Warning, data, member, line );
        public static void Error( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLog( LogLevel.Error, data, member, line );
        public static void Fatal( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => InternalLog( LogLevel.Fatal, data, member, line );
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



        static Log()
        {
            if( logger == null )
            {
                logger = BepInEx.Logging.Logger.CreateLogSource( nameof( ReinCore ) );
            }

            loaded = true;
        }

        private static readonly BepInEx.Logging.ManualLogSource logger;
        private static readonly Dictionary<String,UInt64> counters = new Dictionary<String, UInt64>();


        private static void InternalLog( LogLevel level, System.Object data, String member, Int32 line )
        {
            if( level == LogLevel.None )
            {
                logger.Log( LogLevel.Info, data );
            } else
            {
                ExecutionLevel lv = Translate( level );
#if ALLLOGS
                logger.Log( level, data );
#else
                if( ReinCore.execLevel.HasFlag( lv ) && level != LogLevel.None )
                {
                    logger.Log( level, data );
                }
#endif
            }
            if( ReinCore.execLevel.HasFlag( ExecutionLevel.FindLogs ) )
            {
                logger.Log( LogLevel.Info, String.Format( "{0} : {1}", member, line ) );
            }
        }

        private static ExecutionLevel Translate( LogLevel level )
        {
            switch( level )
            {
                case LogLevel.Debug: return ExecutionLevel.Debug;
                case LogLevel.Info: return ExecutionLevel.Info;
                case LogLevel.Message: return ExecutionLevel.Message;
                case LogLevel.Warning: return ExecutionLevel.Warning;
                case LogLevel.Error: return ExecutionLevel.Error;
                case LogLevel.Fatal: return ExecutionLevel.Fatal;
                default: throw new ArgumentOutOfRangeException( "level" );
            }
        }
    }
}
