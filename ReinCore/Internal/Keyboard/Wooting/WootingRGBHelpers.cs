using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using static ReinCore.Wooting.Native.WootingRGBExtern;

namespace ReinCore.Wooting
{
    internal static class WootingRGBHelpers
    {
        internal static event Action disconnectCallback;
        private static DisconnectedCallback disconnected;
        internal static void SetupDisconnectCallback()
        {
            disconnected = DisconnectCB;
            wooting_rgb_set_disconnected_cb( disconnected );
        }

        private static void DisconnectCB()
        {
            disconnectCallback?.Invoke();
        }

        internal static Boolean UpdateKeys()
        {
            return wooting_rgb_array_update_keyboard();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static RGBDeviceInfo GetDeviceInfo()
        {
            return Marshal.PtrToStructure<RGBDeviceInfo>( wooting_rgb_device_info() );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        internal static Boolean SetKey( GlobalKeys key, KeyColour color )
        {
            var coord = key.GetWootingKeyCoords();
            
            return wooting_rgb_array_set_single(coord.row, coord.col, color.r, color.g, color.b );
        }

        internal static Boolean ResetKey( GlobalKeys key )
        {
            var coords = key.GetWootingKeyCoords();
            return wooting_rgb_direct_reset_key( coords.row, coords.col );
        }

        internal static Boolean IsConnected()
        {
            return wooting_rgb_kbd_connected();
        }

        internal static Boolean AppExit()
        {
            return wooting_rgb_reset();
        }
    }
}
