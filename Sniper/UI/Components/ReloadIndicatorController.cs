//#define TESTING
//#define EDITOR
namespace Sniper.UI.Components
{
    using System;
    using System.Collections.Generic;

    using ReinCore;

    using Sniper.Enums;

    using UnityEngine;
    using UnityEngine.UI;

    public class ReloadIndicatorController : MonoBehaviour
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
        private GameObject indicatorPrefab;


        [SerializeField]
        private Color emptyColor;
        [SerializeField]
        private Color badColor;
        [SerializeField]
        private Color goodColor;
        [SerializeField]
        private Color perfectColor;

        private List<Image> indicators;

        private void Awake()
        {
            this.indicators = ListPool<Image>.item;
        }

        private void OnDestroy()
        {
            ListPool<Image>.item = this.indicators;
            this.indicators = null;
        }

#if TESTING
        public Byte testMaxStock;
#endif

        public Byte maxStock
        {
            set
            {
                while( value > this.indicators.Count )
                {
                    var created = UnityEngine.Object.Instantiate<GameObject>(this.indicatorPrefab, base.transform).GetComponent<Image>();
                    this.indicators.Add( created );
                    created.color = this.indicators.Count <= this._currentStock ? this.currentColor : this.emptyColor;
                }

                

                while( value < this.indicators.Count )
                {
                    var removed = this.indicators[this.indicators.Count - 1];
#if EDITOR
                    removed?.transform?.SetParent( destPar.transform );
                    removed?.gameObject?.SetActive( false );
#else
                    UnityEngine.Object.Destroy( removed.gameObject );
#endif
                    this.indicators.RemoveAt( this.indicators.Count - 1 );
                }
            }
        }

#if TESTING
        public Byte testCurrentStock;
#endif
        public Byte currentStock
        {
            set
            {
                if( value != this._currentStock )
                {
                    this._currentStock = value;

                    for( Int32 i = 0; i < this.indicators.Count; ++i )
                    {
                        var ind = this.indicators[i];
                        if( !ind || ind is null ) continue;
                        ind.color = i < this._currentStock ? this.currentColor : this.emptyColor;
                    }
                }
            }
        }
        private Byte _currentStock;

#if TESTING
        public ReloadTier testReloadTier;
#endif
        public ReloadTier reloadTier
        {
            set
            {
                if( value == this.currentTier ) return;
                this.currentTier = value;
                Color inputColor = this.emptyColor;
                switch( value )
                {
                    case ReloadTier.Bad:
                    inputColor = this.badColor;
                    break;
                    case ReloadTier.Good:
                    inputColor = this.goodColor;
                    break;
                    case ReloadTier.Perfect:
                    inputColor = this.perfectColor;
                    break;
                }
                this.currentColor = inputColor;

                for( Int32 i = 0; i < this.indicators.Count; ++i )
                {
                    this.indicators[i].color = i < this._currentStock ? this.currentColor : this.emptyColor;
                }

            }
        }

        private ReloadTier currentTier = ReloadTier.Good;
        private Color currentColor;


        private void OnValidate()
        {
#if TESTING
            this.maxStock = this.testMaxStock;
            this.currentStock = this.testCurrentStock;
            this.reloadTier = this.testReloadTier;
#endif
        }

    }
}
