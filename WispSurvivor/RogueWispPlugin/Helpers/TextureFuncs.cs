using BepInEx;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using UnityEngine;
using Unity.Collections;
using System.Diagnostics;
using Unity.Jobs;

namespace RogueWispPlugin.Helpers
{
    internal static class TextureGenerator
    {
        internal static Texture2D GenerateBarTexture( Color32 borderColor, Color32 fillColor, Int32 borderWidth, Boolean edgeRounding, Int32 width, Int32 height )
        {
            var tex = new Texture2D( width, height, TextureFormat.RGBAFloat, false );
            var handle = new BarTextureGenerator( borderColor, fillColor, borderWidth, edgeRounding, tex ).Schedule( width * height, 1 );
            tex.wrapMode = TextureWrapMode.Clamp;
            handle.Complete();
            tex.Apply();
            return tex;
        }

        private struct BarTextureGenerator : IJobParallelFor
        {
            internal BarTextureGenerator( Color borderColor, Color fillColor, Int32 borderWidth, Boolean edgeRounding, Texture2D texture )
            {
                this.width = texture.width;
                this.height = texture.height;
                this.tex = texture.GetRawTextureData<Color>();

                this.borderColor = borderColor;
                this.fillColor = fillColor;
                this.borderWidth = borderWidth;
                this.edgeRadiusSq = edgeRounding ? this.borderWidth : 0;
                this.edgeL = this.borderWidth;
                this.edgeR = this.width - this.borderWidth;
                this.edgeB = this.borderWidth;
                this.edgeT = this.height - this.borderWidth;

                this.fillBox = new RectInt( this.borderWidth, this.borderWidth, this.width - this.borderWidth * 2, this.height - this.borderWidth * 2 );
            }
            private readonly Color borderColor;
            private readonly Color fillColor;
            private readonly Int32 borderWidth;
            private readonly Int32 edgeRadiusSq;
            private readonly RectInt fillBox;
            private readonly Int32 edgeL;
            private readonly Int32 edgeR;
            private readonly Int32 edgeT;
            private readonly Int32 edgeB;

            private readonly Int32 width;
            private readonly Int32 height;
            private NativeArray<Color> tex;


            public void Execute( Int32 index )
            {
                this.tex[index] = this.CalculateColor( index );
            }

            private Color CalculateColor( Int32 index )
            {
                var pos = new Vector2Int( index % this.width, index / this.width );
                if( this.fillBox.Contains( pos ) ) return this.fillColor;
                if( this.edgeRadiusSq == 0 ) return this.borderColor;
                
                if( pos.x >= this.edgeR )
                {
                    if( pos.y >= this.edgeT )
                    {
                        //return Color.cyan;
                        if( Vector2Int.Distance( pos, new Vector2Int( this.edgeR, this.edgeT ) ) >= this.edgeRadiusSq ) return Color.clear;
                    } else if( pos.y < this.edgeB )
                    {
                        //return Color.blue;
                        if( Vector2Int.Distance( pos, new Vector2Int( this.edgeR, this.edgeB ) ) >= this.edgeRadiusSq ) return Color.clear;
                    }
                } else if( pos.x < this.edgeL )
                {
                    if( pos.y >= this.edgeT )
                    {
                        //return Color.green;
                        if( Vector2Int.Distance( pos, new Vector2Int( this.edgeL, this.edgeT ) ) >= this.edgeRadiusSq ) return Color.clear;
                    } else if( pos.y < this.edgeB )
                    {
                        if( Vector2Int.Distance( pos, new Vector2Int( this.edgeL, this.edgeB ) ) >= this.edgeRadiusSq ) return Color.clear;
                        //return Color.red;
                    }
                }
                return this.borderColor;
            }
        }




        internal static Texture2D GenerateCircleTexture( Color bgColor, Color borderColor, Color fillColor, Vector2 center, Single borderStart, Single fillStart, Single fillEnd, Single borderEnd, Int32 width, Int32 height )
        {
            var tex = new Texture2D( width, height, TextureFormat.RGBAFloat, false );
            var handle = new CircleTextureGenerator( bgColor, borderColor, fillColor, center, borderStart, fillStart, fillEnd, borderEnd, tex ).Schedule(width * height, 1 );
            tex.wrapMode = TextureWrapMode.Clamp;
            handle.Complete();
            tex.Apply();
            return tex;
        }

        private struct CircleTextureGenerator : IJobParallelFor
        {
            internal CircleTextureGenerator( Color bgColor, Color borderColor, Color fillColor, Vector2 center, Single borderStart, Single fillStart, Single fillEnd, Single borderEnd, Texture2D texture )
            {
                this.width = texture.width;
                this.height = texture.height;
                this.tex = texture.GetRawTextureData<Color>();


                this.bgColor = bgColor;
                this.borderColor = borderColor;
                this.fillColor = fillColor;
                this.center = (center + Vector2.one) / 2;
                this.borderStart = borderStart * borderStart / 4;
                this.fillStart = fillStart * fillStart / 4;
                this.fillEnd = fillEnd * fillEnd / 4;
                this.borderEnd = borderEnd * borderEnd / 4;
            }

            [ReadOnly]
            private readonly Color bgColor;
            [ReadOnly]
            private readonly Color borderColor;
            [ReadOnly]
            private readonly Color fillColor;
            [ReadOnly]
            private readonly Vector2 center;
            [ReadOnly]
            private readonly Single borderStart;
            [ReadOnly]
            private readonly Single fillStart;
            [ReadOnly]
            private readonly Single fillEnd;
            [ReadOnly]
            private readonly Single borderEnd;
            [ReadOnly]
            private readonly Int32 width;
            [ReadOnly]
            private readonly Int32 height;
            private NativeArray<Color> tex;


            public void Execute( Int32 index )
            {
                this.tex[index] = this.SelectColor( index );
            }

            private Color SelectColor( Int32 index )
            {
                var distSq = (new Vector2( (Single)(index % this.width ) / (Single)this.width, (Single)( index / this.width ) / (Single)this.height ) - this.center).sqrMagnitude;
                if( distSq > this.borderStart ) return this.bgColor;
                else if( distSq > this.fillStart ) return this.borderColor;
                else if( distSq > this.fillEnd ) return this.fillColor;
                else if( distSq > this.borderEnd ) return this.borderColor;
                else return this.bgColor;
            }
        }


        internal static void ApplyRampTexture( Texture2D tex, Gradient grad )
        {
            NativeArray<Color> texArray = tex.GetRawTextureData<Color>();
            var alpha = grad.alphaKeys.OrderBy<GradientAlphaKey,Single>( (key) => key.time ).ToArray();
            var color = grad.colorKeys.OrderBy<GradientColorKey,Single>( (key) => key.time ).ToArray();
            NativeArray<GradientAlphaKey> gradAKeys = new NativeArray<GradientAlphaKey>( alpha, Allocator.TempJob );
            NativeArray<GradientColorKey> gradCKeys = new NativeArray<GradientColorKey>( color, Allocator.TempJob );

            var gradLerp = new LerpGradient
            {
                aKeys = gradAKeys,
                cKeys = gradCKeys,
                texArray = texArray,
                texHeight = tex.height,
                texWidth = tex.width,
            };

            var handle = gradLerp.Schedule( tex.width, 1 );
            handle.Complete();
            gradAKeys.Dispose();
            gradCKeys.Dispose();

            tex.Apply();
        }

        internal static Texture2D GenerateRampTexture( Gradient grad, Int32 width = 256, Int32 height = 16, Boolean threaded = true )
        {
#if TIMER
            var timer = new Stopwatch();
#endif

            var tex = new Texture2D( width, height, TextureFormat.RGBAFloat, false );
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
                NativeArray<Color> texArray = tex.GetRawTextureData<Color>();
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
            Main.LogM( timer.ElapsedMilliseconds + " rampTexGen, threaded = " + threaded );
#endif
            return tex;
        }

        internal struct LerpGradient : IJobParallelFor
        {
            internal NativeArray<Color> texArray;
            [ReadOnly]
            internal NativeArray<GradientAlphaKey> aKeys;
            [ReadOnly]
            internal NativeArray<GradientColorKey> cKeys;
            [ReadOnly]
            internal Int32 texWidth;
            [ReadOnly]
            internal Int32 texHeight;


            public void Execute( Int32 ind )
            {
                var t = ind / (Single)this.texWidth;

                Single alpha = 0f;
                var found = false;
                var bestDiff = Mathf.Infinity;
                for( Int32 i = 1; i < this.aKeys.Length; ++i )
                {
                    var key1 = this.aKeys[i];
                    var key2 = this.aKeys[i-1];

                    if( key1.time > t && key2.time <= t )
                    {
                        var dif = key2.time - key1.time;
                        var localT = (t - key1.time) / dif;
                        alpha = Mathf.Lerp( key1.alpha, key2.alpha, localT );
                        found = true;
                        break;
                    } else
                    {
                        var diff1 = Mathf.Abs(key1.time - t);
                        var diff2 = Mathf.Abs(key2.time - t);
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
                found = false;
                bestDiff = Mathf.Infinity;
                for( Int32 i = 1; i < this.cKeys.Length; ++i )
                {
                    var key1 = this.cKeys[i];
                    var key2 = this.cKeys[i-1];

                    if( key1.time > t && key2.time <= t )
                    {
                        var dif = key2.time - key1.time;
                        var localT = (t - key1.time) / dif;
                        color = LerpColorSQ( key1.color, key2.color, localT, ColorMode.Square );
                        found = true;
                        break;
                    } else
                    {
                        var diff1 = Mathf.Abs( key1.time - t );
                        var diff2 = Mathf.Abs( key2.time - t );
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

                color = this.CorrectColor( color );
                color.a = alpha;
                
                for( Int32 i = 0; i < this.texHeight; ++i )
                {
                    var loc = ind + this.texWidth * i;
                    this.texArray[loc] = color;
                }
            }

            private Color CorrectColor( Color input )
            {
                //return input;
                return new Color( input.r * input.r, input.g * input.g, input.b * input.b );
            }
        }

        public enum ColorMode { Standard, Square, Root };
        public static Color LerpColorSQ( Color c1, Color c2, Single t, ColorMode mode )
        {
            if( mode == ColorMode.Standard )
            {
                return Color.Lerp( c1, c2, t );
            } else if( mode == ColorMode.Square )
            {
                var a1s = c1.a * c1.a;
                var a2s = c2.a * c2.a;
                var r1s = c1.r * c1.r;
                var r2s = c2.r * c2.r;
                var g1s = c1.g * c1.g;
                var g2s = c2.g * c2.g;
                var b1s = c1.b * c1.b;
                var b2s = c2.b * c2.b;

                var a = Mathf.Lerp( a1s, a2s, t );
                var r = Mathf.Lerp( r1s, r2s, t );
                var g = Mathf.Lerp( g1s, g2s, t );
                var b = Mathf.Lerp( b1s, b2s, t );

                return new Color( Mathf.Sqrt( r ), Mathf.Sqrt( g ), Mathf.Sqrt( b ), Mathf.Sqrt( a ) );
            } else if( mode == ColorMode.Root )
            {
                var a1s = Mathf.Sqrt( c1.a );
                var a2s = Mathf.Sqrt( c2.a );
                var r1s = Mathf.Sqrt( c1.r );
                var r2s = Mathf.Sqrt( c2.r );
                var g1s = Mathf.Sqrt( c1.g );
                var g2s = Mathf.Sqrt( c2.g );
                var b1s = Mathf.Sqrt( c1.b );
                var b2s = Mathf.Sqrt( c2.b );

                var a = Mathf.Lerp( a1s, a2s, t );
                var r = Mathf.Lerp( r1s, r2s, t );
                var g = Mathf.Lerp( g1s, g2s, t );
                var b = Mathf.Lerp( b1s, b2s, t );

                return new Color( r * r, g * g, b * b, a * a );
            } else
            {
                return Color.Lerp( c1, c2, t );
            }
        }
    }
}
