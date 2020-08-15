namespace ILHelpers
{
    using System;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;

    using MonoMod.Cil;

    using Object = System.Object;

    public interface ICursor
    {
        internal Int32 index { get; set; }
        internal Collection<Instruction> instructions { get; }
    }

    public interface ICursorRead : ICursor
    {
        internal void _Goto( Instruction instr, MoveType moveType, Boolean setTarget );
        //internal void _Goto( Int32 index, MoveType moveType, Boolean setTarget );
        //internal void _GotoLabel( ILLabel label, MoveType moveType, Boolean setTarget );
        //internal void _GotoNext( params Func<Instruction,Boolean>[] predicates );
        //internal void _GotoNext( MoveType moveType, params Func<Instruction,Boolean>[] predicates );
        //internal void _GotoPrev( params Func<Instruction,Boolean>[] predicates );
        //internal void _GotoPrev( MoveType moveType, params Func<Instruction,Boolean>[] predicates );
        internal Boolean _TryGotoNext( MoveType moveType, params Func<Instruction, Boolean>[] predicates );
        internal Boolean _TryGotoPrev( MoveType moveType, params Func<Instruction, Boolean>[] predicates );
        internal Boolean _IsAfter( Instruction instr );
        internal Boolean _IsBefore( Instruction instr );
        //internal void _FindNext( out ILCursor[] cursors, params Func<Instruction,Boolean>[] predicates );
        //internal void _FindPrev( out ILCursor[] cursors, params Func<Instruction,Boolean>[] predicates );
        //internal Boolean _TryFindNext( out ILCursor[] cursors, params Func<Instruction,Boolean>[] predicates );
        //internal Boolean _TryFindPrev( out ILCursor[] cursors, params Func<Instruction,Boolean>[] predicates );
    }

    public interface ICursorWrite : ICursor
    {
        internal void _Emit( OpCode opcode );
        internal void _Emit( OpCode opcode, Object obj );
        internal void _Emit( OpCode opcode, Type type );
        internal void _Emit( OpCode opcode, MethodBase method );
        internal void _Emit( OpCode opcode, FieldInfo field );
        internal void _Emit( OpCode opcode, MethodReference method );
        internal void _Emit( OpCode opcode, Int32 value );
        internal void _Emit( OpCode opcode, TypeReference type );
        internal void _Emit( OpCode opcode, CallSite callsite );
        internal void _Emit( OpCode opcode, FieldReference field );
        internal void _Emit( OpCode opcode, String value );
        internal void _Emit( OpCode opcode, Byte value );
        internal void _Emit( OpCode opcode, SByte value );
        internal void _Emit( OpCode opcode, Int64 value );
        internal void _Emit( OpCode opcode, Single value );
        internal void _Emit( OpCode opcode, Double value );
        internal void _Emit( OpCode opcode, Instruction instruction );
        internal void _Emit( OpCode opcode, Instruction[] instructions );
        internal void _Emit( OpCode opcode, VariableDefinition variable );
        internal void _Emit( OpCode opcode, ParameterDefinition parameter );
        internal void _Remove();
        internal void _RemoveRange( Int32 range );
        internal Int32 _AddReference<T>( T t );
        internal void _EmitGetReference<T>( Int32 index );
        internal Int32 _EmitReference<T>( T t );
        internal Int32 _EmitDelegate<T>( T del ) where T : Delegate;
        internal ILLabel _DefineLabel();
        internal ILLabel _MarkLabel();
        internal void _MarkLabel( ILLabel label );
        internal void _MoveAfterLabels();
        internal void _MoveBeforeLabels();
    }
}