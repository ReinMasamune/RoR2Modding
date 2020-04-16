using System;
using BepInEx;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using System.Linq;
using Unity.Burst;

namespace ReinCore
{
    internal struct BarTextureJob : IJobParallelFor
    {
        #region MAIN THREAD ONLY
        internal BarTextureJob( Int32 width, Int32 height, Boolean roundedCorners, Int32 cornerRadius, Int32 borderWidth, Color borderColor, Color bgColor, (Single start, Single end, Color color)[] regions, Byte aaFactor = 0 )
        {
            this.sampleFactor = (Int32)Mathf.Pow( 2f, aaFactor );
            this.texWidth = width * this.sampleFactor;
            this.texHeight = height * this.sampleFactor;
            this.roundedCorners = roundedCorners;
            this.borderWidth = borderWidth * this.sampleFactor;
            this.borderColor = borderColor * this.sampleFactor;
            this.bgColor = bgColor;
            this.regions = new NativeArray<(Single start, Single end, Color color)>(regions, Allocator.TempJob );
            this.texArray = new NativeArray<Color>( this.texWidth * this.texHeight, Allocator.TempJob );
            this.cornerRadSq = cornerRadius * cornerRadius * this.sampleFactor * this.sampleFactor;
            this.fillWidth = this.texWidth - this.borderWidth - this.borderWidth;
            this.cornerLeft = cornerRadius * this.sampleFactor;
            this.cornerRight = this.texWidth - cornerRadius * this.sampleFactor;
            this.cornerTop = cornerRadius * this.sampleFactor;
            this.cornerBot = this.texHeight - cornerRadius * this.sampleFactor;
            this.borderSquare = this.borderWidth * -this.borderWidth;
        }
        internal JobHandle Start(Int32 innerLoopCount = 1 )
        {
            return this.Schedule( this.texWidth, innerLoopCount );
        }

        internal Texture2D OutputTextureAndDispose()
        {
            if( this.sampleFactor > 1 )
            {
                var job = new AntiAliasJob( this.texWidth / this.sampleFactor, this.texHeight / this.sampleFactor, this.texArray );
                job.Start().Complete();
                var tex = job.OutputTextureAndDispose();
                this.regions.Dispose();
                this.texArray.Dispose();
                return tex;
            } else
            {
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
            if( index < this.borderWidth || index > (this.texWidth - this.borderWidth ) )
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
                if( i < this.borderWidth || i > (this.texHeight - this.borderWidth) )
                {
                    resColor = this.borderColor;
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
                    } else
                    {
                        shouldCompare = false;
                    }

                    if( shouldCompare )
                    {
                        if( pos.y <= this.cornerTop )
                        {
                            corner.y = this.cornerTop;
                        } else if( pos.y >= this.cornerBot )
                        {
                            corner.y = this.cornerBot;
                        } else
                        {
                            shouldCompare = false;
                        }

                        if( shouldCompare )
                        {
                            var dist = (pos-corner).sqrMagnitude - this.cornerRadSq;
                            if( dist > 0f )
                            {
                                resColor = Color.clear;
                            } else if( dist > this.borderSquare )
                            {
                                resColor = this.borderColor;
                            }
                        }
                    }
                }
                this.texArray[index + this.texWidth * i] = resColor;
            }
        }
        private NativeArray<Color> texArray;
        private readonly NativeArray<(Single start, Single end, Color color)> regions;
        private readonly Int32 texWidth;
        private readonly Int32 texHeight;
        private readonly Boolean roundedCorners;
        private readonly Int32 borderWidth;
        private readonly Color borderColor;
        private readonly Color bgColor;
        private readonly Int32 fillWidth;
        private readonly Single cornerRadSq;
        private readonly Int32 cornerTop;
        private readonly Int32 cornerBot;
        private readonly Int32 cornerLeft;
        private readonly Int32 cornerRight;
        private readonly Int32 borderSquare;
        private readonly Int32 sampleFactor;
        #endregion
    }
}
