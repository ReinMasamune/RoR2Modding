//namespace HookGeneratorUsage
//{
//    using System;
//    using System.Globalization;
//    using System.Linq;
//    using System.Reflection;
//    using System.Security.Cryptography;

//    //using Hooks;

//    public class Class1
//    {
//        void DoStuff()
//        {
            
//        }
//    }


//    public abstract class BaseHook
//    {
//        private static BindingFlags binFlags => BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;

//        private protected static MethodBase FindMethod(Type on, String name, IHookData.ReturnInfo returnInfo, IHookData.ParameterInfo[] parameterInfos)
//        {
//            Boolean MatchName(MethodInfo m) => m.Name == name;
//            (MethodInfo method, ParameterInfo returnParam, ParameterInfo[] parameters) Project(MethodInfo m) => (m, m.ReturnParameter, m.GetParameters());
//            Boolean MatchParamCount((MethodInfo method, ParameterInfo returnParam, ParameterInfo[] parameters) data) => data.parameters.Length == parameterInfos.Length;
//            Boolean MatchReturnParam((MethodInfo method, ParameterInfo returnParam, ParameterInfo[] parameters) data)
//            {
//                var ret = data.returnParam;
                
//                return ret.ParameterType == returnInfo.type
//            }


//            var methods = on.GetMethods(binFlags)
//                .Where(MatchName)
//                .Select(Project)
//                .Where(MatchParamCount);
//            return null;
//        }
//    }


//    public abstract class GenHook<THook, TData> : BaseHook
//        where THook : MulticastDelegate
//        where TData : struct, IHookData
//    {
//        private static MethodBase targetMethod { get; }

//        static GenHook()
//        {
//            var data = new TData();

//            targetMethod = FindMethod(data.on, data.name, data.returnInfo, data.parameterInfos);
//        }

//        private protected static void AddOn(THook hook)
//        {

//        }
//        private protected static void RemoveOn(THook hook)
//        {

//        }

//        private protected static void AddIL()
//        {

//        }
//        private protected static void RemoveIL()
//        {

//        }
//    }

//    public sealed class EXHook : GenHook<EXHook.OnHook, EXHook.Data>
//    {
//        public delegate void Orig();
//        public delegate void OnHook(Orig orig);

//        public static event OnHook On
//        {
//            add => AddOn(value);
//            remove => RemoveOn(value);
//        }
//        public static event Action IL
//        {
//            add => AddIL();
//            remove => RemoveIL();
//        }

//        public struct Data : IHookData
//        {

//        }
//    }

//    public interface IHookData
//    {
//        Type on { get; }
//        String name { get; }
//        public struct ReturnInfo
//        {
//            public Type type;
//        }
//        public struct ParameterInfo
//        {
//            public Type type;
//        }
//        ReturnInfo returnInfo { get; }
//        ParameterInfo[] parameterInfos { get; }
//    }


//}
