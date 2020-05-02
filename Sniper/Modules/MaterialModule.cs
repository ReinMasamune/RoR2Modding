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

        // TODO: Cache this
        internal static CloudMaterial GetExplosiveTracerMaterial()
        {
            var mat = new CloudMaterial( "ExplosiveAmmoTracer" )
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
            mat.remapTexture.texture = TextureModule.GetExplosiveAmmoRamp();
            mat.cloudTexture1.texture = AssetsCore.LoadAsset<Texture2D>( Texture2DIndex.refTexCloudDifferenceBW2 );
            mat.cloudTexture1.tiling = new Vector2( 1f, 1f );
            mat.cloudTexture2.texture = AssetsCore.LoadAsset<Texture2D>( Texture2DIndex.refTexCloudIce );
            mat.cloudTexture2.tiling = new Vector2( 1f, 1f );


            //return AssetsCore.LoadAsset<Material>( MaterialIndex.refMatGolemExplosion );
            return mat;
        }

        // TODO: Cache this
        internal static CloudMaterial GetExplosiveTracerTrailMaterial()
        {
            var mat = new CloudMaterial( "ExplosiveAmmoTracerTrail" )
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
            mat.remapTexture.texture = TextureModule.GetExplosiveAmmoRamp();
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
                    mainColor = Color.white,
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


        internal static StandardMaterial GetSniperDefaultBase()
        {
            if( _sniperDefaultBase == null )
            {
                var baseMat = GetSniperBase().Clone();
                var set = TextureModule.GetSniperDefaultTextures();
                set.Apply( baseMat );

                baseMat.mainColor *= 0.8f;
                _sniperDefaultBase = baseMat;
            }

            return _sniperDefaultBase;
        }
        private static StandardMaterial _sniperDefaultBase;

        internal static StandardMaterial GetSniperSkin1Base()
        {
            if( _sniperSkin1Base == null )
            {
                var baseMat = GetSniperBase().Clone();
                var set = TextureModule.GetSniperSkin1Textures();
                set.Apply( baseMat );
                _sniperSkin1Base = baseMat;
            }

            return _sniperSkin1Base;
        }
        private static StandardMaterial _sniperSkin1Base;

        internal static StandardMaterial GetSniperSkin2Base()
        {
            if( _sniperSkin2Base == null )
            {
                var baseMat = GetSniperBase().Clone();
                var set = TextureModule.GetSniperSkin2Textures();
                set.Apply( baseMat );
                _sniperSkin2Base = baseMat;
            }

            return _sniperSkin2Base;
        }
        private static StandardMaterial _sniperSkin2Base;

        internal static Material GenerateAmmoMaterial( StandardMaterial baseMaterial )
        {
            var mat = baseMaterial.Clone();

            mat.mainColor = Color.white;
            mat.emissionColor = new Color( 0f, 1f, 0f, 1f );
            mat.normalStrength = 0.05f;
            mat.emissionPower = 5f;
            mat.smoothness = 0.5f;
            mat.ignoreDiffuseAlphaForSpecular = true;
            mat.rampChoice = MaterialBase.RampInfo.TwoTone;
            mat.specularStrength = 0.75f;
            mat.specularExponent = 2f;
            mat.cull = MaterialBase.CullMode.Back;

            return mat.material;
        }

        internal static Material GenerateArmorMaterial( StandardMaterial baseMaterial )
        {
            var mat = baseMaterial.Clone();

            mat.mainColor = Color.white;
            mat.normalStrength = 0.3f;
            mat.emissionColor = Color.white;
            mat.emissionPower = 1f;
            mat.smoothness = 1f;
            mat.ignoreDiffuseAlphaForSpecular = true;
            mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
            mat.specularStrength = 0.1f;
            mat.specularExponent = 1f;
            mat.cull = MaterialBase.CullMode.Back;

            return mat.material;
        }

        internal static Material GenerateCloakMaterial( StandardMaterial baseMaterial )
        {
            var mat = baseMaterial.Clone();

            mat.mainColor = Color.white;
            mat.normalStrength = 0.6f;
            mat.emissionColor = Color.clear;
            mat.emissionPower = 0f;
            mat.smoothness = 0f;
            mat.ignoreDiffuseAlphaForSpecular = true;
            mat.rampChoice = MaterialBase.RampInfo.SubSurface;
            mat.specularStrength = 0.05f;
            mat.specularExponent = 3f;
            mat.cull = MaterialBase.CullMode.Off;

            return mat.material;
        }

        internal static Material GenerateBodyMaterial( StandardMaterial baseMaterial )
        {
            var mat = baseMaterial.Clone();

            mat.mainColor = new Color( 0.7f, 0.7f, 0.7f, 1f );
            mat.emissionColor = Color.clear;
            mat.emissionPower = 0f;
            mat.normalStrength = 0.8f;
            mat.smoothness = 0.25f;
            mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
            mat.ignoreDiffuseAlphaForSpecular = true;
            mat.specularStrength = 0.5f;
            mat.specularExponent = 3f;
            mat.cull = MaterialBase.CullMode.Back;


            return mat.material;
        }

        internal static Material GenerateEmissionMaterial( StandardMaterial baseMaterial )
        {
            var mat = baseMaterial.Clone();

            mat.emissionColor = Color.white;
            mat.emissionPower = 1f;
            mat.mainColor = Color.white;
            mat.smoothness = 0f;
            mat.cull = MaterialBase.CullMode.Back;
            mat.specularExponent = 1f;
            mat.specularStrength = 0f;
            mat.rampChoice = MaterialBase.RampInfo.Unlitish;
            mat.normalStrength = 0f;



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
                var tex = TextureModule.GetRailDefaultTextures();
                tex.Apply( mat );

                mat.mainColor = Color.white;
                mat.normalStrength = 1f;
                mat.emissionColor = new Color( 0f, 1f, 0.48f, 1f );
                mat.emissionPower = 1f;
                mat.smoothness = 0.5f;
                mat.ignoreDiffuseAlphaForSpecular = true;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularStrength = 0.3f;
                mat.specularExponent = 3f;
                mat.cull = MaterialBase.CullMode.Back;





                _railDefault = mat;
            }

            return _railDefault.material;
        }
        private static StandardMaterial _railDefault;


        internal static Material GetRailAlt1()
        {
            if( _railAlt1 == null )
            {
                var mat = GetRailBase().Clone();
                var tex = TextureModule.GetRailAlt1Textures();
                tex.Apply( mat );
                _railAlt1 = mat;
            }
            return _railAlt1.material;
        }
        private static StandardMaterial _railAlt1;

        internal static Material GetRailAlt2()
        {
            if( _railAlt2 == null )
            {
                var mat = GetRailBase().Clone();
                var tex = TextureModule.GetRailAlt2Textures();
                tex.Apply( mat );
                _railAlt2 = mat;
            }

            return _railAlt2.material;
        }
        private static StandardMaterial _railAlt2;
        #endregion

        #region Throw Knife
        private static StandardMaterial GetThrowKnifeBase()
        {
            if( _throwKnifeBase == null )
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
                    mainColor = Color.white,
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

                _throwKnifeBase = mat;
            }

            return _throwKnifeBase;
        }
        private static StandardMaterial _throwKnifeBase;




        internal static Material GetThrowKnifeDefault()
        {
            if( _throwKnifeDefault == null )
            {
                var mat = GetThrowKnifeBase().Clone();
                var tex = TextureModule.GetThrowKnifeDefaultTextures();
                tex.Apply( mat );
                _throwKnifeDefault = mat;
            }
            return _throwKnifeDefault.material;
        }
        private static StandardMaterial _throwKnifeDefault;


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
