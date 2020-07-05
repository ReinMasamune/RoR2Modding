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
        internal static Color barBackgroundColor { get; } = new Color( 0.05f, 0.05f, 0.05f, 1f );
        internal static Color barPerfectColor { get; } = new Color( 0.7f, 0.9f, 0.8f, 1f );
        internal static Color barGoodColor { get; } = new Color( 0.5f, 0.5f, 0.5f, 1f );

        internal static Texture2D GetReloadTexture( ReloadParams reloadParams )
        {
            if( !cachedBarTextures.ContainsKey( reloadParams ) )
            {
                ITextureJob tex = TexturesCore.GenerateBarTextureBatch( 1280, 128, true, 18, 4, Color.black, barBackgroundColor, 2,
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
            set => this.barHolder.SetActive( value );
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

        protected void Awake()
        {
            this.barHolder = base.transform.Find( "BarHolder" ).gameObject;
            this.reloadSlider = this.barHolder.GetComponent<Slider>();
            this.backgroundImage = this.barHolder.transform.Find( "Background" ).GetComponent<Image>();
            this.showBar = false;
        }
        protected void Start() => _ = base.StartCoroutine( this.Hookup() );

        private IEnumerator Hookup()
        {
            HUD hud = base.GetComponent<HUD>() ?? base.GetComponentInChildren<HUD>() ?? base.GetComponentInParent<HUD>();
            while( hud is null )
            {
                yield return new WaitForEndOfFrame();
                hud = base.GetComponent<HUD>() ?? base.GetComponentInChildren<HUD>() ?? base.GetComponentInParent<HUD>();
            }
            SniperCharacterBody sniperBody = null;
            while( sniperBody is null )
            {
                yield return new WaitForEndOfFrame();
                sniperBody = hud.targetMaster?.GetBody() as SniperCharacterBody;
            }
            sniperBody.reloadUI = this;
        }
    }
}