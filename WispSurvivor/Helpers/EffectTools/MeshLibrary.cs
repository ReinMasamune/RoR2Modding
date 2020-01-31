using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static class MeshLibrary
    {
        private static Dictionary<MeshIndex, GenericAccessor<Mesh>> vanilla = new Dictionary<MeshIndex, GenericAccessor<Mesh>>();
        private static Dictionary<String, GenericAccessor<Mesh>> custom = new Dictionary<String, GenericAccessor<Mesh>>();

        internal static void AddAccessor( GenericAccessor<Mesh> accessor )
        {
            if( accessor.isVanilla )
            {
                var key = (MeshIndex)accessor.index;
                if( vanilla.ContainsKey( key ) )
                {
                    Main.LogE( "Duplicate key: " + key.ToString() );
                    return;
                }

                vanilla[key] = accessor;
            } else
            {
                var key = accessor.name;
                if( custom.ContainsKey( key ) )
                {
                    Main.LogE( "Duplicate key: " + key );
                    return;
                }

                custom[key] = accessor;
            }
        }

        internal static Mesh GetMesh( String key )
        {
            if( !custom.ContainsKey( key ) )
            {
                Main.LogE( "Invalid key: " + key );
                return null;
            }
            return custom[key].value;
        }

        internal static Mesh GetMesh( MeshIndex key )
        {
            if( !vanilla.ContainsKey( key ) )
            {
                Main.LogE( "Invalid key: " + key.ToString() );
                return null;
            }
            return vanilla[key].value;
        }
    }
}