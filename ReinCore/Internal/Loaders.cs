using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using BepInEx;

namespace ReinCore
{
    internal static class Loaders
    {
        internal static Boolean LoadUnmanagedAssemblyFromStream( String streamPath, String locationName )
        {
            var tempFilePath = System.IO.Path.Combine( System.IO.Path.GetTempPath(), String.Format("ReinCore\\{0}\\{1}\\", ReinCore.ver, locationName ) );
            if( !Directory.Exists( tempFilePath ) ) Directory.CreateDirectory( tempFilePath );
            var dllPath = System.IO.Path.Combine( tempFilePath, String.Format( "{0}", streamPath ) );

            using( Stream dllStream = Assembly.GetExecutingAssembly().GetManifestResourceStream( streamPath ) )
            {
                try
                {
                    using( Stream fileStream = File.Create( dllPath ) )
                    {
                        const Int32 size = 4096;
                        var buffer = new Byte[size];

                        while( true )
                        {
                            var read = dllStream.Read(buffer, 0, size );
                            if( read < 1 ) break;
                            fileStream.Write( buffer, 00, read );
                        }
                    }
                } catch
                {
                    Log.Error( String.Format( "Failed to load unmanaged assembly: {0}", streamPath ) );
                    return false;
                }
            }

            var pointer = LoadLibrary( dllPath );
            if( pointer == IntPtr.Zero )
            {
                Log.Error( String.Format( "Load failed for unmanaged assembly: {0}", streamPath ) );
                return false;
            }

            //if( !SetDllDirectory( dllPath ) )
            //{
            //    Log.Error( String.Format( "Failed to set dll path for unmanaged assembly: {0}", streamPath ) );
            //    return false;
            //}

            //try
            //{
            //    Directory.Delete( dllPath, true );
            //} catch
            //{
            //    Log.Error( String.Format( "Failed to clean up temporary .dll for assembly: {0}", streamPath ) );
            //    return false;
            //}
            return true;
        }

        [DllImport( "kernel32", SetLastError = true, CharSet = CharSet.Unicode )]
        internal static extern IntPtr LoadLibrary( String lpFileName );

        [DllImport( "kernel32", SetLastError = true, CharSet = CharSet.Unicode )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern Boolean SetDllDirectory( String lpPathName );
    }
}
