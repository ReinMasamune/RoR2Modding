namespace ReinCore
{
    using System;

    /// <summary>
    /// Mesh asset lookup index
    /// </summary>
    public enum MeshIndex : UInt64
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        #region Unity Primatives
        Sphere,
        Capsule,
        Cylinder,
        Cube,
        Plane,
        Quad,
        #endregion
        #region Hopoo Meshes
        Spiral1,
        TornadoMesh,
        TornadoMesh2,
        MdlTriTip,
        Donut2,
        #endregion

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
