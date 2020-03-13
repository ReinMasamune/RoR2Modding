using System;
using System.Collections.Generic;
using RoR2;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        public class WispFlamesController : MonoBehaviour
        {
            public WispPassiveController passive;
            private CharacterModel model;
            private ParticleSystem[] flames;
            private ParticleSystem.MinMaxCurve[] baseRates;

            private void Awake()
            {
                this.model = base.gameObject.GetComponentInChildren<CharacterModel>();
                this.flames = new ParticleSystem[this.model.baseParticleSystemInfos.Length];
                this.baseRates = new ParticleSystem.MinMaxCurve[this.model.baseParticleSystemInfos.Length];
                for( Int32 i = 0; i < this.model.baseParticleSystemInfos.Length; ++i )
                {
                    var temp = this.model.baseParticleSystemInfos[i].particleSystem;
                    this.flames[i] = temp;
                    this.baseRates[i] = temp.emission.rateOverTime;
                }
            }

            public void Update()
            {
                System.Single mult = (System.Single)(this.passive.ReadCharge() / 100.0);
                for( System.Int32 i = 0; i < this.flames.Length; i++ )
                {
                    var temp = this.flames[i].emission;
                    temp.rateOverTimeMultiplier = this.baseRates[i].Evaluate(0f) * mult;
                }
            }
        }
    }
}
