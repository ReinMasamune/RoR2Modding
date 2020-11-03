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
        private protected sealed override ICatalogHandle catHandle => new Handle();
        public static ICatalogHandle handle => instance.catHandle;
        public static TDef? GetDef(Index index)
        {
            EnsureInitialized();
            return definitions![(UInt64)index];
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
            var ind = (UInt64)index;
            if(ind >= count) return false;
            def = definitions![ind - 1];
            return true;
        }
        public static Boolean TryGetDef(String guid, out TDef? def)
        {
            def = default;
            if(!initialized) return false;
            return guidToDef.TryGetValue(guid, out def);
        }

        public static UInt64 count => (UInt64)(definitions?.LongLength ?? 0L);

        public static RegistrationToken Add(TDef item)
        {
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