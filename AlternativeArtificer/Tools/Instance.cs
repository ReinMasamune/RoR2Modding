namespace AlternativeArtificer.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal static class Instance<TModule>
        where TModule : struct, IModule<TModule>
    {
        private static Boolean ready = false;
        private static TModule instance;
        internal static ref TModule get
        {
            get
            {
                if(ready) return ref instance;
                throw new InvalidOperationException();
            }
        }

        internal static void Supply(TModule module)
        {
            if(ready) throw new InvalidOperationException();
            instance = module;
            ready = true;
        }
    }
}
