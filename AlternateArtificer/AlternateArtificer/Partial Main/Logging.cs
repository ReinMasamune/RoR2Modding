using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using BepInEx;
using ReinCore;

namespace Rein.AlternateArtificer
{
    internal partial class Main
    {
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
            Main.instance.Logger.LogWarning( "Log: " + level.ToString() + " called by: " + member + " : " + line );
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
