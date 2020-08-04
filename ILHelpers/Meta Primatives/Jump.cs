namespace ILHelper
{
    using System;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;

    using MonoMod.Cil;

    using Object = System.Object;


    public readonly struct ILJump<TReturn, TSignature>
        where TReturn : IDMDReturn
        where TSignature : Delegate
    {
        internal TReturn ret {get;}
        internal MethodInfo target { get; }
    }
}
