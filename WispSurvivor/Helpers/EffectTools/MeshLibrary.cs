using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static class MeshLibrary
    {
        private static HashSet<Mesh> mappedMeshes = new HashSet<Mesh>();
        private static Dictionary<MeshIndex, GenericAccessor<Mesh>> vanilla = new Dictionary<MeshIndex, GenericAccessor<Mesh>>();
        private static Dictionary<String, GenericAccessor<Mesh>> custom = new Dictionary<String, GenericAccessor<Mesh>>();

        
        internal static Boolean HasMesh( Mesh mesh )
        {
            return mappedMeshes.Contains( mesh );
        }

        internal static Mesh GetMeshRaw( String key )
        {
            if( !custom.ContainsKey( key ) )
            {
                Main.LogE( "Invalid key: " + key );
                return null;
            }
            var mesh = custom[key].value;
            mappedMeshes.Add( mesh );
            return mesh;
        }

        internal static Mesh GetMeshRaw( MeshIndex key )
        {
            if( !vanilla.ContainsKey( key ) )
            {
                Main.LogE( "Invalid key: " + key.ToString() );
                return null;
            }
            var mesh = vanilla[key].value;
            mappedMeshes.Add( mesh );
            return mesh;
        }

        internal static Mesh GetMeshClone( String key )
        {
            return UnityEngine.Object.Instantiate<Mesh>( GetMeshRaw( key ) );
        }

        internal static Mesh GetMeshClone( MeshIndex key )
        {
            return UnityEngine.Object.Instantiate<Mesh>( GetMeshRaw( key ) );
        }

        internal static void LogAll()
        {
            Main.LogI( "Materials::" );
            foreach( var v in mappedMeshes )
            {
                Main.LogI( v?.name );
            }
        }
    }
}