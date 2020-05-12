namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using BepInEx;
    using Mono.Cecil;
    using RoR2;
    using RoR2.Networking;
    using UnityEngine.Networking;

    [Flags]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public enum NetworkDestination : Byte
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Clients = 1,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Server = 2,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    internal static class DestinationExtensions
    {
        internal static Boolean ShouldSend( this NetworkDestination dest )
        {
            Boolean isServer = NetworkServer.active;
            _ = NetworkClient.active;

            return !(isServer && dest == NetworkDestination.Server); 
        }

        internal static Boolean ShouldRun( this NetworkDestination dest )
        {
            Boolean isServer = NetworkServer.active;
            Boolean isClient = NetworkClient.active;

            return isServer && (dest & NetworkDestination.Server) != 0 ? true : isClient && (dest & NetworkDestination.Clients) != 0;
        }

        internal static Header GetHeader( this NetworkDestination dest, Int32 typeCode ) => new Header( typeCode, dest );
    }
}
