//namespace ReinCore
//{
//    using System;
//    using System.Collections.Concurrent;
//    using System.Collections.Generic;
//    using System.ComponentModel;
//    using System.Diagnostics;
//    using System.Reflection;

//    using EntityStates;

//    using MonoMod.RuntimeDetour;

//    using RoR2;
//    using RoR2.Skills;

//    using UnityEngine;

//    public static class ErrorsCore
//    {
//        public static Boolean loaded { get; internal set; } = false;




//        static ErrorsCore()
//        {
//            loaded = true;

//            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
//        }


//        private enum ModIndex 
//        {
//            Invalid = -1,
//        }
//        private struct ModInfo
//        {
//            internal String guid;
//            internal String version;
//        }
//        private struct ExceptionInfo
//        {
//            internal UInt32 counter;
//            internal Exception exception;
//            internal Dictionary<ModIndex, Single> modsInTrace;
//            internal Dictionary<ModIndex, Single> modsHookedToTrace;


//            internal void Inc() => this.counter = Math.Max(this.counter, this.counter + 1u);

//        }

//        const Single startingInvolvement = 100f;
//        const Single involvementDecayWithDepth = 0.75f;
//        const Single involvementHookMultiplier = 0.5f;

//        private static readonly Assembly ror2Asm = typeof(RoR2Application).Assembly;

//        private static readonly List<ModInfo> currentMods = new();

//        private static readonly Dictionary<MethodBase, HashSet<ModIndex>> hookedBy = new();
//        private static readonly Dictionary<Assembly, ModIndex> assemblyToMod = new();


//        private static readonly ConcurrentQueue<Exception> errorsToProcess = new();

//        private static readonly Dictionary<Exception, ExceptionInfo> handledExceptions = new(new ExceptionEqualityComparer());

//        private struct ExceptionEqualityComparer : IEqualityComparer<Exception>
//        {
//            private static String GetRep(Exception e) => e.GetType().Name + e.Message + e.StackTrace;
//            public Boolean Equals(Exception x, Exception y) => GetRep(x) == GetRep(y);
//            public Int32 GetHashCode(Exception obj) => GetRep(obj).GetHashCode();
//        }

//        internal static void PassTime(Single time)
//        {

//        }


//        private static void ProcessError(Exception? exception)
//        {
//            if(exception is null) return;
//            if(handledExceptions.TryGetValue(exception, out var info))
//            {
//                info.Inc();
//            }



//            var trace = new StackTrace(exception);

//            var involvementDict = new Dictionary<ModIndex, Single>();

//            if(exception is InvalidProgramException ipe)
//            {
//                //Special case for IL hook conflict
//            } else
//            {
//                var involvement = startingInvolvement;
//                var totalInvolvement = 0f;
//                for(Int32 i = 0; i < trace.FrameCount; ++i)
//                {
//                    var frame = trace.GetFrame(i);
//                    if(frame is null) continue;

//                    var method = frame.GetMethod();
//                    var assembly = method.DeclaringType.Assembly;

//                    if(assemblyToMod.TryGetValue(assembly, out var mod))
//                    {
//                        if(!involvementDict.TryGetValue(mod, out var inv)) inv = 0f;
//                        inv += involvement;
//                        totalInvolvement += involvement;
//                        involvementDict[mod] = inv;
//                    }
                    
//                    if(hookedBy.TryGetValue(method, out var mods))
//                    {
//                        foreach(var modInd in mods)
//                        {
//                            if(!involvementDict.TryGetValue(modInd, out var inv)) inv = 0f;
//                            inv += involvement * involvementHookMultiplier;

//                            involvementDict[modInd] = inv;
//                        }
//                    }

//                    involvement *= involvementDecayWithDepth;
//                }

//                //Sort by involvement and cull by some threshold
//            }
//        }

//        private static void CurrentDomain_FirstChanceException(System.Object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
//        {
//            errorsToProcess.Enqueue(e.Exception);
//        }

//        private static Boolean ShouldProcess()
//        {
//            return false;
//        }


        
//    }
//}
