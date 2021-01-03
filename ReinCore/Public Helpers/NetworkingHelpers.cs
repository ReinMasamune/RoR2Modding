namespace ReinCore
{
    using System;

    using RoR2;
    using RoR2.Orbs;

    using UnityEngine;
    using UnityEngine.Networking;

    /// <summary>
    /// Helper functions for various RoR2 networking needs
    /// </summary>
    public static class NetworkingHelpers
    {
        public static void DealDamage( this DamageInfo damage, HurtBox target, Boolean callDamage, Boolean callHitEnemy, Boolean callHitWorld )
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

        public static void ApplyBuff( this CharacterBody body, BuffIndex buff, Int32 stacks = 1, Single duration = -1f )
        {
            if( NetworkServer.active )
            {
                if( duration < 0f )
                {
                    body.SetBuffCount( buff, body.GetBuffCount(buff) + stacks );
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

        public static void ApplyDoT( this HealthComponent victim, GameObject attacker, DotController.DotIndex dotIndex, Single duration = 8f, Single damageMultiplier = 1f )
        {
            if( NetworkServer.active )
            {
                DotController.InflictDot( victim.gameObject, attacker, dotIndex, duration, damageMultiplier );
            } else
            {
                new DoTMessage( victim.gameObject, attacker, dotIndex, duration, damageMultiplier ).Send( NetworkDestination.Server );
            }
        }

        public static void Create<TOrb>(this TOrb orb)
            where TOrb : Orb
        {
            if(NetworkServer.active)
            {
                OrbManager.instance.AddOrb(orb);
            } else
            {
                var index = OrbIndex<TOrb>.index;
                if(index == OrbSerializerCatalog.Index.Invalid) throw new InvalidOperationException("Orb not registered in catalog");
                if(!OrbSerializerCatalog.TryGetDef(index, out var def) || def is NoSerializer) throw new InvalidOperationException("No valid serializer registered for orb");
                new OrbMessage(orb, def).Send(NetworkDestination.Server);
            }
        }
    }
}
