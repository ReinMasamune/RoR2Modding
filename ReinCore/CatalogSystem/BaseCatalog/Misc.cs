namespace ReinCore
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using RoR2;


    public abstract partial class Catalog<TSelf, TDef, TBackend> : Catalog
        where TSelf : Catalog<TSelf, TDef, TBackend>, new()
        where TDef : Catalog<TSelf, TDef, TBackend>.ICatalogDef
		where TBackend : unmanaged, ICatalogBackend<TDef>
    {
        protected static readonly TSelf instance = (TSelf)MetaCatalog.Add(new TSelf()).def;

        private static TBackend backend = new();
        private static readonly List<RegistrationToken> outstandingTokens = new();
        //Convert this to hashset? delegate does not really make sense anymore with this setup as it is abstracted behind Add and defs should not be recreated on reset.
        private static event Func<TDef> moddedEntries;
        private static TDef[]? definitions { get => backend.definitions; set => backend.definitions = value; }
        private static readonly Dictionary<String, TDef> guidToDef = new();
        private static Boolean initialized = false;
        private static UInt64 _curIndex = 0ul;
        private static Boolean firstInitComplete = false;
        private static Index currentIndex
        {
            get
            {
                return (Index)_curIndex;
            }
            set => _curIndex = (UInt64)value;
        }
    }
}