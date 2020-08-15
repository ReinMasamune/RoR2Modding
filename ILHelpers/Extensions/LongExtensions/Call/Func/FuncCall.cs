namespace ILHelpers
{
    public static unsafe partial class CallExtensions
    {
        public static ICursor<IL<TReturn,TStack>> Call<TReturn, TStack>(this ICursor<TStack> stack, delegate*<TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<TReturn, TStack>(this ICursor<TStack> stack, delegate*<ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, TReturn, TStack>(this ICursor<IL<T1, TStack>> stack, delegate*<T1, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, TReturn, TStack>(this ICursor<IL<T1, TStack>> stack, delegate*<T1, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, TReturn, TStack>(this ICursor<IL<ByRef<T1>, TStack>> stack, delegate*<ref T1, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, TReturn, TStack>(this ICursor<IL<ByRef<T1>, TStack>> stack, delegate*<ref T1, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, TReturn, TStack>(this ICursor<IL<T2, IL<T1, TStack>>> stack, delegate*<T1, T2, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, TReturn, TStack>(this ICursor<IL<T2, IL<T1, TStack>>> stack, delegate*<T1, T2, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, TReturn, TStack>(this ICursor<IL<T2, IL<ByRef<T1>, TStack>>> stack, delegate*<ref T1, T2, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, TReturn, TStack>(this ICursor<IL<T2, IL<ByRef<T1>, TStack>>> stack, delegate*<ref T1, T2, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, TReturn, TStack>(this ICursor<IL<ByRef<T2>, IL<T1, TStack>>> stack, delegate*<T1, ref T2, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, TReturn, TStack>(this ICursor<IL<ByRef<T2>, IL<T1, TStack>>> stack, delegate*<T1, ref T2, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, TReturn, TStack>(this ICursor<IL<ByRef<T2>, IL<ByRef<T1>, TStack>>> stack, delegate*<ref T1, ref T2, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, TReturn, TStack>(this ICursor<IL<ByRef<T2>, IL<ByRef<T1>, TStack>>> stack, delegate*<ref T1, ref T2, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<T2, IL<T1, TStack>>>> stack, delegate*<T1, T2, T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<T2, IL<T1, TStack>>>> stack, delegate*<T1, T2, T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<T2, IL<ByRef<T1>, TStack>>>> stack, delegate*<ref T1, T2, T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<T2, IL<ByRef<T1>, TStack>>>> stack, delegate*<ref T1, T2, T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<ByRef<T2>, IL<T1, TStack>>>> stack, delegate*<T1, ref T2, T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<ByRef<T2>, IL<T1, TStack>>>> stack, delegate*<T1, ref T2, T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>> stack, delegate*<ref T1, ref T2, T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>> stack, delegate*<ref T1, ref T2, T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<T2, IL<T1, TStack>>>> stack, delegate*<T1, T2, ref T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<T2, IL<T1, TStack>>>> stack, delegate*<T1, T2, ref T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<T2, IL<ByRef<T1>, TStack>>>> stack, delegate*<ref T1, T2, ref T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<T2, IL<ByRef<T1>, TStack>>>> stack, delegate*<ref T1, T2, ref T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<ByRef<T2>, IL<T1, TStack>>>> stack, delegate*<T1, ref T2, ref T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<ByRef<T2>, IL<T1, TStack>>>> stack, delegate*<T1, ref T2, ref T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>> stack, delegate*<ref T1, ref T2, ref T3, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>> stack, delegate*<ref T1, ref T2, ref T3, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>> stack, delegate*<T1, T2, T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>> stack, delegate*<T1, T2, T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<T2, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, T2, T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<T2, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, T2, T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<ByRef<T2>, IL<T1, TStack>>>>> stack, delegate*<T1, ref T2, T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<ByRef<T2>, IL<T1, TStack>>>>> stack, delegate*<T1, ref T2, T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, ref T2, T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, ref T2, T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<T2, IL<T1, TStack>>>>> stack, delegate*<T1, T2, ref T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<T2, IL<T1, TStack>>>>> stack, delegate*<T1, T2, ref T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<T2, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, T2, ref T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<T2, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, T2, ref T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<ByRef<T2>, IL<T1, TStack>>>>> stack, delegate*<T1, ref T2, ref T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<ByRef<T2>, IL<T1, TStack>>>>> stack, delegate*<T1, ref T2, ref T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, ref T2, ref T3, T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, ref T2, ref T3, T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<T2, IL<T1, TStack>>>>> stack, delegate*<T1, T2, T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<T2, IL<T1, TStack>>>>> stack, delegate*<T1, T2, T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<T2, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, T2, T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<T2, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, T2, T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<ByRef<T2>, IL<T1, TStack>>>>> stack, delegate*<T1, ref T2, T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<ByRef<T2>, IL<T1, TStack>>>>> stack, delegate*<T1, ref T2, T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, ref T2, T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, ref T2, T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<T2, IL<T1, TStack>>>>> stack, delegate*<T1, T2, ref T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<T2, IL<T1, TStack>>>>> stack, delegate*<T1, T2, ref T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<T2, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, T2, ref T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<T2, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, T2, ref T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<ByRef<T2>, IL<T1, TStack>>>>> stack, delegate*<T1, ref T2, ref T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<ByRef<T2>, IL<T1, TStack>>>>> stack, delegate*<T1, ref T2, ref T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, ref T2, ref T3, ref T4, TReturn> func)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> Call<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<ByRef<T2>, IL<ByRef<T1>, TStack>>>>> stack, delegate*<ref T1, ref T2, ref T3, ref T4, ref TReturn> func)
            where TStack : IStack
        {
            return default;
        }
    }
}
