namespace ReinCore
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// A component destroys a tracer after it has reached destination
    /// </summary>
    public class DestroyTracerOnDelay : MonoBehaviour
    {
        /// <summary>
        /// The delay before destruction
        /// </summary>
        public Single delay = 0f;
        /// <summary>
        /// The tracer that is checked
        /// </summary>
        public RoR2.Tracer tracer;

        private void Awake()
        {
            if( this.tracer.onTailReachedDestination == null )
            {
                this.tracer.onTailReachedDestination = new UnityEngine.Events.UnityEvent();
            }

            UnityEngine.Events.UnityEvent uEvent = this.tracer.onTailReachedDestination;
            uEvent.AddListener( new UnityEngine.Events.UnityAction( () => this.StartCoroutine(this.DestroyOnTimer(this.delay)) ) );
        }

        private IEnumerator DestroyOnTimer( Single delay )
        {
            yield return new WaitForSeconds( delay );
            UnityEngine.Object.Destroy( this );
        }
    }
}
