namespace ReinCore
{
    using System;

    using RoR2;

    using UnityEngine;
    using UnityEngine.Networking;

    /// <summary>
    /// Helper functions for various RoR2 networking needs
    /// </summary>
    public static class NetworkingHelpers
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void DealDamage( this DamageInfo damage, HurtBox target, Boolean callDamage, Boolean callHitEnemy, Boolean callHitWorld )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( NetworkServer.active )
            {
                if( callDamage )
                {
                    if( target != null && target.healthComponent != null )
                    {
                        target.healthComponent.TakeDamage( damage );
                    }
                }

                if( callHitEnemy )
                {
                    if( target != null && target.healthComponent != null )
                    {
                        GlobalEventManager.instance.OnHitEnemy( damage, target.healthComponent.gameObject );
                    }
                }

                if( callHitWorld )
                {
                    GlobalEventManager.instance.OnHitAll( damage, target && target.healthComponent ? target.healthComponent.gameObject : null );
                }
            } else
            {
                new DamageMessage( damage, target, callDamage, callHitEnemy, callHitWorld ).Send( NetworkDestination.Server );
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void ApplyBuff( this CharacterBody body, BuffIndex buff, Int32 stacks = 1, Single duration = -1f )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( NetworkServer.active )
            {
                if( duration < 0f )
                {
                    body._SetBuffCount( buff, stacks );
                } else
                {
                    if( stacks < 0 )
                    {
                        Log.Error( "Cannot remove duration from a buff" );
                        return;
                    }

                    for( Int32 i = 0; i < stacks; ++i )
                    {
                        body.AddTimedBuff( buff, duration );
                    }
                }
            } else
            {
                new BuffMessage( body, buff, stacks, duration ).Send( NetworkDestination.Server );
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void ApplyDoT( this HealthComponent victim, GameObject attacker, DotController.DotIndex dotIndex, Single duration = 8f, Single damageMultiplier = 1f )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( NetworkServer.active )
            {
                DotController.InflictDot( victim.gameObject, attacker, dotIndex, duration, damageMultiplier );
            } else
            {
                new DoTMessage( victim.gameObject, attacker, dotIndex, duration, damageMultiplier ).Send( NetworkDestination.Server );
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void CreateOrb() => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
