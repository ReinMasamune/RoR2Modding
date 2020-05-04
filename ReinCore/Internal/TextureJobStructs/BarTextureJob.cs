using System;
using BepInEx;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using System.Linq;
using Unity.Burst;

namespace ReinCore
{
    internal struct BarTextureJob : ITextureJob
    {
        #region MAIN THREAD ONLY
        public JobHandle handle { get; private set; }

        private ITextureJob aaJob;
        internal BarTextureJob( Int32 width, Int32 height, Boolean roundedCorners, Int32 cornerRadius, Int32 borderWidth, Color borderColor, Color bgColor, ColorRegion[] regions, Byte aaFactor = 0 )
        {
            this.sampleFactor = (Int32)Mathf.Pow( 2f, aaFactor );
            this.texWidth = width * this.sampleFactor;
            this.texHeight = height * this.sampleFactor;
            this.roundedCorners = roundedCorners;
            this.borderWidth = borderWidth * this.sampleFactor;
            this.borderColor = borderColor;
            this.bgColor = bgColor;
            this.regions = new NativeArray<ColorRegion>( regions, Allocator.TempJob );
            this.texArray = new NativeArray<Color>( this.texWidth * this.texHeight, Allocator.TempJob );
            this.cornerRadSq = cornerRadius *  this.sampleFactor;
            this.fillWidth = this.texWidth - this.borderWidth - this.borderWidth;
            this.cornerLeft = cornerRadius * this.sampleFactor;
            this.cornerRight = this.texWidth - cornerRadius * this.sampleFactor;
            this.cornerTop = cornerRadius * this.sampleFactor;
            this.cornerBot = this.texHeight - cornerRadius * this.sampleFactor;
            //this.borderSquare = this.borderWidth * this.borderWidth;
            this.outerColor = (this.borderWidth > 0 ? this.borderColor : this.bgColor);
            this.outerColor.a = 0f;

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
                this.regions.Dispose();
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
                this.regions.Dispose();
                return tex;
            }
        }
        #endregion

        #region All threads
        public void Execute( Int32 index )
        {
            var resColor = this.bgColor;
            if( index < this.borderWidth || index >= (this.texWidth - this.borderWidth) )
            {
                resColor = this.borderColor;
            } else
            {
                var frac = (Single)index / (Single)this.fillWidth;

                for( Int32 i = 0; i < this.regions.Length; ++i )
                {
                    var reg = this.regions[i];
                    if( frac >= reg.start && frac <= reg.end )
                    {
                        resColor = reg.color;
                        break;
                    }
                }
            }
            for( Int32 i = 0; i < this.texHeight; ++i )
            {
                var resColor2 = resColor;
                if( i < this.borderWidth || i >= (this.texHeight - this.borderWidth) )
                {
                    resColor2 = this.borderColor;
                }

                if( this.roundedCorners )
                {
                    var pos = new Vector2Int( index, i );
                    var shouldCompare = true;
                    Vector2Int corner = default;

                    if( pos.x <= this.cornerLeft )
                    {
                        corner.x = this.cornerLeft;
                    } else if( pos.x >= this.cornerRight )
                    {
                        corner.x = this.cornerRight;
                    } else shouldCompare = false;

                    if( pos.y <= this.cornerTop )
                    {
                        corner.y = this.cornerTop;
                    } else if( pos.y >= this.cornerBot )
                    {
                        corner.y = this.cornerBot;
                    } else shouldCompare = false;

                    var temp = pos - corner;

                    var dist = Mathf.Sqrt(temp.x*temp.x + temp.y*temp.y);
                    dist -= this.cornerRadSq;
                    if( shouldCompare && dist >= 0f )
                    {
                        resColor2 = this.outerColor;
                    } else if( shouldCompare && dist >= (-this.borderWidth) )
                    {
                        resColor2 = this.borderColor;
                    }
                }
                this.texArray[index + (this.texWidth * i)] = resColor2;
            }
        }
        private NativeArray<Color> texArray;
        private readonly NativeArray<ColorRegion> regions;
        private readonly Int32 texWidth;
        private readonly Int32 texHeight;
        private readonly Boolean roundedCorners;
        private readonly Int32 borderWidth;
        private readonly Color borderColor;
        private readonly Color bgColor;
        private readonly Color outerColor;
        private readonly Int32 fillWidth;
        private readonly Single cornerRadSq;
        private readonly Int32 cornerTop;
        private readonly Int32 cornerBot;
        private readonly Int32 cornerLeft;
        private readonly Int32 cornerRight;
        //private readonly Int32 borderSquare;
        private readonly Int32 sampleFactor;
        #endregion
    }

}
