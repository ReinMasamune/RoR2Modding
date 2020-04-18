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

        public static Texture2D GenerateBarTexture( Int32 width, Int32 height, Boolean roundedCorners, Int32 cornerRadius, Int32 borderWidth, Color borderColor, Color bgColor, Byte sampleFactor = 0, params ColorRegion[] regions )
        {
            if( !loaded ) throw new CoreNotLoadedException( nameof( TexturesCore ) );
            var job = new BarTextureJob( width, height, roundedCorners, cornerRadius, borderWidth, borderColor, bgColor, regions, sampleFactor );
            job.Start().Complete();
            return job.OutputTextureAndDispose();
        }



        static TexturesCore()
        {






            loaded = true;
        }
    }
}
