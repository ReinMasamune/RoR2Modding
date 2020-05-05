using System;
using BepInEx;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using System.Linq;
using Unity.Burst;

namespace ReinCore
{
    internal struct SkinTextureJob : ITextureJob
    {
        #region MAIN THREAD ONLY
        internal JobHandle handle { get; private set; }
        internal AntiAliasJob aaJob { get; private set; }
        internal SkinTextureJob( Int32 width, Int32 height, Int32 crossWidth, Int32 borderWidth, Byte aaFactor,
            Color borderColor, Color crossColor, Color topColor, Color rightColor, Color bottomColor, Color leftColor )
        {
            this.sampleFactor = (Int32)Mathf.Pow( 2f, aaFactor );
            this.texWidth = width * this.sampleFactor;
            this.texHeight = height * this.sampleFactor;
            this.borderWidth = borderWidth * this.sampleFactor;
            this.texArray = new NativeArray<Color>( this.texWidth * this.texHeight, Allocator.TempJob );
            this.crossSize = crossWidth;
            this.borderMaxW = this.texWidth - this.borderWidth;
            this.borderMaxH = this.texHeight - this.borderWidth;
          
            this.borderColor = borderColor;
            this.crossColor = crossColor;
            this.topColor = topColor;
            this.rightColor = rightColor;
            this.bottomColor = bottomColor;
            this.leftColor = leftColor;

            this.handle = default;
            this.aaJob = default;

            this.handle = this.Schedule( this.texWidth * this.texHeight, 1 );
            if( this.sampleFactor > 1 )
            {
                this.aaJob = new AntiAliasJob( this.texWidth / this.sampleFactor, this.texHeight / this.sampleFactor, this.sampleFactor, this.texArray, this.handle );
            }
        }

        public Texture2D OutputTextureAndDispose()
        {
            if( this.sampleFactor > 1 )
            {
                var tex = this.aaJob.OutputTextureAndDispose();
                this.texArray.Dispose();
                return tex;
            } else
            {
                this.handle.Complete();
                var tex = new Texture2D( this.texWidth / this.sampleFactor, this.texHeight / this.sampleFactor, TextureFormat.RGBAFloat, false );
                tex.wrapMode = TextureWrapMode.Clamp;
                tex.LoadRawTextureData<Color>( this.texArray );
                tex.Apply();

                this.texArray.Dispose();
                return tex;
            }
        }
        #endregion

        #region All threads
        public void Execute( Int32 index )
        {
            var x = index % this.texWidth;
            var y = index / this.texWidth;
            Color color;


            if( x < this.borderWidth || y < this.borderWidth || x >= this.borderMaxW || y >= this.borderMaxH )
            {
                color = this.borderColor;
                goto OutputAndReturn;
            }

            var xFrac = (Single)(x - this.borderWidth) / (Single)(this.texWidth - (1 + (2 * this.borderWidth)));
            var yFrac = (Single)(y - this.borderWidth) / (Single)(this.texHeight - (1 + (2 * this.borderWidth)));
            var crossValue = (Single)this.crossSize / Mathf.Min( this.texWidth, this.texHeight );

            xFrac -= 0.5f;
            yFrac -= 0.5f;

            if( Mathf.Min( Mathf.Abs( xFrac - (-yFrac) ), Mathf.Abs( xFrac - yFrac ) ) <= crossValue )
            {
                color = this.crossColor;
                goto OutputAndReturn;
            }

            if( yFrac > xFrac )
            {
                if( yFrac > -xFrac )
                {
                    color = this.topColor;
                    goto OutputAndReturn;
                }

                color = this.leftColor;
                goto OutputAndReturn;
            }

            if( yFrac > -xFrac )
            {
                color = this.rightColor;
                goto OutputAndReturn;
            }

            color = this.bottomColor;
        OutputAndReturn:
            this.texArray[index] = color;
            return;
        }


        private NativeArray<Color> texArray;
        private readonly Int32 texWidth;
        private readonly Int32 texHeight;
        private readonly Int32 sampleFactor;
        private readonly Int32 borderWidth;
        private readonly Int32 borderMaxW;
        private readonly Int32 borderMaxH;
        private readonly Int32 crossSize;
        private readonly Color borderColor;
        private readonly Color crossColor;
        private readonly Color topColor;
        private readonly Color rightColor;
        private readonly Color bottomColor;
        private readonly Color leftColor;
        #endregion
    }

}
