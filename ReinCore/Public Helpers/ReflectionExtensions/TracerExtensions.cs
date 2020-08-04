namespace ReinCore
{
    using System;

    using RoR2;

    public static class _Tracer
    {
        //private static readonly Accessor<Tracer,Single> distanceTraveled = new Accessor<Tracer, Single>( "distanceTraveled" );
        //private static readonly Accessor<Tracer,Single> totalDistance = new Accessor<Tracer, Single>( "totalDistance" );

        [Obsolete( "unneeded", true )]
        public static Single _GetDistanceTraveled( this Tracer inst ) => inst.distanceTraveled;// distanceTraveled.Get( inst );
        [Obsolete( "unneeded", true )]
        public static Single _GetTotalDistance( this Tracer inst ) => inst.totalDistance;// totalDistance.Get( inst );
        [Obsolete( "unneeded", true )]
        public static void _SetDistanceTraveled( this Tracer inst, Single value ) => inst.distanceTraveled = value; // => distanceTraveled.Set( inst, value );
        [Obsolete( "unneeded", true )]
        public static void _SetTotalDistance( this Tracer inst, Single value ) => inst.totalDistance = value;// => totalDistance.Set( inst, value );
    }
}
