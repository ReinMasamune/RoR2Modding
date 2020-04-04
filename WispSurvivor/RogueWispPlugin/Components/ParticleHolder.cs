
using RoR2.ConVar;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Rein.RogueWispPlugin
{
    public class ParticleHolder : MonoBehaviour
    {
        [SerializeField]
        public ParticleSystem[] systems = Array.Empty<ParticleSystem>();
        [SerializeField]
        public ParticleSystemRenderer[] renderers = Array.Empty<ParticleSystemRenderer>();

        public void Add( ParticleSystem ps, ParticleSystemRenderer psr )
        {

            var ind = this.systems.Length;

            Array.Resize( ref this.systems, ind + 1 );
            Array.Resize( ref this.renderers, ind + 1 );

            this.systems[ind] = ps;
            this.renderers[ind] = psr;
        }
    }

}