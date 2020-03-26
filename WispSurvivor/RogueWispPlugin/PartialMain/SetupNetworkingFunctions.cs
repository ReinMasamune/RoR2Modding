#if !NETWORKING
using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Networking;
using static RoR2.NetworkExtensions;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        partial void SetupNetworkingFunctions()
        {
            new NetworkMethodDefinition<TestMessage>( ( message ) => Main.LogW( message.testText ) ).Register();
            new NetworkMethodDefinition<BuffMessage>( ( message ) =>
            {
                if( message.removeAll )
                {
                    for( Int32 i = message.body.GetBuffCount( message.buff ); i > 0; --i )
                    {
                        message.body.RemoveBuff( message.buff );
                    }
                } else if( message.remove )
                {
                    for( Int32 i = message.stacks; i > 0; --i )
                    {
                        message.body.RemoveBuff( message.buff );
                    }
                } else if( message.applyDuration )
                {
                    for( Int32 i = 0; i < message.stacks; ++i )
                    {
                        message.body.AddTimedBuff( message.buff, message.duration );
                    }
                } else
                {
                    for( Int32 i = 0; i < message.stacks; ++i )
                    {
                        message.body.AddBuff( message.buff );
                    }
                }
            } ).Register();
            new NetworkMethodDefinition<DamageMessage>( ( message ) =>
            {
                if( message.damage == null ) return;
                if( NetworkServer.active )
                {
                    if( message.callDamage )
                    {
                        if( message.target == null || message.target.healthComponent == null )
                        {
                            Main.LogE( "DealDamage: callDamage was true with null target or null target healthcomponent" );
                        } else
                        {
                            message.target.healthComponent.TakeDamage( message.damage );
                        }
                    }
                    if( message.callHitEnemy )
                    {
                        if( message.target == null || message.target.healthComponent == null )
                        {
                            Main.LogE( "DealDamage: callHitEnemy was true with null target or null target healthcomponent" );
                        } else
                        {
                            GlobalEventManager.instance.OnHitEnemy( message.damage, message.target.healthComponent.gameObject );
                        }
                    }
                    if( message.callHitWorld )
                    {
                        GlobalEventManager.instance.OnHitAll( message.damage, (message.target && message.target.healthComponent ? message.target.healthComponent.gameObject : null) );
                    }
                }
            } ).Register();
        }

        static void TestNetworking( String text )
        {
            new TestMessage( text ).Send();
        }

        internal class TestMessage : ReinNetMessage
        {
            internal String testText { get; private set; }
            public override void Serialize( NetworkWriter writer )
            {
                writer.Write( this.testText );
            }
            public override void Deserialize( NetworkReader reader )
            {
                this.testText = reader.ReadString();
            }
            public TestMessage( String text )
            {
                this.testText = text;
            }
            public TestMessage() { }
        }

        static void AddBuff( CharacterBody body, BuffIndex buff, Int32 stacks = 1 )
        {
            if( body == null ) return;
            if( NetworkServer.active )
            {
                for( Int32 i = 0; i < stacks; ++i )
                {
                    body.AddBuff( buff );
                }
            } else
            {
                new BuffMessage( body, buff, stacks ).Send( ReinNetMessage.Dest.Server );
            }
        }

        static void AddTimedBuff( CharacterBody body, BuffIndex buff, Single duration, Int32 stacks = 1 )
        {
            if( body == null ) return;
            if( NetworkServer.active )
            {
                for( Int32 i = 0; i < stacks; ++i )
                {
                    body.AddTimedBuff( buff, duration );
                }
            } else
            {
                new BuffMessage( body, buff, stacks, duration, true ).Send( ReinNetMessage.Dest.Server );
            }
        }

        static void RemoveBuff( CharacterBody body, BuffIndex buff, Int32 stacks = 1 )
        {
            if( body == null ) return;
            if( NetworkServer.active )
            {
                for( Int32 i = 0; i < stacks; ++i )
                {
                    body.RemoveBuff( buff );
                }
            } else
            {
                new BuffMessage( body, buff, stacks, 0f, false, true ).Send( ReinNetMessage.Dest.Server );
            }
        }

        static void RemoveAllBuff( CharacterBody body, BuffIndex buff )
        {
            if( body == null ) return;
            if( NetworkServer.active )
            {
                for( Int32 i = body.GetBuffCount( buff ); i > 0; --i )
                {
                    body.RemoveBuff( buff );
                }
            } else
            {
                new BuffMessage( body, buff, 0, 0f, false, false, true ).Send( ReinNetMessage.Dest.Server );
            }
        }

        internal class BuffMessage : ReinNetMessage
        {
            public BuffIndex buff { get; private set; }
            public Int32 stacks { get; private set; }
            public Single duration { get; private set; }
            public Boolean applyDuration { get; private set; }
            public Boolean remove { get; private set; }
            public Boolean removeAll { get; private set; }
            public CharacterBody body { get; private set; }
            private BuffMode mode;
            [Flags]
            private enum BuffMode : byte
            {
                None = 0,
                ApplyDuration = 1,
                Remove = 2,
                RemoveAll = 3,
            }
            public override void Serialize( NetworkWriter writer )
            {
                writer.WriteBuffIndex( this.buff );
                writer.Write( this.stacks );
                writer.Write( this.duration );
                writer.Write( this.body.gameObject );
                writer.Write( (Byte)this.mode );
            }
            public override void Deserialize( NetworkReader reader )
            {
                this.buff = reader.ReadBuffIndex();
                this.stacks = reader.ReadInt32();
                this.duration = reader.ReadSingle();
                this.body = reader.ReadGameObject().GetComponent<CharacterBody>();
                this.mode = (BuffMode)reader.ReadByte();
                this.applyDuration = this.mode.HasFlag( BuffMode.ApplyDuration );
                this.remove = this.mode.HasFlag( BuffMode.Remove );
                this.removeAll = this.mode.HasFlag( BuffMode.RemoveAll );
            }
            public BuffMessage( CharacterBody body, BuffIndex buff, Int32 stacks = 0, Single duration = 0f, Boolean applyDuration = false, Boolean remove = false, Boolean removeAll = false )
            {
                this.buff = buff;
                this.body = body;
                this.stacks = stacks;
                this.duration = duration;
                this.applyDuration = applyDuration;
                this.remove = remove;
                this.removeAll = removeAll;

                this.mode = BuffMode.None;
                if( this.applyDuration ) this.mode |= BuffMode.ApplyDuration;
                if( this.remove ) this.mode |= BuffMode.Remove;
                if( this.removeAll ) this.mode |= BuffMode.RemoveAll;
            }
            public BuffMessage() { }
        }

        static void DealDamage( DamageInfo damage, HurtBox target = null, Boolean callDamage = true, Boolean callHitEnemy = true, Boolean callHitWorld = true )
        {
            if( damage == null ) return;
            if( NetworkServer.active )
            {
                if( callDamage )
                {
                    if( target == null || target.healthComponent == null )
                    {
                        Main.LogE( "DealDamage: callDamage was true with null target or null target healthcomponent" );
                    } else
                    {
                        target.healthComponent.TakeDamage( damage );
                    }
                }
                if( callHitEnemy )
                {
                    if( target == null || target.healthComponent == null )
                    {
                        Main.LogE( "DealDamage: callHitEnemy was true with null target or null target healthcomponent" );
                    } else
                    {
                        GlobalEventManager.instance.OnHitEnemy( damage, target.healthComponent.gameObject );
                    }
                }
                if( callHitWorld )
                {
                    GlobalEventManager.instance.OnHitAll( damage, (target && target.healthComponent ? target.healthComponent.gameObject : null) );
                }
            } else
            {
                new DamageMessage( damage, target, callDamage, callHitEnemy, callHitWorld ).Send( ReinNetMessage.Dest.Server );
            }
        }

        internal class DamageMessage : ReinNetMessage
        {
            public DamageInfo damage { get; private set; }
            public HurtBox target { get; private set; }
            public Boolean callDamage { get; private set; }
            public Boolean callHitEnemy { get; private set; }
            public Boolean callHitWorld { get; private set; }

            private DamageMode mode;

            [Flags]
            private enum DamageMode : byte
            {
                None = 0,
                CallDamage = 1,
                CallHitEnemy = 2,
                CallHitWorld = 4
            }

            public override void Serialize( NetworkWriter writer )
            {
                this.mode = DamageMode.None;
                if( this.callDamage ) this.mode |= DamageMode.CallDamage;
                if( this.callHitEnemy ) this.mode |= DamageMode.CallHitEnemy;
                if( this.callHitWorld ) this.mode |= DamageMode.CallHitWorld;
                writer.Write( (Byte)this.mode );
                HurtBoxReference.FromHurtBox( this.target ).Write( writer );
                Write( writer, this.damage );
            }

            public override void Deserialize( NetworkReader reader )
            {
                this.mode = (DamageMode)reader.ReadByte();
                this.callDamage = (this.mode & DamageMode.CallDamage) > DamageMode.None;
                this.callHitEnemy = (this.mode & DamageMode.CallHitEnemy) > DamageMode.None;
                this.callHitWorld = (this.mode & DamageMode.CallHitWorld) > DamageMode.None;
                this.target = reader.ReadHurtBoxReference().ResolveHurtBox();
                this.damage = ReadDamageInfo( reader );
            }

            public DamageMessage( DamageInfo damage, HurtBox target = null, Boolean callDamage = true, Boolean callHitEnemy = true, Boolean callHitWorld = true )
            {
                this.damage = damage;
                this.target = target;
                this.callDamage = callDamage;
                this.callHitEnemy = callHitEnemy;
                this.callHitWorld = callHitWorld;
            }


            public DamageMessage() { }
        }

        public static DamageInfo ReadDamageInfo( NetworkReader reader )
        {
            return new DamageInfo
            {
                damage = reader.ReadSingle(),
                crit = reader.ReadBoolean(),
                attacker = reader.ReadGameObject(),
                inflictor = reader.ReadGameObject(),
                position = reader.ReadVector3(),
                force = reader.ReadVector3(),
                procChainMask = ReadProcChainMask(reader),
                procCoefficient = reader.ReadSingle(),
                damageType = (DamageType)reader.ReadByte(),
                damageColorIndex = (DamageColorIndex)reader.ReadByte(),
                dotIndex = (DotController.DotIndex)(reader.ReadByte() - 1)
            };
        }

        public static void Write( NetworkWriter writer, DamageInfo damageInfo )
        {
            writer.Write( damageInfo.damage );
            writer.Write( damageInfo.crit );
            writer.Write( damageInfo.attacker );
            writer.Write( damageInfo.inflictor );
            writer.Write( damageInfo.position );
            writer.Write( damageInfo.force );
            Write( writer, damageInfo.procChainMask );
            writer.Write( damageInfo.procCoefficient );
            writer.Write( (byte)damageInfo.damageType );
            writer.Write( (byte)damageInfo.damageColorIndex );
            writer.Write( (byte)(damageInfo.dotIndex + 1) );
        }

        public static ProcChainMask ReadProcChainMask( NetworkReader reader )
        {
            return new ProcChainMask
            {
                mask = reader.ReadUInt16()
            };
        }

        public static void Write( NetworkWriter writer, ProcChainMask procChainMask )
        {
            writer.Write( procChainMask.mask );
        }

    }
}
#endif