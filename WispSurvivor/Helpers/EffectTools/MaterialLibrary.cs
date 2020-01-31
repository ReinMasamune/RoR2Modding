using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static class MaterialLibrary
    {
        private static Dictionary<MaterialIndex, GenericAccessor<Material>> vanilla = new Dictionary<MaterialIndex, GenericAccessor<Material>>();
        private static Dictionary<String, GenericAccessor<Material>> custom = new Dictionary<String, GenericAccessor<Material>>();

        internal static void AddAccessor( GenericAccessor<Material> accessor )
        {
            if( accessor.isVanilla )
            {
                var key = (MaterialIndex)accessor.index;
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

        internal static Material GetMaterial( String key )
        {
            if( !custom.ContainsKey( key ) )
            {
                Main.LogE( "Invalid key: " + key );
                return null;
            }
            return custom[key].value;
        }

        internal static Material GetMaterial( MaterialIndex key )
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