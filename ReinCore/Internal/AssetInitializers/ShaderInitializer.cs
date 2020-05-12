namespace ReinCore
{
    using System;

    using UnityEngine;

    internal static class ShaderInitializer
    {
        private static readonly Boolean completedProperly = false;
        internal static Boolean Initialize() => completedProperly;


        static ShaderInitializer()
        {
            #region Hopoo Deferred
            new AssetAccessor<Shader>( ShaderIndex.HGSnowTopped, () => Resources.Load<Shader>( "Shaders/Deferred/HGSnowTopped" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGStandard, () => Resources.Load<Shader>( "Shaders/Deferred/HGStandard" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGTriPlanarTerrainBlend, () => Resources.Load<Shader>( "Shaders/Deferred/HGTriPlanarTerrainBlend" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGWavyCloth, () => Resources.Load<Shader>( "Shaders/Deferred/HGWavyCloth" ) ).RegisterAccessor();
            #endregion
            #region Hopoo Enviornment
            new AssetAccessor<Shader>( ShaderIndex.HGDistantWater, () => Resources.Load<Shader>( "Shaders/Environment/HGDistantWater" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGGrass, () => Resources.Load<Shader>( "Shaders/Environment/HGGrass" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGWaterfall, () => Resources.Load<Shader>( "Shaders/Environment/HGWaterfall" ) ).RegisterAccessor();
            #endregion
            #region Hopoo FX
            new AssetAccessor<Shader>( ShaderIndex.HGCloudRemap, () => Resources.Load<Shader>( "Shaders/FX/HGCloudRemap" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGDamageNumber, () => Resources.Load<Shader>( "Shaders/FX/HGDamageNumber" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGDistortion, () => Resources.Load<Shader>( "Shaders/FX/HGDistortion" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGIntersectionCloudRemap, () => Resources.Load<Shader>( "Shaders/FX/HGIntersectionCloudRemap" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGOpaqueCloudRemap, () => Resources.Load<Shader>( "Shaders/FX/HGOpaqueCloudRemap" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGSolidParallax, () => Resources.Load<Shader>( "Shaders/FX/HGSolidParallax" ) ).RegisterAccessor();
            #endregion
            #region Hopoo PostProcess
            new AssetAccessor<Shader>( ShaderIndex.HGOutlineHighlight, () => Resources.Load<Shader>( "Shaders/PostProcess/HGOutlineHighlight" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGScopeShader, () => Resources.Load<Shader>( "Shaders/PostProcess/HGScopeShader" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGScreenDamage, () => Resources.Load<Shader>( "Shaders/PostProcess/HGScreenDamage" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGSobelBuffer, () => Resources.Load<Shader>( "Shaders/PostProcess/HGSobelBuffer" ) ).RegisterAccessor();
            #endregion
            #region Hopoo UI
            new AssetAccessor<Shader>( ShaderIndex.HGUIAnimateAlpha, () => Resources.Load<Shader>( "Shaders/UI/HGUIAnimateAlpha" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGUIBarRemap, () => Resources.Load<Shader>( "Shaders/UI/HGUIBarRemap" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGUIBlur, () => Resources.Load<Shader>( "Shaders/UI/HGUIBlur" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGUIIgnoreZ, () => Resources.Load<Shader>( "Shaders/UI/HGGUIIgnoreZ" ) ).RegisterAccessor();

            new AssetAccessor<Shader>( ShaderIndex.HGUIOverBrighten, () => Resources.Load<Shader>( "Shaders/UI/HGUIOverBrighten" ) ).RegisterAccessor();
            #endregion


            completedProperly = true;
        }
    }
}
