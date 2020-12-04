namespace ReinCore
{
    public unsafe interface IMethod
    {

    }

    public unsafe interface IOp : IMethod
    {
        delegate*<void> op { get; }
    }

    public unsafe interface IOp<TIn> : IMethod
    {
        delegate*<TIn, void> op { get; }
    }

    public unsafe interface IFn<TRet> : IMethod
    {
        delegate*<TRet> fn { get; }
    }

    public unsafe interface IFn<TIn, TRet> : IMethod
    {
        delegate*<TIn, TRet> fn { get; }
    }

}
