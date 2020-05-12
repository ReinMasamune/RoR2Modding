namespace ReinCore
{
    using System;
    using BepInEx;
    using Unity.Collections;
    using Unity.Jobs;
    using UnityEngine;
    using System.Linq;
    using Unity.Burst;

    internal struct AntiAliasJob : ITextureJob
    {
        #region MAIN THREAD ONLY
        internal JobHandle handle { get; private set; }

        internal AntiAliasJob( Int32 width, Int32 height, Int32 sampleFactor, NativeArray<Color> samples, JobHandle prereq = default )
        {
            this.samples = samples;
            this.texWidth = width;
            this.texHeight = height;
            this.texArray = new NativeArray<Color>( width * height, Allocator.TempJob );
            this.blockSize = sampleFactor;
            this.blockCount = this.blockSize * this.blockSize;
            this.sampleWidth = this.texWidth * this.blockSize;
            this.sampleHeight = this.texHeight * this.blockSize;
            this.handle = default;

            this.handle = this.Schedule( this.texWidth * this.texHeight, 1, prereq );
        }

        public Texture2D OutputTextureAndDispose()
        {
            this.handle.Complete();
            var tex = new Texture2D( this.texWidth, this.texHeight, TextureFormat.RGBAFloat, false );
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.LoadRawTextureData<Color>( this.texArray );
            tex.Apply();

            this.texArray.Dispose();
            return tex;
        }
        #endregion

        #region All threads
        public void Execute( Int32 index )
        {
            Int32 x = index % this.texWidth;
            Int32 y = index / this.texWidth;

            Int32 blockXMin = x * this.blockSize;
            Int32 blockYMin = y * this.blockSize;
            Int32 blockXMax = blockXMin + this.blockSize;
            Int32 blockYMax = blockYMin + this.blockSize;

            var colAccum = new Color( 0f, 0f, 0f, 0f );

            for( Int32 sampY = blockYMin; sampY < blockYMax; ++sampY )
            {
                Int32 yVal = sampY * this.sampleWidth;
                for( Int32 sampX = blockXMin; sampX < blockXMax; ++sampX )
                {
                    Int32 ind = yVal + sampX;
                    Color col = this.samples[ind];
                    colAccum += col;
                }
            }

            colAccum /= this.blockCount;


            this.texArray[index] = colAccum;
        }
        private NativeArray<Color> texArray;
        private readonly NativeArray<Color> samples;
        private readonly Int32 texWidth;
        private readonly Int32 texHeight;
        private readonly Int32 sampleWidth;
        private readonly Int32 sampleHeight;
        private readonly Int32 blockSize;
        private readonly Int32 blockCount;
        #endregion
    }
}
