using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReinCore
{
    internal static class ShaderInitializer
    {
        private static Boolean completedProperly = false;
        internal static Boolean Initialize()
        {
            return completedProperly;
        }


        static ShaderInitializer()
        {
            #region Hopoo Deferred
            new AssetAccessor<Shader>( ShaderIndex.HGSnowTopped, () =>
            {
                return Resources.Load<Shader>( "Shaders/Deferred/HGSnowTopped" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGStandard, () =>
            {
                return Resources.Load<Shader>( "Shaders/Deferred/HGStandard" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGTriPlanarTerrainBlend, () =>
            {
                return Resources.Load<Shader>( "Shaders/Deferred/HGTriPlanarTerrainBlend" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGWavyCloth, () =>
            {
                return Resources.Load<Shader>( "Shaders/Deferred/HGWavyCloth" );
            } ).RegisterAccessor();
            #endregion
            #region Hopoo Enviornment
            new AssetAccessor<Shader>( ShaderIndex.HGDistantWater, () =>
            {
                return Resources.Load<Shader>( "Shaders/Environment/HGDistantWater" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGGrass, () =>
            {
                return Resources.Load<Shader>( "Shaders/Environment/HGGrass" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGWaterfall, () =>
            {
                return Resources.Load<Shader>( "Shaders/Environment/HGWaterfall" );
            } ).RegisterAccessor();
            #endregion
            #region Hopoo FX
            new AssetAccessor<Shader>( ShaderIndex.HGCloudRemap, () =>
            {
                return Resources.Load<Shader>( "Shaders/FX/HGCloudRemap" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGDamageNumber, () =>
            {
                return Resources.Load<Shader>( "Shaders/FX/HGDamageNumber" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGDistortion, () =>
            {
                return Resources.Load<Shader>( "Shaders/FX/HGDistortion" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGIntersectionCloudRemap, () =>
            {
                return Resources.Load<Shader>( "Shaders/FX/HGIntersectionCloudRemap" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGOpaqueCloudRemap, () =>
            {
                return Resources.Load<Shader>( "Shaders/FX/HGOpaqueCloudRemap" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGSolidParallax, () =>
            {
                return Resources.Load<Shader>( "Shaders/FX/HGSolidParallax" );
            } ).RegisterAccessor();
            #endregion
            #region Hopoo PostProcess
            new AssetAccessor<Shader>( ShaderIndex.HGOutlineHighlight, () =>
            {
                return Resources.Load<Shader>( "Shaders/PostProcess/HGOutlineHighlight" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGScopeShader, () =>
            {
                return Resources.Load<Shader>( "Shaders/PostProcess/HGScopeShader" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGScreenDamage, () =>
            {
                return Resources.Load<Shader>( "Shaders/PostProcess/HGScreenDamage" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGSobelBuffer, () =>
            {
                return Resources.Load<Shader>( "Shaders/PostProcess/HGSobelBuffer" );
            } ).RegisterAccessor();
            #endregion
            #region Hopoo UI
            new AssetAccessor<Shader>( ShaderIndex.HGUIAnimateAlpha, () =>
            {
                return Resources.Load<Shader>( "Shaders/UI/HGUIAnimateAlpha" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGUIBarRemap, () =>
            {
                return Resources.Load<Shader>( "Shaders/UI/HGUIBarRemap" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGUIBlur, () =>
            {
                return Resources.Load<Shader>( "Shaders/UI/HGUIBlur" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGUIIgnoreZ, () =>
            {
                return Resources.Load<Shader>( "Shaders/UI/HGGUIIgnoreZ" );
            } ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGUIOverBrighten, () =>
            {
                return Resources.Load<Shader>( "Shaders/UI/HGUIOverBrighten" );
            } ).RegisterAccessor();
            #endregion


            completedProperly = true;
        }
    }
}
