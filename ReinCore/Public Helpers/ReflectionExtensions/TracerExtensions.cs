namespace ReinCore
{
    using System;

    using RoR2;

    /// <summary>
    /// 
    /// </summary>
#pragma warning disable IDE1006 // Naming Styles
    public static class _Tracer
#pragma warning restore IDE1006 // Naming Styles
    {
        private static readonly Accessor<Tracer,Single> distanceTraveled = new Accessor<Tracer, Single>( "distanceTraveled" );
        private static readonly Accessor<Tracer,Single> totalDistance = new Accessor<Tracer, Single>( "totalDistance" );

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static Single _GetDistanceTraveled( this Tracer inst ) => distanceTraveled.Get( inst );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static Single _GetTotalDistance( this Tracer inst ) => totalDistance.Get( inst );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static void _SetDistanceTraveled( this Tracer inst, Single value ) => distanceTraveled.Set( inst, value );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static void _SetTotalDistance( this Tracer inst, Single value ) => totalDistance.Set( inst, value );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
