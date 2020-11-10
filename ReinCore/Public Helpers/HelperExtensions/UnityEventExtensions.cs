namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using MonoMod.Utils;

    using RoR2;

    using UnityEngine.Events;

    using Object = System.Object;
    using UnityObject = UnityEngine.Object;

    public static unsafe class UnityEventExtensions
    {
        private static void Test()
        {
            UnityEvent ev = default;
            CharacterBody body = default;
            ev.AddPersistentListener(body, (b) => b.RecalculateStats());
        }


        public static void AddPersistentListener<TObj>(this UnityEvent ev, TObj target, Expression<Action<TObj>> method)
            where TObj : UnityObject
        {
            InternalAddPersistentListener();
        }




        private static RuntimeMethodHandle? generatedMethodHandle;
        private static delegate*<void> InternalAddPersistentListener => (delegate*<void>)(generatedMethodHandle ??= EmitMethod()).GetFunctionPointer();


        private const BindingFlags allFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        private const BindingFlags pub = BindingFlags.Public;
        private const BindingFlags pri = BindingFlags.NonPublic;
        private const BindingFlags ins = BindingFlags.Instance;
        private const BindingFlags sta = BindingFlags.Static;
        private const BindingFlags bas = BindingFlags.FlattenHierarchy;

        private struct _UnityEventBase
        {
            internal static readonly Type type = typeof(UnityEventBase);
            internal static readonly FieldInfo m_Calls = type.GetField("m_Calls", allFlags);
            internal static readonly FieldInfo m_CallsDirty = type.GetField("m_CallsDirty", allFlags);
            internal static readonly FieldInfo m_PersistentCalls = type.GetField("m_PersistentCalls", allFlags);
        }

        private struct _PersistentCallGroup
        {
            internal static readonly Type type = _UnityEventBase.m_PersistentCalls.FieldType;
            internal static readonly FieldInfo m_Calls = type.GetField("m_Calls", allFlags);
        }

        private struct _PersistentCall
        {
            internal static readonly Type type = _PersistentCallGroup.m_Calls.FieldType.GetGenericArguments()[0];
            internal static readonly FieldInfo m_Target;
            internal static readonly FieldInfo m_MethodName;
            internal static readonly FieldInfo m_Mode;
            internal static readonly FieldInfo m_Arguments;
            internal static readonly FieldInfo m_CallState;
            internal static readonly ConstructorInfo _new;

        }


        private static RuntimeMethodHandle EmitMethod()
        {
            return default;
        }
    }
}
