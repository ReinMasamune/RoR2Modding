namespace ReinCore
{
    using System;
    using MonoMod.Cil;
    using Mono.Cecil;
    using MonoMod.Utils;
    using System.Linq;
    using Mono.Cecil.Cil;
    using System.Reflection;

    public static class ILCursorExtensions
    {
        public static ILCursor DefLabel(this ILCursor cursor, out ILLabel label)
        {
            label = cursor.DefineLabel();
            return cursor;
        }
        public static ILCursor Mark(this ILCursor cursor, out ILLabel label)
        {
            label = cursor.MarkLabel();
            return cursor;
        }
        public static ILCursor Mark(this ILCursor cursor, ILLabel label)
        {
            cursor.MarkLabel(label);
            return cursor;
        }
        public static ILCursor BrFalse_(this ILCursor cursor, ILLabel target) => cursor.Emit(OpCodes.Brfalse, target);
        public static ILCursor Br_(this ILCursor cursor, ILLabel target) => cursor.Emit(OpCodes.Br, target);
        public static ILCursor Is_(this ILCursor cursor, Type t) => cursor.Emit(OpCodes.Isinst, t);
        public static ILCursor Switch_(this ILCursor cursor, params ILLabel[] targets) => cursor.Emit(OpCodes.Switch, targets);
        public static ILCursor New_(this ILCursor cursor, ConstructorInfo constructor) => cursor.EmitNewObj(constructor);
        public static ILCursor Dup_(this ILCursor cursor) => cursor.Emit(OpCodes.Dup);
        public static ILCursor Pop_(this ILCursor cursor) => cursor.Emit(OpCodes.Pop);
        public static ILCursor Nop_(this ILCursor cursor) => cursor.Emit(OpCodes.Nop);
        public static ILCursor Call_(this ILCursor cursor, MethodInfo target) => cursor.Emit(OpCodes.Call, target);
        public static ILCursor Calli_<TDelegate>(this ILCursor cursor, TDelegate target)
            where TDelegate : Delegate
            => cursor.EmitIndirectCall<TDelegate>(target);
        public static ILCursor CallDel_<TDelegate>(this ILCursor cursor, TDelegate target, out Int32 index)
            where TDelegate : Delegate
        {
            index = cursor.EmitDelegate<TDelegate>(target);
            return cursor;
        }
        public static ILCursor CallDel_<TDelegate>(this ILCursor cursor, TDelegate target)
            where TDelegate : Delegate => cursor.CallDel_(target, out _);

        public static ILCursor LdArg_(this ILCursor cursor, UInt16 index) => cursor.EmitLoadArgument(index);
        public static ILCursor LdC_(this ILCursor cursor, String value) => cursor.Emit(OpCodes.Ldstr, value);
        public static ILCursor LdC_(this ILCursor cursor, Int32 value) => cursor.Emit(OpCodes.Ldc_I4, value);
        public static ILCursor LdC_(this ILCursor cursor, Int64 value) => cursor.Emit(OpCodes.Ldc_I8, value);
        public static ILCursor LdC_(this ILCursor cursor, Single value) => cursor.Emit(OpCodes.Ldc_R4, value);
        public static ILCursor LdC_(this ILCursor cursor, Double value) => cursor.Emit(OpCodes.Ldc_R8, value);
        public static ILCursor StLoc_(this ILCursor cursor, Int32 index) => cursor.Emit(OpCodes.Stloc, index);
        public static ILCursor LdLoc_(this ILCursor cursor, Int32 index) => cursor.Emit(OpCodes.Ldloc, index);
        public static ILCursor LdFld_(this ILCursor cursor, FieldInfo field) => cursor.Emit(OpCodes.Ldfld, field);
        public static ILCursor StSFld_(this ILCursor cursor, FieldInfo field) => cursor.Emit(OpCodes.Stsfld, field);
        public static ILCursor Ret_(this ILCursor cursor) => cursor.Emit(OpCodes.Ret);

        public static ILCursor Move(this ILCursor cursor, Int32 offset)
        {
            cursor.Index += offset;
            return cursor;
        }

        public static ILCursor AddRef<T>(this ILCursor cursor, T value, out Int32 index)
        {
            index = cursor.AddReference<T>(value);
            return cursor;
        }
        public static ILCursor GetRef<T>(this ILCursor cursor, Int32 index)
        {
            cursor.EmitGetReference<T>(index);
            return cursor;
        }

        public static ILCursor EmitRef<T>(this ILCursor cursor, T value, out Int32 index)
        {
            index = cursor.EmitReference<T>(value);
            return cursor;
        }


        public static ILCursor EmitIndirectCall<TDelegate>(this ILCursor cursor, TDelegate del) where TDelegate : Delegate
        {
            var proc = cursor.IL;
            var delMethod = del.Method;

            var site = new CallSite( proc.Import( delMethod.ReturnParameter.ParameterType ) )
            {
                CallingConvention = MethodCallingConvention.StdCall,
            };

            foreach(var param in delMethod.GetParameters())
            {
                var attributes = (Mono.Cecil.ParameterAttributes) param.Attributes;
                site.Parameters.Add(new ParameterDefinition(param.Name, attributes, proc.Import(param.ParameterType)));
            }
            _ = cursor.EmitReference<IntPtr>(delMethod.MethodHandle.GetFunctionPointer());
            _ = cursor.Emit(OpCodes.Calli, site);
            return cursor;
        }

        private static ILCursor EmitLoadArgument(this ILCursor cursor, UInt16 index) => index switch
        {
            0 => cursor.Emit(OpCodes.Ldarg_0),
            1 => cursor.Emit(OpCodes.Ldarg_1),
            2 => cursor.Emit(OpCodes.Ldarg_2),
            3 => cursor.Emit(OpCodes.Ldarg_3),
            UInt16 ind when ind < Byte.MaxValue => cursor.Emit(OpCodes.Ldarg_S, (Byte)index),
            _ => cursor.Emit(OpCodes.Ldarg, index),
        };

        private static ILCursor EmitNewObj(this ILCursor cursor, ConstructorInfo constructor) => cursor.Emit(OpCodes.Newobj, constructor);
    }
}