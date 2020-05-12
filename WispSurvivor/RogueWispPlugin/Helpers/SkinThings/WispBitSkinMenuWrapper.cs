using System;

using ReinCore;

using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal class WispBitSkinMenuWrapper
    {
        private const UInt32 firstBitMask = 0b0000_0000_0000_0000_0000_0000_0000_0001u;
        private UInt32 backingValue = 0u;
        private WispModelBitSkinController controller;

        internal WispBitSkinMenuWrapper( WispModelBitSkinController controller )
        {
            this.controller = controller;
        }

        private void SetBit( Int32 ind, Boolean value )
        {
            if( ind < 0 || ind > 31 ) throw new ArgumentException( "Ind must be between 0 and 31" );

            var mask = ~(firstBitMask << ind);
            var val = (value ? 1u : 0u) << ind;
            var temp = (this.backingValue & mask) | val;
            if( this.backingValue != temp )
            {
                this.backingValue = temp;

            }
            this.UpdateController();
        }
        private Boolean GetBit( Int32 ind )
        {
            return 1u == ( ( this.backingValue >> ind ) & firstBitMask );
        }
        private Boolean SetBits( Int32 start, Int32 end, UInt32 value, Boolean shouldUpdate = true )
        {
            var dist = end - start + 1;
            if( dist <= 0 )
            {
                throw new ArgumentException( "End must be greater than start" );
            }
            var range = Mathf.FloorToInt(Mathf.Pow( 2, dist ) );
            if( value > range )
            {
                throw new ArgumentException( "Value: " + value + " was too high to fit within range: " + range );
            }

            var tempMask = 0u;
            for( Int32 i = start; i <= end; ++i )
            {
                tempMask |= ( firstBitMask << i );
            }

            var tempVal = this.backingValue & ~tempMask;
            tempVal |= value << start;
            if( this.backingValue != tempVal )
            {
                this.backingValue = tempVal;
                if( shouldUpdate )
                {
                    this.UpdateController();
                }
                return true;
            }
            return false;
        }
        private UInt32 GetBits( Int32 start, Int32 end )
        {
            var dist = end - start;
            if( dist <= 0 )
            {
                throw new ArgumentException( "End must be greater than start" );
            }

            var tempVal = this.backingValue;
            var tempMask = 0u;
            for( Int32 i = start; i <= end; ++i )
            {
                tempMask |= firstBitMask << i;
            }
            tempVal &= ~tempMask;
            tempVal >>= start;
            return tempVal;
        }



        private void UpdateController()
        {
            this.controller.Apply( WispBitSkin.GetWispSkin( this.backingValue ) );
        }

        [Menu( sectionName = "Flags" )]
        internal Boolean cracks
        {
            get => this.bit31;
            set => this.bit31 = value;
        }

        [Menu( sectionName = "Flags" )]
        internal Boolean transparent
        {
            get => this.bit30;
            set => this.bit30 = value;
        }

        [Menu( sectionName = "Flags" )]
        internal Boolean iridescent
        {
            get => this.bit29;
            set => this.bit29 = value;
        }

        [Menu( sectionName = "Flags" )]
        internal Boolean useCustomColor
        {
            get => this.bit28;
            set
            {
                if( value != this.bit28 )
                {
                    this.SetBits( 0, 17, 0u, false );
                    this.bit28 = value;
                }
            }
        }

        [Menu( sectionName = "Selections" )]
        internal WispBitSkin.ArmorMaterialType armorMaterial
        {
            get => (WispBitSkin.ArmorMaterialType)this.GetBits( 25, 27 );
            set => this.SetBits( 25, 27, (UInt32)value );
        }
        [Menu( sectionName = "Selections" )]
        internal WispBitSkin.FlameGradientType flameGradient
        {
            get => (WispBitSkin.FlameGradientType)this.GetBits( 18, 20 );
            set => this.SetBits( 18, 20, (UInt32)value );
        }
        [Menu( sectionName = "Selections" )]
        internal WispBitSkin.WispColorIndex color
        {
            get => (WispBitSkin.WispColorIndex)this.GetBits( 0, 3 );
            set => this.SetBits( 0, 3, (UInt32)value );
        }
        [Menu( sectionName = "Custom Color" )]
        internal Color customColor
        {
            get
            {
                if( !this.useCustomColor ) return Color.black;

                var r = this.GetBits( 12,17 ) / (Single)0b11_1111;
                var g = this.GetBits( 6,11 ) / (Single)0b11_1111;
                var b = this.GetBits( 0,5 ) / (Single)0b11_1111;
                var a = 1.0f;

                return new Color( r, g, b, a );
            }
            set
            {
                if( !this.useCustomColor ) return;

                var ri = (Int32)Math.Round( value.r * 0b11_1111u );
                var gi = (Int32)Math.Round( value.g * 0b11_1111u );
                var bi = (Int32)Math.Round( value.b * 0b11_1111u );
                var r = (UInt32)ri;
                var g = (UInt32)gi;
                var b = (UInt32)bi;


                r = Math.Min( r, 0b11_1111u );
                g = Math.Min( g, 0b11_1111u );
                b = Math.Min( b, 0b11_1111u );

                var changed = false;
                changed |= this.SetBits( 0, 5, b, false );
                changed |= this.SetBits( 6, 11, g, false );
                changed |= this.SetBits( 12, 17, r, false );
                if( changed ) this.UpdateController();
            }
        }







        [Menu()]
        internal Boolean bit21
        {
            get => this.GetBit( 21 );
            set => this.SetBit( 21, value );
        }
        [Menu()]
        internal Boolean bit22
        {
            get => this.GetBit( 22 );
            set => this.SetBit( 22, value );
        }
        [Menu()]
        internal Boolean bit23
        {
            get => this.GetBit( 23 );
            set => this.SetBit( 23, value );
        }
        [Menu()]
        internal Boolean bit24
        {
            get => this.GetBit( 24 );
            set => this.SetBit( 24, value );
        }
        internal Boolean bit28
        {
            get => this.GetBit( 28 );
            set => this.SetBit( 28, value );
        }
        internal Boolean bit29
        {
            get => this.GetBit( 29 );
            set => this.SetBit( 29, value );
        }

        internal Boolean bit30
        {
            get => this.GetBit( 30 );
            set => this.SetBit( 30, value );
        }

        internal Boolean bit31
        {
            get => this.GetBit( 31 );
            set => this.SetBit( 31, value );
        }
    }
}
