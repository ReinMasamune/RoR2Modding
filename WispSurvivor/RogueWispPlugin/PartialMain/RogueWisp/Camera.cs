﻿#if ROGUEWISP
using RoR2;

using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        partial void RW_Camera()
        {
            this.Load += this.RW_AddCameraChildren;
            this.Load += this.RW_SetupCamSettings;
        }

        private void RW_SetupCamSettings()
        {
            CharacterCameraParams camSettings = ScriptableObject.CreateInstance<CharacterCameraParams>();
            camSettings.minPitch = -70f;
            camSettings.maxPitch = 70f;
            camSettings.wallCushion = 0.1f;
            camSettings.pivotVerticalOffset = 1.37f;
            camSettings.standardLocalCameraPos = new Vector3( 0f, 0f, -8.18f );
            this.RW_body.GetComponent<CameraTargetParams>().cameraParams = camSettings;
            this.RW_body.GetComponent<CameraTargetParams>().idealLocalCameraPos = new Vector3( 0f, 0f, -4.71f );
        }
        private void RW_AddCameraChildren()
        {
            GameObject cameraPivot = new GameObject("CameraPivot");
            GameObject aimOrigin = new GameObject("AimOrigin");
            MonoBehaviour.DontDestroyOnLoad( cameraPivot );
            MonoBehaviour.DontDestroyOnLoad( aimOrigin );
            cameraPivot.transform.SetParent( this.RW_body.transform, false );
            aimOrigin.transform.SetParent( this.RW_body.transform, false );
            cameraPivot.transform.localPosition = new Vector3( 0f, 1.75f, 0f );
            aimOrigin.transform.localPosition = new Vector3( 0f, 1.3f, 0f );
            this.RW_body.GetComponent<CameraTargetParams>().cameraPivotTransform = cameraPivot.transform;
            this.RW_body.GetComponent<CharacterBody>().aimOriginTransform = aimOrigin.transform;
        }
    }

}
#endif