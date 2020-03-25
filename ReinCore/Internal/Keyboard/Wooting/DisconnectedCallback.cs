using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ReinCore.Wooting
{
    [UnmanagedFunctionPointer( CallingConvention.StdCall )]
    internal delegate void DisconnectedCallback();
}
