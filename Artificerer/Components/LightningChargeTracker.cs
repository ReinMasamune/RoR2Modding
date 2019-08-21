using RoR2;
using UnityEngine;
using RoR2.UI;
using RoR2.Projectile;
using R2API.Utils;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using EntityStates;
using System;

namespace ReinArtificerer
{
    public class ReinLightningBuffTracker : MonoBehaviour
    {
        public ReinDataLibrary data;
        public CharacterBody body;

        private bool active = false;
        private float timeLeft = 0f;
        private float intensity;
        
        private void FixedUpdate()
        {
            if( !active )
            {
                if( timeLeft != 0f )
                {
                    if( timeLeft > 0f )
                    {
                        active = true;
                    }
                    if( timeLeft < 0f )
                    {
                        timeLeft = 0f;
                    }
                }
            }
            if( active )
            {
                timeLeft -= Time.fixedDeltaTime;
                if( timeLeft <= 0f )
                {
                    active = false;
                    intensity = 0f;
                }
            }
        }

        public bool GetBuffed()
        {
            return active;
        }

        public float GetIntensity()
        {
            if( active )
            {
                return intensity;
            }
            return 0f;
        }

        public void AddCharged( float time , float intensity)
        {
            timeLeft += time;
            this.intensity = Mathf.Max(this.intensity, intensity);
        }
    }
}