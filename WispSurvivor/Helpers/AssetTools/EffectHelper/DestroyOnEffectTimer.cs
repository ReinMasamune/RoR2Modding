using R2API;
using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    [RequireComponent(typeof(EffectComponent))]
    internal class DestroyOnEffectTimer : MonoBehaviour
    {
        public EffectComponent effectComp;
        public ParticleSystem[] applyLifetimeTo = Array.Empty<ParticleSystem>();

        public void AddLifetimeParticle( ParticleSystem ps )
        {
            var ind = applyLifetimeTo.Length;
            Array.Resize<ParticleSystem>( ref this.applyLifetimeTo, ind + 1 );
            this.applyLifetimeTo[ind] = ps;
        }

        private void Start()
        {
            if( this.effectComp == null )
            {
                Main.LogE( "No effect component, effect will not be destroyed" );
                return;
            }
            var data = this.effectComp.effectData;
            if( data == null )
            {
                Main.LogE( "No effect data recieved, effect will not be destroyed" );
                return;
            }
            var timer = data.genericFloat;
            if( timer <= 0f )
            {
                Main.LogE( "Timer is less than or equal to 0, effect will be destroyed immediately" );
                Destroy( base.gameObject );
                return;
            }
            base.StartCoroutine( this.DestroyRoutine( timer ) );

            for( Int32 i = 0; i < this.applyLifetimeTo.Length; ++i )
            {
                var ps = this.applyLifetimeTo[i];
                var psMain = ps.main;
                var iDur = psMain.duration;
                var durMult = timer / iDur;
                psMain.duration = timer;

                var iLife = psMain.startLifetime;
                iLife.constant *= durMult;
                iLife.constantMin *= durMult;
                iLife.constantMax *= durMult;
                iLife.curveMultiplier *= durMult;
                psMain.startLifetime = iLife;
            }


        }

        IEnumerator DestroyRoutine( Single delay )
        {
            yield return new WaitForSeconds( delay );
            Destroy( base.gameObject );
        }
    }
}
