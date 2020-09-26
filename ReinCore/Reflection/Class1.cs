//namespace ReinCore.Reflection
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Reflection;
//    using BF = System.Reflection.BindingFlags;
//    using System.Text;

//    using MonoMod.Utils;
//    using Mono.Cecil.Cil;
//    using System.Net.Http.Headers;

//    internal static class SyntaxVerifier
//    {
//        private static void Check()
//        {
//            var thing = 67;
//            thing.GetFieldValue("", out int val);
//        }
//    }

//    public ref struct Ref<T>
//    {
        
//    }


//    public static class ReflectionExtensions
//    {
//        public static Boolean GetFieldValue<TInstance, TValue>(ref this TInstance self, String name, out TValue value)
//            where TInstance : struct
//        {
//            var success = self.GetFieldValue(name, out value, out var ex);
//            if(!success)
//            {
//                if(ex is null)
//                {
//                    Log.Error($"{new NullReferenceException("Reflection failed, but returned a null expection. Good luck!")}");
//                } else
//                {
//                    Log.Error($"Exception thrown during reflection.\n{ex}");
//                }
//            }
//            return success;
//        }
//        public static Boolean GetFieldValue<TInstance, TValue>(ref this TInstance self, String name, out TValue value, out Exception error)
//            where TInstance : struct
//        {

//        }



//        public static Boolean GetFieldValue<TInstance, TValue>(this TInstance self, String name, out TValue value, out Exception error)
//            where TInstance : class
//        {

//        }

//        public static Boolean SetFieldValue<TInstance, TValue>(ref this TInstance self, String name, TValue value1, out Exception error)
//            where TInstance : struct
//        {

//        }

//        public static Boolean SetFieldValue<TInstance, TValue>(this TInstance self, String name, TValue value, out Exception error)
//            where TInstance : class
//        {

//        }
//    }




//    internal static class InternalReflHelpers
//    {
//        private static Int32 dmdCounter = 0;
//        internal static GetDelegate<TInst, TVal> CompileGet<TInst, TVal>(String name)
//        {
//            var field = FindField(typeof(TInst), typeof(TVal), name, false);
//            if(field is null) return null;
//            using(var dmd = new DynamicMethodDefinition($"<name><{dmdCounter++}>", typeof(TVal).MakeByRefType(), new[] { typeof(TInst) }))
//            {
//                var proc = dmd.GetILProcessor();
//                proc.Emit(OpCodes.Ldarg_1);
//                proc.Emit(OpCodes.Ldflda, field);
//                proc.Emit(OpCodes.Ret);
//                return dmd.Generate().ToDelegate<GetDelegate<TInst, TVal>>();
//            }
//        }

//        private static TDelegate ToDelegate<TDelegate>(this MethodInfo method)
//            where TDelegate : Delegate
//        {
//            return (TDelegate)method.CreateDelegate<TDelegate>();
//        }
//        private static FieldInfo FindField(Type on, Type t, String name, Boolean isStatic)
//        {
//            static FieldInfo FindFromFields(FieldInfo[] fields, Type type, String name)
//            {
//                for(Int32 i = 0; i < fields.Length; ++i)
//                {
//                    var f = fields[i];
//                    if(f is null) continue;
//                    if(f.Name == name && f.FieldType == type) return f;
//                }
//                return null;
//            }

//            if(on is null) return null;
//            if(t is null) return null;
//            if(name is null) return null;
//            var curFlags = BF.Public;
//            if(isStatic)
//            {
//                curFlags |= BF.Static;
//            } else
//            {
//                curFlags |= BF.Instance;
//            }
//            var fields = on.GetFields(curFlags);
//            var field = FindFromFields(fields, t, name);
//            if(field is not null) return field;
//            curFlags &= ~BF.Public;

//            curFlags |= BF.NonPublic;
//            fields = on.GetFields(curFlags);
//            field = FindFromFields(fields, t, name);
//            if(field is not null) return field;
//            curFlags &= ~BF.NonPublic;

//            curFlags |= BF.Public;
//            curFlags |= BF.FlattenHierarchy;
//            fields = on.GetFields(curFlags);
//            field = FindFromFields(fields, t, name);
//            if(field is not null) return field;

//            curFlags &= ~BF.Public;
//            fields = on.GetFields(curFlags);
//            field = FindFromFields(fields, t, name);
//            if(field is not null) return field;


//            Log.Error($"Unable to find field: {name} of type: {t.FullName} on type: {on.FullName}");
//            return null;
//        }
//    }


//    public delegate ref TValue GetDelegate<TInstance, TValue>(TInstance instance);
//    public delegate ref TValue RefGetDelegate<TInstance, TValue>(ref TInstance instance);
//    public delegate ref TValue StaticGetDelegate<TValue>();
//    internal static class ClassTypeCache<T>
//        where T : class
//    {
//        internal static GetDelegate<T, TVal> GetGetter<TVal>(String name)
//        {
//            if(ValueTypeCache<TVal>.cachedGetters.TryGetValue(name, out var res))
//            {
//                return res;
//            }
//            return ValueTypeCache<TVal>.cachedGetters[name] = InternalReflHelpers.CompileGet<T, TVal>(name);
//        }

//        private static class ValueTypeCache<TVal>
//        {
//            internal static readonly Dictionary<String, GetDelegate<T, TVal>> cachedGetters = new Dictionary<String, GetDelegate<T, TVal>>();
//        }
//    }

//    internal static class StructTypeCache<T>
//        where T : struct
//    {
//        internal static RefGetDelegate<T, TVal> GetGetter<TVal>(String name)
//        {
//            if(ValueTypeCache<TVal>.cachedGetters.TryGetValue(name, out var res))
//            {
//                return res;
//            }
//            return ValueTypeCache<TVal>.cachedGetters[name] = InternalReflHelpers.CompileRefGet<T, TVal>(name);
//        }

//        private static class ValueTypeCache<TVal>
//        {
//            internal static readonly Dictionary<String, RefGetDelegate<T, TVal>> cachedGetters = new Dictionary<String, GetDelegate<T, TVal>>();
//        }
//    }

//    internal static class ObjectTypeCache
//    {

//        private static class ValueTypeCache<TVal>
//        {

//        }
//    }

//    internal static class StaticTypeCache
//    {

//    }
//}
