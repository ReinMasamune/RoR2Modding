using RoR2;
using RoR2.Networking;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        public abstract class BaseClientOrb
        {
            public Single totalDuration = 0f;
            public Single remainingDuration;

            public ClientOrbController outer;

            public static NetworkWriter write = new NetworkWriter();

            public abstract void Begin();
            public abstract void Tick( Single deltaT );
            public abstract void End();


            protected void SendDamage( DamageInfo damage, GameObject target )
            {
                if( NetworkServer.active )
                {
                    if( target )
                    {
                        HealthComponent hc = target.GetComponent<HealthComponent>();
                        if( hc )
                        {
                            hc.TakeDamage( damage );
                        }
                        GlobalEventManager.instance.OnHitEnemy( damage, target );
                    }

                    GlobalEventManager.instance.OnHitAll( damage, target );
                } else if( ClientScene.ready )
                {
                    write.StartMessage( 53 );
                    write.Write( target );
                    WriteDmgInfo( write, damage );
                    write.Write( (target.GetComponent<HealthComponent>() != null) );
                    write.FinishMessage();
                    ClientScene.readyConnection.SendWriter( write, QosChannelIndex.defaultReliable.intVal );
                }
            }

            protected static void WriteDmgInfo( NetworkWriter writer, DamageInfo damageInfo )
            {
                writer.Write( damageInfo.damage );
                writer.Write( damageInfo.crit );
                writer.Write( damageInfo.attacker );
                writer.Write( damageInfo.inflictor );
                writer.Write( damageInfo.position );
                writer.Write( damageInfo.force );
                writer.Write( damageInfo.procChainMask.mask );
                writer.Write( damageInfo.procCoefficient );
                writer.Write( (System.Byte)damageInfo.damageType );
                writer.Write( (System.Byte)damageInfo.damageColorIndex );
                writer.Write( (System.Byte)(damageInfo.dotIndex + 1) );
            }
        }
    }
}
