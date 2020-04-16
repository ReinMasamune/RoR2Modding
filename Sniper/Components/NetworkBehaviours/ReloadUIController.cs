using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using Sniper.Data;
using UnityEngine;
using UnityEngine.Networking;

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
        #region Internal
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


        private static Dictionary<CharacterBody, ReloadUIController> instances = new Dictionary<CharacterBody, ReloadUIController>();
        #endregion
        #endregion


        private void OnEnable()
        {

        }
        private void OnDisable()
        {

        }
    }
}
