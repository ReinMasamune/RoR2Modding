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
        internal static Material[] GetSniperMaterials()
        {
            var mat = new StandardMaterial("SniperMaterial");

            mat.blendDepth = 0.2f;
            mat.blueChannelBias = 0f;
            mat.blueChannelSmoothness = 0f;
            mat.cull = MaterialBase.CullMode.Off;
            mat.cutout = false;
            mat.decalLayer = StandardMaterial.DecalLayer.Character;
            mat.dither = false;
            mat.eliteBrightnessMax = 3.13f;
            mat.eliteBrightnessMin = -0.93f;
            mat.eliteIndex = EliteIndex.None;
            mat.emissionColor = new Color( 0f, 1f, 0.5937939f, 1f );
            mat.emissionPower = 1.5f;
            mat.fadeBias = 0f;
            mat.flashColor = new Color( 0f, 0f, 0f, 1f );
            mat.flowHeightBias = -0.04f;
            mat.flowHeightEmissionStrength = 1.51f;
            mat.flowHeightPower = 0f;
            mat.flowmapEnabled = false;
            mat.flowMaskStrength = 0f;
            mat.flowNormalStrength = 0f;
            mat.flowSpeed = 1f;
            mat.flowTextureScaleFactor = 1f;
            mat.fresnelBoost = 1f;
            mat.fresnelEmission = false;
            mat.fresnelPower = 1f;
            mat.greenChannelBias = 0f;
            mat.greenChannelSmoothness = 0f;
            mat.ignoreDiffuseAlphaForSpecular = false;
            mat.limbPrimeMask = 1f;
            mat.limbRemovalEnabled = false;
            mat.mainColor = new Color( 0.4f, 0.4f, 0.4f, 0.2f );
            mat.normalStrength = 0.25f;
            mat.printAlphaBias = 0f;
            mat.printAlphaDepth = 0.1f;
            mat.printBandHeight = 1f;
            mat.printColorBoost = 0f;
            mat.printDirection = StandardMaterial.PrintDirection.BottomUp;
            mat.printEmissionToAlbedoLerp = 0f;
            mat.printingEnabled = false;
            mat.rampChoice = MaterialBase.RampInfo.TwoTone;
            mat.sliceHeight = 5f;
            mat.smoothness = 0f;
            mat.specularExponent = 3f;
            mat.specularStrength = 0.2f;
            mat.splatmapEnabled = false;
            mat.splatmapTileScale = 1f;
            mat.useVertexColors = false;

            var mainTex = Tools.LoadTexture2D( Properties.Resources.SniperDiffuse, true, 1024, 1024 );
            mat.mainTexture.texture = mainTex;
            mat.normalMap.texture = Tools.LoadTexture2D( Properties.Resources.SniperNormal, true, 1024, 1024 );
            mat.emissionTexture.texture = Tools.LoadTexture2D( Properties.Resources.Sniper_Emissive_2__1_, true, 1024, 1024);

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

















            var mat2 = new StandardMaterial("SniperMaterial");

            mat2.blendDepth = 0.2f;
            mat2.blueChannelBias = 0f;
            mat2.blueChannelSmoothness = 0f;
            mat2.cull = MaterialBase.CullMode.Off;
            mat2.cutout = false;
            mat2.decalLayer = StandardMaterial.DecalLayer.Character;
            mat2.dither = false;
            mat2.eliteBrightnessMax = 3.13f;
            mat2.eliteBrightnessMin = -0.93f;
            mat2.eliteIndex = EliteIndex.None;
            mat2.emissionColor = new Color( 0f, 1f, 0.5937939f, 1f );
            mat2.emissionPower = 1.5f;
            mat2.fadeBias = 0f;
            mat2.flashColor = new Color( 0f, 0f, 0f, 1f );
            mat2.flowHeightBias = -0.04f;
            mat2.flowHeightEmissionStrength = 1.51f;
            mat2.flowHeightPower = 0f;
            mat2.flowmapEnabled = false;
            mat2.flowMaskStrength = 0f;
            mat2.flowNormalStrength = 0f;
            mat2.flowSpeed = 1f;
            mat2.flowTextureScaleFactor = 1f;
            mat2.fresnelBoost = 1f;
            mat2.fresnelEmission = false;
            mat2.fresnelPower = 1f;
            mat2.greenChannelBias = 0f;
            mat2.greenChannelSmoothness = 0f;
            mat2.ignoreDiffuseAlphaForSpecular = false;
            mat2.limbPrimeMask = 1f;
            mat2.limbRemovalEnabled = false;
            mat2.mainColor = new Color( 0.4f, 0.4f, 0.4f, 0.2f );
            mat2.normalStrength = 0.25f;
            mat2.printAlphaBias = 0f;
            mat2.printAlphaDepth = 0.1f;
            mat2.printBandHeight = 1f;
            mat2.printColorBoost = 0f;
            mat2.printDirection = StandardMaterial.PrintDirection.BottomUp;
            mat2.printEmissionToAlbedoLerp = 0f;
            mat2.printingEnabled = false;
            mat2.rampChoice = MaterialBase.RampInfo.TwoTone;
            mat2.sliceHeight = 5f;
            mat2.smoothness = 0f;
            mat2.specularExponent = 3f;
            mat2.specularStrength = 0.2f;
            mat2.splatmapEnabled = false;
            mat2.splatmapTileScale = 1f;
            mat2.useVertexColors = false;

            var mainTex2 = Tools.LoadTexture2D( Properties.Resources.SniperDiffuse, true, 1024, 1024 );
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
            var mat = new StandardMaterial("SniperMaterial");

            mat.blendDepth = 0.2f;
            mat.blueChannelBias = 0f;
            mat.blueChannelSmoothness = 0f;
            mat.cull = MaterialBase.CullMode.Off;
            mat.cutout = false;
            mat.decalLayer = StandardMaterial.DecalLayer.Character;
            mat.dither = false;
            mat.eliteBrightnessMax = 3.13f;
            mat.eliteBrightnessMin = -0.93f;
            mat.eliteIndex = EliteIndex.None;
            mat.emissionColor = new Color( 0f, 1f, 0.5937939f, 1f );
            mat.emissionPower = 1.5f;
            mat.fadeBias = 0f;
            mat.flashColor = new Color( 0f, 0f, 0f, 1f );
            mat.flowHeightBias = -0.04f;
            mat.flowHeightEmissionStrength = 1.51f;
            mat.flowHeightPower = 0f;
            mat.flowmapEnabled = false;
            mat.flowMaskStrength = 0f;
            mat.flowNormalStrength = 0f;
            mat.flowSpeed = 1f;
            mat.flowTextureScaleFactor = 1f;
            mat.fresnelBoost = 1f;
            mat.fresnelEmission = false;
            mat.fresnelPower = 1f;
            mat.greenChannelBias = 0f;
            mat.greenChannelSmoothness = 0f;
            mat.ignoreDiffuseAlphaForSpecular = false;
            mat.limbPrimeMask = 1f;
            mat.limbRemovalEnabled = false;
            mat.mainColor = new Color( 0.4f, 0.4f, 0.4f, 0.2f );
            mat.normalStrength = 0.25f;
            mat.printAlphaBias = 0f;
            mat.printAlphaDepth = 0.1f;
            mat.printBandHeight = 1f;
            mat.printColorBoost = 0f;
            mat.printDirection = StandardMaterial.PrintDirection.BottomUp;
            mat.printEmissionToAlbedoLerp = 0f;
            mat.printingEnabled = false;
            mat.rampChoice = MaterialBase.RampInfo.TwoTone;
            mat.sliceHeight = 5f;
            mat.smoothness = 0f;
            mat.specularExponent = 3f;
            mat.specularStrength = 0.2f;
            mat.splatmapEnabled = false;
            mat.splatmapTileScale = 1f;
            mat.useVertexColors = false;

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
