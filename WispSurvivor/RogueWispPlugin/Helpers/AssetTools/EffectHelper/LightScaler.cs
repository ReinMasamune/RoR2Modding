using System;

using UnityEngine;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        internal class LightScaler : MonoBehaviour
        {
            public Light targetLight;

            public AnimationCurve lightRange;
            public AnimationCurve lightIntensity;

            public Single duration;

            private Single baseRange;
            private Single baseIntensity;
            private Single timer;

            private void Awake()
            {
                this.baseRange = this.targetLight.range;
                this.baseIntensity = this.targetLight.intensity;
                this.timer = 0f;
            }

            private void Update()
            {
                this.timer += Time.deltaTime;

                var t = Mathf.Clamp01( this.timer / this.duration );

                var curIntensity = this.baseIntensity * this.lightIntensity.Evaluate( t );
                var curRange = this.baseRange * this.lightRange.Evaluate( t );

                this.targetLight.intensity = curIntensity;
                this.targetLight.range = curRange;
                this.targetLight.enabled = curIntensity >= 0f && curRange >= 0f;

                if( t == 1f ) this.enabled = false;
            }
        }
    }
}