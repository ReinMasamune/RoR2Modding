#if NETWORKING
using BepInEx;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Networking;
using static RoR2.NetworkExtensions;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        internal Boolean netLibInstalled = false;
        partial void RegisterNetworkMessages()
        {
            foreach( var p in this.plugins )
            {
                if( p.Metadata.GUID == NetLib.NetLib.guid )
                {
                    this.netLibInstalled = true;
                    break;
                }
            }

            if( !this.netLibInstalled )
            {
                Main.LogW( "Loading networking" );
                Assembly execAssembly = Assembly.GetExecutingAssembly();
                System.IO.Stream stream = execAssembly.GetManifestResourceStream( "RogueWispPlugin.Assemblies.NetLib.dll" );
                var data = new Byte[stream.Length];
                stream.Read( data, 0, data.Length );
                var asm = Assembly.Load( data );
                var plugin = asm.GetType( "NetLib.Internals.Plugin" );
                if( plugin == null )
                {
                    Main.LogE( "Couldn't load networking" );
                    return;
                }

                var netLib = base.gameObject.AddComponent( plugin ) as BaseUnityPlugin;
            }
        }

        /*
        private void NetworkMessageHandlerAttribute_CollectHandlers( On.RoR2.Networking.NetworkMessageHandlerAttribute.orig_CollectHandlers orig )
        {
            orig();
            var clientListInfo = typeof(NetworkMessageHandlerAttribute).GetField("clientMessageHandlers", Const.AllFlags);
            var clientList = (List<NetworkMessageHandlerAttribute>)clientListInfo?.GetValue(typeof(NetworkMessageHandlerAttribute));

            var serverListInfo = typeof(NetworkMessageHandlerAttribute).GetField("serverMessageHandlers", Const.AllFlags);
            var serverList = (List<NetworkMessageHandlerAttribute>)serverListInfo?.GetValue(typeof(NetworkMessageHandlerAttribute));

            FieldInfo netAttribHandler = typeof(NetworkMessageHandlerAttribute).GetField("messageHandler", Const.AllFlags);

            MemberInfo member = typeof(Main).GetMethod( nameof(HandleMessageMain), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public );
            var handlerAttribute = member.GetCustomAttribute<NetworkMessageHandlerAttribute>();

            var method = member as MethodInfo;

            var networkDelegate = Delegate.CreateDelegate( typeof(NetworkMessageDelegate), method ) as NetworkMessageDelegate;

            netAttribHandler?.SetValue( handlerAttribute, networkDelegate );

            if( handlerAttribute.client ) clientList?.Add( handlerAttribute );
            if( handlerAttribute.server ) serverList?.Add( handlerAttribute );

        }

        [NetworkMessageHandler( client = true, server = true, msgType = 31415 )]
        private static void HandleMessageMain( NetworkMessage message )
        {
            NetMessageHolder holder = message.ReadMessage<NetMessageHolder>();
            if( !holder.safe )
            {
                Plugin.LogError( "TEMP" );
                return;
            }

            holder.handler.HandleMessage( holder.message );
        }
        */
    }
}
#endif