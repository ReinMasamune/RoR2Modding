namespace Grandparent.Mod
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using BepInEx;
    using BepInEx.Logging;
    using ReinCore;

    using RoR2;

    using UnityEngine;

    using Res = Properties.Resources;
    using Resources = UnityEngine.Resources;

#pragma warning disable CA2243 // Attribute string literals should parse correctly
    [BepInPlugin("com.Rein.Grandparent", "Grandparent", Properties.Info.ver )]
#pragma warning restore CA2243 // Attribute string literals should parse correctly
    public class Plugin : CorePlugin
    {
        private DirectorCard dirCard;

        protected override void Init()
        {
            logSource = base.logger;

            SpawnsCore.monsterEdits += this.SpawnsCore_monsterEdits;
            base.start += this.Plugin_start;
        }

        private void Plugin_start()
        {
            var card = Resources.Load<CharacterSpawnCard>( "SpawnCards/CharacterSpawnCards/Titan/cscGrandparent" );
            if( !card || card is null ) Log.Error( "No card found" );
            this.dirCard = new DirectorCard
            {
                spawnCard = card,
                selectionWeight = 500,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard,
                allowAmbushSpawn = true,
                preventOverhead = false,
                minimumStageCompletions = 0,
                forbiddenUnlockable = "",
                requiredUnlockable = "",
            };
        }

        private void SpawnsCore_monsterEdits( ClassicStageInfo stageInfo, Run runInstance, DirectorCardCategorySelection monsterSelection )
        {
            Log.Warning( "edits" );
            Log.Warning( monsterSelection.name );
            if( monsterSelection.name == "dccsSkyMeadowMonsters" )
            {
                Log.Warning( "IsRightMap" );
                //monsterSelection.RemoveCardsThatFailFilter( ( card ) => !( card.spawnCard.name.Contains( "Grandparent" ) ) );
                _ = monsterSelection.AddCard( 0, this.dirCard );
                _ = monsterSelection.AddCard( 1, this.dirCard );
                _ = monsterSelection.AddCard( 2, this.dirCard );

            }
        }


        internal static BepInEx.Logging.ManualLogSource logSource;
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

            public void DoLog( String name ) => Log.Message( String.Format( "{0}:\n{1} ticks\n{2} average", name, this.lastTicks, (Double)this.ticks / (Double)this.counter ) );

            private UInt64 lastTicks;
            private UInt64 counter;
            private UInt64 ticks;
        }

        public static void CallProf( String name, Action target )
        {
            Stopwatch timer = watches.Count > 0 ? watches.Pop() : new Stopwatch();
            timer.Restart();
            target();
            timer.Stop();
            if( !timerData.TryGetValue( name, out TimerData data ) )
            {
                data = new TimerData();
            }
            data.Update( (UInt64)timer.ElapsedTicks );
            timerData[name] = data;
            data.DoLog( name );
            watches.Push( timer );
        }
        public static TReturn CallProf<TReturn>( String name, Func<TReturn> target )
        {
            Stopwatch timer = watches.Count > 0 ? watches.Pop() : new Stopwatch();
            timer.Restart();
            TReturn ret = target();
            timer.Stop();
            if( !timerData.TryGetValue( name, out TimerData data ) )
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
