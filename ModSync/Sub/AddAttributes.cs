namespace ModSync
{
    using BepInEx;
    using R2API.Utils;
    using System;
    using System.Reflection;
    using RoR2.Networking;
    using System.Collections.Generic;
    using UnityEngine.Networking;

    public partial class Main
    {
        partial void AddAttributes()
        {
            this.FirstFrame += this.ScanAndAddAttributes;
        }

        private void ScanAndAddAttributes()
        {
            var allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;

            var clientListInfo = typeof(NetworkMessageHandlerAttribute).GetField("clientMessageHandlers", allFlags);
            var clientList = (List<NetworkMessageHandlerAttribute>)clientListInfo.GetValue(typeof(NetworkMessageHandlerAttribute));

            var serverListInfo = typeof(NetworkMessageHandlerAttribute).GetField("serverMessageHandlers", allFlags );
            var serverList = (List<NetworkMessageHandlerAttribute>)serverListInfo.GetValue(typeof(NetworkMessageHandlerAttribute));

            FieldInfo netAtribHandler = typeof(NetworkMessageHandlerAttribute).GetField( "messageHandler", allFlags );

            foreach( MemberInfo m in this.GetType().GetMembers( allFlags ) )
            {
                var attrib = m.GetCustomAttribute<NetworkMessageHandlerAttribute>();
                if( attrib == null ) continue;

                var del = (NetworkMessageDelegate)Delegate.CreateDelegate(typeof(NetworkMessageDelegate), (MethodInfo)m);
                netAtribHandler.SetValue( attrib, del );


                if( attrib.client ) clientList.Add( attrib );
                if( attrib.server ) serverList.Add( attrib );
            }
        }
    }
}
