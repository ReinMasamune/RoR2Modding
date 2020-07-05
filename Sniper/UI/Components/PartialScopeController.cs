//#define TESTING
namespace Sniper.UI.Components
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    using ReinCore;

    using UnityEditor;

    using UnityEngine;

    public class PartialScopeController : MonoBehaviour
    {
#if TESTING
        public Boolean testActive = false;
#endif
        public Boolean active
        {
            set
            {
                if( value != this._active )
                {
                    this._active = value;
                    this.chargeAndReadyIndicator.gameObject.SetActive( value );
                    this.stockController.gameObject.SetActive( value );
                }
            }
        }
        private Boolean _active = true;

        //private CancellationTokenSource activeToken = new CancellationTokenSource();
        //private async void DelaySetActive( Boolean value )
        //{
        //    while( !UnityHelpers.ObjectsSafe( ()=>this.chargeAndReadyIndicator,()=>this.chargeAndReadyIndicator.gameObject,()=>this.stockController,()=>this.stockController.gameObject))
        //    {
        //        await Task.Delay( 1, this.activeToken.Token ).ConfigureAwait( true );
        //    }
        //    if( !UnityHelpers.ObjectsSafe( () => this.chargeAndReadyIndicator, () => this.chargeAndReadyIndicator.gameObject, () => this.stockController, () => this.stockController.gameObject ) ) return;
        //    this.chargeAndReadyIndicator.gameObject.SetActive( value );
        //    this.stockController.gameObject.SetActive( value );
        //}

#if TESTING
        [Range(0f, 1f)]
        public Single testCharge = 0f;
#endif
        public Single charge
        {
            set
            {
                this.chargeAndReadyIndicator.charge = value;
            }
        }

#if TESTING
        [Range(0f, 1f)]
        public Single testReadyFrac = 0f;
#endif
        public Single readyFrac
        {
            set
            {
                this.chargeAndReadyIndicator.readyFrac = value;
            }
        }

#if TESTING
        public Boolean testReady = false;
#endif
        public Boolean ready
        {
            set
            {
                this.chargeAndReadyIndicator.ready = value;
            }
        }

#if TESTING
        public Byte testMaxStock = 0;
#endif
        public Byte maxStock
        {
            set
            {
                this.stockController.maxStock = value;
            }
        }

#if TESTING
        public Byte testCurrentStock = 0;
#endif
        public Byte currentStock
        {
            set
            {
                this.stockController.stock = value;
            }
        }

        [HideInInspector]
        [SerializeField]
        private HexController chargeAndReadyIndicator;
        [HideInInspector]
        [SerializeField]
        private StockController stockController;
        [HideInInspector]
        [SerializeField]
        private Boolean init = false;

        private void OnValidate()
        {
            if( !this.init )
            {
                this.chargeAndReadyIndicator = base.transform.Find( "ChargeAndReadyHolder" ).GetComponent<HexController>();
                this.stockController = base.transform.Find( "StockHolder" ).GetComponent<StockController>();
                this.init = true;
            }

#if TESTING
            this.active = this.testActive;
            this.charge = this.testCharge;
            this.currentStock = this.testCurrentStock;
            this.maxStock = this.testMaxStock;
            this.ready = this.testReady;
            this.readyFrac = this.testReadyFrac;
#endif
        }
    }
}
