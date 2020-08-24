namespace AlternativeArtificer.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Text;

   

    internal interface IResult { }
    internal interface IPassed : IResult { }
    internal interface IFailed : IResult { }



    internal interface IResults<TResult> : IResult
        where TResult : IResult
    {
        void _Add(TResult res);
    }


}
