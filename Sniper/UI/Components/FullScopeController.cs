//#define TESTING
namespace Sniper.UI.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.UI;

    public class FullScopeController : MonoBehaviour
    {
        [SerializeField]
        private Image overlayImg;

        [SerializeField]
        private InfoPanelController panel;

        public Single distance
        {
            set => this.panel.distance = value;
        }

        public Boolean active
        {
            set
            {
                if( value != this._active )
                {
                    this._active = value;
                    this.overlayImg.gameObject.SetActive( value );
                    this.panel.gameObject.SetActive( value );
                }
            }
        }
        private Boolean _active = true;

        // TODO: Implement rest of info panel

        private void OnValidate()
        {
            this.panel = base.transform.Find( "InfoPanel" ).GetComponent<InfoPanelController>();
            this.overlayImg = base.transform.Find( "Overlay" ).GetComponent<Image>();
        }

    }
}