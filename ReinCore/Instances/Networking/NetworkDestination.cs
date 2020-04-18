using System;
using System.Collections.Generic;
using BepInEx;
using Mono.Cecil;
using RoR2;
using RoR2.Networking;
using UnityEngine.Networking;

namespace ReinCore
{
    [Flags]
    public enum NetworkDestination : Byte
    {
        Clients = 1,
        Server = 2,
    }

    internal static class DestinationExtensions
    {
        internal static Boolean ShouldSend( this NetworkDestination dest )
        {
            var isServer = NetworkServer.active;
            var isClient = NetworkClient.active;

            return !(isServer && dest == NetworkDestination.Server); 
        }

        internal static Boolean ShouldRun( this NetworkDestination dest )
        {
            var isServer = NetworkServer.active;
            var isClient = NetworkClient.active;

            if( isServer && (dest & NetworkDestination.Server) != 0 )
            {
                return true;
            }

            if( isClient && (dest & NetworkDestination.Clients) != 0 )
            {
                return true;
            }

            return false;
        }

        internal static Header GetHeader( this NetworkDestination dest, Int32 typeCode )
        {
            return new Header( typeCode, dest );
        }
    }
}
