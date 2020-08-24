namespace AlternativeArtificer.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    #region Message Interfaces


    internal interface IHasInit { }
    internal interface IHasInit<TResult> : IHasInit
    {
        TResult _Init();
    }
    internal interface IHasNextInit : IHasInit 
    {
        internal delegate IHasInit GetNextDelegate();

    }
    internal interface IHasNextInit<TCurrent, TNext, TResult> : IHasInit<TResult>, IHasNextInit
        where TCurrent : IModule<TCurrent>
        where TNext : IHasInit<TResult>, IModule<TNext>
    { }
    internal interface ILastInit : IHasInit { }
    internal interface ILastInit<TCurrent, TResult> : IHasInit<TResult>, ILastInit
        where TCurrent : IModule<TCurrent>
        where TResult : IResult
    { }
    #endregion
    internal interface IModule { }

    internal interface IModule<TModule> : IModule
        where TModule : IModule
    {
        void _Construct();
    }



    internal interface ITopModule : IModule, IHasInit<IResult>
    {

    }
}
