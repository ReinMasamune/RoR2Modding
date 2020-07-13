namespace Sniper.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    using ReinCore;

    using Rewired;

    using RoR2;
    using RoR2.UI;

    using Sniper.Data;
    using Sniper.Enums;
    using Sniper.Modules;

    using UnityEngine;
    using UnityEngine.UI;

    internal class ReloadUIController : MonoBehaviour
    {
        internal static Color barBackgroundColor { get; } = new Color( 0.05f, 0.05f, 0.05f, 0.6f );
        internal static Color barPerfectColor { get; } = new Color( 0.7f, 0.9f, 0.8f, 0.8f );
        internal static Color barGoodColor { get; } = new Color( 0.5f, 0.5f, 0.5f, 0.7f );

        internal static Texture2D GetReloadTexture( ReloadParams reloadParams )
        {
            if( !cachedBarTextures.ContainsKey( reloadParams ) )
            {
                ITextureJob tex = TexturesCore.GenerateBarTextureBatch( 512, 64, true, 8, 5, new Color( 0f, 0f, 0f, 0.7f ), barBackgroundColor, 2,
                    (reloadParams.perfectStart, reloadParams.perfectEnd, barPerfectColor),
                    (reloadParams.goodStart, reloadParams.goodEnd, barGoodColor)
                );

                cachedBarTextures[reloadParams] = tex.OutputTextureAndDispose();
            }
            return cachedBarTextures[reloadParams];
        }

        private static readonly Dictionary<ReloadParams, Texture2D> cachedBarTextures = new Dictionary<ReloadParams, Texture2D>();

        internal Single barPosition
        {
            set => this.reloadSlider.value = value;
        }

        internal Boolean showBar
        {
            set
            {
                this.barHolder.SetActive( value );
            }
        }

        internal ReloadParams currentParams
        {
            set
            {
                if( value == this._curParams ) return;
                this._curParams = value;
                var tex = GetReloadTexture( value );
                this.backgroundImage.sprite = Sprite.Create( tex, new Rect( 0f, 0f, tex.width, tex.height ), new Vector2( 0.5f, 0.5f ) );
            }
        }
        private ReloadParams _curParams;
        private GameObject barHolder;
        private Image backgroundImage;
        private Slider reloadSlider;
        private HUD hud;
        private CharacterMaster master;
        private SniperCharacterBody body;

        protected void Awake()
        {
            this.barHolder = base.transform.Find( "BarHolder" ).gameObject;
            this.reloadSlider = this.barHolder.GetComponent<Slider>();
            this.backgroundImage = this.barHolder.transform.Find( "Background" ).GetComponent<Image>();
            this.showBar = false;

            this.hud = base.GetComponent<HUD>() ?? base.GetComponentInChildren<HUD>() ?? base.GetComponentInParent<HUD>();
        }
        protected void Start()
        {
            _ = base.StartCoroutine( this.Hookup() );
        }

        private IEnumerator Hookup()
        {
            while( ( this.hud = base.GetComponent<HUD>() ?? base.GetComponentInChildren<HUD>() ?? base.GetComponentInParent<HUD>() ) is null ) 
                yield return new WaitForEndOfFrame();
            while( ( this.master = this.hud.targetMaster ) is null || !this.master.hasEffectiveAuthority ) 
                yield return new WaitForEndOfFrame();
            while( ( this.body = this.master.GetBody() as SniperCharacterBody ) is null ) 
                yield return new WaitForEndOfFrame();
            _ = base.StartCoroutine( this.CheckBody() );
            this.body.reloadUI = this;
        }

        private IEnumerator CheckBody()
        {
            for( ; ; )
            {
                yield return new WaitForSeconds( 2f );
                if( !this.body || this.body is null || this.body != (this.hud?.targetMaster?.GetBody() as SniperCharacterBody) )
                {
                    _ = base.StartCoroutine( this.Hookup() );
                    break;
                }
            }
        }
    }
}