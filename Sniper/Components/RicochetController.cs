namespace Sniper.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using BepInEx.Logging;
    using ReinCore;
    using RoR2;
    using Sniper.Expansions;
    using Sniper.Modules;
    using UnityEngine;
    using UnityEngine.Networking;

    internal class RicochetController : MonoBehaviour
    {
        internal static void QueueRicochet( ExpandableBulletAttack bullet, UInt32 delay ) => instance?.StartCoroutine( instance.Ricochet( bullet, delay ) );

        private static RicochetController instance
        {
            get
            {
                if( _instance == null || !_instance )
                {
                    _instance = new GameObject( "RicochetController" ).AddComponent<RicochetController>();
                }
                return _instance;
            }
        }
#pragma warning disable IDE1006 // Naming Styles
        private static RicochetController _instance;
#pragma warning restore IDE1006 // Naming Styles

        private IEnumerator Ricochet( ExpandableBulletAttack bullet, UInt32 frameDelay )
        {
            for( UInt32 i = 0u; i < frameDelay; ++i )
            {
                yield return new WaitForFixedUpdate();
            }
            SoundModule.PlayRicochet( bullet.weapon );
            bullet.Fire();
            Destroy( bullet.weapon );
        }
    }
}
