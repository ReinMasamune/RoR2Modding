using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class FireBallElement : ParticleElementTemplate
    {
        internal enum FireballType
        {
            Preon = 0,
            Lemurian = 1,
            GreaterWisp = 2,
        }
        internal FireBallElement( ParticleElement element ) : base( element )
        {

        }
    }
}
