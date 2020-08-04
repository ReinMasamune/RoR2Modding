namespace ILHelper
{
    using System;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;

    using MonoMod.Cil;

    using Object = System.Object;

    public interface IBranch
    {
        internal OpCode opcode { get; }
    }
    public static class BranchType
    {
        public struct DefaultBranch : IBranch { OpCode IBranch.opcode { get => OpCodes.Br; } }
        public static DefaultBranch Default { get => default; }
        // TODO: Implement
    }
}
