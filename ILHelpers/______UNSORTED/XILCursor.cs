namespace ILHelpers
{
    using System;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;

    using MonoMod.Cil;

    using Object = System.Object;

    public readonly struct XILCursor : ICursorRead, ICursorWrite
    {
        public static XILCursor Create( ILCursor cursor ) => new XILCursor( cursor );
        public static XILCursor Create( ILContext context ) => new XILCursor( context );

        public XILCursor( ILCursor cursor )
        {
            this.cursor = cursor;
        }
        public XILCursor( ILContext context )
        {
            this.cursor = new ILCursor( context );
        }
        private readonly ILCursor cursor;

        Int32 ICursor.index { get => this.cursor.Index; set => this.cursor.Index = value; }
        Collection<Instruction> ICursor.instructions { get => this.cursor.Instrs; }

        void ICursorRead._Goto( Instruction instr, MoveType moveType, Boolean setTarget ) => this.cursor.Goto( instr, moveType, setTarget );
        //void ICursorRead._Goto( Int32 index, MoveType moveType, Boolean setTarget ) => this.cursor.Goto( index, moveType, setTarget );
        //void ICursorRead._GotoLabel( ILLabel label, MoveType moveType, Boolean setTarget ) => this.cursor.GotoLabel( label, moveType, setTarget );
        //void ICursorRead._GotoNext( params Func<Instruction,Boolean>[] predicates ) => this.cursor.GotoNext( predicates );
        //void ICursorRead._GotoNext( MoveType moveType, params Func<Instruction,Boolean>[] predicates ) => this.cursor.GotoNext( moveType, predicates );
        //void ICursorRead._GotoPrev( params Func<Instruction,Boolean>[] predicates ) => this.cursor.GotoPrev( predicates );
        //void ICursorRead._GotoPrev( MoveType moveType, params Func<Instruction,Boolean>[] predicates ) => this.cursor.GotoPrev( moveType, predicates );
        Boolean ICursorRead._IsAfter( Instruction instr ) => this.cursor.IsAfter( instr );
        Boolean ICursorRead._IsBefore( Instruction instr ) => this.cursor.IsBefore( instr );
        //void ICursorRead._FindNext( out ILCursor[] cursors, params Func<Instruction,Boolean>[] predicates ) => this.cursor.FindNext( out cursors, predicates );
        //void ICursorRead._FindPrev( out ILCursor[] cursors, params Func<Instruction,Boolean>[] predicates ) => this.cursor.FindPrev( out cursors, predicates );
        //Boolean ICursorRead._TryFindNext( out ILCursor[] cursors, params Func<Instruction,Boolean>[] predicates ) => this.cursor.TryFindNext( out cursors, predicates );
        //Boolean ICursorRead._TryFindPrev( out ILCursor[] cursors, params Func<Instruction,Boolean>[] predicates ) => this.cursor.TryFindPrev( out cursors, predicates );
        Boolean ICursorRead._TryGotoNext( MoveType moveType, params Func<Instruction,Boolean>[] predicates ) => this.cursor.TryGotoNext( moveType, predicates );
        Boolean ICursorRead._TryGotoPrev( MoveType moveType, params Func<Instruction,Boolean>[] predicates ) => this.cursor.TryGotoPrev( moveType, predicates );

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
        void ICursorWrite._Remove() => throw new NotImplementedException();
        void ICursorWrite._RemoveRange( Int32 range ) => throw new NotImplementedException();
        Int32 ICursorWrite._AddReference<T>( T t ) => throw new NotImplementedException();
        void ICursorWrite._EmitGetReference<T>( Int32 index ) => throw new NotImplementedException();
        Int32 ICursorWrite._EmitReference<T>( T t ) => throw new NotImplementedException();
        Int32 ICursorWrite._EmitDelegate<T>( T del ) => throw new NotImplementedException();
        ILLabel ICursorWrite._DefineLabel() => throw new NotImplementedException();
        ILLabel ICursorWrite._MarkLabel() => throw new NotImplementedException();
        void ICursorWrite._MarkLabel( ILLabel label ) => throw new NotImplementedException();
        void ICursorWrite._MoveAfterLabels() => throw new NotImplementedException();
        void ICursorWrite._MoveBeforeLabels() => throw new NotImplementedException();
    }
}