namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using BepInEx;
    using RoR2;

    /// <summary>
    /// For adding soundbanks
    /// </summary>
    public static class SoundsCore
    {
        /// <summary>
        /// Is this module loaded?
        /// </summary>
        public static Boolean loaded { get; internal set; } = false;

        /// <summary>
        /// Loads a soundbank from a byte array
        /// </summary>
        /// <param name="bankBytes"></param>
        /// <param name="onIndexRecieved"></param>
        public static void LoadSoundbank( Byte[] bankBytes, Action<UInt32> onIndexRecieved )
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( SoundsCore ) );
            }

            if( banksLoaded )
            {
                throw new Exception( "Too late to add bank" );
            }

            IntPtr ptr = PointerArrayHolder.Create( bankBytes );
            RoR2Application_onLoad += () =>
            {
                AKRESULT res = AkSoundEngine.LoadBank( ptr, (UInt32)bankBytes.Length, out UInt32 outId );
                if( !res.Flag( AKRESULT.AK_Success ) )
                {
                    Log.Error( "Bank load failure" );
                } else
                {
                    onIndexRecieved( outId );
                    activeIndicies.Add( outId, ptr );
                }
                PointerArrayHolder.Remove( ptr );
            };
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void UnloadSoundbank( UInt32 index )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( SoundsCore ) );
            }

            if( activeIndicies.TryGetValue( index, out IntPtr val ) )
            {
                _ = AkSoundEngine.UnloadBank( index, val );
            } else
            {
                throw new Exception( "No bank with that index loaded" );
            }
        }






        static SoundsCore()
        {
            RoR2Application_onLoad += () => banksLoaded = true;





            loaded = true;
        }

        private static Boolean banksLoaded = false;

        private static readonly Dictionary<UInt32,IntPtr> activeIndicies = new Dictionary<UInt32, IntPtr>();

#pragma warning disable IDE1006 // Naming Styles
        private static event Action RoR2Application_onLoad
#pragma warning restore IDE1006 // Naming Styles
        {
            add => value.Merge(ref RoR2Application.onLoad);
            remove => value.Unmerge( ref RoR2Application.onLoad );
        }

        private struct PointerArrayHolder
        {
            private static readonly Dictionary<IntPtr,PointerArrayHolder> instances = new Dictionary<IntPtr, PointerArrayHolder>();
            internal static IntPtr Create( Byte[] bytes )
            {
                IntPtr ptr = Marshal.AllocHGlobal( bytes.Length );
                Marshal.Copy( bytes, 0, ptr, bytes.Length );

                var holder = new PointerArrayHolder(null, ptr);
                instances.Add( ptr, holder );

                return ptr;
            }
            internal static void Remove( IntPtr pointer ) => _ = instances.Remove( pointer );

            private PointerArrayHolder( Byte[] bytes, IntPtr ptr )
            {
                this.bytes = bytes;
                this.pointer = ptr;
            }

            private readonly Byte[] bytes;
            private readonly IntPtr pointer;
        }
    }
}
