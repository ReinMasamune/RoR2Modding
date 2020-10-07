namespace Rein.Sniper.Components
{
    using System;

    using UnityEngine;
    using UnityEngine.UI;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
    internal class UIStockController : MonoBehaviour
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
    {
        [SerializeField]
        internal GameObject stockPrefab = null;
        [SerializeField]
        internal GameObject[] stockHolders = null;
        [SerializeField]
        internal Image[] stockImages = null;

        internal Int32 currentStock
        {
            private get => this._currentStock;
            set
            {
                if( value != this._currentStock )
                {
                    this.OnCurrentStockChanged( this._currentStock, value );
                    this._currentStock = value;
                }
            }
        }
#pragma warning disable IDE1006 // Naming Styles
        private Int32 _currentStock = 0;
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE0060 // Remove unused parameter
        private void OnCurrentStockChanged( Int32 oldStock, Int32 newStock )
#pragma warning restore IDE0060 // Remove unused parameter
        {

        }

        internal Int32 maxStock
        {
            private get => this._maxStock;
            set
            {
                if( value != this._maxStock )
                {
                    this.OnMaxStockChanged( this._maxStock, value );
                    this._maxStock = value;
                }
            }
        }
#pragma warning disable IDE1006 // Naming Styles
        private Int32 _maxStock = 0;
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE0060 // Remove unused parameter
        private void OnMaxStockChanged( Int32 oldStock, Int32 newStock )
#pragma warning restore IDE0060 // Remove unused parameter
        {

        }
    }
}
