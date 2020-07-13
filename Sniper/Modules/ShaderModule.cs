namespace Sniper.Modules
{
    using System;

    using ReinCore;
    using Sniper.Properties;
    using Sniper.Enums;

    using UnityEngine;

    using Resources = Properties.Resources;
    using Object = System.Object;
    using System.Runtime.CompilerServices;

    internal static class ShaderModule
    {
        private static ComputeShader _scopeOverlay;
        internal static ComputeShader GetScopeOverlay()
        {
            if( _scopeOverlay is null )
            {
                _scopeOverlay = AssetModule.GetSniperAssetBundle().LoadAsset<ComputeShader>( Resources.cshader__ScopeOverlay );
            }
            return _scopeOverlay;
        }

        private static Texture2D _maskTexture;
        internal static Texture2D GetMaskTexture()
        {
            if( _maskTexture is null )
            {
                _maskTexture = AssetModule.GetSniperAssetBundle().LoadAsset<Texture2D>( Resources.texture__ScopeOverlayMask );
            }

            return _maskTexture;
        }
    }

}
