namespace ILHelper
{
    using System;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;

    using MonoMod.Cil;

    using Object = System.Object;
    public interface IConversion
    {
        internal OpCode opcode { get; }
    }
    public static class ConversionType
    {
        // TODO: Implement
    }
}
