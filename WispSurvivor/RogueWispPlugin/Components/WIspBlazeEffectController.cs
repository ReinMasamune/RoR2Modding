using System.Collections;

using RoR2;

using UnityEngine;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        [RequireComponent( typeof( EffectComponent ) )]
        public class WispBlazeEffectController : MonoBehaviour
        {
            private System.Single timeLeft;
            public void Start()
            {
                EffectComponent effectComp = this.GetComponent<EffectComponent>();
                EffectData data = effectComp.effectData;

                this.timeLeft = data.genericFloat;

                this.StartCoroutine( this.DestroyOnTimer( this.timeLeft ) );
            }

            private IEnumerator DestroyOnTimer( System.Single timer )
            {
                yield return new WaitForSeconds( timer );

                MonoBehaviour.Destroy( this.gameObject );
            }
        }
    }
}
