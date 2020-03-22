using System;
using System.Collections.Generic;
using System.Text;

namespace ReinCore
{
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
        #endregion

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
