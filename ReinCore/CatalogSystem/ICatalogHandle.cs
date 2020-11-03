namespace ReinCore
{
    using System;

    public interface ICatalogHandle
    {
        event Action onCatalogReset;
        event Action onPreInit;
        event Action onPostInit;

        void InitializeIfNeeded();
        void EnsureInitialized();

        Catalog catalog { get; }
    }
}