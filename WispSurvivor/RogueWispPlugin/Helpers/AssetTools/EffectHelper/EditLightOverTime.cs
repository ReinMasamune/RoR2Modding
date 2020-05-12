using System;

using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal static partial class EffectHelper
    {
        internal static void EditLightOverTime( Light light, Single duration, AnimationCurve intensityOverLifetime, AnimationCurve rangeOverLifetime )
        {
            var scaler = light.gameObject.AddComponent<Main.LightScaler>();
            scaler.targetLight = light;
            scaler.duration = duration;
            scaler.lightIntensity = intensityOverLifetime;
            scaler.lightRange = rangeOverLifetime;
        }
    }
}
