﻿namespace ILHelpers
{
    using System;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;

    using MonoMod.Cil;

    using Object = System.Object;


    public interface IDMDReturn { }
    public struct DMDReturn : IDMDReturn { }
    public struct DMDReturn<T> : IDMDReturn { }
}
