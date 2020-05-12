namespace ReinCore.Wooting
{
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer( CallingConvention.StdCall )]
    internal delegate void DisconnectedCallback();
}
