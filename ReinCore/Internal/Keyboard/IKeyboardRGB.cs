using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReinCore
{
    interface IKeyboardRGB
    {
        IList<GlobalKeys> GetAllKeys();
        Boolean CheckConnected();
        Boolean CheckIsValid( KeyboardType type );
        void LogDeviceInfo();
        Boolean SetKeyRGB( GlobalKeys key, Color rgb );
        void Activate();
        void Deactivate();
    }
}
