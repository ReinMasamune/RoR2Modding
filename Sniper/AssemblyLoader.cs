namespace Sniper.Properties
{
    using System;
    using System.Reflection;

    using BepInEx;

#pragma warning disable CA2243 // Attribute string literals should parse correctly
    [BepInPlugin( AssemblyLoad.guid, "Rein Assembly-PreLoad", AssemblyLoad.version )]
#pragma warning restore CA2243 // Attribute string literals should parse correctly
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
    internal class AssemblyLoad : BaseUnityPlugin
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
    {
        public const String guid = "___AssemblyLoader-com.Rein.Core";
        public const String version = ReinCore.ReinCore.ver;
        private static readonly Assembly coreAssembly;
#pragma warning disable CA1810 // Initialize reference type static fields inline
        static AssemblyLoad()
#pragma warning restore CA1810 // Initialize reference type static fields inline
        {
            coreAssembly = Assembly.Load( Resources.ReinCore );
        }

        private AssemblyLoad()
        {
            var InitCore = (InitCoreDelegate)Delegate.CreateDelegate( typeof(InitCoreDelegate), coreAssembly.GetType( "ReinCore.ReinCore" ).GetMethod( "Init" ) );

            InitCore( true, false, false, true, true, true, true );
        }

        private delegate void InitCoreDelegate( Boolean net, Boolean debug, Boolean info, Boolean message, Boolean warning, Boolean error, Boolean fatal );
    }
}