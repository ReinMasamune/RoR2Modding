namespace ILHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using Object = System.Object;

    public static class UnstackedCursorExtensions
    {
        public static TCursor Move<TCursor>( this TCursor cursor, Int32 offset )
            where TCursor : ICursor
        {
            cursor.index += offset;
            return cursor;
        }
    }
}