using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal abstract class MaterialBase
    {
        /// <summary>
        /// The name of the material
        /// </summary>
        internal String name
        {
            get
            {
                return this.material?.name;
            }
            set
            {
                if( this.material ) this.material.name = value;
            }
        }

        /// <summary>
        /// The shader to use
        /// </summary>
        internal ShaderIndex shader { get; set; }

        /// <summary>
        /// The Material that is being created
        /// </summary>
        internal Material material { get; private set; }

        internal MaterialBase( String name, ShaderIndex index )
        {
            this.shader = index;
            this.material = new Material( AssetLibrary<Shader>.i[index] );
            this.name = name;
        }
    }

}
