using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReinCore
{
    /// <summary>
    /// A component that plays a particle system on behaviour Start
    /// </summary>
    public class ParticleSystemPlayOnStart : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem[] particleSystems;

        /// <summary>
        /// Adds the particle systems to play on start
        /// </summary>
        /// <param name="targets">The target particle systems</param>
        public void SetTargets( params ParticleSystem[] targets ) => this.particleSystems = targets;

        private void Start()
        {
            if( this.particleSystems != null )
            {
                for( Int32 i = 0; i < this.particleSystems.Length; ++i )
                {
                    this.particleSystems[i]?.Play();
                }
            }
        }
    }
}
