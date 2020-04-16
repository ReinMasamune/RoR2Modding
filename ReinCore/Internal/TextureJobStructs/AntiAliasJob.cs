using System;
using BepInEx;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using System.Linq;
using Unity.Burst;

namespace ReinCore
{
    internal struct AntiAliasJob : IJobParallelFor
    {
        #region MAIN THREAD ONLY
        internal AntiAliasJob( Int32 width, Int32 height, NativeArray<Color> samples  )
        {
            this.samples = samples;
            this.texWidth = width;
            this.texHeight = height;
            this.texArray = new NativeArray<Color>( width * height, Allocator.TempJob );
            this.blockSize = samples.Length / (width * height);
            this.blockCount = (Int32)Mathf.Sqrt( this.blockSize );
            this.sampleWidth = this.texWidth * this.blockSize;
            this.sampleHeight = this.texHeight * this.blockSize;
        }
        internal JobHandle Start(Int32 innerLoopCount = 1 )
        {
            return this.Schedule( this.texWidth * this.texHeight, innerLoopCount );
        }

        internal Texture2D OutputTextureAndDispose()
        {
            var tex = new Texture2D( this.texWidth, this.texHeight, TextureFormat.RGBAFloat, false );
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.LoadRawTextureData<Color>( this.texArray );
            tex.Apply();

            this.texArray.Dispose();
            this.samples.Dispose();
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

            Single r = 0f, g = 0f, b = 0f, a = 0f;

            for( var sampY = blockYMin; sampY < blockYMax; ++sampY )
            {
                var yVal = sampY * this.sampleWidth;
                for( var sampX = blockXMin; sampX < blockXMax; ++sampX )
                {
                    var ind = yVal + sampX;
                    var col = this.samples[ind];
                    r += col.r;
                    g += col.g;
                    b += col.b;
                    a += col.a;
                }
            }

            r /= this.blockSize;
            g /= this.blockSize;
            b /= this.blockSize;
            a /= this.blockSize;

            this.texArray[index] = new Color( r, g, b, a );
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
