namespace ILHelpers.Emit
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    internal interface IEmitter
    {
        internal void Emit(OpCode opcode);
        internal void Emit(OpCode opcode, MethodReference method);
        internal void Emit(OpCode opcode, Int32 value);
        internal void Emit(OpCode opcode, TypeReference type);
        internal void Emit(OpCode opcode, CallSite callsite);
        internal void Emit(OpCode opcode, FieldReference field);
        internal void Emit(OpCode opcode, String value);
        internal void Emit(OpCode opcode, Byte value);
        internal void Emit(OpCode opcode, SByte value);
        internal void Emit(OpCode opcode, Int64 value);
        internal void Emit(OpCode opcode, Single value);
        internal void Emit(OpCode opcode, Double value);
        internal void Emit(OpCode opcode, Instruction instruction);
        internal void Emit(OpCode opcode, Instruction[] instructions);
        internal void Emit(OpCode opcode, VariableDefinition variable);
        internal void Emit(OpCode opcode, ParameterDefinition parameter);
    }



    internal static class Emit
    {
        internal static IEmitter GetEmitter(MethodBody method)
        {
            return new CecilEmitter(method);
        }



        private class CecilEmitter : IEmitter
        {
            internal CecilEmitter(MethodBody body)
            {
                this.body = body;
            }

            private MethodBody _body;
            private MethodBody body
            {
                get => this._body;
                set
                {
                    this.proc = value.GetILProcessor();
                    this._body = value;
                }
            }

            private ILProcessor proc { get; set; }

            void IEmitter.Emit(OpCode opcode) => this.proc.Emit(opcode);
            void IEmitter.Emit(OpCode opcode, Byte value) => this.proc.Emit(opcode, value);
            void IEmitter.Emit(OpCode opcode, CallSite callsite) => this.proc.Emit(opcode, callsite);
            void IEmitter.Emit(OpCode opcode, Double value) => this.proc.Emit(opcode, value);
            void IEmitter.Emit(OpCode opcode, FieldReference field) => this.proc.Emit(opcode, field);
            void IEmitter.Emit(OpCode opcode, Single value) => this.proc.Emit(opcode, value);
            void IEmitter.Emit(OpCode opcode, Instruction target) => this.proc.Emit(opcode, target);
            void IEmitter.Emit(OpCode opcode, Instruction[] targets) => this.proc.Emit(opcode, targets);
            void IEmitter.Emit(OpCode opcode, Int32 value) => this.proc.Emit(opcode, value);
            void IEmitter.Emit(OpCode opcode, Int64 value) => this.proc.Emit(opcode, value);
            void IEmitter.Emit(OpCode opcode, MethodReference method) => this.proc.Emit(opcode, method);
            void IEmitter.Emit(OpCode opcode, ParameterDefinition parameter) => this.proc.Emit(opcode, parameter);
            void IEmitter.Emit(OpCode opcode, SByte value) => this.proc.Emit(opcode, value);
            void IEmitter.Emit(OpCode opcode, String value) => this.proc.Emit(opcode, value);
            void IEmitter.Emit(OpCode opcode, TypeReference type) => this.proc.Emit(opcode, type);
            void IEmitter.Emit(OpCode opcode, VariableDefinition variable) => this.proc.Emit(opcode, variable);
        }
    }
}
