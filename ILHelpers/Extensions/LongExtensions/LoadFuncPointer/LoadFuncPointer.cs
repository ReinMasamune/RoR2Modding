namespace ILHelpers
{
    public static unsafe partial class CallExtensions
    {
        public static ICursor<IL<ActionPointer, TStack>> LoadFuncPointer<TStack>(this ICursor<TStack> stack, delegate*<void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<TReturn>, TStack>> LoadFuncPointer<TReturn, TStack>(this ICursor<TStack> stack, delegate*<TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<TReturn>>, TStack>> LoadFuncPointer<TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<T1>, TStack>> LoadFuncPointer<T1, TStack>(this ICursor<TStack> stack, delegate*<T1, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, TReturn>, TStack>> LoadFuncPointer<T1, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<ByRef<T1>>, TStack>> LoadFuncPointer<T1, TStack>(this ICursor<TStack> stack, delegate*<ref T1, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, TReturn>, TStack>> LoadFuncPointer<T1, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<T1, T2>, TStack>> LoadFuncPointer<T1, T2, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, T2, TReturn>, TStack>> LoadFuncPointer<T1, T2, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, T2, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<ByRef<T1>, T2>, TStack>> LoadFuncPointer<T1, T2, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, T2, TReturn>, TStack>> LoadFuncPointer<T1, T2, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, T2, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<T1, ByRef<T2>>, TStack>> LoadFuncPointer<T1, T2, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, ByRef<T2>, TReturn>, TStack>> LoadFuncPointer<T1, T2, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, ByRef<T2>, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<ByRef<T1>, ByRef<T2>>, TStack>> LoadFuncPointer<T1, T2, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, ByRef<T2>, TReturn>, TStack>> LoadFuncPointer<T1, T2, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, ByRef<T2>, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<T1, T2, T3>, TStack>> LoadFuncPointer<T1, T2, T3, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, T2, T3, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, T2, T3, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<ByRef<T1>, T2, T3>, TStack>> LoadFuncPointer<T1, T2, T3, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, T2, T3, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, T2, T3, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<T1, ByRef<T2>, T3>, TStack>> LoadFuncPointer<T1, T2, T3, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, ByRef<T2>, T3, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, ByRef<T2>, T3, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<ByRef<T1>, ByRef<T2>, T3>, TStack>> LoadFuncPointer<T1, T2, T3, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, ByRef<T2>, T3, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, ByRef<T2>, T3, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<T1, T2, ByRef<T3>>, TStack>> LoadFuncPointer<T1, T2, T3, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, ref T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, T2, ByRef<T3>, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, ref T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, T2, ByRef<T3>, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, ref T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<ByRef<T1>, T2, ByRef<T3>>, TStack>> LoadFuncPointer<T1, T2, T3, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, ref T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, T2, ByRef<T3>, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, ref T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, T2, ByRef<T3>, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, ref T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<T1, ByRef<T2>, ByRef<T3>>, TStack>> LoadFuncPointer<T1, T2, T3, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, ref T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, ByRef<T2>, ByRef<T3>, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, ref T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, ByRef<T2>, ByRef<T3>, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, ref T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<ByRef<T1>, ByRef<T2>, ByRef<T3>>, TStack>> LoadFuncPointer<T1, T2, T3, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, ref T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, ByRef<T2>, ByRef<T3>, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, ref T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, ByRef<T2>, ByRef<T3>, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, ref T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<T1, T2, T3, T4>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, T2, T3, T4, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, T2, T3, T4, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<ByRef<T1>, T2, T3, T4>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, T2, T3, T4, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, T2, T3, T4, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<T1, ByRef<T2>, T3, T4>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, ByRef<T2>, T3, T4, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, ByRef<T2>, T3, T4, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<ByRef<T1>, ByRef<T2>, T3, T4>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, ByRef<T2>, T3, T4, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, ByRef<T2>, T3, T4, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<T1, T2, ByRef<T3>, T4>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, ref T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, T2, ByRef<T3>, T4, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, ref T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, T2, ByRef<T3>, T4, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, ref T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<ByRef<T1>, T2, ByRef<T3>, T4>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, ref T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, T2, ByRef<T3>, T4, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, ref T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, T2, ByRef<T3>, T4, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, ref T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<T1, ByRef<T2>, ByRef<T3>, T4>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, ref T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, ByRef<T2>, ByRef<T3>, T4, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, ref T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, ByRef<T2>, ByRef<T3>, T4, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, ref T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<ByRef<T1>, ByRef<T2>, ByRef<T3>, T4>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, ref T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, ByRef<T2>, ByRef<T3>, T4, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, ref T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, ByRef<T2>, ByRef<T3>, T4, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, ref T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<T1, T2, T3, ByRef<T4>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, T2, T3, ByRef<T4>, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, T2, T3, ByRef<T4>, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<ByRef<T1>, T2, T3, ByRef<T4>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, T2, T3, ByRef<T4>, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, T2, T3, ByRef<T4>, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<T1, ByRef<T2>, T3, ByRef<T4>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, ByRef<T2>, T3, ByRef<T4>, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, ByRef<T2>, T3, ByRef<T4>, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<ByRef<T1>, ByRef<T2>, T3, ByRef<T4>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, ByRef<T2>, T3, ByRef<T4>, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, ByRef<T2>, T3, ByRef<T4>, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<T1, T2, ByRef<T3>, ByRef<T4>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, ref T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, T2, ByRef<T3>, ByRef<T4>, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, ref T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, T2, ByRef<T3>, ByRef<T4>, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, T2, ref T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<ByRef<T1>, T2, ByRef<T3>, ByRef<T4>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, ref T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, T2, ByRef<T3>, ByRef<T4>, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, ref T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, T2, ByRef<T3>, ByRef<T4>, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, T2, ref T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<T1, ByRef<T2>, ByRef<T3>, ByRef<T4>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, ref T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, ByRef<T2>, ByRef<T3>, ByRef<T4>, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, ref T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<T1, ByRef<T2>, ByRef<T3>, ByRef<T4>, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<T1, ref T2, ref T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ActionPointer<ByRef<T1>, ByRef<T2>, ByRef<T3>, ByRef<T4>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, ref T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, ByRef<T2>, ByRef<T3>, ByRef<T4>, TReturn>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, ref T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<FuncPointer<ByRef<T1>, ByRef<T2>, ByRef<T3>, ByRef<T4>, ByRef<TReturn>>, TStack>> LoadFuncPointer<T1, T2, T3, T4, TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref T1, ref T2, ref T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
    }
}
