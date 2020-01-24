using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace RogueWispPlugin
{
#if NETWORKING
    internal partial class Main
    {
        const Int16 NT_messageIndex = 300;
        internal static NetworkWriter NT_writer = new NetworkWriter();
        internal static Dictionary<String,BaseNetMethodDef> NT_messageLookup = new Dictionary<String, BaseNetMethodDef>();

        partial void SetupNetworking()
        {
            this.Load += this.NT_LogNetworkInfo;
        }

        private void NT_LogNetworkInfo() => throw new NotImplementedException();

        internal class ReinNetMessage : MessageBase
        {
            internal BaseNetMethodDef handler { get; private set; }
            internal NetworkMessageData data { get; private set; }

            public override void Serialize( NetworkWriter writer )
            {
                base.Serialize( writer );
                if( NT_messageLookup.ContainsKey( this.GetType().ToString() ) )
                {
                    writer.Write( this.GetType().ToString() );
                } else
                {
                    writer.Write( "Invalid" );
                    Main.LogE( "Message type: " + this.GetType().ToString() + " does not have an associated definition." );
                }
            }

            public override void Deserialize( NetworkReader reader )
            {
                base.Deserialize( reader );
                var messageType = reader.ReadString();
                if( NT_messageLookup.ContainsKey( messageType ) )
                {
                    this.handler = NT_messageLookup[messageType];
                } else
                {

                }
            }
        }

        internal class BaseNetMethodDef
        {
            internal Type type;

            internal BaseNetMethodDef( Type t )
            {
                this.type = t;
                NT_messageLookup[this.type.ToString()] = this;
            }
        }

        internal class NetworkMethodDefinition<T> : BaseNetMethodDef where T : ReinNetMessage
        {
            internal Action<T> netAction;
            internal NetworkMethodDefinition(Action<T> netAction) : base( typeof( T ) )
            {
                this.netAction = netAction;
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
#endif
}
