namespace Sniper.Data
{
    using System;

    using UnityEngine;

    [Serializable]
    internal struct ZoomParams
    {
        [SerializeField]
        private readonly Single inputScale;
        [SerializeField]
        private readonly Single defaultZoom;
        [SerializeField]
        private readonly Single shoulderFrac;
        [SerializeField]
        private readonly Single shoulderZoomStart;
        [SerializeField]
        private readonly Single shoulderZoomEnd;
        [SerializeField]
        private readonly Single scopeZoomStart;
        [SerializeField]
        private readonly Single scopeZoomEnd;


        [SerializeField]
        internal Single viewAngleStart;

        internal ZoomParams( Single shoulderStart, Single shoulderEnd, Single scopeStart, Single scopeEnd, Single shoulderFrac, Single defaultZoom, Single inputScale, Single baseFoV )
        {
            this.inputScale = inputScale;
            this.defaultZoom = defaultZoom;
            this.shoulderFrac = shoulderFrac;
            this.shoulderZoomStart = shoulderStart;
            this.shoulderZoomEnd = shoulderEnd;
            this.scopeZoomStart = scopeStart;
            this.scopeZoomEnd = scopeEnd;

            Single start = baseFoV * Mathf.Deg2Rad;
            start *= Mathf.Tan( start );
            this.viewAngleStart = start;
        }


        internal Single GetFoV( Single currentZoom ) => this.ZoomToFoV( this.IsInScope( currentZoom ) ? this.GetScopeZoom( currentZoom ) : this.GetShoulderZoom( currentZoom ) );

        internal Single UpdateZoom( Single inputValue, Single currentZoom ) => Mathf.Clamp01( currentZoom + ( inputValue * this.inputScale ) );

        internal Boolean IsInScope( Single currentZoom ) => currentZoom > this.shoulderFrac;

        private Single GetScopeZoom( Single currentZoom ) => Mathf.Lerp( this.scopeZoomStart, this.scopeZoomEnd, ( currentZoom - this.shoulderFrac ) / ( 1f - this.shoulderFrac ) );

        private Single GetShoulderZoom( Single currentZoom ) => Mathf.Lerp( this.shoulderZoomStart, this.shoulderZoomEnd, 1f - ( ( this.shoulderFrac - currentZoom ) / this.shoulderFrac ) );

        private Single ZoomToFoV( Single zoomFactor ) => Mathf.Atan( this.viewAngleStart / zoomFactor ) * Mathf.Rad2Deg;
    }
}