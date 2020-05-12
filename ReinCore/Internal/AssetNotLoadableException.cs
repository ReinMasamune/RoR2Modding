namespace ReinCore
{
    using System;
    using BepInEx;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class AssetNotLoadableException : Exception
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public AssetNotLoadableException( Enum index ) : base( String.Format( "{0} cannot be loaded", index.GetName() ) )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
        }
    }
}
