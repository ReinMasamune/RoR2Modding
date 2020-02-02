using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal enum MaterialIndex : ulong
    {
        #region Reference Materials
        refMatTracer,
        refMatTracerBright,
        refMatDebris1,
        refMatDistortionFaded,
        refMatInverseDistortion,
        refMatOpagueDustSpeckledLarge,

        refMatNullifierStarParticle,
        refMatNullifierStarTrail,
        refMatNullifierStarPortalEdge,
        refMatNullifierExplosionAreaIndicatorSoft,
        refMatNullifierExplosionAreaIndicatorHard,
        refMatNullBombAreaIndicator,
        refMatNullifierGemPortal,

        refMatWillowispSpiral,
        refMatWillowispRadial,
        #endregion
    }
}
