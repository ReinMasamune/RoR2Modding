namespace ILHelpers
{
    using System;

    using Mono.Cecil;

    using MonoMod.Utils;

    public static unsafe partial class DMD
    {
        public static unsafe delegate*<void> Compile(delegate*<ICursor<Empty>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, Type.EmptyTypes))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                return (delegate*<void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<T1, void> Compile<T1>(delegate*<ICursor<Empty>, Arg<T1>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1) }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<T1>>(dmd.Definition.Parameters[1]);
                return (delegate*<T1, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<ref T1, void> Compile<T1>(delegate*<ICursor<Empty>, Arg<ByRef<T1>>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1).MakeByRefType() }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<ByRef<T1>>>(dmd.Definition.Parameters[1]);
                return (delegate*<ref T1, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<T1, T2, void> Compile<T1, T2>(delegate*<ICursor<Empty>, Arg<T1>, Arg<T2>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1), typeof(T2) }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<T1>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<T2>>(dmd.Definition.Parameters[2]);
                return (delegate*<T1, T2, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<ref T1, T2, void> Compile<T1, T2>(delegate*<ICursor<Empty>, Arg<ByRef<T1>>, Arg<T2>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1).MakeByRefType(), typeof(T2) }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<ByRef<T1>>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<T2>>(dmd.Definition.Parameters[2]);
                return (delegate*<ref T1, T2, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<T1, ref T2, void> Compile<T1, T2>(delegate*<ICursor<Empty>, Arg<T1>, Arg<ByRef<T2>>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1), typeof(T2).MakeByRefType() }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<T1>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<ByRef<T2>>>(dmd.Definition.Parameters[2]);
                return (delegate*<T1, ref T2, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<ref T1, ref T2, void> Compile<T1, T2>(delegate*<ICursor<Empty>, Arg<ByRef<T1>>, Arg<ByRef<T2>>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1).MakeByRefType(), typeof(T2).MakeByRefType() }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<ByRef<T1>>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<ByRef<T2>>>(dmd.Definition.Parameters[2]);
                return (delegate*<ref T1, ref T2, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<T1, T2, T3, void> Compile<T1, T2, T3>(delegate*<ICursor<Empty>, Arg<T1>, Arg<T2>, Arg<T3>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1), typeof(T2), typeof(T3) }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<T1>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<T2>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<T3>>(dmd.Definition.Parameters[3]);
                return (delegate*<T1, T2, T3, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<ref T1, T2, T3, void> Compile<T1, T2, T3>(delegate*<ICursor<Empty>, Arg<ByRef<T1>>, Arg<T2>, Arg<T3>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1).MakeByRefType(), typeof(T2), typeof(T3) }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<ByRef<T1>>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<T2>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<T3>>(dmd.Definition.Parameters[3]);
                return (delegate*<ref T1, T2, T3, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<T1, ref T2, T3, void> Compile<T1, T2, T3>(delegate*<ICursor<Empty>, Arg<T1>, Arg<ByRef<T2>>, Arg<T3>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1), typeof(T2).MakeByRefType(), typeof(T3) }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<T1>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<ByRef<T2>>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<T3>>(dmd.Definition.Parameters[3]);
                return (delegate*<T1, ref T2, T3, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<ref T1, ref T2, T3, void> Compile<T1, T2, T3>(delegate*<ICursor<Empty>, Arg<ByRef<T1>>, Arg<ByRef<T2>>, Arg<T3>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1).MakeByRefType(), typeof(T2).MakeByRefType(), typeof(T3) }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<ByRef<T1>>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<ByRef<T2>>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<T3>>(dmd.Definition.Parameters[3]);
                return (delegate*<ref T1, ref T2, T3, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<T1, T2, ref T3, void> Compile<T1, T2, T3>(delegate*<ICursor<Empty>, Arg<T1>, Arg<T2>, Arg<ByRef<T3>>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1), typeof(T2), typeof(T3).MakeByRefType() }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<T1>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<T2>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<ByRef<T3>>>(dmd.Definition.Parameters[3]);
                return (delegate*<T1, T2, ref T3, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<ref T1, T2, ref T3, void> Compile<T1, T2, T3>(delegate*<ICursor<Empty>, Arg<ByRef<T1>>, Arg<T2>, Arg<ByRef<T3>>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1).MakeByRefType(), typeof(T2), typeof(T3).MakeByRefType() }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<ByRef<T1>>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<T2>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<ByRef<T3>>>(dmd.Definition.Parameters[3]);
                return (delegate*<ref T1, T2, ref T3, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<T1, ref T2, ref T3, void> Compile<T1, T2, T3>(delegate*<ICursor<Empty>, Arg<T1>, Arg<ByRef<T2>>, Arg<ByRef<T3>>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1), typeof(T2).MakeByRefType(), typeof(T3).MakeByRefType() }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<T1>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<ByRef<T2>>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<ByRef<T3>>>(dmd.Definition.Parameters[3]);
                return (delegate*<T1, ref T2, ref T3, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<ref T1, ref T2, ref T3, void> Compile<T1, T2, T3>(delegate*<ICursor<Empty>, Arg<ByRef<T1>>, Arg<ByRef<T2>>, Arg<ByRef<T3>>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1).MakeByRefType(), typeof(T2).MakeByRefType(), typeof(T3).MakeByRefType() }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<ByRef<T1>>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<ByRef<T2>>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<ByRef<T3>>>(dmd.Definition.Parameters[3]);
                return (delegate*<ref T1, ref T2, ref T3, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<T1, T2, T3, T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<T1>, Arg<T2>, Arg<T3>, Arg<T4>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<T1>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<T2>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<T3>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<T4>>(dmd.Definition.Parameters[4]);
                return (delegate*<T1, T2, T3, T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<ref T1, T2, T3, T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<ByRef<T1>>, Arg<T2>, Arg<T3>, Arg<T4>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1).MakeByRefType(), typeof(T2), typeof(T3), typeof(T4) }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<ByRef<T1>>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<T2>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<T3>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<T4>>(dmd.Definition.Parameters[4]);
                return (delegate*<ref T1, T2, T3, T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<T1, ref T2, T3, T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<T1>, Arg<ByRef<T2>>, Arg<T3>, Arg<T4>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1), typeof(T2).MakeByRefType(), typeof(T3), typeof(T4) }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<T1>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<ByRef<T2>>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<T3>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<T4>>(dmd.Definition.Parameters[4]);
                return (delegate*<T1, ref T2, T3, T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<ref T1, ref T2, T3, T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<ByRef<T1>>, Arg<ByRef<T2>>, Arg<T3>, Arg<T4>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1).MakeByRefType(), typeof(T2).MakeByRefType(), typeof(T3), typeof(T4) }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<ByRef<T1>>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<ByRef<T2>>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<T3>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<T4>>(dmd.Definition.Parameters[4]);
                return (delegate*<ref T1, ref T2, T3, T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<T1, T2, ref T3, T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<T1>, Arg<T2>, Arg<ByRef<T3>>, Arg<T4>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1), typeof(T2), typeof(T3).MakeByRefType(), typeof(T4) }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<T1>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<T2>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<ByRef<T3>>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<T4>>(dmd.Definition.Parameters[4]);
                return (delegate*<T1, T2, ref T3, T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<ref T1, T2, ref T3, T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<ByRef<T1>>, Arg<T2>, Arg<ByRef<T3>>, Arg<T4>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1).MakeByRefType(), typeof(T2), typeof(T3).MakeByRefType(), typeof(T4) }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<ByRef<T1>>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<T2>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<ByRef<T3>>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<T4>>(dmd.Definition.Parameters[4]);
                return (delegate*<ref T1, T2, ref T3, T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<T1, ref T2, ref T3, T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<T1>, Arg<ByRef<T2>>, Arg<ByRef<T3>>, Arg<T4>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1), typeof(T2).MakeByRefType(), typeof(T3).MakeByRefType(), typeof(T4) }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<T1>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<ByRef<T2>>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<ByRef<T3>>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<T4>>(dmd.Definition.Parameters[4]);
                return (delegate*<T1, ref T2, ref T3, T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<ref T1, ref T2, ref T3, T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<ByRef<T1>>, Arg<ByRef<T2>>, Arg<ByRef<T3>>, Arg<T4>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1).MakeByRefType(), typeof(T2).MakeByRefType(), typeof(T3).MakeByRefType(), typeof(T4) }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<ByRef<T1>>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<ByRef<T2>>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<ByRef<T3>>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<T4>>(dmd.Definition.Parameters[4]);
                return (delegate*<ref T1, ref T2, ref T3, T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<T1, T2, T3, ref T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<T1>, Arg<T2>, Arg<T3>, Arg<ByRef<T4>>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4).MakeByRefType() }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<T1>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<T2>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<T3>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<ByRef<T4>>>(dmd.Definition.Parameters[4]);
                return (delegate*<T1, T2, T3, ref T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<ref T1, T2, T3, ref T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<ByRef<T1>>, Arg<T2>, Arg<T3>, Arg<ByRef<T4>>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1).MakeByRefType(), typeof(T2), typeof(T3), typeof(T4).MakeByRefType() }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<ByRef<T1>>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<T2>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<T3>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<ByRef<T4>>>(dmd.Definition.Parameters[4]);
                return (delegate*<ref T1, T2, T3, ref T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<T1, ref T2, T3, ref T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<T1>, Arg<ByRef<T2>>, Arg<T3>, Arg<ByRef<T4>>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1), typeof(T2).MakeByRefType(), typeof(T3), typeof(T4).MakeByRefType() }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<T1>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<ByRef<T2>>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<T3>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<ByRef<T4>>>(dmd.Definition.Parameters[4]);
                return (delegate*<T1, ref T2, T3, ref T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<ref T1, ref T2, T3, ref T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<ByRef<T1>>, Arg<ByRef<T2>>, Arg<T3>, Arg<ByRef<T4>>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1).MakeByRefType(), typeof(T2).MakeByRefType(), typeof(T3), typeof(T4).MakeByRefType() }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<ByRef<T1>>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<ByRef<T2>>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<T3>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<ByRef<T4>>>(dmd.Definition.Parameters[4]);
                return (delegate*<ref T1, ref T2, T3, ref T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<T1, T2, ref T3, ref T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<T1>, Arg<T2>, Arg<ByRef<T3>>, Arg<ByRef<T4>>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1), typeof(T2), typeof(T3).MakeByRefType(), typeof(T4).MakeByRefType() }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<T1>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<T2>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<ByRef<T3>>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<ByRef<T4>>>(dmd.Definition.Parameters[4]);
                return (delegate*<T1, T2, ref T3, ref T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<ref T1, T2, ref T3, ref T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<ByRef<T1>>, Arg<T2>, Arg<ByRef<T3>>, Arg<ByRef<T4>>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1).MakeByRefType(), typeof(T2), typeof(T3).MakeByRefType(), typeof(T4).MakeByRefType() }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<ByRef<T1>>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<T2>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<ByRef<T3>>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<ByRef<T4>>>(dmd.Definition.Parameters[4]);
                return (delegate*<ref T1, T2, ref T3, ref T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<T1, ref T2, ref T3, ref T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<T1>, Arg<ByRef<T2>>, Arg<ByRef<T3>>, Arg<ByRef<T4>>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1), typeof(T2).MakeByRefType(), typeof(T3).MakeByRefType(), typeof(T4).MakeByRefType() }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<T1>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<ByRef<T2>>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<ByRef<T3>>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<ByRef<T4>>>(dmd.Definition.Parameters[4]);
                return (delegate*<T1, ref T2, ref T3, ref T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
        public static unsafe delegate*<ref T1, ref T2, ref T3, ref T4, void> Compile<T1, T2, T3, T4>(delegate*<ICursor<Empty>, Arg<ByRef<T1>>, Arg<ByRef<T2>>, Arg<ByRef<T3>>, Arg<ByRef<T4>>, DMDReturn> body, String name = null)
        {
            using(var dmd = new DynamicMethodDefinition(DMD.CheckName(name), null, new[] { typeof(T1).MakeByRefType(), typeof(T2).MakeByRefType(), typeof(T3).MakeByRefType(), typeof(T4).MakeByRefType() }))
            {
                var manager = MethodManager.Create(dmd.Definition);
                var cursor = manager.InitCursor();
                var arg1 = new Arg<ICursor<Empty>>(dmd.Definition.Parameters[0]);
                var arg2 = new Arg<Arg<ByRef<T1>>>(dmd.Definition.Parameters[1]);
                var arg3 = new Arg<Arg<ByRef<T2>>>(dmd.Definition.Parameters[2]);
                var arg4 = new Arg<Arg<ByRef<T3>>>(dmd.Definition.Parameters[3]);
                var arg5 = new Arg<Arg<ByRef<T4>>>(dmd.Definition.Parameters[4]);
                return (delegate*<ref T1, ref T2, ref T3, ref T4, void>)(void*)dmd.Generate().MethodHandle.GetFunctionPointer();
            }
        }
    }
}
