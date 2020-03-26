using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal static partial class EffectHelper
    {
        private static Dictionary<GameObject, UInt32> beamCounter = new Dictionary<GameObject, UInt32>();
        internal static BeamController SetupBeam( GameObject mainObj, Transform endObj, Single beamEmisRatio, Single forwardOffset )
        {
            if( !beamCounter.ContainsKey( mainObj ) ) beamCounter[mainObj] = 0u;
            var beamPar = new GameObject( "Beam" + beamCounter[mainObj]++ );
            beamPar.transform.parent = mainObj.transform;
            beamPar.transform.localPosition = new Vector3( 0f, 0f, forwardOffset);
            beamPar.transform.localScale = Vector3.one;
            beamPar.transform.localRotation = Quaternion.identity;

            var obj = new GameObject( "BeamMid" + (beamCounter[mainObj] - 1) );
            obj.transform.parent = beamPar.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;

            var beamControl = beamPar.AddComponent<BeamController>();
            beamControl.endTransform = endObj;
            beamControl.midTransform = obj.transform;
            beamControl.distRateRatio = beamEmisRatio;

            return beamControl;
        }
    }
}
