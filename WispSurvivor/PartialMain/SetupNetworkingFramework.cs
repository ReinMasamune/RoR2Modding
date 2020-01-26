#if NETWORKING
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Networking;

namespace RogueWispPlugin
{

    internal partial class Main
    {
        const Int16 NT_messageIndex = 300;
        internal static NetworkWriter NT_writer = new NetworkWriter();
        internal static Dictionary<String,BaseNetMethodDef> NT_messageLookup = new Dictionary<String, BaseNetMethodDef>();

        partial void SetupNetworkingFramework()
        {
            this.Enable += this.NT_AddHooks;
            this.Disable += this.NT_RemoveHooks;
        }

        private void NT_RemoveHooks()
        {
            On.RoR2.Networking.NetworkMessageHandlerAttribute.CollectHandlers -= ScanForNetworkAttributes;
        }
        private void NT_AddHooks()
        {
            On.RoR2.Networking.NetworkMessageHandlerAttribute.CollectHandlers += ScanForNetworkAttributes;
        }

        private static void ScanForNetworkAttributes( On.RoR2.Networking.NetworkMessageHandlerAttribute.orig_CollectHandlers orig )
        {
            orig();
            const BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;

            var clientListInfo = typeof(NetworkMessageHandlerAttribute).GetField("clientMessageHandlers", allFlags);
            var clientList = (List<NetworkMessageHandlerAttribute>)clientListInfo?.GetValue(typeof(NetworkMessageHandlerAttribute));

            var serverListInfo = typeof(NetworkMessageHandlerAttribute).GetField("serverMessageHandlers", allFlags);
            var serverList = (List<NetworkMessageHandlerAttribute>)serverListInfo?.GetValue(typeof(NetworkMessageHandlerAttribute));

            FieldInfo netAttribHandler = typeof(NetworkMessageHandlerAttribute).GetField("messageHandler", allFlags);

            var members = typeof( Main ).GetMember( methodName, allFlags );
            MemberInfo handler = null;
            if( members.Length == 1 )
            {
                handler = members[0];
            } else
            {
                Main.LogF( "Did not find member: " + methodName + " properly." + members.Length + " members found" );
            }

            if( handler == null )
            {
                Main.LogF( "Handler was null" );
                return;
            }
            var attrib = handler.GetCustomAttribute<NetworkMessageHandlerAttribute>();
            if( attrib == null )
            {
                Main.LogF( "Handler did not have proper attribute" );
                return;
            }
            var del = (NetworkMessageDelegate)Delegate.CreateDelegate( typeof( NetworkMessageDelegate ), (MethodInfo)handler );
            netAttribHandler?.SetValue( attrib, del );

            if( attrib.client )
                clientList?.Add( attrib );
            if( attrib.server )
                serverList?.Add( attrib );
        }

        const String methodName = "NT_HandleMessage";
        [NetworkMessageHandler( client = true, server = true, msgType = NT_messageIndex )]
        internal static void NT_HandleMessage( NetworkMessage message )
        {
            var msg = message.ReadMessage<ReinNetMessageHolder>();
            if( !msg.safeToSend )
            {
                Main.LogF( "Placeholder7" );
                return;
            }

            msg.handler.HandleMessage( msg.message );
        }

        internal class ReinNetMessageHolder : MessageBase
        {
            private String messageType;
            internal Boolean safeToSend { get; private set; } = false;
            internal BaseNetMethodDef handler { get; private set; }
            internal NetworkMessageData contextData { get; private set; }
            internal ReinNetMessage message { get; private set; }


            public override void Serialize( NetworkWriter writer )
            {
                writer.Write( this.safeToSend );
                if( !this.safeToSend )
                {
                    Main.LogF( "Placeholder5" );
                    return;
                }
                writer.Write( this.messageType );
                this.contextData.Write( writer );
                this.handler.Serialize( this.message, writer );
            }

            public override void Deserialize( NetworkReader reader )
            {
                this.safeToSend = reader.ReadBoolean();
                if( !this.safeToSend )
                {
                    Main.LogF( "Placeholder6" );
                    return;
                }
                this.messageType = reader.ReadString();
                this.contextData = NetworkMessageData.Read( reader );
                if( NT_messageLookup.ContainsKey( this.messageType ) )
                {
                    this.handler = NT_messageLookup[this.messageType];
                } else
                {
                    this.handler = null;
                    Main.LogF( "Unregistered message type: " + this.messageType + " recieved.\nThis means that the message was not fully Deserialized, and may cause other networking issues.");
                    return;
                }
                this.message = this.handler?.Deserialize( reader );
            }

            internal ReinNetMessageHolder( ReinNetMessage message, NetworkMessageData context )
            {
                this.message = message;
                if( this.message == null )
                {
                    Main.LogF( "Placeholder1" );
                    return;
                }
                this.messageType = message.GetType().ToString();
                if( !NT_messageLookup.ContainsKey( this.messageType ) )
                {
                    Main.LogF( "Placeholder2" );
                    return;
                }
                this.handler = NT_messageLookup[this.messageType];
                if( this.handler == null )
                {
                    Main.LogF( "Placeholder3" );
                    return;
                }
                this.contextData = context;
                if( this.contextData == null )
                {
                    Main.LogF( "Placeholder4" );
                    return;
                }
                this.safeToSend = true;
            }

            public ReinNetMessageHolder() { }
        }

        internal abstract class ReinNetMessage
        {
            internal BaseNetMethodDef handler { get; private set; }
            internal NetworkMessageData context { get; private set; }

            public abstract void Serialize( NetworkWriter writer );
            public abstract void Deserialize( NetworkReader reader );

            [Flags]
            public enum Dest
            {
                None = 0,
                Client = 1,
                Server = 2,
                All = 3
            }

            public void Send(Dest destination = Dest.All, Boolean authority = false)
            {
                this.context = new NetworkMessageData( authority );

                var holder = new ReinNetMessageHolder( this, this.context );

                if( !holder.safeToSend )
                {
                    Main.LogF( "PlaceholderPi" );
                    return;
                }

                if( this.context.fromServer && ( destination & Dest.Server ) > Dest.None )
                {
                    foreach( NetworkConnection connection in NetworkServer.connections )
                    {
                        connection.SendByChannel( NT_messageIndex, holder, (Int32)QosType.Reliable );
                    }
                }
                if( this.context.fromClient && ( destination & Dest.Client ) > Dest.None )
                {
                    ClientScene.readyConnection.SendByChannel( NT_messageIndex, holder, (Int32)QosType.Reliable );
                }
            }
        }

        internal class BaseNetMethodDef
        {
            internal Type type;

            internal BaseNetMethodDef( Type t )
            {
                this.type = t;
            }

            internal void Int_Register()
            {
                NT_messageLookup[this.type.ToString()] = this;
            }

            internal virtual void HandleMessage( ReinNetMessage message ) { }
            internal virtual ReinNetMessage Deserialize( NetworkReader reader ) { return null; }
            internal virtual void Serialize( ReinNetMessage message, NetworkWriter writer ) { }
        }

        internal class NetworkMethodDefinition<T> : BaseNetMethodDef where T : ReinNetMessage, new()
        {
            private MethodInfo deserializeMethod;
            internal Action<T> netAction;
            internal NetworkMethodDefinition(Action<T> netAction) : base(typeof(T))
            {
                this.netAction = netAction;
            }

            public void Register()
            {
                base.Int_Register();
            }

            internal override ReinNetMessage Deserialize( NetworkReader reader )
            {
                T value = new T();
                value.Deserialize( reader );
                return value;
            }

            internal override void Serialize( ReinNetMessage message, NetworkWriter writer )
            {
                var msg = message as T;
                msg.Serialize( writer );
            }

            internal override void HandleMessage( ReinNetMessage message )
            {
                var msg = message as T;
                if( msg == null )
                {
                    Main.LogF( "Placeholder42" );
                    return;
                }

                this.netAction( msg );
            }

        }

        internal sealed class NetworkMessageData
        {
            internal Boolean fromClient { get; private set; }
            internal Boolean fromAuthority { get; private set; }
            internal Boolean fromServer { get; private set; }
            
            internal NetworkMessageData(Boolean authority)
            {
                this.fromClient = NetworkClient.active;
                this.fromAuthority = authority;
                this.fromServer = NetworkServer.active;
            }

            private NetworkMessageData( Boolean client, Boolean authority, Boolean server )
            {
                this.fromClient = client;
                this.fromAuthority = authority;
                this.fromServer = server;
            }

            internal void Write( NetworkWriter writer )
            {
                writer.Write( this.fromClient );
                writer.Write( this.fromAuthority );
                writer.Write( this.fromServer );
            }

            internal static NetworkMessageData Read( NetworkReader reader )
            {
                var client = reader.ReadBoolean();
                var authority = reader.ReadBoolean();
                var server = reader.ReadBoolean();
                return new NetworkMessageData( client, authority, server );
            }
        }
    }
}
#endif