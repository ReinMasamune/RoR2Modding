using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal enum MeshIndex : ulong
    {
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
        #endregion
    }
}
