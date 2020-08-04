namespace ILHelper
{
    using System;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;

    using MonoMod.Cil;

    using Object = System.Object;

    public static class Test
    {
        public static void Testing()
        {
        }
    }

    public struct Empty : IStack
    { }
    public struct IL<TValue, TRest> : IStack
        where TRest : IStack
    { }

    public struct Label<TStack> : ILabel<TStack>, IStack
        where TStack : IStack
    { }

    public struct LabelCursor : ICursorWrite, ICursor
    {
        Int32 ICursor.index { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        Collection<Instruction> ICursor.instructions { get => throw new NotImplementedException(); }

        Int32 ICursorWrite._AddReference<T>( T t ) => throw new NotImplementedException();
        ILLabel ICursorWrite._DefineLabel() => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, Object obj ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, Type type ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, MethodBase method ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, FieldInfo field ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, MethodReference method ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, Int32 value ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, TypeReference type ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, CallSite callsite ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, FieldReference field ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, String value ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, Byte value ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, SByte value ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, Int64 value ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, Single value ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, Double value ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, Instruction instruction ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, Instruction[] instructions ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, VariableDefinition variable ) => throw new NotImplementedException();
        void ICursorWrite._Emit( OpCode opcode, ParameterDefinition parameter ) => throw new NotImplementedException();
        Int32 ICursorWrite._EmitDelegate<T>( T del ) => throw new NotImplementedException();
        void ICursorWrite._EmitGetReference<T>( Int32 index ) => throw new NotImplementedException();
        Int32 ICursorWrite._EmitReference<T>( T t ) => throw new NotImplementedException();
        ILLabel ICursorWrite._MarkLabel() => throw new NotImplementedException();
        void ICursorWrite._MarkLabel( ILLabel label ) => throw new NotImplementedException();
        void ICursorWrite._MoveAfterLabels() => throw new NotImplementedException();
        void ICursorWrite._MoveBeforeLabels() => throw new NotImplementedException();
        void ICursorWrite._Remove() => throw new NotImplementedException();
        void ICursorWrite._RemoveRange( Int32 range ) => throw new NotImplementedException();
    }



    internal static class StackExtensions
    {
        internal static IL<TPushed, TStack> Push<TPushed, TStack>( this TStack stack )
            where TStack : IStack
        {
            return default;
        }
        internal static TStack Pop<TPopped, TStack>( this IL<TPopped, TStack> stack )
            where TStack : IStack
        {
            return default;
        }
    }


}
