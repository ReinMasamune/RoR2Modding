using System;
using System.Collections.Generic;

using UnityEngine;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        [RequireComponent( typeof( WispPassiveController ) )]
        internal class WispCrosshairManager : MonoBehaviour
        {
            #region Static External
            internal static Boolean AddDisplay( IWispPassiveDisplay display, GameObject target )
            {
                if( !instanceLookup.ContainsKey( target ) ) return false;
                instanceLookup[target].AddDisplay( display );
                return true;
            }
            internal static Boolean RemoveDisplay( IWispPassiveDisplay display, GameObject target )
            {
                if( !instanceLookup.ContainsKey( target ) ) return false;
                instanceLookup[target].RemoveDisplay( display );
                return true;
            }
            #endregion



            #region Static Internal
            private static Dictionary<GameObject, WispCrosshairManager> instanceLookup = new Dictionary<GameObject, WispCrosshairManager>();
            private static Color[] barColors = new[]
            {
                new Color( 0f, 0f, 0f, 0.85f ),
                new Color( 1f, 1f, 1f, 0.85f ),
                new Color( 1f, 0f, 0f, 0.85f ),
                new Color( 0f, 1f, 0f, 0.85f ),
                new Color( 0f, 0f, 1f, 0.85f ),
                new Color( 1f, 1f, 0f, 0.85f ),
                new Color( 0f, 1f, 1f, 0.85f ),
                new Color( 1f, 0f, 1f, 0.85f ),
            };
            #endregion



            #region External

            #endregion



            #region Internal
            private HashSet<IWispPassiveDisplay> managedDisplays = new HashSet<IWispPassiveDisplay>();
            private WispPassiveController passive;

            private Double charge
            {
                get => this._charge;
                set
                {
                    if( this._charge != value )
                    {
                        this._charge = value;
                        this.UpdateCharge( this._charge );
                    }
                }
            }
            private Double _charge;


            private Single fillFrac
            {
                get => this._fillFrac;
                set
                {
                    if( this._fillFrac != value )
                    {
                        this.fillFracDirty = true;
                        this._fillFrac = value;
                    }
                }
            }
            private Single _fillFrac;
            private Boolean fillFracDirty = true;

            private Color bgColor
            {
                get => this._bgColor;
                set
                {
                    if( this._bgColor != value )
                    {
                        this.bgColorDirty = true;
                        this._bgColor = value;
                    }
                }
            }
            private Color _bgColor;
            private Boolean bgColorDirty = true;

            private Color fgColor
            {
                get => this._fgColor;
                set
                {
                    if( this._fgColor != value )
                    {
                        this.fgColorDirty = true;
                        this._fgColor = value;
                    }
                }
            }
            private Color _fgColor;
            private Boolean fgColorDirty = true;



            private void OnEnable()
            {
                this.passive = base.gameObject.GetComponent<WispPassiveController>();
                if( this.passive == null ) return;
                instanceLookup[base.gameObject] = this;
            }
            private void OnDisable()
            {
                instanceLookup.Remove( base.gameObject );
            }
            private void Update()
            {
                if( this.passive == null ) return;
                this.charge = this.passive.ReadCharge();
            }
            private void LateUpdate()
            {
                this.UpdateDirty();
            }
            private void UpdateCharge( Double charge )
            {
                var unbasedCharge = (charge) / 100;
                if( unbasedCharge < 1 )
                {
                    this.fillFrac = (Single)unbasedCharge;
                    this.bgColor = barColors[0];
                    this.fgColor = barColors[1];
                } else
                {
                    var powerInd = (UInt32)Math.Floor( Math.Log( unbasedCharge, 2 ) );
                    var curStart = Math.Pow( 2, powerInd++ );
                    unbasedCharge -= curStart;
                    this.fillFrac = (Single)( unbasedCharge / curStart );
                    this.bgColor = barColors[powerInd++ % barColors.Length];
                    this.fgColor = barColors[powerInd % barColors.Length];
                }
            }
            private void UpdateDirty()
            {
                foreach( var v in this.managedDisplays )
                {
                    if( this.fillFracDirty ) v.UpdateBarFrac( this.fillFrac );
                    if( this.bgColorDirty ) v.UpdateBGColor( this.bgColor );
                    if( this.fgColorDirty ) v.UpdateFGColor( this.fgColor );
                }
                this.fillFracDirty = false;
                this.bgColorDirty = false;
                this.fgColorDirty = false;
            }
            private void AddDisplay( IWispPassiveDisplay display )
            {
                this.managedDisplays.Add( display );
                display.UpdateBarFrac( this.fillFrac );
                display.UpdateBGColor( this.bgColor );
                display.UpdateFGColor( this.fgColor );
            }
            private void RemoveDisplay( IWispPassiveDisplay display )
            {
                this.managedDisplays.Remove( display );
            }

            #endregion
        }

        internal interface IWispPassiveDisplay
        {
            void UpdateBarFrac( Single newFrac );
            void UpdateBGColor( Color newColor );
            void UpdateFGColor( Color newColor );
        }
    }
}