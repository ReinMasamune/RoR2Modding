﻿namespace Rein
{
    using System;
    using System.Reflection;

    using BepInEx;

    [BepInPlugin( AssemblyLoad.guid, "Rein Assembly-PreLoad", AssemblyLoad.version )]
    internal class AssemblyLoad : BaseUnityPlugin
    {
        public const String guid = "___AssemblyLoader-com.Rein.Core";
        public const String version = ReinCore.ReinCore.ver;
        private static readonly Assembly coreAssembly;
        static AssemblyLoad()
        {
            coreAssembly = Assembly.Load( Properties.Resources.ReinCore );
        }

        private AssemblyLoad()
        {
            var InitCore = (InitCoreDelegate)Delegate.CreateDelegate( typeof(InitCoreDelegate), coreAssembly.GetType( "ReinCore.ReinCore" ).GetMethod( "Init" ) );

            InitCore( true, false, false, true, true, true, true );
        }

        private delegate void InitCoreDelegate( Boolean net, Boolean debug, Boolean info, Boolean message, Boolean warning, Boolean error, Boolean fatal );
    }
}