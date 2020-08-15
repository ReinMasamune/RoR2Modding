namespace ILHelpers
{
    using System;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;

    using MonoMod.Cil;

    using Object = System.Object;


    public class Local<T> 
    {
        internal readonly VariableDefinition variable;
        internal Local(VariableDefinition variable)
        {
            this.variable = variable;
        }
    }
}
