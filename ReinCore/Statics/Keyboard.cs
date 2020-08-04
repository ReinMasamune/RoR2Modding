namespace ReinCore
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public static class KeyboardCore
    { 
        public static Boolean loaded { get; internal set; } = false;
        public static Boolean SetKey( GlobalKeys key, Color color )
        {
            if( key == GlobalKeys.AllKeys )
            {
                IList<GlobalKeys> list = activeKeyboardRGB.GetAllKeys();
                Boolean flag = true;
                for( Int32 i = 0; i < list.Count; ++i )
                {
                    GlobalKeys k = list[i];

                    if( k != GlobalKeys.AllKeys )
                    {
                        flag |= SetKey( k, color );
                    }
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
            Log.Warning( "KeyboardCore loaded" );
            rgbOptions[KeyboardType.Wooting] = new Wooting.WootingRGB( KeyboardType.Wooting );




            UpdateConnectedKeyboard();
            Log.Warning( "KeyboardCore loaded" );
            loaded = true;
        }

        internal static void UpdateConnectedKeyboard()
        {
            foreach( KeyValuePair<KeyboardType, IKeyboardRGB> types in rgbOptions )
            {
                _ = types.Key;
                IKeyboardRGB keyboard = types.Value;

                if( keyboard.CheckConnected() )
                {
                    activeKeyboardRGB = keyboard;
                    return;
                }
            }
            activeKeyboardRGB = null;
        }



        private static readonly Dictionary<KeyboardType,IKeyboardRGB> rgbOptions = new Dictionary<KeyboardType, IKeyboardRGB>();

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
#pragma warning disable IDE1006 // Naming Styles
        private static IKeyboardRGB _activeKeyboardRGB;
#pragma warning restore IDE1006 // Naming Styles
    }
}
