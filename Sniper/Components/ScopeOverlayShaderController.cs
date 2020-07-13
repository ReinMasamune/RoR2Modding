namespace Sniper.Components
{
    using System;

    using Sniper.Modules;

    using UnityEngine;

    internal class ScopeOverlayShaderController : MonoBehaviour
    {
        internal static Boolean active = false;


        private Int32 kernelId;
        private ComputeShader shader;
        private RenderTexture renderTexture;
        private Texture2D maskTexture;

        protected void OnRenderImage( RenderTexture source, RenderTexture destination )
        {
            if( active )
            {
                this.CheckRenderTexture();

                this.shader.SetTexture( this.kernelId, "_Input", source );
                this.shader.SetTexture( this.kernelId, "_Map", this.maskTexture );
                this.shader.SetTexture( this.kernelId, "_Output", this.renderTexture );
                this.shader.Dispatch( this.kernelId, Mathf.CeilToInt( this.renderTexture.width / 8f ), Mathf.CeilToInt( this.renderTexture.height / 8f ), 1 );
                Graphics.Blit( this.renderTexture, destination );

            } else
            {
                Graphics.Blit( source, destination );
            }
        }

        protected void Awake()
        {
            this.shader = ShaderModule.GetScopeOverlay();
            this.maskTexture = ShaderModule.GetMaskTexture();
            this.kernelId = this.shader.FindKernel( "RefractionBlur" );
            this.renderTexture = new RenderTexture(Screen.width, Screen.height, 1, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear );
            this.renderTexture.enableRandomWrite = true;
            _ = this.renderTexture.Create();
        }
        
        protected void OnDestroy()
        {
            this.renderTexture.Release();
        }

        private void CheckRenderTexture()
        {
            if( this.renderTexture.width == Screen.width && this.renderTexture.height == Screen.height ) return;

            this.renderTexture.Release();
            this.renderTexture = new RenderTexture( Screen.width, Screen.height, 1, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear );
            this.renderTexture.enableRandomWrite = true;
            _ = this.renderTexture.Create();
        }
    }
}