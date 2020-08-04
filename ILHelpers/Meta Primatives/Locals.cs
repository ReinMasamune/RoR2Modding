namespace ILHelper
{
    using System;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;

    using MonoMod.Cil;

    using Object = System.Object;


    public readonly struct ILLocal<T> 
    {
        internal readonly UInt16 index;

        internal ILLocal( UInt16 ind ) => this.index = ind;
    }
}
