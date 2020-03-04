using RoR2.ConVar;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        internal class WispCrosshairChargeController : MonoBehaviour, IWispPassiveDisplay
        {
            #region Outwards
            [SerializeField]
            internal Image bgImage;
            [SerializeField]
            internal Image fgImage;

            public void UpdateBarFrac( Single newFrac )
            {
                this.fgImage.fillAmount = newFrac;
                this.bgImage.fillAmount = 1f - newFrac;
            }
            public void UpdateBGColor( Color newColor )
            {
                this.bgImage.color = newColor;
            }
            public void UpdateFGColor( Color newColor )
            {
                this.fgImage.color = newColor;
            }
            #endregion


            #region Instance

            private GameObject targetBody
            {
                get => this._targetBody;
                set
                {
                    if( this._targetBody != value )
                    {
                        if( this._targetBody != null ) WispCrosshairManager.RemoveDisplay( this, this._targetBody );
                        this._targetBody = value;
                        this.active = false;
                        if( this._targetBody != null ) this.active = WispCrosshairManager.AddDisplay( this, this._targetBody );
                    }
                }
            }

            private Boolean active
            {
                get => this._active;
                set
                {
                    if( this._active != value )
                    {
                        this._active = value;
                        this.bgImage.gameObject.SetActive( this._active );
                        this.fgImage.gameObject.SetActive( this._active );
                    }
                }
            }
            private Boolean _active = false;

            private GameObject _targetBody;
            private Func<GameObject> getTargetBody;
            private void OnEnable()
            {
                var hudElem = base.transform.parent.GetComponent<HudElement>();
                if( hudElem )
                {
                    this.getTargetBody = () => hudElem.targetBodyObject;
                    return;
                }
                var hud = base.GetComponentInParent<HUD>();
                if( hud )
                {
                    this.getTargetBody = () => hud.targetBodyObject;
                }
            }
            private void Update()
            {
                this.targetBody = this.getTargetBody?.Invoke();
            }
            private void OnDisable()
            {
                this.targetBody = null;
            }
            #endregion
        }
    }
}