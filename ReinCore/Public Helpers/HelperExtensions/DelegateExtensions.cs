namespace ReinCore
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public static class DelegateExtensions
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void Merge<TDelegate>( this TDelegate source, ref TDelegate destination ) where TDelegate : MulticastDelegate => destination = (TDelegate)MulticastDelegate.Combine( destination, source );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void Unmerge<TDelegate>( this TDelegate source, ref TDelegate destination ) where TDelegate : MulticastDelegate => destination = (TDelegate)MulticastDelegate.Remove( destination, source );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
