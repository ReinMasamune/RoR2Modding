namespace ReinCore
{
    public struct DefaultBackend<TSelf, TDef> : ICatalogBackend<TDef>
		where TSelf : Catalog<TSelf, TDef, DefaultBackend<TSelf, TDef>>, new()
		where TDef : Catalog<TSelf, TDef, DefaultBackend<TSelf, TDef>>.ICatalogDef
    {
		private static TDef[] _definitions;
        public ref TDef[] definitions => ref _definitions;
    }
}