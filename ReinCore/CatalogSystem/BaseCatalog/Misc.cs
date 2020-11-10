namespace ReinCore
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using Mono.Security.Authenticode;

    using RoR2;


    public abstract partial class Catalog<TSelf, TDef, TBackend> : Catalog
        where TSelf : Catalog<TSelf, TDef, TBackend>, new()
        where TDef : Catalog<TSelf, TDef, TBackend>.ICatalogDef
		where TBackend : unmanaged, ICatalogBackend<TDef>
    {
        static Catalog()
        {
            _outstandingTokens = new();
            _guidToDef = new();
            definitions = new TDef[0];
            instance = new TSelf();
            if(instance is not MetaCatalog)
            {
                MetaCatalog.Add(instance);
            }
        }




        private static TSelf instance;
        private static TBackend backend => default;
        private static List<RegistrationToken> _outstandingTokens;
        private static List<RegistrationToken> outstandingTokens => _outstandingTokens;
        //Convert this to hashset? delegate does not really make sense anymore with this setup as it is abstracted behind Add and defs should not be recreated on reset.
        private static event Func<TDef> moddedEntries;
        private static ref TDef[] definitions => ref backend.definitions;
        private static Dictionary<String, TDef> _guidToDef;
        private static Dictionary<String, TDef> guidToDef => _guidToDef;
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