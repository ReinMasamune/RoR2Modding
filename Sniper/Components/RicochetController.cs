namespace Sniper.Components
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    using RoR2;

    using Sniper.Expansions;
    using Sniper.Modules;

    using UnityEngine;

    internal class RicochetController : MonoBehaviour
    {
        internal static GameObject ricochetEffectPrefab;

        private static EffectIndex ricochetIndex
        {
            get
            {
                if( _ricochetIndex == EffectIndex.Invalid )
                {
                    _ricochetIndex = EffectCatalog.FindEffectIndexFromPrefab( ricochetEffectPrefab );
                }
                return _ricochetIndex;
            }
        }
        private static EffectIndex _ricochetIndex = EffectIndex.Invalid;

        internal static void QueueRicochet( ExpandableBulletAttack<RicochetData> bullet, UInt32 delay )
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

        private IEnumerator Ricochet( ExpandableBulletAttack<RicochetData> bullet, UInt32 frameDelay )
        {
            for( UInt32 i = 0u; i < frameDelay; ++i )
            {
                yield return new WaitForFixedUpdate();
            }
            var data = new EffectData
            {
                origin = bullet.weapon.transform.position,
                start = bullet.weapon.transform.position,
            };
            EffectManager.SpawnEffect( ricochetIndex, data, true );
            bullet.Fire();
            Destroy( bullet.weapon );
        }
    }
}
