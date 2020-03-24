using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System.IO;

namespace ResourceTools
{
    //We mark this is internal because allowing other code to cause files to be loaded into the context of your code is just bad practice in many ways.
    //We mark this as static because it is a set of helper functions, and having multiple sets of these functions associated with instances makes no sense whatsoever.
    //We also do a "triple-/" below to create XML documentation for this class. That way in 6 months when we need to use it again we have some idea what it does.
    //In most cases these functions are direct mappings to existing functions, I am writing wrappers to show usage and for conveinence. Feel free to use these if you like.
    /// <summary>
    /// A class that contains various helper methods for loading embedded resources.
    /// </summary>
    internal static class EmbeddedResourceHelpers
    {
        /// <summary>
        /// Loads an embedded assetbundle.
        /// </summary>
        /// <param name="resourceBytes">The bytes returned from Properties.[resourcename]</param>
        /// <returns>The loaded assetbundle</returns>
        internal static AssetBundle LoadAssetBundle( Byte[] resourceBytes )
        {
            //Check to make sure that the byte array supplied is not null, and throw an appropriate exception if they are.
            if( resourceBytes == null ) throw new ArgumentNullException( nameof( resourceBytes ) );

            //Actually load the bundle with a Unity function.
            var bundle = AssetBundle.LoadFromMemory(resourceBytes);

            //Return the bundle as our result
            return bundle;
        }


        //This function is more towards the advanced side, you would use this if you wanted to load another library at runtime.
        //My particular use case is for loading a central framework that I use across all my mods, but there are many use
        /// <summary>
        /// Loads an embedded assenbly
        /// </summary>
        /// <param name="resourceBytes">The bytes returned from Properties.[resourcename]</param>
        /// <returns>The loaded assembly</returns>
        internal static Assembly LoadAssembly( Byte[] resourceBytes )
        {
            //Check to make sure that the byte array supplied is not null, and throw an appropriate exception if they are.
            if( resourceBytes == null ) throw new ArgumentNullException( nameof( resourceBytes ) );

            //Actually load the assembly
            var assembly = Assembly.Load(resourceBytes);

            //Return the assembly as our result
            return assembly;
        }


        //This is a very very niche function. It is used to load an embedded unmanaged assembly. If you don't know what that is, you don't need to load one.
        //But, for sake of explanation: it is compiled native code, meaning it was written in languages like C, C++, or Rust.
        //An example use case would be if you wanted to make a mod that interfaces with some kind of special hardware. For example, controlling RGB on a keyboard.
        /// <summary>
        /// Loads an embedded unmanaged library for use with [DllImport] and extern
        /// </summary>
        /// <param name="resourceDllName">The name of the file for use with [DllImport]. Should end in .dll</param>
        /// <param name="resourceBytes">The bytes returned from Properties.[resourcename]</param>
        internal static void LoadUnmanagedLibrary( String resourceDllName, Byte[] resourceBytes )
        {
            //Make sure that the arguments are not null, and throw an exception if they are.
            if( resourceBytes == null ) throw new ArgumentNullException( nameof( resourceBytes ) );
            if( String.IsNullOrEmpty( resourceDllName ) ) throw new ArgumentException( "Must not be null or empty", nameof( resourceDllName ) );
            if( !resourceDllName.EndsWith( ".dll" ) ) throw new ArgumentException( "Must end in .dll", nameof( resourceDllName ) );

            //Get the AssemblyName for the current assembly
            var assemblyName = Assembly.GetExecutingAssembly()?.GetName();
            if( assemblyName == null ) throw new Exception( "GetExecutingAssembly returned null... wut?" );

            //Uses name and version of the executing assembly to make help deal with conflicts.
            var directory = String.Format( "{0}.{1}", assemblyName.Name, assemblyName.Version );

            //If the directory doesn't exist, create it.
            if( !Directory.Exists( directory ) ) Directory.CreateDirectory( directory );

            //Gets the main path
            var environmentPath = Environment.GetEnvironmentVariable( "PATH" );
            
            //Check if the directory we are using is already there, if not, add it.
            if( !environmentPath.Split( ';' ).Contains( directory ) ) Environment.SetEnvironmentVariable( "PATH", String.Format( "{0};{1}", directory, environmentPath ) );

            //Add the dll name to the directory to make our path.
            var dllPath = Path.Combine( directory, resourceDllName );

            //If the file already exists, and is identical to what we want to write, then return.
            if( File.Exists( dllPath ) && resourceBytes.SequenceEqual( File.ReadAllBytes( dllPath ) ) )
            {
                pathHolder.AddPath( dllPath );
                return;
            }

            File.WriteAllBytes( dllPath, resourceBytes );
        }

        //Used to clean up the created files on close.
        private static PathHolder pathHolder = new PathHolder();
        private class PathHolder
        {
            private List<String> paths = new List<String>();
            internal void AddPath( String path ) => this.paths.Add( path );

            //This is a destructor, its called when this object is destroyed. For Unity things you should be using OnDisable/Destroy, but this is better in this situation.
            ~PathHolder()
            {
                foreach( var path in this.paths )
                {
                    try
                    {
                        File.Delete( path );
                    } catch { }
                }
            }
        }
    }
}