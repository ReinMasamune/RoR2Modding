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
                var tex = TexturesCore.GenerateBarTexture( 512, 64, true, 4, 2, Color.white, Color.black, 16,
                    (reloadParams.perfectStart, reloadParams.perfectEnd, Color.white),
                    (reloadParams.goodStart, reloadParams.goodEnd, Color.grey)
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
            this.currentReloadParams = reloadParams;
            this.isReloading = true;
            this.reloadTimer = 0f;
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
            // TODO: Assign to UI elements
        }

        private Image backgroundImage;
        private Image handleImage;



        private void OnEnable()
        {

        }
        private void OnDisable()
        {

        }

        private void Awake()
        {

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
