using UnityEngine;
using System.Collections.Generic;

namespace WispSurvivor.Components
{
    public class WispFlamesController : MonoBehaviour
    {
        public WispPassiveController passive;
        public List<ParticleSystem> flames = new List<ParticleSystem>();
        public List<float> flameInfos = new List<float>();

        public void Update()
        {
            float mult = (float) (passive.ReadCharge() / 100.0);
            ParticleSystem.EmissionModule temp;
            for( int i = 0; i < flames.Count; i++ )
            {
                temp = flames[i].emission;
                temp.rateMultiplier = mult * flameInfos[i];
            }
        }
    }
}
