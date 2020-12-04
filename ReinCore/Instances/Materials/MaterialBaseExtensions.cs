namespace ReinCore
{
    public static class MaterialBaseExtensions
    {
        public static TMaterial Clone<TMaterial>( this TMaterial orig ) where TMaterial : MaterialBase, new()
        {
            var inst = new TMaterial();
            inst.Init( orig.material.Instantiate(), orig.name );
            return inst;
        }
    }
}
