using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;

namespace Sniper.Data
{
    internal class TextureSet
    {
        internal TextureSet( Texture2D dif, Texture2D nor, Texture2D emi )
        {
            this.diffuse = dif;
            this.normal = nor;
            this.emissive = emi;
        }

        internal Texture2D diffuse { get; private set; }
        internal Texture2D normal { get; private set; }
        internal Texture2D emissive { get; private set; }

        internal void Apply( StandardMaterial mat )
        {
            mat.mainTexture.texture = this.diffuse;
            mat.normalMap.texture = this.normal;
            mat.emissionTexture.texture = this.emissive;
        }
    }
}
