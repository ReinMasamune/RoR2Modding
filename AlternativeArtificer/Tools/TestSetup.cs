namespace AlternativeArtificer.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal static class Test
    {
        internal static void Do()
        {

        }
    }

    internal struct Module1 : IModule<Module1>, IHasNextInit<Module1, Module2, IResult>
    {
        public void _Construct() => throw new NotImplementedException();
        public IResult _Init() => throw new NotImplementedException();
    }
    internal struct Module2 : IModule<Module2>, IHasNextInit<Module2, Module3, IResult>
    {
        public void _Construct() => throw new NotImplementedException();
        public IResult _Init() => throw new NotImplementedException();
    }
    internal struct Module3 : IModule<Module3>, IHasNextInit<Module3, Module4, IResult>
    {
        public void _Construct() => throw new NotImplementedException();
        public IResult _Init() => throw new NotImplementedException();
    }
    internal struct Module4 : IModule<Module4>, IHasNextInit<Module4, Module5, IResult>
    {
        public void _Construct() => throw new NotImplementedException();
        public IResult _Init() => throw new NotImplementedException();
    }
    internal struct Module5 : IModule<Module5>, ILastInit<Module5, IResult>
    {
        public void _Construct() => throw new NotImplementedException();
        public IResult _Init() => throw new NotImplementedException();
    }
}
