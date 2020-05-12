namespace ReinCore.Wooting
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout( LayoutKind.Sequential )]
    internal struct RGBDeviceInfo
    {
#pragma warning disable IDE1006 // Naming Styles
        public Boolean Connected { get; private set; }
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
        public String Model { get; private set; }
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
        public Byte MaxRows { get; private set; }
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
        public Byte MaxColumns { get; private set; }
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
        public Byte KeycodeLimit { get; private set; }
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
        public DeviceType DeviceType { get; private set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
