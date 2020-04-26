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
            mat.remapTexture.texture = TexturesCore.GenerateRampTexture( new Gradient
            {
                mode = GradientMode.Blend,
                alphaKeys = new[]
                {
                    new GradientAlphaKey(0f, 0f),
                    new GradientAlphaKey(0f, 0.5f),
                    new GradientAlphaKey(1f, 1f ),
                },
                colorKeys = new[]
                {
                    new GradientColorKey(Color.black, 0f),
                    new GradientColorKey(Color.black, 0.2f ),
                    new GradientColorKey(Color.white, 1f ),
                },
            } );
            mat.cloudTexture1.texture = AssetsCore.LoadAsset<Texture2D>( Texture2DIndex.refTexCloudDifferenceBW2 );
            mat.cloudTexture1.tiling = new Vector2( 1f, 1f );
            mat.cloudTexture2.texture = AssetsCore.LoadAsset<Texture2D>( Texture2DIndex.refTexCloudIce );
            mat.cloudTexture2.tiling = new Vector2( 1f, 1f );


            //return AssetsCore.LoadAsset<Material>( MaterialIndex.refMatGolemExplosion );
            return mat;
        }

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
            mat.remapTexture.texture = TexturesCore.GenerateRampTexture( new Gradient
            {
                mode = GradientMode.Blend,
                alphaKeys = new[]
                {
                    new GradientAlphaKey(0f, 0f),
                    new GradientAlphaKey(0f, 0.5f),
                    new GradientAlphaKey(1f, 1f ),
                },
                colorKeys = new[]
                {
                    new GradientColorKey(Color.black, 0f),
                    new GradientColorKey(Color.black, 0.2f ),
                    new GradientColorKey(Color.white, 1f ),
                },
            } );
            mat.cloudTexture1.texture = AssetsCore.LoadAsset<Texture2D>( Texture2DIndex.refTexMageMatrixMaskDirectional );
            mat.cloudTexture1.tiling = new Vector2( 0.11f, 1f );
            mat.cloudTexture2.texture = AssetsCore.LoadAsset<Texture2D>( Texture2DIndex.refTexCloudWaterRipples );
            mat.cloudTexture2.tiling = new Vector2( 0.13f, 1f );

            return mat;
            //return AssetsCore.LoadAsset<Material>( MaterialIndex.refMatGolemExplosion );
        }


        internal static Material[] GetSniperMaterials()
        {
            var mat = new StandardMaterial( "SniperMaterial" )
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

            Texture2D mainTex = Tools.LoadTexture2D( Properties.Resources.SniperDiffuse, true, 1024, 1024 );
            mat.mainTexture.texture = mainTex;
            mat.normalMap.texture = Tools.LoadTexture2D( Properties.Resources.SniperNormal, true, 1024, 1024 );
            mat.emissionTexture.texture = Tools.LoadTexture2D( Properties.Resources.Sniper_Emissive_2__1_, true, 1024, 1024);

            Material material = mat.material;
            material.SetFloat( "_AOON", 0f );
            material.SetFloat( "_DiffuseBias", 0.5f );
            material.SetFloat( "_DiffuseExponent", 0f );
            material.SetFloat( "_DiffuseScale", 0.5f );
            material.SetFloat( "_EliteOn", 0f );
            material.SetFloat( "_Fade", 1f );
            material.SetFloat( "_FadeDistance", 0f );
            material.SetFloat( "_FlowDiffuseStrength", 1f );
            material.SetFloat( "_LightWarpMaxDistance", 100f );
            material.SetFloat( "_LightWarpMinDistance", 10f );
            material.SetFloat( "_OcclusionStrength", 1f );
            material.SetFloat( "_RipPower", 0.5f );
            material.SetFloat( "_RimStrength", 1f );
            material.SetColor( "_RimTint", Color.white );
            material.SetColor( "_SpecularTint", new Color( 0.5441177f, 0.3247047f, 0.2600562f, 1f ) );

            material.doubleSidedGI = false;
            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            material.enableInstancing = false;
            material.renderQueue = -1;

















            var mat2 = new StandardMaterial( "SniperMaterial" )
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

            Texture2D mainTex2 = Tools.LoadTexture2D( Properties.Resources.SniperDiffuse, true, 1024, 1024 );
            mat2.mainTexture.texture = mainTex;
            mat2.normalMap.texture = Tools.LoadTexture2D( Properties.Resources.SniperNormal, true, 1024, 1024 );
            mat2.emissionTexture.texture = Tools.LoadTexture2D( Properties.Resources.Sniper_Emissive_2__1_, true, 1024, 1024 );

            var material2 = mat2.material;
            material2.SetFloat( "_AOON", 0f );
            material2.SetFloat( "_DiffuseBias", 0.5f );
            material2.SetFloat( "_DiffuseExponent", 0f );
            material2.SetFloat( "_DiffuseScale", 0.5f );
            material2.SetFloat( "_EliteOn", 0f );
            material2.SetFloat( "_Fade", 1f );
            material2.SetFloat( "_FadeDistance", 0f );
            material2.SetFloat( "_FlowDiffuseStrength", 1f );
            material2.SetFloat( "_LightWarpMaxDistance", 100f );
            material2.SetFloat( "_LightWarpMinDistance", 10f );
            material2.SetFloat( "_OcclusionStrength", 1f );
            material2.SetFloat( "_RipPower", 0.5f );
            material2.SetFloat( "_RimStrength", 1f );
            material2.SetColor( "_RimTint", Color.white );
            material2.SetColor( "_SpecularTint", new Color( 0.5441177f, 0.3247047f, 0.2600562f, 1f ) );

            material2.doubleSidedGI = false;
            material2.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            material2.enableInstancing = false;
            material2.renderQueue = -1;



            return new[] { material, material2 };
        }

        internal static Material GetRifleMaterial()
        {
            var mat = new StandardMaterial("RifleMaterial");
            mat.cutout = false;
            mat.mainColor = new Color( 0.6f, 0.6f, 0.6f, 1f );
            mat.mainTexture.texture = Tools.LoadTexture2D( Properties.Resources.Gauss_Diffuse, false );
            mat.normalStrength = 1f;
            mat.normalMap.texture = Tools.LoadTexture2D( Properties.Resources.Gauss_Normals, false );
            mat.emissionTexture.texture = Tools.LoadTexture2D( Properties.Resources.Gauss_Emissive, false );
            mat.emissionColor = Color.black;
            mat.emissionPower = 0.6f;
            mat.smoothness = 1f;
            mat.ignoreDiffuseAlphaForSpecular = false;
            mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
            mat.decalLayer = StandardMaterial.DecalLayer.Default;
            mat.specularStrength = 1f;
            mat.specularExponent = 1f;
            mat.cull = MaterialBase.CullMode.Off;
            mat.dither = false;
            mat.fadeBias = 0f;
            mat.fresnelEmission = false;
            mat.printingEnabled = false;
            mat.splatmapEnabled = false;
            mat.flowmapEnabled = false;
            mat.limbRemovalEnabled = false;
            mat.limbPrimeMask = 1f;
            mat.flashColor = Color.clear;



            return mat.material;
        }

        internal static Material GetKnifeMaterial()
        {
            var mat = new StandardMaterial( "SniperMaterial" )
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

            var mainTex = Tools.LoadTexture2D( Properties.Resources.SniperDiffuse, true, 1024, 1024 );
            mat.mainTexture.texture = mainTex;
            mat.normalMap.texture = Tools.LoadTexture2D( Properties.Resources.SniperNormal, true, 1024, 1024 );
            mat.emissionTexture.texture = Tools.LoadTexture2D( Properties.Resources.Sniper_Emissive_2__1_, true, 1024, 1024 );

            var material = mat.material;
            material.SetFloat( "_AOON", 0f );
            material.SetFloat( "_DiffuseBias", 0.5f );
            material.SetFloat( "_DiffuseExponent", 0f );
            material.SetFloat( "_DiffuseScale", 0.5f );
            material.SetFloat( "_EliteOn", 0f );
            material.SetFloat( "_Fade", 1f );
            material.SetFloat( "_FadeDistance", 0f );
            material.SetFloat( "_FlowDiffuseStrength", 1f );
            material.SetFloat( "_LightWarpMaxDistance", 100f );
            material.SetFloat( "_LightWarpMinDistance", 10f );
            material.SetFloat( "_OcclusionStrength", 1f );
            material.SetFloat( "_RipPower", 0.5f );
            material.SetFloat( "_RimStrength", 1f );
            material.SetColor( "_RimTint", Color.white );
            material.SetColor( "_SpecularTint", new Color( 0.5441177f, 0.3247047f, 0.2600562f, 1f ) );

            material.doubleSidedGI = false;
            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            material.enableInstancing = false;
            material.renderQueue = -1;


            return material;
        }

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
