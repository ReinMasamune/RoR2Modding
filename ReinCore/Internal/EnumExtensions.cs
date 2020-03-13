using System;
using BepInEx;

namespace ReinCore
{
    internal static class EnumExtensions
    {
        internal static TValue GetValue<TValue>( this Enum self ) where TValue : struct, IComparable, IComparable<TValue>, IConvertible, IEquatable<TValue>, IFormattable
        {
            if( self.GetTypeCode() != default( TValue ).GetTypeCode() ) throw new ArgumentException( "Incorrect value type" );

            return (TValue)Convert.ChangeType(self, typeof(TValue) );
        }

        internal static String GetName( this Enum self )
        {
            return Enum.GetName( self.GetType(), self );
        }
    }

}
