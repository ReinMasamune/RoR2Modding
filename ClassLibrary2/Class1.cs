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

            var info = new AppDomainSetup( )

            var domain = AppDomain.CreateDomain( "TestShit", null,  )
        }
    }
}
