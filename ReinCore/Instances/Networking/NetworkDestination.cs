namespace ReinCore
{
    using System;

    using UnityEngine.Networking;

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
            Boolean isServer = NetworkServer.active;
            _ = NetworkClient.active;

            return !( isServer && dest == NetworkDestination.Server );
        }

        internal static Boolean ShouldRun( this NetworkDestination dest )
        {
            Boolean isServer = NetworkServer.active;
            Boolean isClient = NetworkClient.active;

            return isServer && ( dest & NetworkDestination.Server ) != 0 ? true : isClient && ( dest & NetworkDestination.Clients ) != 0;
        }

        internal static Header GetHeader( this NetworkDestination dest, Int32 typeCode ) => new Header( typeCode, dest );
    }
}
