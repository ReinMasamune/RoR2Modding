using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using Sniper.Data;
using Sniper.Enums;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Sniper.Components
{
    internal class ReloadUIController : NetworkBehaviour
    {
        internal static Color barBackgroundColor { get; } = new Color( 0.3f, 0.3f, 0.3f, 1f );
        internal static Color barPerfectColor { get; } = new Color( 0.9f, 0.9f, 0.9f, 1f );
        internal static Color barGoodColor { get; } = new Color( 0.5f, 0.5f, 0.5f, 1f );

        #region Static
        #region External
        internal static ReloadUIController FindController( CharacterBody body )
        {
            if( instances.TryGetValue( body, out var controller ) )
            {
                return controller;
            }
            return null;
        }
        #endregion
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Internal
        private static Dictionary<CharacterBody, ReloadUIController> instances = new Dictionary<CharacterBody, ReloadUIController>();
        private static Dictionary<ReloadParams, Texture2D> cachedBarTextures = new Dictionary<ReloadParams, Texture2D>();
        private static Texture2D GetReloadTexture( ReloadParams reloadParams )
        {
            if( !cachedBarTextures.ContainsKey( reloadParams ) )
            {
                var tex = TexturesCore.GenerateBarTexture( 1280, 128, true, 18, 4, Color.black, barBackgroundColor, 2,
                    (reloadParams.perfectStart, reloadParams.perfectEnd, barPerfectColor),
                    (reloadParams.goodStart, reloadParams.goodEnd, barGoodColor)
                );

                cachedBarTextures[reloadParams] = tex;
            }
            return cachedBarTextures[reloadParams];
        }



        #endregion
        #endregion
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Instance
        #region External
        internal void StartReload( ReloadParams reloadParams )
        {
            base.gameObject.SetActive( true );
            this.currentReloadParams = reloadParams;
            this.isReloading = true;
            this.reloadTimer = 0f;
            this.barTexture = GetReloadTexture( this.currentReloadParams );
        }

        internal ReloadTier StopReload( ReloadParams reloadParams )
        {
            return ReloadTier.None;
        }




        #endregion
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Internal
        private ReloadParams currentReloadParams;
        private Boolean isReloading;
        private Single reloadTimer;

        private Texture2D barTexture
        {
            get => this._barTexture;
            set
            {
                if( value != this._barTexture )
                {
                    this.OnBarTextureChanged( this._barTexture, value );
                    this._barTexture = value;
                }
            }
        }
        private Texture2D _barTexture;
        private void OnBarTextureChanged( Texture2D oldTex, Texture2D newTex )
        {
            this.backgroundImage.sprite = Sprite.Create( newTex, new Rect( 0f, 0f, newTex.width, newTex.height ), new Vector2( 0.5f, 0.5f ) );
        }

        private Image backgroundImage;
        private Slider reloadSlider;



        private void OnEnable()
        {

        }
        private void OnDisable()
        {

        }

        private void Awake()
        {
            this.reloadSlider = base.GetComponent<Slider>();
            this.backgroundImage = base.transform.Find( "Background" ).GetComponent<Image>();
        }

        private void Start()
        {

        }

        private void Update()
        {

        }
        #endregion
        #endregion
    }
}
