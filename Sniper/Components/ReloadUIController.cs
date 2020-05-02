using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using RoR2.UI;
using Sniper.Data;
using Sniper.Enums;
using Sniper.Modules;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Sniper.Components
{
    internal class ReloadUIController : MonoBehaviour
    {
        internal static Color barBackgroundColor { get; } = new Color( 0.3f, 0.3f, 0.3f, 1f );
        internal static Color barPerfectColor { get; } = new Color( 0.9f, 0.9f, 0.9f, 1f );
        internal static Color barGoodColor { get; } = new Color( 0.5f, 0.5f, 0.5f, 1f );

        #region Static
        #region External
        internal static ReloadUIController FindController( CharacterBody body )
        {
            if( body == null )
            {
                return null;
            }
            if( instances.TryGetValue( body, out ReloadUIController controller ) )
            {
                return controller;
            }
            var res = body.master.playerCharacterMasterController.networkUser.cameraRigController.hud.GetComponentInChildren<ReloadUIController>();
            if( res != null )
            {
                instances[body] = res;
                res.Init( body );
            }
            return res;
        }
        internal static Texture2D GetReloadTexture( ReloadParams reloadParams )
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
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Internal
        private static Dictionary<CharacterBody, ReloadUIController> instances = new Dictionary<CharacterBody, ReloadUIController>();
        private static Dictionary<ReloadParams, Texture2D> cachedBarTextures = new Dictionary<ReloadParams, Texture2D>();




        #endregion
        #endregion
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Instance
        #region External
        internal void Init( CharacterBody body )
        {
            this.body = body;
            instances[this.body] = this;
            this.showBar = false;
        }

        internal void StartReload( ReloadParams reloadParams )
        {
            this.currentReloadParams = reloadParams;
            this.reloadTimer = 0f;
            this.reloadSlider.value = 0f;
            this.barTexture = GetReloadTexture( this.currentReloadParams );
            base.StartCoroutine( this.ReloadStartDelay( this.currentReloadParams.reloadDelay / this.body.attackSpeed ) );
        }

        internal ReloadTier StopReload( SkillDefs.SniperReloadableFireSkillDef.SniperPrimaryInstanceData data )
        {
            this.isReloading = false;
            base.StartCoroutine( this.ReloadStopDelay( this.currentReloadParams.reloadEndDelay / this.body.attackSpeed, data ) );
            return this.currentReloadParams.GetReloadTier(this.reloadTimer);
        }

        internal Boolean CanReload()
        {
            return this.isReloading;
        }




        #endregion
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Internal
        private ReloadParams currentReloadParams;
        private Boolean isReloading;
        private Single reloadTimer;
        private GameObject barHolder;
        private Image backgroundImage;
        private Slider reloadSlider;
        private CharacterBody body;
        private Boolean showBar
        {
            set
            {
                this.barHolder?.SetActive( value );
            }
        }

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

        private void OnEnable()
        {

        }
        private void OnDisable()
        {

        }

        private void Awake()
        {
            this.barHolder = base.transform.Find( "BarHolder" ).gameObject;
            this.reloadSlider = this.barHolder.GetComponent<Slider>();
            this.backgroundImage = this.barHolder.transform.Find( "Background" ).GetComponent<Image>();

            this.showBar = false;
        }

        private void Update()
        {
            if( this.isReloading )
            {
                this.reloadTimer = this.currentReloadParams.Update( Time.deltaTime, this.body.attackSpeed, this.reloadTimer );
                this.reloadSlider.value = Mathf.Clamp01( this.reloadTimer / this.currentReloadParams.baseDuration );
            }
        }

        private void Start()
        {
            //base.gameObject.SetActive( false );
        }
        #endregion

        #region Coroutines
        private IEnumerator ReloadStartDelay(Single delayTime)
        {
            yield return new WaitForSeconds( delayTime );
            this.showBar = true;
            this.isReloading = true;
            SoundModule.PlayOpenReload( this.body.gameObject );
        }
        private IEnumerator ReloadStopDelay( Single delayTime, SkillDefs.SniperReloadableFireSkillDef.SniperPrimaryInstanceData data )
        {
            yield return new WaitForSeconds( delayTime );
            this.showBar = false;
            data.isReloading = false;
        }
        #endregion
        #endregion
    }
}
