namespace ReinCore
{
    using System;

    public abstract class Catalog : Catalog<MetaCatalog, Catalog, DefaultBackend<MetaCatalog, Catalog>>.ICatalogDef
    {
        public Catalog<MetaCatalog, Catalog, DefaultBackend<MetaCatalog, Catalog>>.Entry? entry { get; set; }


        internal void InitializeIfNeeded() => this.catHandle.InitializeIfNeeded();

        private protected abstract ICatalogHandle catHandle { get; }
        protected internal abstract Int32 order { get; }
        public abstract String guid { get; }
    }
}