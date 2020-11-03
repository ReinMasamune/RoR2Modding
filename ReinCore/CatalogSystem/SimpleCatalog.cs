namespace ReinCore
{
    public abstract class Catalog<TSelf, TDef> : Catalog<TSelf, TDef, DefaultBackend<TSelf, TDef>>
        where TSelf : Catalog<TSelf, TDef>, new()
        where TDef : Catalog<TSelf, TDef, DefaultBackend<TSelf, TDef>>.ICatalogDef
    { }
}