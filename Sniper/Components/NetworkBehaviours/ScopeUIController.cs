using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using Sniper.Data;
using Sniper.States.Bases;
using UnityEngine;
using UnityEngine.Networking;

namespace Sniper.Components
{
    internal class ScopeUIController : NetworkBehaviour
    {
        // TODO: Add all functionality

        internal static ScopeUIController Create( GameObject prefab, CharacterBody body, ZoomParams zoomParams )
        {
            // TODO: Implement
            return null;
        }







        internal void StartZoomSession(ScopeBaseState state)
        {

        }

        internal void UpdateUI( CameraTargetParams cameraParams, Single zoom )
        {

        }

        internal void EndZoomSession()
        {

        }



        internal void SetActivity( Boolean active )
        {
            // TODO: Implement
        }
    }
}
