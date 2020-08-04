namespace ReinCore
{
    using System;

    using UnityEngine;

#pragma warning disable IDE1006 // Naming Styles
    /// <summary>
    /// 
    /// </summary>
    public static class _BeamPointsFromTransforms
    {
        //private static readonly Accessor<BeamPointsFromTransforms,Transform[]> _pointTransforms = new Accessor<BeamPointsFromTransforms, Transform[]>( "pointTransforms" );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="pointTransforms"></param>
        [Obsolete( "unneeded", true )]
        public static void _SetPointTransforms( this BeamPointsFromTransforms inst, params Transform[] pointTransforms ) => inst.pointTransforms = pointTransforms;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inst"></param>
        /// <returns></returns>
        [Obsolete( "unneeded", true )]
        public static Transform[] _GetPointTransforms( this BeamPointsFromTransforms inst ) => inst.pointTransforms;
#pragma warning restore IDE1006 // Naming Styles
    }
}
