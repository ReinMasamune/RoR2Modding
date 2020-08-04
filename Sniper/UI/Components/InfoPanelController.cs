namespace Sniper.UI.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    public class InfoPanelController : MonoBehaviour
    {
        [SerializeField]
        private RangefinderController rangefinder;

        public Single distance
        {
            set => this.rangefinder.distance = value;
        }

        // FUTURE: Other stuff for the info panel

        private void OnValidate()
        {
            this.rangefinder = base.transform.Find( "Rangefinder" ).GetComponent<RangefinderController>();
        }
    }


}