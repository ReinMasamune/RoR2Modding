using System;
using System.Reflection;
using BepInEx;

namespace Rein
{
    [BepInPlugin( AssemblyLoad.guid, "Rein Assembly-PreLoad", AssemblyLoad.version )]
    internal class AssemblyLoad : BaseUnityPlugin
    {
        public const String guid = "___AssemblyLoader-com.Rein.Core";
        public const String version = ReinCore.ReinCore.ver;
        private static Assembly coreAssembly;
        static AssemblyLoad()
        {
            Assembly execAssembly = Assembly.GetExecutingAssembly();
            System.IO.Stream stream = execAssembly.GetManifestResourceStream( "Rein.Assemblies.ReinCore.dll" );
            var data = new Byte[stream.Length];
            stream.Read( data, 0, data.Length );
            coreAssembly = Assembly.Load( data );
        }

        private AssemblyLoad()
        {
            Boolean r2apiPresent = false;
            foreach( var guid in BepInEx.Bootstrap.Chainloader.PluginInfos.Keys )
            {
                if( guid == "com.bepis.r2api" )
                {
                    r2apiPresent = true;
                    break;
                }
            }
            var InitCore = (InitCoreDelegate)Delegate.CreateDelegate( typeof(InitCoreDelegate), coreAssembly.GetType( "ReinCore.ReinCore" ).GetMethod( "Init" ) );

            InitCore( r2apiPresent, false, false, true, true, true, true );
        }

        private delegate void InitCoreDelegate( Boolean r2api, Boolean debug, Boolean info, Boolean message, Boolean warning, Boolean error, Boolean fatal );
    }
}