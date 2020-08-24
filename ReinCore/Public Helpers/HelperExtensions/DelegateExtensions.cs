namespace ReinCore
{
    using System;


    public static class DelegateHelper
    {
        public static TDelegate Combine<TDelegate>(params TDelegate[] delegates)
            where TDelegate : Delegate
        {
            return (TDelegate)Delegate.Combine(delegates);
        }
    }
    public static class DelegateExtensions
    {
        public static void Merge<TDelegate>( this TDelegate source, ref TDelegate destination ) where TDelegate : MulticastDelegate => destination = (TDelegate)MulticastDelegate.Combine( destination, source );

        public static void Unmerge<TDelegate>( this TDelegate source, ref TDelegate destination ) where TDelegate : MulticastDelegate => destination = (TDelegate)MulticastDelegate.Remove( destination, source );
    }
}
