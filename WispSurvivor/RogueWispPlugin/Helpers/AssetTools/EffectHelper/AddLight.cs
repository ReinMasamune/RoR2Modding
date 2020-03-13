using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static partial class EffectHelper
    {
        private static Dictionary<GameObject,UInt32> lightCounter = new Dictionary<GameObject, UInt32>();
        internal static Light AddLight( GameObject mainObj, WispSkinnedEffect skin, Boolean applyColor, Single range, Single intensity )
        {
            if( !lightCounter.ContainsKey( mainObj ) ) lightCounter[mainObj] = 0u;
            var obj = new GameObject( "Light" + lightCounter[mainObj]++ );
            obj.transform.parent = mainObj.transform;

            var light = obj.AddComponent<Light>();

            if( applyColor )
            {
                skin.AddLight( light );
            }

            light.type = LightType.Point;
            light.intensity = intensity;
            light.range = range;
            light.color = Color.white;

            return light;
        }
    }
}
