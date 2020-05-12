namespace ReinCore
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class MaterialBaseExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static TMaterial Clone<TMaterial>( this TMaterial orig ) where TMaterial : MaterialBase, new()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            var inst = new TMaterial();
            inst.Init( orig.material.Instantiate(), orig.name );
            return inst;
        }
    }
}
