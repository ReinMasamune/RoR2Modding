namespace ILHelper
{
    using System;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using Object = System.Object;

    public interface IStack 
    { }
    public interface IHistory
    { }
    public interface ILabel<TStack>
        where TStack : IStack
    { }
}
