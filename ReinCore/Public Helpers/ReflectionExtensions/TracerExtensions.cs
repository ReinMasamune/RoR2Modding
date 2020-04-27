using System;
using System.Linq.Expressions;
using System.Reflection;
using BepInEx;
using RoR2;
using UnityEngine;

namespace ReinCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class _Tracer
    {
        private static Accessor<Tracer,Single> distanceTraveled = new Accessor<Tracer, Single>( "distanceTraveled" );
        private static Accessor<Tracer,Single> totalDistance = new Accessor<Tracer, Single>( "totalDistance" );

        public static Single _GetDistanceTraveled( this Tracer inst )
        {
            return distanceTraveled.Get( inst );
        }

        public static Single _GetTotalDistance( this Tracer inst )
        {
            return totalDistance.Get( inst );
        }

        public static void _SetDistanceTraveled( this Tracer inst, Single value )
        {
            distanceTraveled.Set( inst, value );
        }

        public static void _SetTotalDistance( this Tracer inst, Single value )
        {
            totalDistance.Set( inst, value );
        }
    }
}
