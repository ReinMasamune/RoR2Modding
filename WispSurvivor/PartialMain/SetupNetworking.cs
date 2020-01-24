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
        internal static Dictionary<String, NetworkMethodDefinition> NT_messageLookup = new Dictionary<String, NetworkMethodDefinition>();

        partial void SetupNetworking()
        {
            this.Load += this.NT_LogNetworkInfo;
        }

        private void NT_LogNetworkInfo() => throw new NotImplementedException();

        internal virtual class ReinNetMessage : MessageBase
        {
            public override void Serialize( NetworkWriter writer )
            {
                base.Serialize( writer );
                if( NT_messageLookup.ContainsKey( this.GetType().ToString() ) )
                {
                    writer.Write( this.messageType );
                } else
                {
                    writer.Write( "Invalid" );
                }
                writer.Write( this.messageType );
            }

            public override void Deserialize( NetworkReader reader )
            {
                base.Deserialize( reader );

            }
        }

        internal virtual class 
    }
#endif
}
