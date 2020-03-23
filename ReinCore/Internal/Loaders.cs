using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using BepInEx;

namespace ReinCore
{
    internal static class Loaders
    {
        internal static PathHolder pathHolder = new PathHolder();
        internal static Boolean ExtractUnmanagedDll( String dllName, Byte[] bytes )
        {
            var asm = Assembly.GetExecutingAssembly();
            var names = asm.GetManifestResourceNames();
            var name = asm.GetName();

            var dir = String.Format( "{0}.{1}", name.Name, name.Version );

            if( !Directory.Exists( dir ) )
            {
                Directory.CreateDirectory( dir );
            }

            var path = Environment.GetEnvironmentVariable( "PATH" );
            var pathParts = path.Split( ';' );
            var found = false;

            for( Int32 i = 0; i < pathParts.Length; ++i )
            {
                if( pathParts[i] == dir )
                {
                    found = true;
                    break;
                }
            }

            if( !found )
            {
                Environment.SetEnvironmentVariable( "PATH", String.Format( "{0};{1}", dir, path ) );
            }

            var dllPath = Path.Combine( dir, dllName );
            var rewrite = true;
            if( File.Exists( dllPath ) )
            {
                var existing = File.ReadAllBytes( dllPath );
                if( bytes.SequenceEqual( existing ) )
                {
                    rewrite = false;
                }

            }
            if( rewrite )
            {
                File.WriteAllBytes( dllPath, bytes );
                pathHolder.AddPath( dllPath );
            }

            return true;
        }


        //internal static Boolean LoadUnmanagedAssemblyFromStream( String resourceName, String locationName )
        //{
        //    //var tempFilePath = System.IO.Path.Combine( System.IO.Path.GetTempPath(), String.Format("ReinCore\\{0}\\{1}\\", ReinCore.ver, locationName ) );
        //    var tempFilePath = "";
        //    Log.Debug( String.Format( "temp path: {0}", tempFilePath ) );
        //    //if( !Directory.Exists( tempFilePath ) ) Directory.CreateDirectory( tempFilePath );
        //    var dllPath = System.IO.Path.Combine( tempFilePath, String.Format( "{0}", resourceName ) );
        //    Log.Debug( String.Format( "dll path: {0}", dllPath ) );

        //    using( Stream dllStream = Assembly.GetExecutingAssembly().GetManifestResourceStream( String.Format( "ReinCore.Assemblies.{0}", resourceName ) ) )
        //    {
        //        try
        //        {
        //            using( Stream fileStream = new FileStream( resourceName, FileMode.Create ) )
        //            {
        //                const Int32 size = 4096;
        //                var buffer = new Byte[size];

        //                while( true )
        //                {
        //                    var read = dllStream.Read(buffer, 0, size );
        //                    if( read < 1 ) break;
        //                    fileStream.Write( buffer, 0, read );
        //                }
        //            }
        //        } catch
        //        {
        //            Log.Error( String.Format( "Failed to load unmanaged assembly: {0}", resourceName ) );
        //            return false;
        //        }
        //    }

        //    //if( !SetDllDirectory( tempFilePath ) )
        //    //{
        //    //    Log.Error( String.Format( "Failed to set dll path for unmanaged assembly: {0}", resourceName ) );
        //    //    return false;
        //    //}

        //    var pointer = LoadLibrary( dllPath );
        //    if( pointer == IntPtr.Zero )
        //    {
        //        Log.Error( String.Format( "Load failed for unmanaged assembly: {0}", resourceName ) );
        //        return false;
        //    }



        //    //try
        //    //{
        //    //    Directory.Delete( dllPath, true );
        //    //} catch
        //    //{
        //    //    Log.Error( String.Format( "Failed to clean up temporary .dll for assembly: {0}", streamPath ) );
        //    //    return false;
        //    //}
        //    return true;
        //}

        //[DllImport( "kernel32", SetLastError = true, CharSet = CharSet.Unicode )]
        //internal static extern IntPtr LoadLibrary( String lpFileName );

        //[DllImport( "kernel32", SetLastError = true, CharSet = CharSet.Unicode )]
        //[return: MarshalAs( UnmanagedType.Bool )]
        //internal static extern Boolean SetDllDirectory( String lpPathName );
    }
}
