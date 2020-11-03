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
        protected virtual IEnumerable<ICatalogHandle> dependencies { get => Enumerable.Empty<ICatalogHandle>(); }
        protected virtual void OnDefRegistered(TDef def) { }
        protected virtual void ProcessAllDefinitions(TDef[] definitions) { }
        protected virtual IEnumerable<TDef> GetBaseEntries() => Enumerable.Empty<TDef>();
        protected virtual void FirstInitSetup() { }
    }
}