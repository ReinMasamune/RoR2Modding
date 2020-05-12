using System;
using BepInEx;

namespace ReinCore
{
    public class AssetNotLoadableException : Exception
    {
        public AssetNotLoadableException( Enum index ) : base( String.Format( "{0} cannot be loaded", index.GetName() ) )
        {
        }
    }
}
