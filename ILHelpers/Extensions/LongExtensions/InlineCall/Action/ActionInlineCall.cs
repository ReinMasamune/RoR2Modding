namespace ILHelpers
{
    public static unsafe partial class CallInlineExtensions
    {
        public static ICursor<TStack> CallInline<TStack>(this ICursor<TStack> stack, delegate*<void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, TStack>(this ICursor<IL<T1, TStack>> stack, delegate*<T1, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, TStack>(this ICursor<IL<ByRef<T1>, TStack>> stack, delegate*<ref T1, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, TStack>(this ICursor<IL<T2, IL<T1, TStack>>> stack, delegate*<T1, T2, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, TStack>(this ICursor<IL<T2, IL<ByRef<T1>, TStack>>> stack, delegate*<ref T1, T2, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, TStack>(this ICursor<IL<ByRef<T2>, IL<T1, TStack>>> stack, delegate*<T1, ref T2, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, TStack>(this ICursor<IL<ByRef<T2>, IL<ByRef<T1>, TStack>>> stack, delegate*<ref T1, ref T2, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, TStack>(this ICursor<IL<T3, IL<T2, IL<T1, TStack>>>> stack, delegate*<T1, T2, T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, TStack>(this ICursor<IL<T3, IL<T2, IL<ByRef<T1>, TStack>>>> stack, delegate*<ref T1, T2, T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, TStack>(this ICursor<IL<T3, IL<ByRef<T2>, IL<T1, TStack>>>> stack, delegate*<T1, ref T2, T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, TStack>(this ICursor<IL<T3, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>> stack, delegate*<ref T1, ref T2, T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, TStack>(this ICursor<IL<ByRef<T3>, IL<T2, IL<T1, TStack>>>> stack, delegate*<T1, T2, ref T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, TStack>(this ICursor<IL<ByRef<T3>, IL<T2, IL<ByRef<T1>, TStack>>>> stack, delegate*<ref T1, T2, ref T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, TStack>(this ICursor<IL<ByRef<T3>, IL<ByRef<T2>, IL<T1, TStack>>>> stack, delegate*<T1, ref T2, ref T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, TStack>(this ICursor<IL<ByRef<T3>, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>> stack, delegate*<ref T1, ref T2, ref T3, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>> stack, delegate*<T1, T2, T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<T4, IL<T3, IL<T2, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, T2, T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<T4, IL<T3, IL<ByRef<T2>, IL<T1, TStack>>>>> stack, delegate*<T1, ref T2, T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<T4, IL<T3, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, ref T2, T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<T2, IL<T1, TStack>>>>> stack, delegate*<T1, T2, ref T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<T2, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, T2, ref T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<ByRef<T2>, IL<T1, TStack>>>>> stack, delegate*<T1, ref T2, ref T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, ref T2, ref T3, T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<T2, IL<T1, TStack>>>>> stack, delegate*<T1, T2, T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<T2, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, T2, T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<ByRef<T2>, IL<T1, TStack>>>>> stack, delegate*<T1, ref T2, T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, ref T2, T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<T2, IL<T1, TStack>>>>> stack, delegate*<T1, T2, ref T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<T2, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, T2, ref T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<ByRef<T2>, IL<T1, TStack>>>>> stack, delegate*<T1, ref T2, ref T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<TStack> CallInline<T1, T2, T3, T4, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, ref T2, ref T3, ref T4, void> action)
            where TStack : IStack
        {
            return default;
        }
    }
}
