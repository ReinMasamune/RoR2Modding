using System;
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

        public static void SetKeyColor( Byte row, Byte col, Color32 color, Boolean setImmediate = false )
        {
            if( setImmediate )
            {
                wooting_rgb_direct_set_key( row, col, color.r, color.g, color.b );
            } else
            {
                wooting_rgb_array_set_single( row, col, color.r, color.g, color.b );
                setOnNextFrame = true;
            }
        }


        static KeyboardCore()
        {
            ResourceTools.EmbeddedResourceHelpers.LoadUnmanagedLibrary( wootingDllName, Properties.Resources.wooting_rgb_sdk64 );


            handle = GCHandle.Alloc( new WootingDisconnectDelegate(InternalWootingDisconnect), GCHandleType.Pinned );
            wooting_rgb_set_disconnected_cb( handle.AddrOfPinnedObject() );


            loaded = true;
            ReinCore.onDisable += ReinCore_onDisable;
            lastFrameJob = default;
            ReinCore.lateUpdate += ReinCore_lateUpdate;
        }

        

        internal const String wootingDllName = "wooting-rgb-sdk64.dll";


        private static KeyboardType kbType = KeyboardType.None;

        private static NativeArray<Color32> prevColorsArray;
        private static JobHandle lastFrameJob;
        private static GCHandle handle;
        private static Boolean setOnNextFrame = false;

        private static void ReinCore_onDisable()
        {
            lastFrameJob.Complete();

            try
            {
                if( wooting_rgb_reset() > 0 )
                {
                    Log.Message( "Wooting colors reset succcessfully" );
                } else
                {
                    Log.Error( "Wooting colors not reset successfully" );
                }
                handle.Free();
            } catch
            {
                Log.Error( "Error resetting wooting colors" );
            }
        }

        private static void ReinCore_lateUpdate()
        {
            if( setOnNextFrame )
            {
                setOnNextFrame = wooting_rgb_array_update_keyboard() == 0;
            }
        }

        private static void OnWootingDisconnect()
        {
            ReinCore.fixedUpdate += PollForWootingConnected;
        }

        private static void PollForWootingConnected()
        {
            if( wooting_rgb_kbd_connected() != 0 )
            {
                onWootingConnect?.Invoke();
                ReinCore.fixedUpdate -= PollForWootingConnected;
            }
        }

        private static void InternalWootingDisconnect()
        {
            onWootingDisconnect?.Invoke();
        }


        private delegate void WootingDisconnectDelegate();
        private static event WootingDisconnectDelegate onWootingDisconnect;
        private delegate void WootingConnectDelegate();
        private static event WootingConnectDelegate onWootingConnect;
        #region Wooting SDK
        /// <summary>
        /// This function offers a quick check if the keyboard is connected. It is recommended to poll this function at the start of your application and after a disconnect.
        /// </summary>
        /// <returns>true if keyboard is found</returns>
        [DllImport( wootingDllName )]
        private static extern Byte wooting_rgb_kbd_connected();


        /// <summary>
        /// The callback will be called when a Wooting keyboard disconnects. This will trigger after a failed color change.
        /// </summary>
        /// <param name="cb">The function pointer of the callback</param>
        [DllImport( wootingDllName )]
        private static extern void wooting_rgb_set_disconnected_cb( IntPtr cb );


        /// <summary>
        /// This function will restore all the colours to the colours that were originally on the keyboard. This function must be called when you close the application.
        /// </summary>
        /// <returns>true if keyboard is reset</returns>
        [DllImport( wootingDllName )]
        private static extern Byte wooting_rgb_reset();


        /// <summary>
        /// This function will directly change the color of 1 key on the keyboard. This will not influence the keyboard color array. Use this function for simple amplifications, like a notification. Use the array functions if you want to change the entire keyboard.
        /// </summary>
        /// <param name="row">horizontal index of the key</param>
        /// <param name="column">vertical index of the key</param>
        /// <param name="red">value of the red color</param>
        /// <param name="green">value of the green color</param>
        /// <param name="blue">value of the blue color</param>
        /// <returns>true if the colour is set</returns>
        [DllImport( wootingDllName )]
        private static extern Byte wooting_rgb_direct_set_key( Byte row, Byte column, Byte red, Byte green, Byte blue );


        /// <summary>
        /// This function will directly reset the color of 1 key on the keyboard. This will not influence the keyboard color array. Use this function for simple amplifications, like a notification. Use the array functions if you want to change the entire keyboard
        /// </summary>
        /// <param name="row">horizontal index of the key</param>
        /// <param name="column">vertical index of the key</param>
        /// <returns>true if the colour is reset</returns>
        [DllImport( wootingDllName )]
        private static extern Byte wooting_rgb_direct_reset_key( Byte row, Byte column );


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport( wootingDllName )]
        private static extern Byte wooting_rgb_array_update_keyboard();

        /*
        /// <summary>
        /// This function can be used to set a auto update trigger after every change with the wooting_rgb_array single and full functions function. Standard is set to false.
        /// </summary>
        /// <param name="auto_update">auto update flag value</param>
        [DllImport( wootingDllName )]
        private static extern void wooting_rbg_array_auto_update( Byte auto_update );
        */


        /// <summary>
        /// This function will set a single color in the colour array. This will not directly update the keyboard (unless the flag is set), so it can be called frequently. For example in a loop that updates the entire keyboard. This way you can avoid dealing with C arrays from different languages.
        /// </summary>
        /// <param name="row">horizontal index of the key</param>
        /// <param name="column">vertical index of the key</param>
        /// <param name="red">value of the red color</param>
        /// <param name="green">value of the green color</param>
        /// <param name="blue">value of the blue color</param>
        /// <returns>true if the colours are changed (if auto update flag: updated)</returns>
        [DllImport( wootingDllName )]
        private static extern Byte wooting_rgb_array_set_single( Byte row, Byte column, Byte red, Byte green, Byte blue );


        /*
        /// <summary>
        /// This function will set a complete color array. This will not directly update the keyboard (unless the flag is set). If you use a non-C language it is recommended to use the wooting_rgb_array_set_single function to change the colors to avoid compatibility issues.
        /// 
        /// Buffer should be layed out as [Row0Col0Red, Row0Col0Green, Row0Col0Blue, Row0Col1Red, Row0Col1Green, Row0Col1Blue, ..., Row5Row20Red, Row5Row20Green, Row5Row20Blue].
        /// 
        /// Expected size is 6 row * 21 columns * 3 colors per key = 378 bytes.
        /// </summary>
        /// <param name="buffer">pointer to a buffer of a full color array</param>
        /// <returns>true if the colours are changed (if auto update flag: updated)</returns>
        [DllImport( wootingDllName )]
        private static extern Boolean wooting_rgb_array_set_full( Byte[] buffer );
        */
        #endregion

    }
}
