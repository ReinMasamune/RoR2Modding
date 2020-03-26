using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
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
