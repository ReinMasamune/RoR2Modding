using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    /// <summary>
    /// Unknown
    /// </summary>
    internal class IntersectionCloudMaterial : MaterialBase
    {
        /// <summary>
        /// Creates a standard material.
        /// </summary>
        /// <param name="name">The name of the material</param>
        internal IntersectionCloudMaterial( String name ) : base( name, ShaderIndex.HGIntersectionCloudRemap )
        {

        }
    }

}
