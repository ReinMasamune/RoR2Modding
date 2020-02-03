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
        #region Direct Load
        refMatShatteredGlass,
        refMatVagrantEnergized,
        refMatTPInOut,
        refMatMercEnergized,
        refMatElitePoisonParticleSystemReplacement,
        refMatOnHelfire,
        #endregion

        #region Prefab Dependencies
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

        refMatBazaarIceCore,
        refMatBazaarIceDistortion,

        refMatGenericFlash,
        refMatDustDirectionalDark,
        refMatFireRingRunes,
        
        refMatGolemExplosion,
        refMatTitanBeam,
        refMatArcaneCircle1,
        refMatDistortion,
        #endregion
        #endregion
    }
}
