using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using Sniper.Expansions;
using UnityEngine;
using UnityEngine.Networking;

namespace Sniper.Components
{
    internal class RicochetController : MonoBehaviour
    {
        internal static void QueueRicochet( ExpandableBulletAttack bullet, UInt32 delay )
        {
            instance?.StartCoroutine( instance.Ricochet( bullet, delay ) );
        }

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
        private static RicochetController _instance;

        private IEnumerator Ricochet( ExpandableBulletAttack bullet, UInt32 frameDelay )
        {
            for( UInt32 i = 0u; i < frameDelay; ++i )
            {
                yield return new WaitForFixedUpdate();
            }
            bullet.Fire();
            DestroyImmediate( bullet.weapon );
        }
    }
}
