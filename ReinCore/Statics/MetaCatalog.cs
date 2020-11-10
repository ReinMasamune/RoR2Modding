namespace ReinCore
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public sealed class MetaCatalog : Catalog<MetaCatalog, Catalog>
    {
        const String _guid = "Rein.MetaCatalog";
        public sealed override String guid => _guid;
        protected internal sealed override Int32 order => 0;


        protected sealed override void ProcessAllDefinitions(Catalog[] definitions)
        {
            base.ProcessAllDefinitions(definitions);
            foreach(var cat in definitions.Where((def) => def is not null).OrderBy((def) => def.order).ToArray())
            {
                cat.InitializeIfNeeded();
                //Log.Message($"Catalog {cat.guid} init");
            }
        }


        public static void InitAllCatalogs()
        {
            InitializeIfNeeded();
        }



        protected override void OnDefRegistered(Catalog def)
        {
            base.OnDefRegistered(def);
            //Log.Message($"Catalog {def.guid} registered");
        }
    }
}