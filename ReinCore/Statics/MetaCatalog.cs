namespace ReinCore
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public sealed class MetaCatalog : Catalog<MetaCatalog, Catalog>
    {
        public sealed override String guid => "Rein.MetaCatalog";
        protected internal sealed override Int32 order => 0;

        
        protected sealed override void FirstInitSetup()
        {
            base.FirstInitSetup();
            foreach(var atr in typeof(CatalogAttribute).Assembly.GetCustomAttributes<CatalogAttribute>()) RuntimeHelpers.RunClassConstructor(atr.type.TypeHandle);
        }

        protected sealed override void ProcessAllDefinitions(Catalog[] definitions)
        {
            base.ProcessAllDefinitions(definitions);
            foreach(var cat in definitions.OrderBy((def) => def.order))
            {
                cat.InitializeIfNeeded();
            }
        }


        public static void InitAllCatalogs()
        {
            InitializeIfNeeded();
        }

        static MetaCatalog()
        {
            HooksCore.RoR2.SystemInitializer.Execute.On += Execute_On;
        }

        private static void Execute_On(HooksCore.RoR2.SystemInitializer.Execute.Orig orig)
        {
            orig();
            InitAllCatalogs();
        }
    }
}