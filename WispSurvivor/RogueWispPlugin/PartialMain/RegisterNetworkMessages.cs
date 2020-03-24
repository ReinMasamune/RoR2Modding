#if NETWORKING
using BepInEx;
using System;
using System.Reflection;
using ReinCore;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        //private Accessor<BaseUnityPlugin,BepInEx.Logging.ManualLogSource> logger = new Accessor<BaseUnityPlugin, BepInEx.Logging.ManualLogSource>( "Logger" );
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
                Main.LogM( "NetLib is not installed, loading fallback networking." );
                
                var asm = Assembly.Load( Rein.Properties.Resources.NetLib );
                var plugin = asm.GetType( "NetLib.Internals.Plugin" );
                if( plugin == null )
                {
                    Main.LogF( "Failed to load fallback networking. Multiplayer will not work." );
                    return;
                }

                var netLib = base.gameObject.AddComponent( plugin ) as BaseUnityPlugin;

                //this.logger.Set( netLib, base.Logger );
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