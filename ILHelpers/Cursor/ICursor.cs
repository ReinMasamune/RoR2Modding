namespace ILHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ILHelpers.Emit;

    public interface ICursor<TStack>
        where TStack : IStack
    {
        internal IEmitter emitter { get; }
        internal IMethodManager manager { get; }
        internal ICursor<TNewStack> CreateNew<TNewStack>()
            where TNewStack : IStack;
    }

    internal static class Cursor
    {
        internal static ICursor<TStack> Create<TStack>(IEmitter emitter, IMethodManager manager)
            where TStack : IStack
        {
            return new CecilCursor<TStack>(emitter, manager);
        }

        internal static ICursor<IL<TPushed,TStack>> Push<TPushed, TStack>( this ICursor<TStack> cursor )
            where TStack : IStack
        {
            return cursor.CreateNew<IL<TPushed, TStack>>();
        }

        internal static ICursor<TStack> Pop<TPopped, TStack>( this ICursor<IL<TPopped, TStack>> cursor )
            where TStack : IStack
        {
            return cursor.CreateNew<TStack>();
        }
    }

    internal class CecilCursor<TStack> : ICursor<TStack>
    where TStack : IStack
    {
        internal CecilCursor(IEmitter emitter, IMethodManager manager)
        {
            this.emitter = emitter;
            this.manager = manager;
        }
        private readonly IEmitter emitter;
        private readonly IMethodManager manager;

        IEmitter ICursor<TStack>.emitter { get => this.emitter; }

        IMethodManager ICursor<TStack>.manager { get => this.manager; }

        ICursor<TNewStack> ICursor<TStack>.CreateNew<TNewStack>()
        {
            return new CecilCursor<TNewStack>(this.emitter, this.manager);
        }
    }
}
