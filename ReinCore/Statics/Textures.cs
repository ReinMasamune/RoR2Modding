namespace ReinCore
{
    using System;

    using Unity.Jobs;

    using UnityEngine;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class TexturesCore
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean loaded { get; internal set; } = false;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        [Obsolete]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Texture2D GenerateRampTexture( Gradient gradient, Boolean outputSquared = true, Int32 width = 256, Int32 height = 16 )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Texture2D GenerateBarTexture( Int32 width, Int32 height, Boolean roundedCorners, Int32 cornerRadius, Int32 borderWidth, Color borderColor, Color bgColor, Byte sampleFactor = 0, params ColorRegion[] regions )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( TexturesCore ) );
            }

            var job = new BarTextureJob( width, height, roundedCorners, cornerRadius, borderWidth, borderColor, bgColor, regions, sampleFactor );
            JobHandle.ScheduleBatchedJobs();
            return job.OutputTextureAndDispose();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static ITextureJob GenerateRampTextureBatch( Gradient gradient, Boolean outputSquared = true, Int32 width = 256, Int32 height = 16 )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( TexturesCore ) );
            }

            return new GradientTextureJob( gradient.alphaKeys, gradient.colorKeys, outputSquared, width, height );
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static ITextureJob GenerateRampTextureBatch( GradientAlphaKey[] aKeys, GradientColorKey[] cKeys, Boolean outputSquared = true, Int32 width = 256, Int32 height = 16 )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( TexturesCore ) );
            }

            return new GradientTextureJob( aKeys, cKeys, outputSquared, width, height );
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static ITextureJob GenerateBarTextureBatch( Int32 width, Int32 height, Boolean roundedCorners, Int32 cornerRadius, Int32 borderWidth, Color borderColor, Color bgColor, Byte sampleFactor = 0, params ColorRegion[] regions )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( TexturesCore ) );
            }

            return new BarTextureJob( width, height, roundedCorners, cornerRadius, borderWidth, borderColor, bgColor, regions, sampleFactor );
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static ITextureJob GenerateCrossTextureBatch( Int32 width, Int32 height, Int32 crossWidth, Int32 borderWidth, Byte aaFactor,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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






            loaded = true;
        }
    }
}
