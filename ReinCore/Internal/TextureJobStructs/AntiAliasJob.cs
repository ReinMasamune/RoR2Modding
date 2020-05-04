using System;
using BepInEx;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using System.Linq;
using Unity.Burst;

namespace ReinCore
{
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
            var x = index % this.texWidth;
            var y = index / this.texWidth;

            var blockXMin = x * this.blockSize;
            var blockYMin = y * this.blockSize;
            var blockXMax = blockXMin + this.blockSize;
            var blockYMax = blockYMin + this.blockSize;

            Color colAccum = new Color( 0f, 0f, 0f, 0f );

            for( var sampY = blockYMin; sampY < blockYMax; ++sampY )
            {
                var yVal = sampY * this.sampleWidth;
                for( var sampX = blockXMin; sampX < blockXMax; ++sampX )
                {
                    var ind = yVal + sampX;
                    var col = this.samples[ind];
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
