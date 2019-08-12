using RoR2;
using UnityEngine;
using RoR2.UI;
using RoR2.Projectile;
using R2API.Utils;
using System.Reflection;

namespace ReinSniperRework
{
    class ReinHurtBoxManager : MonoBehaviour
    {
        public HurtBox managedHB;
        public HurtBox refHB;

        public void Awake()
        {
            HurtBoxGroup gr = refHB.hurtBoxGroup;
            HurtBox[] hbs = new HurtBox[gr.hurtBoxes.Length + 1];
            for( int i = 0; i < gr.hurtBoxes.Length; i++ )
            {
                hbs[i] = gr.hurtBoxes[i];
            }
            hbs[gr.hurtBoxes.Length] = managedHB;
            gr.hurtBoxes = hbs;
        }

        public void FixedUpdate()
        {
            if( managedHB && refHB )
            {
                managedHB.enabled = refHB.enabled;
                managedHB.teamIndex = refHB.teamIndex;
                managedHB.healthComponent = refHB.healthComponent;
            }
        }
    }
}
