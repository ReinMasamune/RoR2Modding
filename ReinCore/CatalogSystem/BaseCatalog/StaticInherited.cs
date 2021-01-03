namespace ReinCore
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using RoR2;


    public abstract partial class Catalog<TSelf, TDef, TBackend> : Catalog
        where TSelf : Catalog<TSelf, TDef, TBackend>, new()
        where TDef : Catalog<TSelf, TDef, TBackend>.ICatalogDef
		where TBackend : unmanaged, ICatalogBackend<TDef>
    {
        private protected sealed override ICatalogHandle catHandle => new Handle();
        public static ICatalogHandle handle => instance.catHandle;
        public static TDef? GetDef(Index index)
        {
            EnsureInitialized();
            return definitions![(UInt64)index - 1];
        }
        public static TDef? GetDef(String guid)
        {
            EnsureInitialized();
            return guidToDef[guid];
        }
        public static Boolean TryGetDef(Index index, out TDef? def)
        {
            def = default;
            if(!initialized) return false;
            if(index == Index.Invalid) return false;
            var ind = (UInt64)index;
            if(ind > count) return false;
            def = definitions![ind - 1];
            return true;
        }
        public static Boolean TryGetDef(String guid, out TDef? def)
        {
            def = default;
            if(!initialized) return false;
            return guidToDef.TryGetValue(guid, out def);
        }

        public static IEnumerable<TDef> EnumerateEntries()
        {
            if(!initialized) return Enumerable.Empty<TDef>();
            return definitions!;
        }

        public static void LogCatalogState()
        {
            Log.Message($"State for catalog: {instance.guid}");
            Log.Message($"Count: {count}");
            Log.Message($"CurIndex: {currentIndex}");
            Log.Message($"Initialized? {initialized}");
            foreach(var entry in EnumerateEntries())
            {
                Log.Message($"Def Info\nguid: {entry.guid}\nindex {entry.entry?.index}\nguidIndex: {GetDef(entry.guid)?.entry?.index}");
                instance.OnDefLogged(entry);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void RequestLoad()
        {

        }

        public static UInt64 count => (UInt64)(definitions?.LongLength ?? 0L);

        public static RegistrationToken Add(TDef item)
        {
            item.entry = new Entry(item);
            var tok = new RegistrationToken(item);
            tok.Register();
            outstandingTokens.Add(tok);
            return tok;
        }
        public static event Action onPreInit;
        public static event Action onPostInit;
        public static event Action onCatalogReset;
    }
}