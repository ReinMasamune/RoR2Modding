namespace ILHelper
{
    using System;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;

    using MonoMod.Cil;

    using Object = System.Object;


    public readonly struct MethodRef<TSig>
        where TSig : Delegate
    {
        internal readonly MethodInfo method;

        public MethodRef(MethodInfo method)
        {
            this.method = method;
            // TODO: Check method against signature
        }
    }
}
