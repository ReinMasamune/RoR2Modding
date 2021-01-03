namespace Rein.Sniper.Components
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Rein.Sniper.Orbs;

    using ReinCore;

    using RoR2;

    using UnityEngine;

    [RequireComponent(typeof(EffectComponent))]
    internal sealed class SporeEffectManager : MonoBehaviour, IRuntimePrefabComponent
    {
        private Single duration = 1f;
        private Single minRadius;
        private Single maxRadius;

        private Single age = 0f;

        [SerializeField]
        private ParticleSystem[] childParticles;
        private Single[] rotMults;

        //TODO: Remove field
        public IntersectionCloudMaterial indicatorMaterial;


        private unsafe void Start()
        {
            var efc = base.GetComponent<EffectComponent>();
            var data = efc.effectData;
            this.duration = data.genericFloat;
            this.minRadius = data.scale;
            var ptr = stackalloc UInt32[1];
            ptr[0] = data.genericUInt;
            this.maxRadius = *(Single*)ptr;

            this.rotMults = new Single[this.childParticles.Length];
            for(Int32 i = 0; i < this.rotMults.Length; i++)
            {
                var em = this.childParticles[i].emission;

                this.rotMults[i] = em.rateOverTimeMultiplier;
            }
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            this.age += dt;

            var r = Mathf.Lerp(this.minRadius, this.maxRadius, (this.age * SporeOrb.expansionSpeed) / this.duration);
            base.transform.localScale = new Vector3(r, r, r);

            var m = 1f + (r / this.minRadius);
            m *= m;
            for(Int32 i = 0; i < this.childParticles.Length; i++)
            {
                var em = this.childParticles[i].emission;
                em.rateOverTimeMultiplier = this.rotMults[i] * m;
            }
            if(this.age > this.duration)
            {
                this.gameObject.Destroy();
            }
        }

        public void InitializePrefab()
        {
            this.childParticles = base.GetComponentsInChildren<ParticleSystem>();
        }
    }
}