using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ReinCore.Wooting
{
    [StructLayout( LayoutKind.Sequential )]
    internal struct RGBDeviceInfo
    {
        public bool Connected { get; private set; }

        public string Model { get; private set; }

        public byte MaxRows { get; private set; }

        public byte MaxColumns { get; private set; }

        public byte KeycodeLimit { get; private set; }

        public DeviceType DeviceType { get; private set; }
    }
}
