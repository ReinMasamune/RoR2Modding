namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

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
