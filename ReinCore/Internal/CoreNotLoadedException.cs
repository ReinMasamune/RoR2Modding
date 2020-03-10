using System;
using BepInEx;

namespace ReinCore
{
    public class CoreNotLoadedException : Exception
    {
        public String coreName { get; private set; }
        public CoreNotLoadedException( String coreName ) : base( String.Format( "{0} did not load properly", coreName ) )
        {
            this.coreName = coreName;
        }
    }
}
