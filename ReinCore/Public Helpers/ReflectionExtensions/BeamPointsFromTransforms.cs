namespace ReinCore
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using BepInEx;
    using UnityEngine;

#pragma warning disable IDE1006 // Naming Styles
    /// <summary>
    /// 
    /// </summary>
    public static class _BeamPointsFromTransforms
    {
        private static readonly Accessor<BeamPointsFromTransforms,Transform[]> _pointTransforms = new Accessor<BeamPointsFromTransforms, Transform[]>( "pointTransforms" );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="pointTransforms"></param>
        public static void _SetPointTransforms( this BeamPointsFromTransforms inst, params Transform[] pointTransforms ) => _pointTransforms.Set( inst, pointTransforms );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inst"></param>
        /// <returns></returns>
        public static Transform[] _GetPointTransforms( this BeamPointsFromTransforms inst ) => _pointTransforms.Get( inst );
#pragma warning restore IDE1006 // Naming Styles
    }
}
