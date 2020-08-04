namespace ILHelper
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using Object = System.Object;

    public static class UnstackedCursorReadExtensions
    {
        public static TCursor Goto<TCursor>( this TCursor cursor, Instruction instruction, MoveType moveType = MoveType.Before, Boolean setTarget = false )
            where TCursor : ICursorRead, ICursor
        {
            cursor._Goto( instruction, moveType, setTarget );
            return cursor;
        }
        public static Boolean TryGotoNext<TCursor>( this TCursor cursor, MoveType moveType = MoveType.Before, params Func<Instruction, Boolean>[] predicates )
            where TCursor : ICursorRead, ICursor
        {
            return cursor._TryGotoNext( moveType, predicates );
        }
        public static Boolean TryGotoPrev<TCursor>( this TCursor cursor, MoveType moveType = MoveType.Before, params Func<Instruction, Boolean>[] predicates )
            where TCursor : ICursorRead, ICursor
        {
            return cursor._TryGotoPrev( moveType, predicates );
        }
        public static Boolean IsAfter<TCursor>( this TCursor cursor, Instruction instr )
            where TCursor : ICursorRead, ICursor
        {
            return cursor._IsAfter( instr );
        }
        public static Boolean IsBefore<TCursor>( this TCursor cursor, Instruction instr )
            where TCursor : ICursorRead, ICursor
        {
            return cursor._IsBefore( instr );
        }



        public static TCursor Goto<TCursor>( this TCursor cursor, Int32 index, MoveType moveType = MoveType.Before, Boolean setTarget = false )
            where TCursor : ICursorRead, ICursor
        {
            if( index < 0 ) index += cursor.instructions.Count;
            return cursor.Goto( index == cursor.instructions.Count ? null : cursor.instructions[index], moveType, setTarget );
        }
        public static TCursor GotoLabel<TCursor>( this TCursor cursor, ILLabel label, MoveType moveType = MoveType.AfterLabel, Boolean setTarget = false )
            where TCursor : ICursorRead, ICursor
        {
            return cursor.Goto( label.Target, moveType, setTarget );
        }
        public static TCursor GotoNext<TCursor>( this TCursor cursor, params Func<Instruction, Boolean>[] predicates )
            where TCursor : ICursorRead, ICursor
        {
            return cursor.GotoNext( MoveType.Before, predicates );
        }
        public static TCursor GotoPrev<TCursor>( this TCursor cursor, params Func<Instruction, Boolean>[] predicates )
            where TCursor : ICursorRead, ICursor
        {
            return cursor.GotoPrev( MoveType.Before, predicates );
        }
        public static Boolean TryGotoNext<TCursor>( this TCursor cursor, params Func<Instruction, Boolean>[] predicates )
            where TCursor : ICursorRead, ICursor
        {
            return cursor.TryGotoNext( MoveType.Before, predicates );
        }
        public static Boolean TryGotoPrev<TCursor>( this TCursor cursor, params Func<Instruction, Boolean>[] predicates )
            where TCursor : ICursorRead, ICursor
        {
            return cursor.TryGotoPrev( MoveType.Before, predicates );
        }
        public static TCursor GotoNext<TCursor>( this TCursor cursor, MoveType moveType = MoveType.Before, params Func<Instruction, Boolean>[] predicates )
            where TCursor : ICursorRead, ICursor
        {
            if( !cursor.TryGotoNext( moveType, predicates ) ) throw new KeyNotFoundException();
            return cursor;
        }
        public static TCursor GotoPrev<TCursor>( this TCursor cursor, MoveType moveType = MoveType.Before, params Func<Instruction, Boolean>[] predicates )
            where TCursor : ICursorRead, ICursor
        {
            if( !cursor.TryGotoPrev( moveType, predicates ) ) throw new KeyNotFoundException();
            return cursor;
        }
    }
}