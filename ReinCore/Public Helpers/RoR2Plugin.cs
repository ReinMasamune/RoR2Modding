//namespace RoR2PluginBase
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Diagnostics;
//    using System.Runtime.CompilerServices;
//    using System.Text;

//    using BepInEx;
//    using BepInEx.Configuration;
//    using BepInEx.Logging;

//    public abstract class RoR2Plugin<TSelf> : BaseUnityPlugin
//        where TSelf : RoR2Plugin<TSelf>
//    {
//        public static TSelf instance { get; private set; }

//        internal RoR2Plugin()
//        {
//            instance = this as TSelf;
//        }


//        public static ConfigFile config => instance.Config;
//        public static ManualLogSource logger => instance.Logger;

//        #region LogHelpers
//        public static class Log
//        {
//            private static Stack<Stopwatch> watches = new Stack<Stopwatch>();

//            private static Dictionary<String,TimerData> timerData = new Dictionary<String, TimerData>();
//            private struct TimerData
//            {
//                public TimerData(UInt64 ticks)
//                {
//                    this.counter = 1ul;
//                    this.ticks = ticks;
//                    this.lastTicks = ticks;
//                }
//                public void Update(UInt64 ticks)
//                {
//                    this.counter++;
//                    this.ticks += ticks;
//                    this.lastTicks = ticks;
//                }

//                public void DoLog(String name) => Log.Message($"{name}:\n{this.lastTicks} ticks\n{(Double)this.ticks / (Double)this.counter} average");

//                private UInt64 lastTicks;
//                private UInt64 counter;
//                private UInt64 ticks;
//            }

//            public static void CallProf(String name, Action target)
//            {
//                Stopwatch timer = watches.Count > 0 ? watches.Pop() : new Stopwatch();
//                timer.Restart();
//                target();
//                timer.Stop();
//                if(!timerData.TryGetValue(name, out TimerData data))
//                {
//                    data = new TimerData();
//                }
//                data.Update((UInt64)timer.ElapsedTicks);
//                timerData[name] = data;
//                data.DoLog(name);
//                watches.Push(timer);
//            }
//            public static TReturn CallProf<TReturn>(String name, Func<TReturn> target)
//            {
//                Stopwatch timer = watches.Count > 0 ? watches.Pop() : new Stopwatch();
//                timer.Restart();
//                TReturn ret = target();
//                timer.Stop();
//                if(!timerData.TryGetValue(name, out TimerData data))
//                {
//                    data = new TimerData();
//                }
//                data.Update((UInt64)timer.ElapsedTicks);
//                timerData[name] = data;
//                data.DoLog(name);
//                watches.Push(timer);
//                return ret;
//            }

//            public static void Debug(System.Object data) => InternalLog(LogLevel.Debug, data);
//            public static void Info(System.Object data) => InternalLog(LogLevel.Info, data);
//            public static void Message(System.Object data) => InternalLog(LogLevel.Message, data);
//            public static void Warning(System.Object data) => InternalLog(LogLevel.Warning, data);
//            public static void Error(System.Object data) => InternalLog(LogLevel.Error, data);
//            public static void Fatal(System.Object data) => InternalLog(LogLevel.Fatal, data);

            
//            public static void Counter([CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0)
//            {
//                if(!counters.ContainsKey(member))
//                {
//                    counters[member] = 0UL;
//                }

//                InternalLog(LogLevel.None, $"{counters[member]++}, member: {member}, line: {line}", member, line);
//            }
//            public static void ClearCounter(Boolean toConsole = false, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0)
//            {
//                if(!counters.ContainsKey(member))
//                {
//                    return;
//                }

//                counters[member] = 0UL;
//                if(toConsole)
//                {
//                    InternalLog(LogLevel.None, $"Counter cleared for member: {member}, line: {line}", member, line);
//                }
//            }
//            private static readonly Dictionary<String,UInt64> counters = new Dictionary<String, UInt64>();

//            private static void InternalLog(LogLevel level, System.Object data)
//            {

//                Boolean log = false;
//                switch(level)
//                {
//                    case LogLevel.Debug:
//                        log = true;
//                        break;
//                    case LogLevel.Info:
//                        log = true;
//                        break;
//                    case LogLevel.Message:
//                        log = true;
//                        break;
//                    case LogLevel.Warning:
//                        log = true;
//                        break;
//                    case LogLevel.Error:
//                        log = true;
//                        break;
//                    case LogLevel.Fatal:
//                        log = true;
//                        break;
//                    default:
//                        logger.Log(LogLevel.Info, data);
//                        break;

//                }

//                if(log)
//                {
//                    logger.Log(level, data);
//                }
//            }
//            private static void InternalLog(LogLevel level, System.Object data, String member, Int32 line)
//            {

//                Boolean log = false;
//                switch(level)
//                {
//                    case LogLevel.Debug:
//                        log = true;
//                        break;
//                    case LogLevel.Info:
//                        log = true;
//                        break;
//                    case LogLevel.Message:
//                        log = true;
//                        break;
//                    case LogLevel.Warning:
//                        log = true;
//                        break;
//                    case LogLevel.Error:
//                        log = true;
//                        break;
//                    case LogLevel.Fatal:
//                        log = true;
//                        break;
//                    default:
//                        logger.Log(LogLevel.Info, data);
//                        break;

//                }

//                if(log)
//                {
//                    logger.Log(level, data);
//                }
//            }
//        }


//        #endregion


//        protected void OnEnable() => this.AddHooks();
//        protected void OnDisable() => this.RemoveHooks();

//        protected abstract void AddHooks();
//        protected abstract void RemoveHooks();
//    }

//}

//public class TestPlugin : RoR2Plugin<TestPlugin>
//{

//}