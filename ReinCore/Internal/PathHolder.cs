using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;

namespace ReinCore
{
    internal class PathHolder
    {
        private List<String> paths = new List<String>();

        internal void AddPath( String path )
        {
            this.paths.Add( path );
        }

        ~PathHolder()
        {
            for( Int32 i = 0; i < this.paths.Count; ++i )
            {
                var file = this.paths[i];
                try
                {
                    File.Delete( file );
                } catch
                {
                    Log.Error( "Failed to remove file: {0}", file );
                }
            }
        }
    }
}
