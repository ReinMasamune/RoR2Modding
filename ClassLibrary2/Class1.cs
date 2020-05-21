namespace ClassLibrary2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BepInEx;

    [BepInPlugin( "com.Rein.DomainTest", "DomainTest", "1.0.0.0" )]
    public class Class1 : BaseUnityPlugin
    {
        public void Awake()
        {
            base.Logger.LogWarning( "Loading domain" );

            var info = new AppDomainSetup
            {
                ActivationArguments = null,                                 // Manifest based applications only?
                AppDomainInitializer = null,                                // Delegate invoked when domain is loaded
                AppDomainInitializerArguments = null,                       // Arguments passed to the delegate
                AppDomainManagerAssembly = default, // TODO:
                AppDomainManagerType = default, // TODO:
                ApplicationBase = default, // TODO: Assign                  // Path to the directory of the stuff in the domain.
                ApplicationName = "Test Domain",                            // Simple name
                //ApplicationTrust = default                                // DO NOT SET UNLESS NEEDED
                CachePath = default, // TODO:
                ConfigurationFile = default, // TODO:
                DisallowApplicationBaseProbing = false,                     // Has to do with assembly resolution order
                DisallowBindingRedirects = false,                           // Uncertain
                DisallowCodeDownload = true,                                // ...
                DisallowPublisherPolicy = false,                            // Uncertain
                DynamicBase = null,                                         // Maybe not needed?
                LicenseFile = null,                                         // Most likely not needed?
                LoaderOptimization = LoaderOptimization.MultiDomain,        // Has to do with resource sharing across domains?
                PartialTrustVisibleAssemblies = Array.Empty<String>(),      // Might not be needed, or may need to include all assemblies in ror2 folder
                PrivateBinPath = null,                                      // Not needed
                PrivateBinPathProbe = null,                                 // Not needed
                SandboxInterop = false,                                     // disables caching for native calls?
                ShadowCopyDirectories = "",                                 // Most likely not needed
                ShadowCopyFiles = "false",                                  // wtf
                TargetFrameworkName = default, // TODO:
            };


            var domain = AppDomain.CreateDomain( "TestShit", null, info );
        }
    }
}
