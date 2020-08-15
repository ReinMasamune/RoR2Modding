namespace ILHelpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;

    using MonoMod.Cil;

    using Object = System.Object;

    public interface IDMD<out TDelegate>
        where TDelegate : Delegate
    {
        TDelegate function { get; }
    }
}