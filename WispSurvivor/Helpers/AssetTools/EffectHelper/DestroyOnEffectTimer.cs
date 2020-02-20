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

        }

        IEnumerator DestroyRoutine( Single delay )
        {
            yield return new WaitForSeconds( delay );
            Destroy( base.gameObject );
        }
    }
}
