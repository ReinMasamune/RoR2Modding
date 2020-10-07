//#define TESTING
//#define EDITOR
namespace Rein.Sniper.UI.Components
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public class StockController : MonoBehaviour
    {
#if EDITOR
        private static GameObject _destPar;
        private static GameObject destPar
        {
            get
            {
                if( _destPar == null || !_destPar )
                {
                    _destPar = new GameObject( "DESTROY" );
                }

                return _destPar;
            }
        }
#endif


        [SerializeField]
        private GameObject stockPrefab;
        [SerializeField]
        private Byte maxDisplayed = 0;

        [HideInInspector]
        [SerializeField]
        private Byte currentStock = 0;
        [HideInInspector]
        [SerializeField]
        private Byte currentMaxStock = 0;

        [HideInInspector]
        [SerializeField]
        private List<StockIndicatorController> stockIndicators = new List<StockIndicatorController>();


#if TESTING
        public Byte testStock;
#endif
        public Byte stock
        {
            set
            {
                var displayedValue = Math.Min(this.maxDisplayed, value);
                if( displayedValue != this.currentStock )
                {
                    this.currentStock = displayedValue;
                    this.numDisplayed = Math.Max( this.currentStock, this.currentMaxStock );
                    this.UpdateDisplayed();
                }
            }
        }

#if TESTING
        public Byte testMaxStock;
#endif
        public Byte maxStock
        {
            set
            {
                var displayedValue = Math.Min(this.maxDisplayed, value);

                if( displayedValue != this.currentMaxStock )
                {
                    this.currentMaxStock = displayedValue;
                    this.numDisplayed = Math.Max( this.currentStock, this.currentMaxStock );
                    this.UpdateDisplayed();
                }
            }
        }




        private Byte numDisplayed
        {
            set
            {
                while( value > this.stockIndicators.Count )
                {
                    var created = UnityEngine.Object.Instantiate(this.stockPrefab, base.transform).GetComponent<StockIndicatorController>();
                    this.stockIndicators.Add( created );
                }

                while( value < this.stockIndicators.Count )
                {
                    var removed = this.stockIndicators[this.stockIndicators.Count - 1];
#if EDITOR
                    removed.transform.SetParent( destPar.transform );
                    removed.gameObject.SetActive( false );
#else
                    UnityEngine.Object.DestroyImmediate(removed.gameObject);
#endif
                    this.stockIndicators.RemoveAt( this.stockIndicators.Count - 1 );
                }
            }
        }

        private void UpdateDisplayed()
        {
            for( Int32 i = 0; i < this.stockIndicators.Count; ++i )
            {
                var ind = this.stockIndicators[i];
                ind.holder = i < this.currentMaxStock;
                ind.bullet = i < this.currentStock;
            }
        }

#if TESTING
        public Boolean RESET = false;
#endif

        public void OnValidate()
        {
#if TESTING
            if( this.RESET )
            {
                foreach( var ind in this.stockIndicators )
                {
                    ind?.transform?.SetParent(null);
                    ind?.gameObject?.SetActive(false);
                }
                this.stockIndicators = new List<StockIndicatorController>();

                this.currentMaxStock = 0;
                this.currentStock = 0;
                this.testStock = 0;
                this.testMaxStock = 0;

                this.RESET = false;
            }
            this.stock = this.testStock;
            this.maxStock = this.testMaxStock;
#endif
        }
    }
}
