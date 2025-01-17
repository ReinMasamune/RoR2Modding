﻿namespace ReinCore
{
    using System;
    using System.Linq;

    using Unity.Collections;
    using Unity.Jobs;

    using UnityEngine;

    internal struct GradientTextureJob : ITextureJob
    {
        #region MAIN THREAD ONLY
        internal JobHandle handle;
        [Obsolete]
        internal GradientTextureJob( Gradient gradient, Boolean outputSquared, Int32 width, Int32 height )
        {
            this.texWidth = width;
            this.texHeight = height;

            this.aKeys = new NativeArray<GradientAlphaKey>( gradient.alphaKeys.OrderBy( ( key ) => key.time ).ToArray(), Allocator.TempJob );
            this.cKeys = new NativeArray<GradientColorKey>( gradient.colorKeys.OrderBy( ( key ) => key.time ).ToArray(), Allocator.TempJob );

            this.texArray = new NativeArray<Color>( width * height, Allocator.TempJob );
            this.outputSquared = outputSquared;

            this.handle = default;

            this.handle = this.Schedule( this.texWidth, 1 );
        }
        internal GradientTextureJob( GradientAlphaKey[] aKeys, GradientColorKey[] cKeys, Boolean outputSquared, Int32 width, Int32 height )
        {
            this.aKeys = new NativeArray<GradientAlphaKey>( aKeys.OrderBy( key => key.time ).ToArray(), Allocator.TempJob );
            this.cKeys = new NativeArray<GradientColorKey>( cKeys.OrderBy( key => key.time ).ToArray(), Allocator.TempJob );
            this.texWidth = width;
            this.texHeight = height;
            this.texArray = new NativeArray<Color>( width * height, Allocator.TempJob );
            this.outputSquared = outputSquared;


            this.handle = default;

            this.handle = this.Schedule( this.texWidth, 1 );
        }

        public Texture2D OutputTextureAndDispose()
        {
            this.handle.Complete();
            var tex = new Texture2D( this.texWidth, this.texHeight, TextureFormat.RGBAFloat, false )
            {
                wrapMode = TextureWrapMode.Clamp
            };
            tex.LoadRawTextureData<Color>( this.texArray );
            tex.Apply();

            this.texArray.Dispose();
            this.aKeys.Dispose();
            this.cKeys.Dispose();

            return tex;
        }
        #endregion

        #region All threads
        public void Execute( Int32 index )
        {
            Single t = index / (Single)this.texWidth;

            Single alpha = 0f;
            Single bestDiff = Mathf.Infinity;
            for( Int32 i = 1; i < this.aKeys.Length; ++i )
            {
                GradientAlphaKey key1 = this.aKeys[i];
                GradientAlphaKey key2 = this.aKeys[i-1];

                if( key1.time > t && key2.time <= t )
                {
                    Single dif = key2.time - key1.time;
                    Single localT = (t - key1.time) / dif;
                    alpha = Mathf.Lerp( key1.alpha, key2.alpha, localT );
                    break;
                } else
                {
                    Single diff1 = Mathf.Abs( key1.time - t );
                    Single diff2 = Mathf.Abs( key2.time - t );
                    if( diff1 < bestDiff )
                    {
                        bestDiff = diff1;
                        alpha = key1.alpha;
                    }
                    if( diff2 < bestDiff )
                    {
                        bestDiff = diff2;
                        alpha = key2.alpha;
                    }
                }
            }

            Color color = Color.white;
            bestDiff = Mathf.Infinity;
            for( Int32 i = 1; i < this.cKeys.Length; ++i )
            {
                GradientColorKey key1 = this.cKeys[i];
                GradientColorKey key2 = this.cKeys[i-1];

                if( key1.time > t && key2.time <= t )
                {
                    Single dif = key2.time - key1.time;
                    Single localT = (t - key1.time) / dif;
                    color = ExpandedInterpolation.Interpolate( key1.color, key2.color, localT, ExpandedInterpolation.Mode.Squares, ExpandedInterpolation.Style.Linear, ExpandedInterpolation.Clamping.Unclamped );
                    break;
                } else
                {
                    Single diff1 = Mathf.Abs( key1.time - t );
                    Single diff2 = Mathf.Abs( key2.time - t );
                    if( diff1 < bestDiff )
                    {
                        bestDiff = diff1;
                        color = key1.color;
                    }
                    if( diff2 < bestDiff )
                    {
                        bestDiff = diff2;
                        color = key2.color;
                    }
                }
            }

            if( this.outputSquared )
            {
                color *= color;
            }

            color.a = alpha;

            for( Int32 i = 0; i < this.texHeight; ++i )
            {
                _ = index + ( this.texWidth * i );
                this.texArray[index + ( this.texWidth * i )] = color;
            }
        }
        private NativeArray<Color> texArray;
        private readonly NativeArray<GradientAlphaKey> aKeys;
        private readonly NativeArray<GradientColorKey> cKeys;
        private readonly Int32 texWidth;
        private readonly Int32 texHeight;
        private readonly Boolean outputSquared;
        #endregion
    }
}
