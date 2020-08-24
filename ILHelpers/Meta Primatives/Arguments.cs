namespace ILHelpers
{
    using System;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;

    using MonoMod.Cil;

    using Object = System.Object;


    public class Arg<T>
    {
        internal readonly ParameterDefinition parameter;
        internal Arg( ParameterDefinition parameter )
        {
            this.parameter = parameter;
        }
    }

    internal static class Argument
    {
        internal static Arg<T> Create<T>(ParameterDefinition parameter) => new Arg<T>(parameter);
    }
}
