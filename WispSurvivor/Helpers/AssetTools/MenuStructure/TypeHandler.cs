#if MATEDITOR
using RoR2;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    /*
    internal abstract class TypeHandler
    {
        internal static TypeHandler FindOrCreate( Type t, PropertyInfo p )
        {
            if( typeLookup.ContainsKey( t ) && typeLookup[t] != null )
            {
                var temp = typeLookup[t];
                return typeLookup[t];
            } else
            {
                Main.LogE( "Unhandled type: " + t.Name );
                return null;
            }
        }
        private static Dictionary<Type, TypeHandler> typeLookup = new Dictionary<Type, TypeHandler>()
        {
            { typeof(Single), new GenericTypeHandler<Single>( SingleMenu.Draw ) },
        
            { typeof(Boolean), new GenericTypeHandler<Boolean>( BooleanMenu.Draw ) },

            { typeof(Int32), new GenericTypeHandler<Int32>( Int32Menu.Draw ) },

            { typeof(Vector2), new GenericTypeHandler<Vector2>( Vector2Menu.Draw ) },

            { typeof(Vector3), new GenericTypeHandler<Vector3>( Vector3Menu.Draw ) },

            { typeof(Vector4), new GenericTypeHandler<Vector4>( Vector4Menu.Draw ) },

            { typeof(Color), new GenericTypeHandler<Color>( ColorMenu.Draw ) },

            { typeof(StandardMaterial.RampInfo), new GenericTypeHandler<StandardMaterial.RampInfo>( EnumMenu<StandardMaterial.RampInfo>.Draw ) },

            { typeof(StandardMaterial.DecalLayer), new GenericTypeHandler<StandardMaterial.DecalLayer>( EnumMenu<StandardMaterial.DecalLayer>.Draw ) },

            { typeof(StandardMaterial.CullMode), new GenericTypeHandler<StandardMaterial.CullMode>( EnumMenu<StandardMaterial.CullMode>.Draw ) },

            { typeof(StandardMaterial.PrintDirection), new GenericTypeHandler<StandardMaterial.PrintDirection>( EnumMenu<StandardMaterial.PrintDirection>.Draw ) },

            { typeof(EliteIndex), new GenericTypeHandler<EliteIndex>( EnumMenu<EliteIndex>.Draw ) },
            
            { typeof(MaterialBase.TextureData), new GenericTypeHandler<MaterialBase.TextureData>( TextureDataMenu.Draw ) },

            { typeof(MaterialBase.ScaleOffsetTextureData), new GenericTypeHandler<MaterialBase.ScaleOffsetTextureData>( ScaleOffsetTextureDataMenu.Draw ) },
        };

        internal abstract System.Object Draw( System.Object instance, MenuRenderer renderer, MenuAttribute settings );
    }
    */
}
#endif