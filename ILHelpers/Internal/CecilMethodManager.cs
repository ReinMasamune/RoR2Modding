namespace ILHelpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using ILHelpers.Emit;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    internal interface IMethod { }

    internal interface IMethodManager
    {
        internal ICursor<Empty> InitCursor();
        internal (ICursor<TStack> cursor, IBranchRef<TStack> branchRef) GetNewBranchCursor<TStack>(ICursor<TStack> from)
            where TStack : IStack;
        internal void Finish(Boolean optimize = true);
        internal Local<T> AddLocal<T>();
    }



    internal static class MethodManager
    {
        internal static IMethodManager Create(MethodDefinition method)
        {
            return new CecilMethodManager(method);
        }
    }

    internal sealed class CecilMethodManager : IMethodManager
    {
        internal CecilMethodManager(MethodDefinition method)
        {
            this.method = method;
        }

        private readonly MethodDefinition method;
        internal readonly Instruction nop = Instruction.Create(OpCodes.Nop);
        private Branch entryBranch;


        ICursor<Empty> IMethodManager.InitCursor()
        {
            if(this.entryBranch is not null) throw new InvalidOperationException("Main cursor already created");
            var entryBranch = new Branch(this, this.method, true);
            this.entryBranch = entryBranch;
            return Cursor.Create<Empty>(entryBranch, this);
        }

        (ICursor<TStack> cursor, IBranchRef<TStack> branchRef) IMethodManager.GetNewBranchCursor<TStack>(ICursor<TStack> from)
        {
            var fromBranch = from.emitter as Branch;
            var fromInstr = fromBranch.instrSeq.Last();
            var toBranch = new Branch(this, this.method);
            fromBranch.next.Add(toBranch);
            var cursor = Cursor.Create<TStack>(toBranch, this);
            var branchRef = new CecilBranchRef<TStack>(cursor, fromInstr);
            toBranch.from = branchRef;
            return (cursor, branchRef);
        }
        void IMethodManager.Finish(Boolean optimize)
        {
            if(this.entryBranch is null) throw new InvalidOperationException("No method body");
            var proc = this.method.Body.GetILProcessor();

            Instruction current = null, prev = null;
            void EmitBranch(Branch branch)
            {
                while((current = branch.instrSeq.Dequeue()) is not null)
                {
                    current.Previous = prev;
                    if(prev is not null) prev.Next = current;
                    prev = current;
                    proc.Append(current);
                }
                foreach(var br in branch.next)
                {
                    if(br.instrSeq.Count == 0) continue;
                    EmitBranch(br);
                }
            }
            EmitBranch(this.entryBranch);
            // TODO: Emit Stuff god this is a pain
            //if(optimize) 
        }


        Local<T> IMethodManager.AddLocal<T>()
        {
            VariableDefinition def;
            switch(typeof(T))
            {
                case Type t when t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ByRef<>):
                    var arg = t.GetGenericArguments()[0];
                    def = new VariableDefinition(this.method.Module.ImportReference(arg.MakeByRefType()));
                    break;
                default:
                    def = new VariableDefinition(this.method.Module.ImportReference(typeof(T)));
                    break;
            }
            this.method.Body.Variables.Add(def);
            return new Local<T>(def);
        }

        private abstract class CecilBranchRef
        {
            protected internal CecilBranchRef(Instruction from)
            {
                this.from = from;
            }
            internal readonly Instruction from;
            internal void Update(Instruction target)
            {
                from.Operand = target;
            }
        }

        private class CecilBranchRef<TStack> : CecilBranchRef, IBranchRef<TStack>
            where TStack : IStack
        {
            internal CecilBranchRef(ICursor<TStack> cursor, Instruction from) : base(from)
            {
                this.cursor = cursor;
            }

            private readonly ICursor<TStack> cursor;
            ICursor<TStack> IBranchRef<TStack>.cursor { get => this.cursor; }
        }

        private class Branch : IEmitter
        {
            internal Branch(CecilMethodManager manager, MethodDefinition method, Boolean isFirst = false)
            {
                this.parent = manager;
                this.method = method;
                this.isComplete = false;
                this.isFirst = isFirst;
            }
            internal CecilBranchRef from;
            internal CecilMethodManager parent;
            internal MethodDefinition method;
            internal Boolean isComplete = false;
            internal readonly Queue<Instruction> instrSeq;

            internal readonly List<Branch> next = new List<Branch>();

            private Boolean isFirst;

            internal Instruction firstInstr;
            internal Instruction lastInstr;

            private void AddInstr(Instruction instruction)
            {
                if(this.isFirst)
                {
                    this.from.Update(instruction);
                    this.firstInstr = instruction;
                    this.isFirst = false;
                }
                this.instrSeq.Enqueue(instruction);
            }

            private MethodReference Import(MethodReference methodRef) => this.method.Module.ImportReference(methodRef);
            private TypeReference Import(TypeReference typeRef) => this.method.Module.ImportReference(typeRef);
            private FieldReference Import(FieldReference fieldRef) => this.method.Module.ImportReference(fieldRef);

            void IEmitter.Emit(OpCode opcode) => this.AddInstr(Instruction.Create(opcode));
            void IEmitter.Emit(OpCode opcode, String value) => this.AddInstr(Instruction.Create(opcode, value));
            void IEmitter.Emit(OpCode opcode, Byte value) => this.AddInstr(Instruction.Create(opcode, value));
            void IEmitter.Emit(OpCode opcode, SByte value) => this.AddInstr(Instruction.Create(opcode, value));
            void IEmitter.Emit(OpCode opcode, Int64 value) => this.AddInstr(Instruction.Create(opcode, value));
            void IEmitter.Emit(OpCode opcode, Single value) => this.AddInstr(Instruction.Create(opcode, value));
            void IEmitter.Emit(OpCode opcode, Double value) => this.AddInstr(Instruction.Create(opcode, value));
            void IEmitter.Emit(OpCode opcode, Int32 value) => this.AddInstr(Instruction.Create(opcode, value));

            void IEmitter.Emit(OpCode opcode, MethodReference method) => this.AddInstr(Instruction.Create(opcode, this.Import(method)));
            void IEmitter.Emit(OpCode opcode, TypeReference type) => this.AddInstr(Instruction.Create(opcode, this.Import(type)));
            void IEmitter.Emit(OpCode opcode, CallSite callsite) => this.AddInstr(Instruction.Create(opcode, callsite));
            void IEmitter.Emit(OpCode opcode, FieldReference field) => this.AddInstr(Instruction.Create(opcode, this.Import(field)));
            void IEmitter.Emit(OpCode opcode, Instruction instruction) => this.AddInstr(Instruction.Create(opcode, instruction));
            void IEmitter.Emit(OpCode opcode, Instruction[] instructions) => this.AddInstr(Instruction.Create(opcode, instructions));
            void IEmitter.Emit(OpCode opcode, VariableDefinition variable) => this.AddInstr(Instruction.Create(opcode, variable));
            void IEmitter.Emit(OpCode opcode, ParameterDefinition parameter) => this.AddInstr(Instruction.Create(opcode, parameter));
        }
    }
}
