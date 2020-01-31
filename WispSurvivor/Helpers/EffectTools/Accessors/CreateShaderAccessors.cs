
using RogueWispPlugin.Helpers;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void CreateShaderAccessors()
        {
            #region Hopoo Deferred
            new GenericAccessor<Shader>( ShaderIndex.HGSnowTopped, () =>
            {
                return Resources.Load<Shader>( "Shaders/Deferred/HGSnowTopped" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGStandard, () =>
            {
                return Resources.Load<Shader>( "Shaders/Deferred/HGStandard" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGTriPlanarTerrainBlend, () =>
            {
                return Resources.Load<Shader>( "Shaders/Deferred/HGTriPlanarTerrainBlend" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGWavyCloth, () =>
            {
                return Resources.Load<Shader>( "Shaders/Deferred/HGWavyCloth" );
            }, ExecutionState.Constructor ).RegisterAccessor();
            #endregion
            #region Hopoo Enviornment
            new GenericAccessor<Shader>( ShaderIndex.HGDistantWater, () =>
            {
                return Resources.Load<Shader>( "Shaders/Environment/HGDistantWater" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGGrass, () =>
            {
                return Resources.Load<Shader>( "Shaders/Environment/HGGrass" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGWaterfall, () =>
            {
                return Resources.Load<Shader>( "Shaders/Environment/HGWaterfall" );
            }, ExecutionState.Constructor ).RegisterAccessor();
            #endregion
            #region Hopoo FX
            new GenericAccessor<Shader>( ShaderIndex.HGCloudRemap, () =>
            {
                return Resources.Load<Shader>( "Shaders/FX/HGCloudRemap" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGDamageNumber, () =>
            {
                return Resources.Load<Shader>( "Shaders/FX/HGDamageNumber" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGDistortion, () =>
            {
                return Resources.Load<Shader>( "Shaders/FX/HGDistortion" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGIntersectionCloudRemap, () =>
            {
                return Resources.Load<Shader>( "Shaders/FX/HGIntersectionCloudRemap" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGOpagueCloudRemap, () =>
            {
                return Resources.Load<Shader>( "Shaders/FX/HGOpagueCloudRemap" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGSolidParallax, () =>
            {
                return Resources.Load<Shader>( "Shaders/FX/HGSolidParallax" );
            }, ExecutionState.Constructor ).RegisterAccessor();
            #endregion
            #region Hopoo PostProcess
            new GenericAccessor<Shader>( ShaderIndex.HGOutlineHighlight, () =>
            {
                return Resources.Load<Shader>( "Shaders/PostProcess/HGOutlineHighlight" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGScopeShader, () =>
            {
                return Resources.Load<Shader>( "Shaders/PostProcess/HGScopeShader" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGScreenDamage, () =>
            {
                return Resources.Load<Shader>( "Shaders/PostProcess/HGScreenDamage" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGSobelBuffer, () =>
            {
                return Resources.Load<Shader>( "Shaders/PostProcess/HGSobelBuffer" );
            }, ExecutionState.Constructor ).RegisterAccessor();
            #endregion
            #region Hopoo UI
            new GenericAccessor<Shader>( ShaderIndex.HGUIAnimateAlpha, () =>
            {
                return Resources.Load<Shader>( "Shaders/UI/HGUIAnimateAlpha" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGUIBarRemap, () =>
            {
                return Resources.Load<Shader>( "Shaders/UI/HGUIBarRemap" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGUIBlur, () =>
            {
                return Resources.Load<Shader>( "Shaders/UI/HGUIBlur" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGUIIgnoreZ, () =>
            {
                return Resources.Load<Shader>( "Shaders/UI/HGGUIIgnoreZ" );
            }, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Shader>( ShaderIndex.HGUIOverBrighten, () =>
            {
                return Resources.Load<Shader>( "Shaders/UI/HGUIOverBrighten" );
            }, ExecutionState.Constructor ).RegisterAccessor();
            #endregion
        }
    }
}
