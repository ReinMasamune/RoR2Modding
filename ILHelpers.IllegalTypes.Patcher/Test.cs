namespace ILHelpers.IllegalTypes.Patcher
{
    using System;
    using System.Linq;
    using Mono.Cecil;
    using Mono.Cecil.Cil;

    public static class Stuff
    {
        public static void Test()
        {
            IL<int,IL<int,IL<int,IL<int,Empty>>>> v = default;

            var t = new MyType<float>();
        }
    }
}
