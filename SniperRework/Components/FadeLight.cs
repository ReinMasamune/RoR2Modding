using RoR2;
using UnityEngine;
using RoR2.UI;
using RoR2.Projectile;
using R2API.Utils;
using System.Reflection;

namespace ReinSniperRework
{
    class FadeLight : MonoBehaviour
    {
        private float timer = 0f;
        private float startInt;
        private float startRad;

        public float time;
        public bool scaleInt = true;
        public bool scaleRad = true;
        public Light light;

        private void Awake()
        {
            startInt = light.intensity;
            startRad = light.range;
        }

        private void Update()
        {
            if( timer < time )
            {
                float frac = 1f - timer / time;
                if( scaleInt )
                {
                    light.intensity = startInt * frac;
                }
                if( scaleRad )
                {
                    light.range = startRad * frac;
                }

                timer += Time.deltaTime;
            }
            else
            {
                if( scaleInt )
                {
                    light.intensity = 0f;
                }
                if( scaleRad )
                {
                    light.range = 0f;
                }
            }
        }
    }
}
