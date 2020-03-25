using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace ReinCore
{
    public static class KeyboardCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static Boolean SetKey( GlobalKeys key, Color color )
        {
            if( key == GlobalKeys.AllKeys )
            {
                var list = activeKeyboardRGB.GetAllKeys();
                var flag = true;
                for( Int32 i = 0; i < list.Count; ++i )
                {
                    var k = list[i];
                    
                    if( k != GlobalKeys.AllKeys ) flag |= SetKey( k, color );
                }
                return flag;
            } else
            {
                try
                {
                    return activeKeyboardRGB.SetKeyRGB( key, color );
                } catch
                {
                    return false;
                }
            }
        }

        static KeyboardCore()
        {
            rgbOptions[KeyboardType.Wooting] = new Wooting.WootingRGB( KeyboardType.Wooting );




            UpdateConnectedKeyboard();
            loaded = true;
        }

        internal static void UpdateConnectedKeyboard()
        {
            foreach( var types in rgbOptions )
            {
                var type = types.Key;
                var keyboard = types.Value;

                if( keyboard.CheckConnected() )
                {
                    activeKeyboardRGB = keyboard;
                    return;
                }
            }
            activeKeyboardRGB = null;
        }



        private static Dictionary<KeyboardType,IKeyboardRGB> rgbOptions = new Dictionary<KeyboardType, IKeyboardRGB>();

        private static IKeyboardRGB activeKeyboardRGB
        {
            get => _activeKeyboardRGB;
            set
            {
                if( value != _activeKeyboardRGB )
                {
                    _activeKeyboardRGB?.Deactivate();
                    _activeKeyboardRGB = value;
                    _activeKeyboardRGB?.Activate();
                }
            }
        }
        private static IKeyboardRGB _activeKeyboardRGB;
    }
}
