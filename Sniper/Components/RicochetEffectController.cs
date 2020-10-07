namespace Rein.Sniper.Components
{
    using System;
    using System.Collections;

    using ReinCore;

    using Rein.Sniper.Modules;

    using UnityEngine;

    internal class RicochetEffectController : MonoBehaviour
    {
        [SerializeField]
        internal Single destroyDelay = 5f;

        protected void Start()
        {
            SoundModule.PlayRicochet( base.gameObject );
            _ = base.StartCoroutine( this.DestroyOnTimer( this.destroyDelay ) );
        }
    }
}