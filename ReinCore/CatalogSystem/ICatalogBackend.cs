namespace ReinCore
{
    public interface ICatalogBackend<TDef>
    {
		ref TDef[] definitions { get; }
    }
}