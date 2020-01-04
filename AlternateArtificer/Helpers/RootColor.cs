namespace AlternativeArtificer.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    public class RootColor
    {
        public Color color
        {
            get
            {
                return new Color
                {
                    r = Mathf.Sqrt( this.r ),
                    g = Mathf.Sqrt( this.g ),
                    b = Mathf.Sqrt( this.b ),
                    a = this.a
                };
            }
        }

        public RootColor clone
        {
            get
            {
                return new RootColor
                {
                    r = this.r,
                    g = this.g,
                    b = this.b,
                    a = this.a
                };
            }
        }

        public Single intensity
        {
            get
            {
                return (this.r + this.g + this.b) / 3f;
            }
        }

        public Single r;
        public Single g;
        public Single b;
        public Single a;

        public RootColor()
        {
            this.r = 0f;
            this.g = 0f;
            this.b = 0f;
            this.a = 0f;
        }

        public RootColor( Color input )
        {
            this.r = input.r * input.r;
            this.g = input.g * input.g;
            this.b = input.b * input.b;
            this.a = input.a;
        }

        public RootColor( Single r, Single g, Single b, Single a )
        {
            this.r = r * r;
            this.g = g * g;
            this.b = b * b;
            this.a = a;
        }

        public RootColor Rebase( RootColor modifier )
        {
            var mod = modifier.clone;
            var inputIntensity = this.intensity;
            var modIntensity = mod.intensity;

            var modR = mod.r;
            var modG = mod.g;
            var modB = mod.b;

            modR -= modIntensity;
            modG -= modIntensity;
            modB -= modIntensity;

            modR /= modIntensity;
            modG /= modIntensity;
            modB /= modIntensity;

            modR *= inputIntensity;
            modG *= inputIntensity;
            modB *= inputIntensity;

            modR += inputIntensity;
            modG += inputIntensity;
            modB += inputIntensity;

            this.r = modR;
            this.g = modG;
            this.b = modB;

            return this;
        }

        public static RootColor operator *(RootColor a, Single b)
        {
            a.r *= b;
            a.g *= b;
            a.b *= b;
            return a;
        }

        public static RootColor operator /(RootColor a, Single b)
        {
            a.r /= b;
            a.g /= b;
            a.b /= b;
            return a;
        }
    }
}
