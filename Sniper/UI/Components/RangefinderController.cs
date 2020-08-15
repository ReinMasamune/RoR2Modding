//#define TESTING
namespace Sniper.UI.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using TMPro;
    //using Unity.TextMeshPro;
    using UnityEngine;

    public class RangefinderController : MonoBehaviour
    {
        const String formatString = "g8";

        protected void Awake()
        {
            this.unitType = UnitType.Meter;
            this.metricPrefix = MetricPrefix.None;
        }

#if TESTING
        [Range(0f,10000000f)]
        public Single testDistance;
#endif
        public Single distance
        {
            set
            {
                if( value != this._distance )
                {
                    this._distance = value;

                    var converted = this.currentUnitInfo.ConvertUnit((Double)value);
                    this.numbersText.text = converted.ToString( formatString );
                }
            }
        }
        private Single _distance = 0.0f;

#if TESTING
        public MetricPrefix testMetPrefix;
#endif
        public MetricPrefix metricPrefix
        {
            set
            {
                if( value != this._metricPrefix )
                {
                    this._metricPrefix = value;

                    this.currentUnitInfo = new UnitInfo( this._metricPrefix, this._unitType );
                    this.unitText.text = this.currentUnitInfo.name;

                    var converted = this.currentUnitInfo.ConvertUnit((Double)this._distance);
                    this.numbersText.text = converted.ToString( formatString );
                }
            }
        }
        private MetricPrefix _metricPrefix = MetricPrefix.None;

#if TESTING
        public UnitType testUnitType;
#endif
        public UnitType unitType
        {
            set
            {
                if( value != this._unitType )
                {
                    this._unitType = value;

                    this.currentUnitInfo = new UnitInfo( this._metricPrefix, this._unitType );
                    this.unitText.text = this.currentUnitInfo.name;

                    var converted = this.currentUnitInfo.ConvertUnit((Double)this._distance);
                    this.numbersText.text = converted.ToString( formatString );
                }
            }
        }
        private UnitType _unitType;


        [HideInInspector]
        [SerializeField]
        private TextMeshProUGUI labelText;
        [HideInInspector]
        [SerializeField]
        private TextMeshProUGUI numbersText;
        [HideInInspector]
        [SerializeField]
        private TextMeshProUGUI unitText;

        // FUTURE: In awake read current unit from config



        private UnitInfo currentUnitInfo = new UnitInfo(MetricPrefix.None, UnitType.Meter);

        private void OnValidate()
        {
            this.labelText = base.transform.Find( "LabelText" ).GetComponent<TextMeshProUGUI>();
            this.numbersText = base.transform.Find( "NumbersText" ).GetComponent<TextMeshProUGUI>();
            this.unitText = base.transform.Find( "UnitText" ).GetComponent<TextMeshProUGUI>();
#if TESTING
            this.unitType = this.testUnitType;
            this.metricPrefix = this.testMetPrefix;
            this.distance = this.testDistance;
#endif
        }

        private readonly struct UnitInfo
        {
            private readonly Double conversionFromMeters;
            public readonly String name;

            public UnitInfo(MetricPrefix prefix, UnitType baseUnit)
            {
                Double mult = Math.Pow(10.0, -(Double)(Int32)prefix);
                if(!prefixLookup.TryGetValue(prefix, out var unitPref)) unitPref = "?";

                if(!unitNameLookup.TryGetValue(baseUnit, out var unitName)) unitName = "?";
                if(!unitConversionLookup.TryGetValue(baseUnit, out var conversion)) conversion = 1.0f;
                this.conversionFromMeters = conversion * mult;
                this.name = String.Format("{0}{1}", prefixLookup[prefix], unitName);
            }

            public Double ConvertUnit(Double input) => input * this.conversionFromMeters;
        }

        public enum MetricPrefix : Int32
        {
            [HideInInspector]
            Invalid = Int32.MinValue,
            yocto = -24,
            zepto = -21,
            atto = -18,
            femto = -15,
            pico = -12,
            nano = -9,
            micro = -6,
            milli = -3,
            centi = -2,
            deci = -1,
            None = 0,
            deca = 1,
            hecto = 2,
            kilo = 3,
            mega = 6,
            giga = 9,
            tera = 12,
            peta = 15,
            exa = 18,
            zetta = 21,
            yotta = 24,
        }

        public static readonly Dictionary<MetricPrefix, String> prefixLookup = new Dictionary<MetricPrefix, String>()
        {
            { MetricPrefix.yocto, "y" },
            { MetricPrefix.zepto, "z" },
            { MetricPrefix.atto, "a" },
            { MetricPrefix.femto, "f" },
            { MetricPrefix.pico, "p" },
            { MetricPrefix.nano, "n" },
            { MetricPrefix.micro, "μ" },
            { MetricPrefix.milli, "m" },
            { MetricPrefix.centi, "c" },
            { MetricPrefix.deci, "d" },
            { MetricPrefix.None, "" },
            { MetricPrefix.deca, "da" },
            { MetricPrefix.hecto, "h" },
            { MetricPrefix.kilo, "k" },
            { MetricPrefix.mega, "M" },
            { MetricPrefix.giga, "G" },
            { MetricPrefix.tera, "T" },
            { MetricPrefix.peta, "P" },
            { MetricPrefix.exa, "E" },
            { MetricPrefix.zetta, "Z" },
            { MetricPrefix.yotta, "Y" },
        };

        public enum UnitType
        {
            [HideInInspector]
            Invalid,
            Meter,

            //Thou,
            //Inch,
            //Foot,
            //Yard,
            //Chain,
            //Furlong,
            //Mile,
            //League,
            //Fathom,
            //Cable,
            //Nautical_Mile,
            //Link,
            //Rod,

            //Point,
            //Pica,
            //US_Survey_Foot,
            //US_Survey_Mile,

        }

        public static readonly Dictionary<UnitType, Double> unitConversionLookup = new Dictionary<UnitType, Double>()
        {
            { UnitType.Meter, 1.0 },
            //{ UnitType.Thou, 0.0000254 },
            //{ UnitType.Inch, 0.0254 },
            //{ UnitType.Foot, 0.3048 },
            //{ UnitType.Yard, 0.9144 },
            //{ UnitType.Chain, 20.1168 },
            //{ UnitType.Furlong, 201.168 },
            //{ UnitType.Mile, 1609.344 },
            //{ UnitType.League, 4828.032 },
            //{ UnitType.Fathom, 1.852 },
            //{ UnitType.Cable, 185.2 },
            //{ UnitType.Nautical_Mile, 1852.0 },
            //{ UnitType.Link, 0.201168 },
            //{ UnitType.Rod, 5.0292 },

            //{ UnitType.Point, 0.35277777777777777777777777777778 },
            //{ UnitType.Pica,  }
        };


        public static readonly Dictionary<UnitType, String> unitNameLookup = new Dictionary<UnitType, String>()
        {
            { UnitType.Meter, "m" },
            //{ UnitType.Thou, "th" },
            //{ UnitType.Inch, "in" },
            //{ UnitType.Foot, "ft" },
            //{ UnitType.Yard, "yd" },
            //{ UnitType.Chain, "ch" },
            //{ UnitType.Furlong, "fur" },
            //{ UnitType.Mile, "mi" },
            //{ UnitType.League, "lea" },
            //{ UnitType.Fathom, "ftm" },
            //{ UnitType.Cable, "cb" },
            //{ UnitType.Nautical_Mile, "nmi" },
            //{ UnitType.Link, "li" },
            //{ UnitType.Rod, "rd" },
        };
    }
}