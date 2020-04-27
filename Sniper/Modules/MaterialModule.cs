using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;
using Sniper.Properties;
using Unity.Jobs;
using System.Diagnostics;
using Unity.Collections;

namespace Sniper.Modules
{
    internal static class MaterialModule
    {
        // TODO: Cache this
        internal static CloudMaterial GetStandardTracerMaterial()
        {
            var mat = new CloudMaterial( "StandardAmmoTracer" )
            {
                alphaBias = 0f,
                alphaBoost = 1f,
                boost = 15f,
                calcTexAlpha = false,
                cloudDistortionOn = false,
                cloudRemappingOn = true,
                cull = MaterialBase.CullMode.Off,
                destinationBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha,
                fadeClose = false,
                externalAlpha = 1f,
                disableRemapping = false,
                distortionStrength = 0f,
                emissionOn = false,
                fadeCloseDistance = 0.5f,
                fresnelFade = false,
                fresnelPower = 0f,
                internalSimpleBlendMode = 0f,
                invFade = 0f,
                sourceBlend = UnityEngine.Rendering.BlendMode.SrcAlpha,
                useUV1 = false,
                vertexOffsetAmount = 0f,
                vertexAlphaOn = true,
                vertexColorOn = false,
                vertexOffset = false,
                color = Color.white,
                cutoffScrollSpeed = new Vector4( 15f, 15f, 13f, 13f ),
                emissionColor = Color.white,
                tintColor = Color.white
            };
            mat.mainTexture.texture = AssetsCore.LoadAsset<Texture2D>( Texture2DIndex.refTexParticleDust2Mask );
            mat.remapTexture.texture = TextureModule.GetStandardAmmoRamp();
            mat.cloudTexture1.texture = AssetsCore.LoadAsset<Texture2D>( Texture2DIndex.refTexCloudDifferenceBW2 );
            mat.cloudTexture1.tiling = new Vector2( 1f, 1f );
            mat.cloudTexture2.texture = AssetsCore.LoadAsset<Texture2D>( Texture2DIndex.refTexCloudIce );
            mat.cloudTexture2.tiling = new Vector2( 1f, 1f );


            //return AssetsCore.LoadAsset<Material>( MaterialIndex.refMatGolemExplosion );
            return mat;
        }

        // TODO: Cache this
        internal static CloudMaterial GetStandardTracerTrailMaterial()
        {
            var mat = new CloudMaterial( "StandardAmmoTracerTrail" )
            {
                alphaBias = 0f,
                alphaBoost = 2f,
                boost = 5f,
                calcTexAlpha = false,
                cloudDistortionOn = false,
                cloudRemappingOn = true,
                cull = MaterialBase.CullMode.Off,
                destinationBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha,
                fadeClose = false,
                externalAlpha = 1f,
                disableRemapping = false,
                distortionStrength = 0f,
                emissionOn = false,
                fadeCloseDistance = 0.5f,
                fresnelFade = false,
                fresnelPower = 0f,
                internalSimpleBlendMode = 0f,
                invFade = 0f,
                sourceBlend = UnityEngine.Rendering.BlendMode.SrcAlpha,
                useUV1 = false,
                vertexOffsetAmount = 0f,
                vertexAlphaOn = true,
                vertexColorOn = false,
                vertexOffset = false,
                color = Color.white,
                cutoffScrollSpeed = new Vector4( 15f, 15f, 13f, 13f ),
                emissionColor = Color.white,
                tintColor = Color.white
            };
            mat.mainTexture.texture = AssetsCore.LoadAsset<Texture2D>( Texture2DIndex.refTexAlphaGradient2Mask );
            mat.remapTexture.texture = TextureModule.GetStandardAmmoRamp();
            mat.cloudTexture1.texture = AssetsCore.LoadAsset<Texture2D>( Texture2DIndex.refTexMageMatrixMaskDirectional );
            mat.cloudTexture1.tiling = new Vector2( 0.13f, 1f );
            mat.cloudTexture2.texture = AssetsCore.LoadAsset<Texture2D>( Texture2DIndex.refTexCloudWaterRipples );
            mat.cloudTexture2.tiling = new Vector2( 0.17f, 1f );

            return mat;
            //return AssetsCore.LoadAsset<Material>( MaterialIndex.refMatGolemExplosion );
        }



        #region Sniper
        private static StandardMaterial GetSniperBase()
        {
            if( _sniperBase == null )
            {
                var mat = new StandardMaterial( "SniperBase" )
                {
                    blendDepth = 0.2f,
                    blueChannelBias = 0f,
                    blueChannelSmoothness = 0f,
                    cull = MaterialBase.CullMode.Off,
                    cutout = false,
                    decalLayer = StandardMaterial.DecalLayer.Character,
                    dither = false,
                    eliteBrightnessMax = 3.13f,
                    eliteBrightnessMin = -0.93f,
                    eliteIndex = EliteIndex.None,
                    emissionColor = new Color( 0f, 1f, 0.5937939f, 1f ),
                    emissionPower = 1.5f,
                    fadeBias = 0f,
                    flashColor = new Color( 0f, 0f, 0f, 1f ),
                    flowHeightBias = -0.04f,
                    flowHeightEmissionStrength = 1.51f,
                    flowHeightPower = 0f,
                    flowmapEnabled = false,
                    flowMaskStrength = 0f,
                    flowNormalStrength = 0f,
                    flowSpeed = 1f,
                    flowTextureScaleFactor = 1f,
                    fresnelBoost = 1f,
                    fresnelEmission = false,
                    fresnelPower = 1f,
                    greenChannelBias = 0f,
                    greenChannelSmoothness = 0f,
                    ignoreDiffuseAlphaForSpecular = false,
                    limbPrimeMask = 1f,
                    limbRemovalEnabled = false,
                    mainColor = new Color( 0.4f, 0.4f, 0.4f, 0.2f ),
                    normalStrength = 0.25f,
                    printAlphaBias = 0f,
                    printAlphaDepth = 0.1f,
                    printBandHeight = 1f,
                    printColorBoost = 0f,
                    printDirection = StandardMaterial.PrintDirection.BottomUp,
                    printEmissionToAlbedoLerp = 0f,
                    printingEnabled = false,
                    rampChoice = MaterialBase.RampInfo.TwoTone,
                    sliceHeight = 5f,
                    smoothness = 0f,
                    specularExponent = 3f,
                    specularStrength = 0.2f,
                    splatmapEnabled = false,
                    splatmapTileScale = 1f,
                    useVertexColors = false
                };
                _sniperBase = mat;
            }

            return _sniperBase;
        }
        private static StandardMaterial _sniperBase;


        internal static StandardMaterial GetSniperClassicBase()
        {
            if( _sniperClassicBase == null )
            {
                var baseMat = GetSniperBase().Clone();
                var set = TextureModule.GetSniperClassicTextures();
                set.Apply( baseMat );
                _sniperClassicBase = baseMat;
            }

            return _sniperClassicBase;
        }
        private static StandardMaterial _sniperClassicBase;

        internal static StandardMaterial GetSniperRedBase()
        {
            if( _sniperRedBase == null )
            {
                var baseMat = GetSniperBase().Clone();
                var set = TextureModule.GetSniperRedTextures();
                set.Apply( baseMat );
                _sniperRedBase = baseMat;
            }

            return _sniperRedBase;
        }
        private static StandardMaterial _sniperRedBase;

        internal static StandardMaterial GetSniperRedBlackBase()
        {
            if( _sniperRedBlackBase == null )
            {
                var baseMat = GetSniperBase().Clone();
                var set = TextureModule.GetSniperRedBlackTextures();
                set.Apply( baseMat );
                _sniperRedBlackBase = baseMat;
            }

            return _sniperRedBlackBase;
        }
        private static StandardMaterial _sniperRedBlackBase;

        internal static Material GenerateAmmoMaterial( StandardMaterial baseMaterial )
        {
            var mat = baseMaterial.Clone();

            return mat.material;
        }

        internal static Material GenerateArmorMaterial( StandardMaterial baseMaterial )
        {
            var mat = baseMaterial.Clone();

            return mat.material;
        }

        internal static Material GenerateCloakMaterial( StandardMaterial baseMaterial )
        {
            var mat = baseMaterial.Clone();

            return mat.material;
        }

        internal static Material GenerateBodyMaterial( StandardMaterial baseMaterial )
        {
            var mat = baseMaterial.Clone();

            return mat.material;
        }

        internal static Material GenerateEmissionMaterial( StandardMaterial baseMaterial )
        {
            var mat = baseMaterial.Clone();

            return mat.material;
        }
        #endregion

        #region Railgun
        private static StandardMaterial GetRailBase()
        {
            if( _railBase == null )
            {
                var mat = new StandardMaterial( "RailgunBase" )
                {
                    cutout = false,
                    mainColor = new Color( 0.6f, 0.6f, 0.6f, 1f ),
                    normalStrength = 1f,
                    emissionColor = Color.black,
                    emissionPower = 0.6f,
                    smoothness = 1f,
                    ignoreDiffuseAlphaForSpecular = false,
                    rampChoice = MaterialBase.RampInfo.SmoothedTwoTone,
                    decalLayer = StandardMaterial.DecalLayer.Default,
                    specularStrength = 1f,
                    specularExponent = 1f,
                    cull = MaterialBase.CullMode.Off,
                    dither = false,
                    fadeBias = 0f,
                    fresnelEmission = false,
                    printingEnabled = false,
                    splatmapEnabled = false,
                    flowmapEnabled = false,
                    limbRemovalEnabled = false,
                    limbPrimeMask = 1f,
                    flashColor = Color.clear
                };

                _railBase = mat;
            }

            return _railBase;
        }
        private static StandardMaterial _railBase;

        internal static Material GetRailDefault()
        {
            if( _railDefault == null )
            {
                var mat = GetRailBase().Clone();
                var tex = TextureModule.GetSniperRailTextures();
                tex.Apply( mat );
                _railDefault = mat;
            }

            return _railDefault.material;
        }
        private static StandardMaterial _railDefault;
        #endregion

        

        public struct SamplerJob : IJobParallelFor, IDisposable
        {
            public SamplerJob( Texture2D source, Texture2D dest, Int32 radius, Single weight )
            {
                this.sourceTex = source.GetRawTextureData<Color32>();
                this.destTex = dest.GetRawTextureData<Color>();
                this.width = source.width;
                this.height = source.height;
                this.radius = radius;
                this.weight = weight;
            }

            private readonly NativeArray<Color32> sourceTex;
            private readonly Int32 width;
            private readonly Int32 height;
            private readonly Int32 radius;
            private readonly Single weight;

            private NativeArray<Color> destTex;

            public void Execute( Int32 index )
            {
                try
                {
                    var x = index % this.width;
                    var y = index / this.width;

                    var firstPix = this.sourceTex[index];

                    Single r = firstPix.r, g = firstPix.g, b = firstPix.b, a = firstPix.a;
                    var counter = 1;

                    for( Int32 i = 0; i < this.radius; ++i )
                    {
                        var sampX = x + i;
                        if( sampX >= 0 && sampX < this.width )
                        {
                            for( Int32 j = 0; j < this.radius; ++j )
                            {
                                var sampY = y + j;
                                if( sampY >= 0 && sampY < this.height )
                                {
                                    Color res = this.sourceTex[sampY * this.width + sampX];
                                    a += res.r;
                                    r += res.g;
                                    g += res.b;
                                    b += res.a;
                                }

                                sampY = y - j;
                                if( sampY >= 0 && sampY < this.height )
                                {
                                    Color res = this.sourceTex[sampY * this.width + sampX];
                                    a += res.r;
                                    r += res.g;
                                    g += res.b;
                                    b += res.a;
                                }
                            }
                        }
                        sampX = x - i;
                        if( sampX >= 0 && sampX < this.width )
                        {
                            for( Int32 j = 0; j < this.radius; ++j )
                            {
                                var sampY = y + j;
                                if( sampY >= 0 && sampY < this.height )
                                {
                                    Color res = this.sourceTex[sampY * this.width + sampX];
                                    a += res.r;
                                    r += res.g;
                                    g += res.b;
                                    b += res.a;
                                }

                                sampY = y - j;
                                if( sampY >= 0 && sampY < this.height )
                                {
                                    Color res = this.sourceTex[sampY * this.width + sampX];
                                    a += res.r;
                                    r += res.g;
                                    g += res.b;
                                    b += res.a;
                                }
                            }
                        }
                    }
                    r /= counter;
                    g /= counter;
                    b /= counter;
                    a /= counter;

                    this.destTex[index] = Color.Lerp( firstPix, new Color( r, g, b, a ), this.weight );
                } catch
                {
                    Log.Error( index );
                }
            }

            public void Dispose()
            {

            }
        }
    }
}
