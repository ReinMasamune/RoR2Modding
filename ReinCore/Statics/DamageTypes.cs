namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using BepInEx;
    using RoR2;

    public static class DamageTypesCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static DamageType RegisterNewDamageType( DamageTypeDelegate damageTypeDelegate )
        {
            if( loaded != true ) throw new CoreNotLoadedException( nameof( DamageTypesCore ) );
            if( damageTypeDelegate == null ) throw new ArgumentNullException( nameof( damageTypeDelegate ) );
            var last = maxPossibleIndex / 2;
            if( currentMaxIndex >= last ) throw new OverflowException( "Cannot add additional damage types, max value reached." );
            currentMaxIndex *= 2;
            var damageType = (DamageType)currentMaxIndex;
            addedDamageTypes[damageType] = damageTypeDelegate;
            return damageType;
        }
        public delegate void DamageTypeDelegate();

        static DamageTypesCore()
        {
            //Log.Warning( "DamageTypesCore loaded" );
            var underType = Enum.GetUnderlyingType( typeof(DamageType) );
            if( underType != typeof( UInt32 ) )
            {
                Log.Error( String.Format( "DamageType is currently using {0} as its backing type", underType.Name ) );
                return;
            }
            maxPossibleIndex = 0b_1ul << 32;
            var values = Enum.GetValues( typeof(DamageType) ) as UInt32[];
            for( Int32 i = 0; i < values.Length; ++i )
            {
                var curValue = values[i];
                if( curValue > currentMaxIndex ) currentMaxIndex = curValue;
            }
            //Log.Warning( "DamageTypesCore loaded" );
            loaded = true;
        }

        private static UInt32 currentMaxIndex = 0u;
        private static UInt64 maxPossibleIndex = 0ul;
        private static Dictionary<DamageType,DamageTypeDelegate> addedDamageTypes = new Dictionary<DamageType, DamageTypeDelegate>();
    }
}
