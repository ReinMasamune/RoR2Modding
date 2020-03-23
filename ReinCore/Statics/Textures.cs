using System;
using BepInEx;
using RoR2;
using UnityEngine;

namespace ReinCore
{
    public static class TexturesCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static Texture2D GenerateRampTexture( Gradient gradient, Boolean outputSquared = true, Int32 width = 256, Int32 height = 16 )
        {
            if( !loaded ) throw new CoreNotLoadedException( nameof( TexturesCore ) );
            var job = new GradientTextureJob( gradient, outputSquared, width, height );
            job.Start().Complete();
            return job.OutputTextureAndDispose();
        }



        static TexturesCore()
        {






            loaded = true;
        }
    }
}
