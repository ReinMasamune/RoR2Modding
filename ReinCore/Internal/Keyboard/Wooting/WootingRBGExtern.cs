using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace ReinCore.Wooting.Native
{
    internal static class WootingRGBExtern
    {
        /// <summary>
        /// Check if keyboard connected.
        ///
        /// This function offers a quick check if the keyboard is connected.This doesn't open the keyboard or influences reading.
        /// It is recommended to poll this function at the start of your application and after a disconnect.
        /// </summary>
        /// <returns>This function returns true (1) if keyboard is found.</returns>
        [DllImport( WootingRGB.wootingDllName, EntryPoint = "wooting_rgb_kbd_connected", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.I1 )]
        internal static extern Boolean wooting_rgb_kbd_connected();


        /// <summary>
        /// Set callback for when a keyboard disconnects.
        /// The callback will be called when a Wooting keyboard disconnects.This will trigger after a failed color change.
        /// </summary>
        /// <param name="cb">The function pointer of the callback</param>
        [DllImport( WootingRGB.wootingDllName, EntryPoint = "wooting_rgb_set_disconnected_cb", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void wooting_rgb_set_disconnected_cb( DisconnectedCallback cb );


        /// <summary>
        /// Reset all colors on keyboard to the original colors. 
        /// This function will restore all the colours to the colours that were originally on the keyboard.This function
        /// should be called when you close the application.
        /// </summary>
        /// <returns>None</returns>
        [DllImport( WootingRGB.wootingDllName, EntryPoint = "wooting_rgb_reset", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.I1 )]
        internal static extern Boolean wooting_rgb_reset();


        /// <summary>
        /// Directly reset 1 key on the keyboard to the original color.
        /// This function will directly reset the color of 1 key on the keyboard.This will not influence the keyboard color array.
        /// Use this function for simple applifications, like a notification.Use the array functions if you want to change the entire keyboard.
        /// </summary>
        /// <param name="row">The horizontal index of the key</param>
        /// <param name="column">The vertical index of the key</param>
        /// <returns>This functions return true (1) if the colour is reset.</returns>
        [DllImport( WootingRGB.wootingDllName, EntryPoint = "wooting_rgb_direct_reset_key", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.I1 )]
        internal static extern Boolean wooting_rgb_direct_reset_key( Byte row, Byte column );


        /// <summary>
        /// Send the colors from the color array to the keyboard.
        /// This function will send the changes made with the wooting_rgb_array_**_** functions to the keyboard.
        /// </summary>
        /// <returns>This functions return true (1) if the colours are updated.</returns>
        [DllImport( WootingRGB.wootingDllName, EntryPoint = "wooting_rgb_array_update_keyboard", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.I1 )]
        internal static extern Boolean wooting_rgb_array_update_keyboard();


        /// <summary>
        /// Change the auto update flag for the wooting_rgb_array_**_** functions.
        /// This function can be used to set a auto update trigger after every change with a wooting_rgb_array_** _** function.
        /// Standard is set to false.
        /// </summary>
        /// <param name="auto_update">Change the auto update flag</param>
        [DllImport( WootingRGB.wootingDllName, EntryPoint = "wooting_rgb_array_auto_update", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void wooting_rgb_array_auto_update( [MarshalAs( UnmanagedType.I1 )] Boolean auto_update );


        /// <summary>
        /// Directly set and update 1 key on the keyboard.
        /// This function will directly change the color of 1 key on the keyboard.This will not influence the keyboard color array.
        /// Use this function for simple applifications, like a notification.Use the array functions if you want to change the entire keyboard.
        /// </summary>
        /// <param name="row">The horizontal index of the key</param>
        /// <param name="column">The vertical index of the key</param>
        /// <param name="red">A 0-255 value of the red color</param>
        /// <param name="green">A 0-255 value of the green color</param>
        /// <param name="blue">A 0-255 value of the blue color</param>
        /// <returns>This functions return true (1) if the colour is set.</returns>
        [DllImport( WootingRGB.wootingDllName, EntryPoint = "wooting_rgb_direct_set_key", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.I1 )]
        internal static extern Boolean _DirectSetKey( Byte row, Byte column, Byte red, Byte green, Byte blue );


        /// <summary>
        /// Set a single color in the colour array.
        /// This function will set a single color in the colour array.This will not directly update the keyboard(unless the flag is set), so it can be called frequently.For example in a loop that updates the entire keyboard, if you don't want to send a C array from a different programming language.
        /// </summary>
        /// <param name="row">The horizontal index of the key</param>
        /// <param name="column">The vertical index of the key</param>
        /// <param name="red">A 0-255 value of the red color</param>
        /// <param name="green">A 0-255 value of the green color</param>
        /// <param name="blue">A 0-255 value of the blue color</param>
        /// <returns>This functions return true (1) if the colours are changed (optional: updated).</returns>
        [DllImport( WootingRGB.wootingDllName, EntryPoint = "wooting_rgb_array_set_single", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.I1 )]
        internal static extern Boolean wooting_rgb_array_set_single( Byte row, Byte column, Byte red, Byte green, Byte blue );


        /// <summary>
        /// Set a full colour array.
        /// This function will set a complete color array.This will not directly update the keyboard (unless the flag is set). 
        /// Use our online tool to generate a color array:
        /// If you use a non-C language it is recommended to use the wooting_rgb_array_set_single function to change the colors to avoid compatibility issues.
        /// </summary>
        /// <param name="colors_buffer">Pointer to a buffer of a full color array</param>
        /// <returns>This functions return true (1) if the colours are changed (optional: updated).</returns>
        [DllImport( WootingRGB.wootingDllName, EntryPoint = "wooting_rgb_array_set_full", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.I1 )]
        internal static extern Boolean wooting_rgb_array_set_full( [MarshalAs( UnmanagedType.LPArray, SizeConst = WootingRGB.maxRows * WootingRGB.maxCols )] KeyColour[,] colors_buffer );


        [DllImport( WootingRGB.wootingDllName, CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr wooting_rgb_device_info();


    }
}
