using RoR2;
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
            public void TurnOff()
            {
                this.bgImage.gameObject.SetActive( false );
                this.fgImage.gameObject.SetActive( false );
                this.on = false;
            }
            public void TurnOn()
            {
                this.bgImage.gameObject.SetActive( true );
                this.fgImage.gameObject.SetActive( true );
                this.on = true;
            }
            #endregion


            #region Instance
            private HUD outer;
            private CharacterBody targetBody;
            private GameObject targetObject;

            private Boolean on;
            private Boolean registered;
            private void OnEnable()
            {
                this.TurnOff();
                this.registered = false;
                this.outer = base.GetComponentInParent<HUD>();
            }
            private void OnDisable()
            {
                this.TurnOff();
            }
            private void FixedUpdate()
            {
                var newObj = this.outer?.targetBodyObject;
                if( newObj != null && newObj == this.targetObject )
                {
                    return;
                } else
                {
                    if( this.on )
                    {
                        this.TurnOff();
                    }
                    if( this.registered )
                    {
                        WispCrosshairManager.RemoveDisplay( this, this.targetObject );
                        this.registered = false;
                    }

                    this.targetObject = newObj;
                    if( newObj != null )
                    {
                        if( WispCrosshairManager.AddDisplay( this, newObj ) )
                        {
                            this.registered = true;
                            this.TurnOn();
                        }
                    }
                }
            }
            #endregion
        }
    }
}