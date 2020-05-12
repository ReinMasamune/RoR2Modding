namespace ReinCore.Wooting
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;

    [UnmanagedFunctionPointer( CallingConvention.StdCall )]
    internal delegate void DisconnectedCallback();
}
