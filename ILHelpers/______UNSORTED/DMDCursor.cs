namespace ILHelper
{
    using System;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;

    using MonoMod.Cil;

    using Object = System.Object;

    public interface IDMDCursor<TEmitReciever>
        where TEmitReciever : ICursorWrite, ICursor
    {
        ILLocal<T> AddLocal<T>();
        internal TEmitReciever reciever { get; }
    }


    public readonly struct DMDCursor<TStack, TSignature> : IDMDCursor<XILCursor>
        where TStack : IStack
        where TSignature : Delegate
    {
        internal DMDCursor( TStack stack, ILContext context )
        {
            this.cursor = new XILCursor( context );
            this.stack = stack;
        }
        internal DMDCursor( TStack stack, XILCursor cursor )
        {
            this.cursor = cursor;
            this.stack = stack;
        }
        internal readonly XILCursor cursor;
        internal readonly TStack stack;

        public ILLocal<T> AddLocal<T>() => default;


        internal DMDCursor<IL<TValue, TStack>, TSignature> _Push<TValue>( XILCursor cursor ) => new DMDCursor<IL<TValue, TStack>, TSignature>( this.stack.Push<TValue, TStack>(), cursor );
        internal DMDCursor<IL<TValue, TStack>, TSignature> _Push<TValue>() => new DMDCursor<IL<TValue, TStack>, TSignature>( this.stack.Push<TValue, TStack>(), this.cursor );
        internal DMDCursor<TStack, TSignature> _Emit( XILCursor cursor ) => new DMDCursor<TStack, TSignature>( this.stack, cursor );

        XILCursor IDMDCursor<XILCursor>.reciever => this.cursor;
    }

    internal static class DMDCursorExtensions
    {
        internal static DMDCursor<TStack, TSig> _Pop<TValue, TStack, TSig>(this DMDCursor<IL<TValue, TStack>, TSig> stack, XILCursor cursor)
            where TStack : IStack
            where TSig : Delegate => new DMDCursor<TStack, TSig>( stack.stack.Pop(), cursor );
        internal static DMDCursor<TStack, TSig> _Pop<TValue, TStack, TSig>(this DMDCursor<IL<TValue, TStack>, TSig> stack)
            where TStack : IStack
            where TSig : Delegate => new DMDCursor<TStack, TSig>( stack.stack.Pop(), stack.cursor );
    }



}