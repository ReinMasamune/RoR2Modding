namespace ReinCore
{
    using System;
    using System.Linq;

    using RoR2;

    using UnityEngine;

    public static class EffectXtn
    {
        public static void ApplyEffectColor(this EffectComponent effect, params ParticleSystem[] systems)
        {
            var applier = effect.AddOrGetComponent<ApplyEffectColor>();


        }
    }


    [RequireComponent(typeof(EffectComponent))]
    internal sealed class ApplyEffectColor : MonoBehaviour
    {
        [SerializeField]
        private EffectComponent _effect;

        internal EffectComponent effect { set => this.effect = value; }

        [SerializeField]
        private ParticleSystem[] _targets;
        internal void AddParticles(params ParticleSystem[] particles)
        {
            if(this._targets is not null)
            {
                this._targets = particles;
                return;
            }

            this._targets = this._targets.Concat(particles).ToArray();
        }

        private void Awake()
        {
            var color = this._effect.effectData.color;

            foreach(var t in this._targets)
            {
                var main = t.main;
                main.startColor = (Color)color;
            }

            this.enabled = false;
        }
    }
}
