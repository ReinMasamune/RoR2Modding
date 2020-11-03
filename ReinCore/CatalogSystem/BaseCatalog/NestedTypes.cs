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
        public interface ICatalogDef
        {
            String guid { get; }
            Entry? entry { get; set; }
        }
        public class Entry
        {
            public Index? index { get; internal set; }
            public TDef def { get; }
            public Boolean active => this.index is not null;

            internal Entry(TDef def)
            {
                this.def = def;
            }
        }

        private struct Handle : ICatalogHandle
        {
            public Catalog catalog => instance;

            public void InitializeIfNeeded() => Catalog<TSelf, TDef, TBackend>.InitializeIfNeeded();
            public void EnsureInitialized() => Catalog<TSelf, TDef, TBackend>.EnsureInitialized();

            public event Action onCatalogReset
            {
                add => Catalog<TSelf, TDef, TBackend>.onCatalogReset += value;
                remove => Catalog<TSelf, TDef, TBackend>.onCatalogReset -= value;
            }
            public event Action onPreInit
            {
                add => Catalog<TSelf, TDef, TBackend>.onPreInit += value;
                remove => Catalog<TSelf, TDef, TBackend>.onPreInit -= value;
            }
            public event Action onPostInit
            {
                add => Catalog<TSelf, TDef, TBackend>.onPostInit += value;
                remove => Catalog<TSelf, TDef, TBackend>.onPostInit -= value;
            }
        }
        public sealed class RegistrationToken
        {
            public TDef def => this.entry.def;
            public Entry entry { get; }
            public Boolean registered { get; private set; }
            public Boolean added => this.index is not null;
            public Index? index => this.entry.index;
            public void Register()
            {
                if(this.registered) return;
                moddedEntries += this.RegistrationFunc;
                this.registered = true;
            }
            public void Unregister()
            {
                if(!this.registered) return;
                moddedEntries -= this.RegistrationFunc;
                this.registered = false;
            }
            internal RegistrationToken(TDef def) => this.entry = def.entry;
            private TDef RegistrationFunc() => this.def;
        }

        public enum Index : UInt64
        {
            Invalid = 0ul,
        }
    }
}