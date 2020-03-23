using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ReinCore
{
    public static class KeyboardCore
    {
        public static Boolean loaded { get; internal set; } = false;

        [DllImport(wootingDllName)]
        public static extern Boolean wooting_rgb_kbd_connected();



        static KeyboardCore()
        {
            loaded = Loaders.ExtractUnmanagedDll( wootingDllName, Properties.Resources.wooting_rgb_sdk64 );
        }

        internal const String wootingDllName = "wooting-rgb-sdk64.dll";
        


    }
}
