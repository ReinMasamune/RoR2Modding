using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using Sniper.ScriptableObjects;
using UnityEngine;

namespace Sniper.Modules
{
    internal static class MiscModule
    {
        internal static GameObject GetPodPrefab()
        {
            if( _podPrefab == null )
            {
                _podPrefab = CreatePodPrefab();
            }
            return _podPrefab;
        }
        private static GameObject _podPrefab;
        private static GameObject CreatePodPrefab()
        {
            // TODO: Create sniper survivor pod prefab
            return null;
        }

        internal static CharacterCameraParams GetCharCameraParams()
        {
            if( _sniperCharCameraParams == null )
            {
                _sniperCharCameraParams = CreateSniperCharCameraParams();
            }
            return _sniperCharCameraParams;
        }
        private static CharacterCameraParams _sniperCharCameraParams;
        private static CharacterCameraParams CreateSniperCharCameraParams()
        {
            var param = SniperCameraParams.Create( new Vector3( 1f, 0.8f, -3f ) );
            param.name = "ccpSniper";
            param.minPitch = -70f;
            param.maxPitch = 70f;
            param.wallCushion = 0.1f;
            param.pivotVerticalOffset = 1.37f;
            param.standardLocalCameraPos = new Vector3( 0f, 0f, -8.18f );
            return param;
        }
    }
}
