namespace ILHelpers
{
    using System;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;

    using MonoMod.Cil;

    using Object = System.Object;


    public readonly struct ILAdd<TVal1, TVal2, TResult>
    {
        internal readonly Boolean unsigned;
        internal readonly Boolean overflow;
    }

    public static class Add
    {
        public static ILAdd<int, int, int> int_signed { get => new ILAdd<int, int, int>(); }
    }
}
