using System;
using BepInEx;
using System.Linq;

namespace ReinCore
{
    internal static class EnumExtensions
    {
        internal static TValue GetValue<TValue>( this Enum self ) where TValue : struct, IComparable, IComparable<TValue>, IConvertible, IEquatable<TValue>, IFormattable
        {
            if( self.GetTypeCode() != default( TValue ).GetTypeCode() ) throw new ArgumentException( "Incorrect value type" );

            return (TValue)Convert.ChangeType(self, typeof(TValue) );
        }

        internal static String GetName( this Enum self ) => Enum.GetName( self.GetType(), self );

        internal static TEnum GetMax<TEnum>() where TEnum : struct, Enum => ( Enum.GetValues( typeof( TEnum ) ) as TEnum[] ).Max();

        internal static TEnum GetMin<TEnum>() where TEnum : struct, Enum => ( Enum.GetValues( typeof( TEnum ) ) as TEnum[] ).Min();

        internal static (TEnum min, TEnum max) GetRange<TEnum>() where TEnum : struct, Enum
        {
            var vals = Enum.GetValues( typeof(TEnum) ) as TEnum[];
            return (vals.Min(), vals.Max());
        }
    }

}
