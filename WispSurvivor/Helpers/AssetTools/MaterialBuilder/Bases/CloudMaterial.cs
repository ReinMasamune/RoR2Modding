using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    /// <summary>
    /// A material using CloudRemap. Generally for particle effects.
    /// </summary>
    internal class CloudMaterial : MaterialBase
    {
        /// <summary>
        /// Creates a cloud remap material.
        /// </summary>
        /// <param name="name">The name of the material</param>
        internal CloudMaterial( String name ) : base( name, ShaderIndex.HGCloudRemap )
        {

        }
    }

}
