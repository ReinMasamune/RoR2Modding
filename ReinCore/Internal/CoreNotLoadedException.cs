namespace ReinCore
{
    using System;
    using BepInEx;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class CoreNotLoadedException : Exception
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public String coreName { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public CoreNotLoadedException( String coreName ) : base( String.Format( "{0} did not load properly", coreName ) )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            this.coreName = coreName;
        }
    }
}
