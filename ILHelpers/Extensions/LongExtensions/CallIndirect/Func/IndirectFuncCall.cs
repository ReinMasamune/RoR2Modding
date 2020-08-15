namespace ILHelpers
{
    public static unsafe partial class CallExtensions
    {
        public static ICursor<IL<TReturn,TStack>> CallIndirect<TReturn, TStack>(this ICursor<IL<FuncPointer<TReturn>, TStack>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<TReturn, TStack>(this ICursor<IL<FuncPointer<ByRef<TReturn>>, TStack>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, TReturn, TStack>(this ICursor<IL<T1, IL<FuncPointer<T1, TReturn>, TStack>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, TReturn, TStack>(this ICursor<IL<T1, IL<FuncPointer<T1, ByRef<TReturn>>, TStack>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, TReturn, TStack>(this ICursor<IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, TReturn>, TStack>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, TReturn, TStack>(this ICursor<IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, ByRef<TReturn>>, TStack>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, TReturn, TStack>(this ICursor<IL<T2, IL<T1, IL<FuncPointer<T1, T2, TReturn>, TStack>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, TReturn, TStack>(this ICursor<IL<T2, IL<T1, IL<FuncPointer<T1, T2, ByRef<TReturn>>, TStack>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, TReturn, TStack>(this ICursor<IL<T2, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, T2, TReturn>, TStack>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, TReturn, TStack>(this ICursor<IL<T2, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, T2, ByRef<TReturn>>, TStack>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, TReturn, TStack>(this ICursor<IL<ByRef<T2>, IL<T1, IL<FuncPointer<T1, ByRef<T2>, TReturn>, TStack>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, TReturn, TStack>(this ICursor<IL<ByRef<T2>, IL<T1, IL<FuncPointer<T1, ByRef<T2>, ByRef<TReturn>>, TStack>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, TReturn, TStack>(this ICursor<IL<ByRef<T2>, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, ByRef<T2>, TReturn>, TStack>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, TReturn, TStack>(this ICursor<IL<ByRef<T2>, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, ByRef<T2>, ByRef<TReturn>>, TStack>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<T2, IL<T1, IL<FuncPointer<T1, T2, T3, TReturn>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<T2, IL<T1, IL<FuncPointer<T1, T2, T3, ByRef<TReturn>>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<T2, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, T2, T3, TReturn>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<T2, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, T2, T3, ByRef<TReturn>>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<ByRef<T2>, IL<T1, IL<FuncPointer<T1, ByRef<T2>, T3, TReturn>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<ByRef<T2>, IL<T1, IL<FuncPointer<T1, ByRef<T2>, T3, ByRef<TReturn>>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<ByRef<T2>, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, ByRef<T2>, T3, TReturn>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<T3, IL<ByRef<T2>, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, ByRef<T2>, T3, ByRef<TReturn>>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<T2, IL<T1, IL<FuncPointer<T1, T2, ByRef<T3>, TReturn>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<T2, IL<T1, IL<FuncPointer<T1, T2, ByRef<T3>, ByRef<TReturn>>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<T2, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, T2, ByRef<T3>, TReturn>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<T2, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, T2, ByRef<T3>, ByRef<TReturn>>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<ByRef<T2>, IL<T1, IL<FuncPointer<T1, ByRef<T2>, ByRef<T3>, TReturn>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<ByRef<T2>, IL<T1, IL<FuncPointer<T1, ByRef<T2>, ByRef<T3>, ByRef<TReturn>>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<ByRef<T2>, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, ByRef<T2>, ByRef<T3>, TReturn>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, TReturn, TStack>(this ICursor<IL<ByRef<T3>, IL<ByRef<T2>, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, ByRef<T2>, ByRef<T3>, ByRef<TReturn>>, TStack>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<T2, IL<T1, IL<FuncPointer<T1, T2, T3, T4, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<T2, IL<T1, IL<FuncPointer<T1, T2, T3, T4, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<T2, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, T2, T3, T4, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<T2, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, T2, T3, T4, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<ByRef<T2>, IL<T1, IL<FuncPointer<T1, ByRef<T2>, T3, T4, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<ByRef<T2>, IL<T1, IL<FuncPointer<T1, ByRef<T2>, T3, T4, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<ByRef<T2>, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, ByRef<T2>, T3, T4, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<T3, IL<ByRef<T2>, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, ByRef<T2>, T3, T4, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<T2, IL<T1, IL<FuncPointer<T1, T2, ByRef<T3>, T4, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<T2, IL<T1, IL<FuncPointer<T1, T2, ByRef<T3>, T4, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<T2, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, T2, ByRef<T3>, T4, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<T2, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, T2, ByRef<T3>, T4, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<ByRef<T2>, IL<T1, IL<FuncPointer<T1, ByRef<T2>, ByRef<T3>, T4, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<ByRef<T2>, IL<T1, IL<FuncPointer<T1, ByRef<T2>, ByRef<T3>, T4, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<ByRef<T2>, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, ByRef<T2>, ByRef<T3>, T4, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<T4, IL<ByRef<T3>, IL<ByRef<T2>, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, ByRef<T2>, ByRef<T3>, T4, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<T2, IL<T1, IL<FuncPointer<T1, T2, T3, ByRef<T4>, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<T2, IL<T1, IL<FuncPointer<T1, T2, T3, ByRef<T4>, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<T2, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, T2, T3, ByRef<T4>, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<T2, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, T2, T3, ByRef<T4>, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<ByRef<T2>, IL<T1, IL<FuncPointer<T1, ByRef<T2>, T3, ByRef<T4>, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<ByRef<T2>, IL<T1, IL<FuncPointer<T1, ByRef<T2>, T3, ByRef<T4>, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<ByRef<T2>, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, ByRef<T2>, T3, ByRef<T4>, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<T3, IL<ByRef<T2>, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, ByRef<T2>, T3, ByRef<T4>, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<T2, IL<T1, IL<FuncPointer<T1, T2, ByRef<T3>, ByRef<T4>, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<T2, IL<T1, IL<FuncPointer<T1, T2, ByRef<T3>, ByRef<T4>, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<T2, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, T2, ByRef<T3>, ByRef<T4>, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<T2, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, T2, ByRef<T3>, ByRef<T4>, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<ByRef<T2>, IL<T1, IL<FuncPointer<T1, ByRef<T2>, ByRef<T3>, ByRef<T4>, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<ByRef<T2>, IL<T1, IL<FuncPointer<T1, ByRef<T2>, ByRef<T3>, ByRef<T4>, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<TReturn,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<ByRef<T2>, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, ByRef<T2>, ByRef<T3>, ByRef<T4>, TReturn>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
        public static ICursor<IL<ByRef<TReturn>,TStack>> CallIndirect<T1, T2, T3, T4, TReturn, TStack>(this ICursor<IL<ByRef<T4>, IL<ByRef<T3>, IL<ByRef<T2>, IL<ByRef<T1>, IL<FuncPointer<ByRef<T1>, ByRef<T2>, ByRef<T3>, ByRef<T4>, ByRef<TReturn>>, TStack>>>>>> stack)
            where TStack : IStack
        {
            return default;
        }
    }
}
