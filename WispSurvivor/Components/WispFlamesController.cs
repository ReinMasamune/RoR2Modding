using System.Collections.Generic;
using UnityEngine;

namespace WispSurvivor.Components
{
    public class WispFlamesController : MonoBehaviour
    {
        public WispPassiveController passive;
        public List<ParticleSystem> flames = new List<ParticleSystem>();
        public List<System.Single> flameInfos = new List<System.Single>();

        public void Update()
        {
            System.Single mult = (System.Single)(this.passive.ReadCharge() / 100.0);
            ParticleSystem.EmissionModule temp;
            for( System.Int32 i = 0; i < this.flames.Count; i++ )
            {
                temp = this.flames[i].emission;
                temp.rateMultiplier = mult * this.flameInfos[i];
            }
        }
    }
}
