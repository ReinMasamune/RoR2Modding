namespace AlternativeArtificer.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ReinCore;

    internal interface IConstructionSequence
    {
        internal delegate IModuleSeq ConstructionSequenceDelegate();
        internal ConstructionSequenceDelegate _GetConstructionSequence { get; }
    }

    internal interface IModuleSeq
    {
        internal delegate IModuleSeq PopDelegate();
        internal delegate IModuleSeq ConstructPopDelegate(out Exception thrown);

        internal PopDelegate _Pop { get; }
        internal ConstructPopDelegate _ConstructPop { get; }
        internal UInt32 length { get; }
    }
    internal struct EmptySeq : IModuleSeq 
    {
        IModuleSeq.PopDelegate IModuleSeq._Pop => throw new InvalidOperationException();
        IModuleSeq.ConstructPopDelegate IModuleSeq._ConstructPop => throw new InvalidOperationException();

        UInt32 IModuleSeq.length => 0u;

        internal ModSeq<TPushed, EmptySeq> Push<TPushed>()
            where TPushed : struct, IModule<TPushed>
        {
            return default;
        }
    }
    internal struct ModSeq<T, TRest> : IModuleSeq
        where T : struct, IModule<T>
        where TRest : struct, IModuleSeq
    {
        UInt32 IModuleSeq.length => new TRest().length + 1;
        IModuleSeq.PopDelegate IModuleSeq._Pop => () => default(TRest);
        IModuleSeq.ConstructPopDelegate IModuleSeq._ConstructPop => (out Exception exception) =>
        {
            exception = null;
            try
            {
                Module.Construct<T>();
            } catch( Exception e) { exception = e; }
            return default(TRest);
        };

        

        private ModSeq<TPushed, ModSeq<T, TRest>> Push<TPushed>()
            where TPushed : struct, IModule<TPushed>
        {
            return default;
        }
    }

    internal static class ModuleSeqExtensions
    {
        internal static void ConstructAll<T, TRest>(this ModSeq<T, TRest> self)
            where T : struct, IModule<T>
            where TRest : struct, IModuleSeq
        {
            dynamic a = self;
            while(a is not EmptySeq)
            {
                a = a._ConstructPop();
            }
        }

        internal static void ConstructAll(this IConstructionSequence self)
        {     
            var exceptionList = new List<Exception>();
            IModuleSeq seq = self._GetConstructionSequence();
            while(seq is not EmptySeq)
            {
                seq = seq._ConstructPop(out Exception ex);
                if(ex is not null) exceptionList.Add(ex);
            }
            if(exceptionList.Count > 0) throw new CombinedException(exceptionList);
        }
    }

    public class CombinedException : Exception
    {
        private readonly Exception[] exceptions;
        public CombinedException(params Exception[] exceptions)
        {
            this.exceptions = exceptions;
        }
        public CombinedException(IEnumerable<Exception> exceptions)
        {
            this.exceptions = exceptions.ToArray();
        }

        public override String ToString()
        {
            var builder = new StringBuilder();
            foreach(var ex in this)
            {
                builder = builder
                    .Append(ex.ToString())
                    .Append(Environment.NewLine)
                    .Append(Environment.NewLine);
            }
            return builder.ToString();
        }

        public IEnumerator<Exception> GetEnumerator() => ArrayExtensions.GetEnumerator(this.exceptions);
    }
}
