//#define TESTING
namespace Rein.Sniper.UI.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using ReinCore;

    using Rein.Sniper.UI.Components.Util;

    using UnityEngine;
    using UnityEngine.UI;

    public class HexController : MonoBehaviour
    {
        [SerializeField]
        private Single startRotation;
        [SerializeField]
        private Single endRotation;
        [SerializeField]
        private Color startColor;
        [SerializeField]
        private Color readyColor;



#if TESTING
        [Range(0f,1f)]
        public Single testReadyFrac = 0.0f;
#endif
        public Single readyFrac
        {
            set
            {
                var displayed = Mathf.Clamp01(value);
                if( this._readyFrac != displayed )
                {
                    this._readyFrac = displayed;
                    var temp = this.indicatorTransform.localEulerAngles;
                    temp.z = Mathf.Lerp( this.startRotation, this.endRotation, displayed );
                    this.indicatorTransform.localEulerAngles = temp;
                }
            }
        }
        private Single _readyFrac = 1.0f;

#if TESTING
        [Range(0f,1f)]
        public Single testCharge = 0.0f;
#endif
        public Single charge
        {
            set
            {
                this.indicatorFiller.fill = Mathf.Clamp01( value );
                var tempCol = this.indicatorFill.color;
                tempCol.a = this._ready ? 1f : 0.35f;
                this.indicatorFill.color = tempCol;
            }
        }

#if TESTING
        public Boolean testReady = false;
#endif
        public Boolean ready
        {
            set
            {
                if( value != this._ready )
                {
                    this._ready = value;
                    this.indicatorBG.color =  value ? this.readyColor : this.startColor;
                    var tempCol = this.indicatorFill.color;
                    tempCol.a = this._ready ? 1f : 0.35f;
                    this.indicatorFill.color = tempCol;
                }
            }
        }
        private Boolean _ready = true;

        //private CancellationTokenSource readyToken = new CancellationTokenSource();
        //private async void DelaySetReady( Boolean value )
        //{
        //    while( !UnityHelpers.ObjectsSafe(()=>this.indicatorBG,()=>this.indicatorFill ))
        //    {
        //        await Task.Delay( 1, this.readyToken.Token ).ConfigureAwait( true );
        //    }
        //    if( !UnityHelpers.ObjectsSafe( () => this.indicatorBG, () => this.indicatorFill ) ) return;
        //    this.indicatorBG.color = ( value ? this.readyColor : this.startColor );
        //    var tempCol = this.indicatorFill.color;
        //    tempCol.a = this._ready ? 1f : 0.35f;
        //    this.indicatorFill.color = tempCol;
        //}


        [HideInInspector]
        [SerializeField]
        private RectTransform indicatorTransform;
        [HideInInspector]
        [SerializeField]
        private Image indicatorBG;
        [HideInInspector]
        [SerializeField]
        private Image indicatorFill;
        [HideInInspector]
        [SerializeField]
        private Filler indicatorFiller;

        [SerializeField]
        private Boolean init = false;

        private void OnValidate()
        {
            if( !this.init )
            {
                this.indicatorTransform = base.transform.Find( "CompositeIndicator" ) as RectTransform;

                this.indicatorBG = this.indicatorTransform.GetComponent<Image>();

                var fill = this.indicatorTransform.Find("CompositeIndicatorFill");
                this.indicatorFill = fill.GetComponent<Image>();
                this.indicatorFiller = fill.GetComponent<Filler>();

                this.init = true;
            }
#if TESTING
            this.readyFrac = this.testReadyFrac;
            this.ready = this.testReady;
            this.charge = this.testCharge;
#endif
        }
    }
}