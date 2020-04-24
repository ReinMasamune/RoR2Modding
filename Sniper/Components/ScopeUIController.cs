using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using RoR2.UI;
using Sniper.Data;
using Sniper.States.Bases;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Sniper.Components
{
    [RequireComponent(typeof(HudElement), typeof(CrosshairController))]
    internal class ScopeUIController : MonoBehaviour
    {
        internal void HookUpComponents()
        {
            this.hudElement = base.GetComponent<HudElement>();
            this.crosshair = base.GetComponent<CrosshairController>();
        }

        [SerializeField]
        internal HudElement hudElement;
        [SerializeField]
        internal CrosshairController crosshair;
        [SerializeField]
        internal GameObject stockHolder;
        [SerializeField]
        internal UIStockController stockController;
        [SerializeField]
        internal GameObject stockChildPrefab;
        [SerializeField]
        internal HGTextMeshProUGUI rangeIndicator;
        [SerializeField]
        internal HGTextMeshProUGUI zoomIndicator;
        [SerializeField]
        internal Image chargeIndicator;
        [SerializeField]
        internal GameObject scopeActive;
        [SerializeField]
        internal GameObject scopeInactive;

        private Transform camTargetTransform;
        private Transform origParent;
        private Vector3 origPos;
        private Quaternion origRot;
        private Transform scopeParent;



        private void Start()
        {
            this.body = this.hudElement.targetCharacterBody as SniperCharacterBody;
            this.body.scopeInstanceData.CrosshairCheckIn( this );
            this.camTargetTransform = this.body.aimOriginTransform;
            this.origParent = this.camTargetTransform.parent;
            this.origPos = this.camTargetTransform.localPosition;
            this.origRot = this.camTargetTransform.localRotation;
        }


        internal void StartZoomSession(ScopeBaseState state, ZoomParams zoomParams)
        {
            this.stateInstance = state;
            this.zoomParams = zoomParams;
            this.camTarget = this.stateInstance.cameraTarget;

            this.chargeIndicator.enabled = this.stateInstance.usesCharge;
            this.stockHolder?.SetActive( this.stateInstance.usesStock );
            this.scoped = false;
            this.OnScopedChange( false );

            if( this.stateInstance.usesStock )
            {
                // TODO: Implement
            }
        }

        internal void UpdateUI( Single zoom )
        {
            this.scoped = this.zoomParams.IsInScope( zoom );
            var fov = this.zoomParams.GetFoV( zoom );
            Log.Warning( fov );
            this.camTarget.fovOverride = fov;
            // TODO: Camera position?

            this.chargeIndicator.fillAmount = this.stateInstance.currentCharge;
        }

        internal void EndZoomSession()
        {
            this.ResetCamera();
            this.stateInstance = null;
        }

        private void ResetCamera()
        {
            this.camTarget.fovOverride = -1f;
            this.camTarget.aimMode = CameraTargetParams.AimType.Standard;
        }


        private ScopeBaseState stateInstance;
        private ZoomParams zoomParams;
        private SniperCharacterBody body;
        private CameraTargetParams camTarget;
        private Boolean scoped
        {
            get => this._scoped == true;
            set
            {
                if( value != this._scoped )
                {
                    this._scoped = value;
                    this.OnScopedChange( value );
                }
            }
        }
        private Boolean _scoped;
        private void OnScopedChange( Boolean enabled )
        {
            this.camTarget.aimMode = enabled ? CameraTargetParams.AimType.FirstPerson : CameraTargetParams.AimType.AimThrow;
            if( this.scopeActive != null ) this.scopeActive.SetActive( enabled );
            if( this.scopeInactive != null ) this.scopeInactive.SetActive( !enabled );
        }

    }
}
