namespace ReinCore
{
    using System;
    using Unity.Jobs;
    using UnityEngine;
    using System.Collections.Generic;



    public static class TexturesCore
    {
        public static Boolean loaded { get; internal set; } = false;

        [Obsolete]
        public static Texture2D GenerateRampTexture( Gradient gradient, Boolean outputSquared = true, Int32 width = 256, Int32 height = 16 )
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( TexturesCore ) );
            }

            var job = new GradientTextureJob( gradient, outputSquared, width, height );
            JobHandle.ScheduleBatchedJobs();
            return job.OutputTextureAndDispose();
        }

        [Obsolete]
        public static Texture2D GenerateBarTexture( Int32 width, Int32 height, Boolean roundedCorners, Int32 cornerRadius, Int32 borderWidth, Color borderColor, Color bgColor, Byte sampleFactor = 0, params ColorRegion[] regions )
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( TexturesCore ) );
            }

            var job = new BarTextureJob( width, height, roundedCorners, cornerRadius, borderWidth, borderColor, bgColor, regions, sampleFactor );
            JobHandle.ScheduleBatchedJobs();
            return job.OutputTextureAndDispose();
        }

        public static ITextureJob GenerateRampTextureBatch( Gradient gradient, Boolean outputSquared = true, Int32 width = 256, Int32 height = 16 )
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( TexturesCore ) );
            }

            return new GradientTextureJob( gradient.alphaKeys, gradient.colorKeys, outputSquared, width, height );
        }

        public static ITextureJob GenerateRampTextureBatch( GradientAlphaKey[] aKeys, GradientColorKey[] cKeys, Boolean outputSquared = true, Int32 width = 256, Int32 height = 16 )
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( TexturesCore ) );
            }

            return new GradientTextureJob( aKeys, cKeys, outputSquared, width, height );
        }

        public static ITextureJob GenerateBarTextureBatch( Int32 width, Int32 height, Boolean roundedCorners, Int32 cornerRadius, Int32 borderWidth, Color borderColor, Color bgColor, Byte sampleFactor = 0, params ColorRegion[] regions )
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( TexturesCore ) );
            }

            return new BarTextureJob( width, height, roundedCorners, cornerRadius, borderWidth, borderColor, bgColor, regions, sampleFactor );
        }

        public static ITextureJob GenerateCrossTextureBatch( Int32 width, Int32 height, Int32 crossWidth, Int32 borderWidth, Byte aaFactor,
    Color borderColor, Color crossColor, Color topColor, Color rightColor, Color bottomColor, Color leftColor )
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( TexturesCore ) );
            }

            return new SkinTextureJob( width, height, crossWidth, borderWidth, aaFactor, borderColor, crossColor, topColor, rightColor, bottomColor, leftColor );
        }


        static TexturesCore()
        {
            Log.Warning( "TexturesCore loaded" );




            Log.Warning( "TexturesCore loaded" );
            loaded = true;
        }


    }
}
