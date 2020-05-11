using System;
using System.Collections.Generic;
using System.Text;
using RoR2;

namespace ReinCore
{
    public sealed class DoTDef
    {
        public Single interval;
        public Single damageCoefficient;
        public DamageColorIndex damageColorIndex;
        public BuffIndex associatedBuff = BuffIndex.None;
    }

}
