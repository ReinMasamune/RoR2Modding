using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static class MaterialLibrary
    {
        private static HashSet<Material> mappedMaterials = new HashSet<Material>();
        private static Dictionary<MaterialIndex, GenericAccessor<Material>> vanilla = new Dictionary<MaterialIndex, GenericAccessor<Material>>();
        private static Dictionary<String, GenericAccessor<Material>> custom = new Dictionary<String, GenericAccessor<Material>>();



        internal static Boolean HasMaterial( Material mat)
        {
            return mappedMaterials.Contains( mat );
        }

        internal static Material GetMaterialRaw( String key )
        {
            if( !custom.ContainsKey( key ) )
            {
                Main.LogE( "Invalid key: " + key );
                return null;
            }
            var mat = custom[key].value;
            mappedMaterials.Add( mat );
            return mat;
        }

        internal static Material GetMaterialRaw( MaterialIndex key )
        {
            if( !vanilla.ContainsKey( key ) )
            {
                Main.LogE( "Invalid key: " + key.ToString() );
                return null;
            }
            var mat = vanilla[key].value;
            mappedMaterials.Add( mat );
            return mat;
        }

        internal static Material GetMaterialClone( String key )
        {
            return UnityEngine.Object.Instantiate<Material>( GetMaterialRaw( key ) );
        }

        internal static Material GetMeshClone( MaterialIndex key )
        {
            return UnityEngine.Object.Instantiate<Material>( GetMaterialRaw( key ) );
        }

        

        internal static void LogAll()
        {
            Main.LogI( "Materials::" );
            foreach( var v in mappedMaterials )
            {
                Main.LogI( v?.name );
            }
        }
    }
}