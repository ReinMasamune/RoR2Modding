using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using Sniper.Enums;
using UnityEngine;

namespace Sniper.Data
{
    [Serializable]
    internal struct ZoomParams
    {
        [SerializeField]
        internal Single inputScale;
        [SerializeField]
        internal Single minZoom;
        [SerializeField]
        internal Single maxZoom;
        [SerializeField]
        internal Single defaultZoom;
        [SerializeField]
        internal Single scopeZoomStart;


        internal void UpdateCameraParams( CameraTargetParams cameraParams, Single zoom )
        {

        }

        internal Single UpdateZoom( Single inputValue, Single currentZoom )
        {
            if( inputValue != 0f )
            {
                currentZoom = Mathf.Clamp( currentZoom + ( inputValue * this.inputScale ), this.minZoom, this.maxZoom );
            }

            return currentZoom;
        }

    }


}