using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace ReinCore
{
    // TODO: Finish Interpolation Library

    public static class ExpandedInterpolation
    {
        public enum Mode { Standard, Squares, SquareRoots }
        public enum Style { Linear, Nearest, Polynomial, Spline, Spherical, Cubic }
        public enum Clamping { Clamped, Unclamped }
        public struct InterpolationSettings
        {
            public static InterpolationSettings standard = new InterpolationSettings( Mode.Standard, Style.Linear, Clamping.Clamped );
            public InterpolationSettings( Mode mode, Style style, Clamping clamping )
            {
                this.mode = mode;
                this.style = style;
                this.clamping = clamping;
            }
            /// <summary>
            /// This is slow as fuck and only exists for lazy people like me
            /// </summary>
            /// <param name="settings"></param>
            public InterpolationSettings( params System.Enum[] settings )
            {
                this.mode = Mode.Standard;
                this.style = Style.Linear;
                this.clamping = Clamping.Clamped;

                for( Int32 i = 0; i < settings.Length; ++i )
                {
                    var en = settings[i];
                    var type = en.GetType();
                    if( type == typeof(Mode) )
                    {
                        this.mode = (Mode)en;
                    } else if( type == typeof( Style ) )
                    {
                        this.style = (Style)en;
                    } else if( type == typeof( Clamping ) )
                    {
                        this.clamping = (Clamping)en;
                    }
                }
            }



            internal Mode mode { get; private set; }
            internal Style style { get; private set; }
            internal Clamping clamping { get; private set; }
        }

        public static Color Interpolate( Color from, Color to, Single t, InterpolationSettings settings )
        {
            return Interpolate( from, to, t, settings.mode, settings.style, settings.clamping );
        }

        public static Color Interpolate( Color from, Color to, Single t, Mode mode = Mode.Standard, Style style = Style.Linear, Clamping clamp = Clamping.Unclamped )
        {
            var r = Interpolate( from.r, to.r, t, mode, style, clamp );
            var g = Interpolate( from.g, to.g, t, mode, style, clamp );
            var b = Interpolate( from.b, to.b, t, mode, style, clamp );
            var a = Interpolate( from.a, to.a, t, mode, style, clamp );
            return new Color( r, g, b, a );
        }

        public static Single Interpolate( Single from, Single to, Single t, Mode mode = Mode.Standard, Style style = Style.Linear, Clamping clamp = Clamping.Unclamped )
        {
            Single output = 0f;
            switch( mode )
            {
                case Mode.Standard:
                    break;
                case Mode.Squares:
                    from *= from;
                    to *= to;
                    break;
                case Mode.SquareRoots:
                    from = Mathf.Sqrt( from );
                    to = Mathf.Sqrt( to );
                    break;
                default:
                    throw new ArgumentOutOfRangeException( nameof( mode ) );
            }
            switch( style )
            {
                case Style.Nearest:
                    output = t < 0.5f ? from : to;
                    break;
                case Style.Linear:
                    output = from + (to - from) * t;
                    break;
                case Style.Polynomial:
                    throw new NotImplementedException();
                    break;
                case Style.Spline:
                    throw new NotImplementedException();
                    break;
                case Style.Cubic:
                    throw new NotImplementedException();
                    break;
                case Style.Spherical:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException( nameof( style ) );
            }
            switch( mode )
            {
                case Mode.Standard:
                    break;
                case Mode.Squares:
                    output = Mathf.Sqrt( output );
                    break;
                case Mode.SquareRoots:
                    output *= output;
                    break;
                default:
                    throw new ArgumentOutOfRangeException( nameof( mode ) );
            }
            switch( clamp )
            {
                case Clamping.Unclamped:
                    break;
                case Clamping.Clamped:
                    output = Mathf.Clamp( output, Mathf.Min( from, to ), Mathf.Max( from, to ) );
                    break;
                default:
                    throw new ArgumentOutOfRangeException( nameof( clamp ) );
            }
            return output;
        }
    }
}
