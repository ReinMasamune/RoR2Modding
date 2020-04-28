﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using Sniper.Expansions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Sniper.Components
{
    internal class UIStockController : MonoBehaviour
    {
        [SerializeField]
        internal GameObject stockPrefab;
        [SerializeField]
        internal GameObject[] stockHolders;
        [SerializeField]
        internal Image[] stockImages;

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
        private Int32 _currentStock = 0;
        private void OnCurrentStockChanged( Int32 oldStock, Int32 newStock )
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
        private Int32 _maxStock = 0;
        private void OnMaxStockChanged( Int32 oldStock, Int32 newStock )
        {

        }
    }
}