using BepInEx;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using R2API;
using UnityEngine;
using Unity.Collections;
using System.Diagnostics;
using Unity.Jobs;

namespace RogueWispPlugin.Helpers
{
    internal static class RampTextureGenerator
    {
        internal static Texture2D GenerateRampTexture( Gradient grad, Int32 width = 256, Int32 height = 16, Boolean threaded = false )
        {
#if TIMER
            var timer = new Stopwatch();
#endif

            var tex = new Texture2D( width, height, TextureFormat.ARGB32, false );
            tex.wrapMode = TextureWrapMode.Clamp;
            if( !threaded )
            {
#if TIMER
                timer.Start();
#endif
                for( Int32 x = 0; x < width; ++x )
                {
                    var color = grad.Evaluate( (Single)x / (Single)width );
                    for( Int32 y = 0; y < height; ++y )
                    {
                        tex.SetPixel( x, y, color );
                    }
                }
            } else
            {
#if TIMER
                timer.Start();
#endif
                NativeArray<Color32> texArray = tex.GetRawTextureData<Color32>();
                var alpha = grad.alphaKeys.OrderBy<GradientAlphaKey,Single>( (key) => key.time ).ToArray();
                var color = grad.colorKeys.OrderBy<GradientColorKey,Single>( (key) => key.time ).ToArray();
                NativeArray<GradientAlphaKey> gradAKeys = new NativeArray<GradientAlphaKey>( alpha, Allocator.TempJob );
                NativeArray<GradientColorKey> gradCKeys = new NativeArray<GradientColorKey>( color, Allocator.TempJob );

                var gradLerp = new LerpGradient
                {
                    aKeys = gradAKeys,
                    cKeys = gradCKeys,
                    texArray = texArray,
                    texHeight = height,
                    texWidth = width,
                };

                var handle = gradLerp.Schedule( width, 1 );
                handle.Complete();
                gradAKeys.Dispose();
                gradCKeys.Dispose();
            }

            tex.Apply();
#if TIMER
            timer.Stop();
            Main.LogI( timer.ElapsedMilliseconds + " rampTexGen, threaded = " + threaded );
#endif
            return tex;
        }

        internal struct LerpGradient : IJobParallelFor
        {
            internal NativeArray<Color32> texArray;
            internal NativeArray<GradientAlphaKey> aKeys;
            internal NativeArray<GradientColorKey> cKeys;
            internal Int32 texWidth;
            internal Int32 texHeight;


            public void Execute( Int32 ind )
            {
                var t = ind / (Single)this.texWidth;

                Single alpha = 0f;
                for( Int32 i = 1; i < this.aKeys.Length; ++i )
                {
                    var key1 = this.aKeys[i];
                    var key2 = this.aKeys[i-1];

                    if( key1.time <= t && key2.time > t )
                    {
                        var dif = key2.time - key1.time;
                        var localT = (t - key1.time) / dif;
                        alpha = Mathf.Lerp( key1.alpha, key2.alpha, localT );
                    }
                }

                Color32 color = Color.white;
                for( Int32 i = 1; i < this.cKeys.Length; ++i )
                {
                    var key1 = this.cKeys[i];
                    var key2 = this.cKeys[i-1];

                    if( key1.time <= t && key2.time > t )
                    {
                        var dif = key2.time - key1.time;
                        var localT = (t - key1.time) / dif;
                        color = Color32.Lerp( key1.color, key2.color, localT );
                    }
                }

                for( Int32 i = 0; i < this.texHeight; ++i )
                {
                    var loc = ind + this.texHeight * i;
                    this.texArray[loc] = color;
                }
            }
        }
    }
}
