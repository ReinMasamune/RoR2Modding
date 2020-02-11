
using RoR2;
using System;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class WispBitSkinController : BitSkinController
    {

        private WispBitSkin skin
        {
            get => this._skin;
            set
            {
                if( value != this._skin )
                {
                    this._skin = value;
                    this.OnSkinChanged( value );
                }
            }
        }
        private WispBitSkin _skin;


        internal override void Apply( IBitSkin skin )
        {
            if( !(skin is WispBitSkin) )
            {
                throw new ArgumentException( "Provided skin was not a WispBitSkin" );
            }

            this.skin = (WispBitSkin)skin;
        }


        private void OnSkinChanged( WispBitSkin newSkin )
        {

        }
    }
}
