namespace ReinCore
{
    public interface ICatalogBackend<TDef>
    {
		TDef[] definitions { get; set; }
    }
}