namespace AlternativeArtificer.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal static class Module
    {
        internal static void Construct<T>()
            where T : struct, IModule<T>
        {
            var t = new T();
            t._Construct();
            Instance<T>.Supply(t);
        }

        internal static void Init<T>()
            where T : struct, IHasInit, IModule<T>
        {
            switch(Instance<T>.get)
            {
                case 
            }
        }
    }

    internal static class ModuleExtensions
    {




        internal static ref TNext Init<TModule, TNext, TResult, TResults>(this IHasNextInit<TModule, TNext, TResult> _, TResults results)
            where TModule : struct, IModule<TModule>, IHasNextInit<TModule, TNext, TResult>
            where TNext : struct, IModule<TNext>, IHasInit<TResult>
            where TResult : IResult
            where TResults : IResults<TResult>
        {
            results._Add(Instance<TModule>.get._Init());
            return ref Instance<TNext>.get;
        }

        internal static TResults Init<TModule, TNext, TResult, TResults>(this ILastInit<TModule,TResult> _, TResults results)
            where TModule : struct, IModule<TModule>, IHasNextInit<TModule, TNext, TResult>
            where TNext : struct, IModule<TNext>, IHasInit<TResult>
            where TResult : IResult
            where TResults : IResults<TResult>
        {
            results._Add(Instance<TModule>.get._Init());
            return results;
        }

    }
}
